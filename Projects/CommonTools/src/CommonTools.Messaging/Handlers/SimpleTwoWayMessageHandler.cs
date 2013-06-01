using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace CommonTools.Messaging.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TIncomingMessageType">The type of the incoming message type.</typeparam>
    /// <typeparam name="TOutgoingMessageType">The type of the outgoing message type.</typeparam>
    public partial class SimpleTwoWayMessageHandler<TIncomingMessageType, TOutgoingMessageType> : SimpleBaseMessageHandler<TIncomingMessageType, TOutgoingMessageType>
        where TIncomingMessageType : struct, IConvertible
        where TOutgoingMessageType : struct, IConvertible
    {
        #region members
        private string _ListenExchange;
        private string _SendExchange;
        private bool _HasSubscription = false;

        private static Type _LogType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
        #endregion

        #region events
        #endregion

        #region abstract implementations
        /// <summary>
        /// Gets the type of the log.
        /// </summary>
        /// <value>The type of the log.</value>
        protected override Type LogType { get { return _LogType; } }

        /// <summary>
        /// Called when subscriptions get resent.
        /// </summary>
        protected override void DoResendSubscriptions()
        {
            Subscribe();
        }

        /// <summary>
        /// Handles the model shutdown.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="reason">The RabbitMQ.Client.ShutdownEventArgs instance containing the event data.</param>
        public override void HandleModelShutdown(IModel model, ShutdownEventArgs reason)
        {
            this._HasSubscription = false;
            base.HandleModelShutdown(model, reason);
        }

        /// <summary>
        /// Does the subscribe.
        /// </summary>
        protected override void DoSubscribe()
        {
            Unsubscribe();

            bool isBoundToListenExchange = true;
            bool hasSubscribedToPublisher = true;

            lock (_ListenLock)
            {
                try { Model.QueueBind(_ReceiveQ, _ListenExchange, string.Empty, false, null); }
                catch (Exception ex)
                {
                    Log("Error at subscribing", ex, LogLevel.Warning);
                    isBoundToListenExchange = false;
                }
            }

            if (isBoundToListenExchange)
            {
                IBasicProperties prop = Model.CreateBasicProperties();
                prop.ReplyTo = _ReceiveQ;
                lock (_SendLock)
                {
                    try { Sender.BasicPublish(_SendExchange, string.Empty, prop, new byte[0]); }
                    catch (Exception ex)
                    {
                        try { base.Model.QueueUnbind(_ReceiveQ, _ListenExchange, null, null); }
                        catch (Exception e) { Log("Error at unsubscribing", e, LogLevel.Warning); }

                        Log("Error at subscribing", ex, LogLevel.Warning);

                        hasSubscribedToPublisher = false;
                    }
                }
            }

            _HasSubscription = hasSubscribedToPublisher && isBoundToListenExchange;
        }
        /// <summary>
        /// Called when [unsubscribe] method is executed.
        /// </summary>
        protected override void DoUnsubscribe()
        {
            if (this._HasSubscription)
            {
                try { base.Model.QueueUnbind(_ReceiveQ, _ListenExchange, null, null); }
                catch (Exception ex) { Log("Error at unsubscribing", ex, LogLevel.Warning); }
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <returns></returns>
        public bool SendMessage(TOutgoingMessageType messageType)
        {
            return SendMessage(messageType, _SendExchange);
        }
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageBody">The message body.</param>
        /// <returns></returns>
        public bool SendMessage<T>(TOutgoingMessageType messageType, T messageBody)
        {
            return SendMessage<T>(messageType, messageBody, _SendExchange);
        }
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <returns></returns>
        public bool SendRoutedMessage(TOutgoingMessageType messageType, string routingKey)
        {
            return SendMessage(messageType, _SendExchange, routingKey);
        }
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageBody">The message body.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <returns></returns>
        public bool SendRoutedMessage<T>(TOutgoingMessageType messageType, T messageBody, string routingKey)
        {
            return SendMessage<T>(messageType, messageBody, _SendExchange, routingKey);
        }
        /// <summary>
        /// Sends the reply message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="replyToQueue">The reply to queue.</param>
        /// <returns></returns>
        public bool SendReplyMessage(TOutgoingMessageType messageType, string replyToQueue)
        {
            return SendReply(messageType, _SendExchange, replyToQueue);
        }
        /// <summary>
        /// Sends the reply message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageBody">The message body.</param>
        /// <param name="replyToQueue">The reply to queue.</param>
        /// <returns></returns>
        public bool SendReplyMessage<T>(TOutgoingMessageType messageType, T messageBody, string replyToQueue)
        {
            return SendReply<T>(messageType, messageBody, replyToQueue);
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTwoWayMessageHandler&lt;TIncomingMessageType, TOutgoingMessageType&gt;"/> class.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <param name="listenExchange">The listen exchange.</param>
        /// <param name="listenExchangeType">Type of the listen exchange.</param>
        /// <param name="sendExchange">The send exchange.</param>
        /// <param name="sendExchangeType">Type of the send exchange.</param>
        public SimpleTwoWayMessageHandler(string ip
                                          , string listenExchange, string listenExchangeType
                                          , string sendExchange, string sendExchangeType)
            : this(ip, listenExchange, listenExchangeType, sendExchange, sendExchangeType, null) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTwoWayMessageHandler&lt;TIncomingMessageType, TOutgoingMessageType&gt;"/> class.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <param name="listenExchange">The listen exchange.</param>
        /// <param name="listenExchangeType">Type of the listen exchange.</param>
        /// <param name="sendExchange">The send exchange.</param>
        /// <param name="sendExchangeType">Type of the send exchange.</param>
        /// <param name="logManager">The log manager.</param>
        public SimpleTwoWayMessageHandler(string ip
                                          , string listenExchange, string listenExchangeType
                                          , string sendExchange, string sendExchangeType
                                          , ILogManager logManager)
            : base("SimpleTestMessageHandler"
                   , ip
                   , new List<DataExchangeInfo>() { new DataExchangeInfo(listenExchange, listenExchangeType) }
                   , new List<DataExchangeInfo>() { new DataExchangeInfo(sendExchange, sendExchangeType) }
                   , logManager)
        {
            _ListenExchange = listenExchange;
            _SendExchange = sendExchange;
        }
        #endregion
    }
}
