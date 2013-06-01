using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using System.Threading;
using RabbitMQ.Client.Exceptions;
using System.IO;

namespace CommonTools.Messaging.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TIncomingMessageType">The type of the incoming message type.</typeparam>
    /// <typeparam name="TOutgoingMessageType">The type of the outgoing message type.</typeparam>
    public abstract class SimpleBaseMessageHandler<TIncomingMessageType, TOutgoingMessageType> : AbstractMessageHandlerBase
        where TIncomingMessageType : struct, IConvertible
        where TOutgoingMessageType : struct, IConvertible
    {
        #region members
        Dictionary<string, TIncomingMessageType> _IncomingMessageTypeLookup;
        #endregion

        #region events
        /// <summary>
        /// Occurs when [message received].
        /// </summary>
        public event EventHandler<MessageEventArgs<TIncomingMessageType>> MessageReceived;
        #endregion

        #region abstracts

        #region abstract methods
        /// <summary>
        /// Called when subscribe method is executed
        /// </summary>
        protected abstract void DoSubscribe();
        /// <summary>
        /// Called when [unsubscribe] method is executed.
        /// </summary>
        protected abstract void DoUnsubscribe();
        #endregion

        #endregion

        #region overrides
        /// <summary>
        /// Called when a basic deliver message is received.
        /// </summary>
        /// <param name="consumerTag">The consumer tag.</param>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="redelivered">if set to <c>true</c> [redelivered].</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="body">The body.</param>
        protected override void OnHandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            if (properties != null
                && !string.IsNullOrEmpty(properties.Type)
                && _IncomingMessageTypeLookup.ContainsKey(properties.Type))
            {
                if (MessageReceived != null)
                    MessageReceived(this, new MessageEventArgs<TIncomingMessageType>(_IncomingMessageTypeLookup[properties.Type], body, properties.ReplyTo));
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Subscribes the specified currency.
        /// </summary>
        public void Subscribe()
        {
            lock (_SubscriptionLock)
            {
                DoSubscribe();
            }
        }

        /// <summary>
        /// Unsubscribes the specified currency.
        /// </summary>
        public void Unsubscribe()
        {
            lock (_SubscriptionLock)
            {
                DoUnsubscribe();
            }
        }
        #endregion

        #region public methods

        /// <summary>
        /// Sends the reply.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="body">The body.</param>
        /// <param name="senderQueue">The sender queue.</param>
        /// <returns></returns>
        protected bool SendReply<T>(TOutgoingMessageType messageType, T body, string senderQueue)
        {
            byte[] bytes = null;
            using (MemoryStream stream = new MemoryStream())
            {
                try { ProtoBuf.Serializer.Serialize<T>(stream, body); }
                catch (Exception e)
                {
                    Log("Error serializing message, see inner exception for further details.", e, LogLevel.Error);
                    return false;
                }
                bytes = stream.ToArray();
            }

            return Send(messageType, string.Empty, senderQueue, bytes);
        }
        /// <summary>
        /// Sends the reply.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="senderQueue">The sender queue.</param>
        /// <returns></returns>
        protected bool SendReply(TOutgoingMessageType messageType, string senderQueue)
        {
            return Send(messageType, string.Empty, senderQueue, null);
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="body">The body.</param>
        /// <param name="exchange">The exchange.</param>
        /// <returns></returns>
        protected bool SendMessage<T>(TOutgoingMessageType messageType, T body, string exchange)
        {
            return SendMessage<T>(messageType, body, exchange, string.Empty);
        }
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="body">The body.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <returns></returns>
        protected bool SendMessage<T>(TOutgoingMessageType messageType, T body, string exchange, string routingKey)
        {
            byte[] bytes = null;
            using (MemoryStream stream = new MemoryStream())
            {
                try { ProtoBuf.Serializer.Serialize<T>(stream, body); }
                catch (Exception e)
                {
                    Log("Error serializing message, see inner exception for further details.", e, LogLevel.Error);
                    return false;
                }
                bytes = stream.ToArray();
            }

            return Send(messageType, exchange, string.Empty, bytes);
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="exchange">The exchange.</param>
        /// <returns></returns>
        protected bool SendMessage(TOutgoingMessageType messageType, string exchange)
        {
            return Send(messageType, exchange, string.Empty, null);
        }
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <returns></returns>
        protected bool SendMessage(TOutgoingMessageType messageType, string exchange, string routingKey)
        {
            return Send(messageType, exchange, routingKey, null);
        }

        private bool Send(TOutgoingMessageType messageType, string exchange, string routingKey, byte[] body)
        {
            if (this.Status != BaseMessageHandlerStatus.Connected)
                return false;

            try
            {
                IBasicProperties basicProperties = Sender.CreateBasicProperties();
                basicProperties.Type = (messageType as Enum).ToString("d");
                basicProperties.ReplyTo = _ReceiveQ;

                Sender.BasicPublish(exchange, routingKey, false, false, basicProperties, body);
            }
            catch (AlreadyClosedException)
            {// that's fine, this exception will be thrown when the connection gets closed just before this call...
                return false;
            }
            catch (Exception e)
            {
                Log("Error while sending message, see inner exception for further details.", e, LogLevel.Warning);
                return false;
            }

            return true;
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMessageHandler&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="ip">The ip.</param>
        /// <param name="receiverExchanges">The receiver exchanges.</param>
        /// <param name="senderExchanges">The sender exchanges.</param>
        protected SimpleBaseMessageHandler(string providerName, string ip
            , List<DataExchangeInfo> receiverExchanges, List<DataExchangeInfo> senderExchanges)
            : this(providerName, ip, receiverExchanges, senderExchanges, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMessageHandler&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="ip">The ip.</param>
        /// <param name="receiverExchanges">The receiver exchanges.</param>
        /// <param name="senderExchanges">The sender exchanges.</param>
        /// <param name="logManager">The log manager.</param>
        protected SimpleBaseMessageHandler(string providerName, string ip
            , List<DataExchangeInfo> receiverExchanges, List<DataExchangeInfo> senderExchanges
            , ILogManager logManager)
            : base(providerName, ip, receiverExchanges, senderExchanges, logManager)
        {
            Type type = typeof(TIncomingMessageType);

            if (!type.IsEnum)
                throw new ArgumentException("TIncomingMessageType must be an enumerated type");

            _IncomingMessageTypeLookup = new Dictionary<string, TIncomingMessageType>();
            foreach (int value in Enum.GetValues(type))
                _IncomingMessageTypeLookup.Add(value.ToString(), (TIncomingMessageType)Enum.Parse(type, value.ToString()));
        }

        #endregion
    }
}
