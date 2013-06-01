using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Messaging;
using RabbitMQ.Client;
using System.IO;
using RabbitMQ.Client.Exceptions;

namespace CommonTools.TestSuite.Messaging.Stubs
{
    public class TestMessageEventArgs : EventArgs
    {
        public string RoutingKey { get; set; }
        public string ConsumerTag { get; set; }
        public string Exchange { get; set; }
        public IBasicProperties Properties { get; set; }

        public override string ToString()
        {
            return string.Format("Routing key: {1}{0}Consumer tag: {2}{0}Exchange: {3}{0}Properties: {4}"
                , Environment.NewLine
                , this.RoutingKey ?? "NULL"
                , this.ConsumerTag ?? "NULL"
                , this.Exchange ?? "NULL"
                , this.Properties == null ? "NULL" : (this.Properties.Type));
        }

        public TestMessageEventArgs(string routingKey, string consumerTag, string exchange, IBasicProperties properties)
        {
            this.RoutingKey = routingKey;
            this.ConsumerTag = consumerTag;
            this.Exchange = exchange;
            this.Properties = properties;
        }
    }

    public partial class BasicTestMessageHandler : BaseMessageHandler
    {
        #region members
        private string _ListenExchange;
        private string _SendExchange;
        private bool _HasSubscription = false;

        private static Type _LogType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
        #endregion

        #region events
        public event EventHandler<TestMessageEventArgs> MessageReceived;
        #endregion

        #region abstract implementations
        /// <summary>
        /// Gets the type of the log.
        /// </summary>
        /// <value>The type of the log.</value>
        protected override Type LogType { get { return _LogType; } }

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
            if (MessageReceived != null)
                MessageReceived(this, new TestMessageEventArgs(routingKey, consumerTag, exchange, properties));
        }

        /// <summary>
        /// Called when subscriptions get resent.
        /// </summary>
        protected override void DoResendSubscriptions()
        {
            Subscribe();
        }
        /// <summary>
        /// Does the subscribe.
        /// </summary>
        /// <param name="unused">The unused.</param>
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
        /// <param name="adjustment">The base ETF info.</param>
        protected override void DoUnsubscribe()
        {
            if (this._HasSubscription)
            {
                try { base.Model.QueueUnbind(_ReceiveQ, _ListenExchange, null, null); }
                catch (Exception ex) { Log("Error at unsubscribing", ex, LogLevel.Warning); }

                this._HasSubscription = false;
            }
        }

        public override void HandleModelShutdown(IModel model, ShutdownEventArgs reason)
        {
            this._HasSubscription = false;
            base.HandleModelShutdown(model, reason);
        }
        #endregion

        #region public methods

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rawMessage"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public bool SendMessage<T>(T rawMessage, string messageType)
        {
            return SendMessage<T>(rawMessage, messageType, null);
        }
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rawMessage">The raw message.</param>
        /// <param name="requestType">Type of the request.</param>
        /// <returns></returns>
        public bool SendMessage<T>(T rawMessage, string messageType, string userId)
        {
            if (this.Status != BaseMessageHandlerStatus.Connected)
                return false;

            byte[] bytes;
            using (MemoryStream stream = new MemoryStream())
            {
                try { ProtoBuf.Serializer.Serialize<T>(stream, rawMessage); }
                catch (Exception e)
                {
                    Log("Error serializing message, see inner exception for further details.", e, LogLevel.Error);
                    return false;
                }
                bytes = stream.ToArray();
            }

            IBasicProperties basicProperties = Sender.CreateBasicProperties();
            basicProperties.Type = messageType;
            if (!string.IsNullOrEmpty(userId))
                basicProperties.UserId = userId;

            try { Sender.BasicPublish(_SendExchange, string.Empty, false, false, basicProperties, bytes); }
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

        /// <summary>
        /// Sends the request.
        /// </summary>
        /// <param name="requestType">Type of the request.</param>
        /// <returns></returns>
        public bool SendMessage(string messageType)
        {
            if (this.Status != BaseMessageHandlerStatus.Connected)
                return false;

            IBasicProperties basicProperties = Sender.CreateBasicProperties();
            basicProperties.Type = messageType;

            try { Sender.BasicPublish(_SendExchange, string.Empty, false, false, basicProperties, null); }
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
        /// Initializes a new instance of the <see cref="CurrencyProvider"/> class.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <param name="listenExchange">The listen exchange.</param>
        /// <param name="listenExchangeType">Type of the listen exchange.</param>
        /// <param name="sendExchange">The send exchange.</param>
        /// <param name="sendExchangeType">Type of the send exchange.</param>
        /// <param name="server">The server.</param>
        public BasicTestMessageHandler(string ip
                                          , string listenExchange, string listenExchangeType
                                          , string sendExchange, string sendExchangeType)
            : base("BasicTestMessageHandler"
                   , ip
                   , new List<DataExchangeInfo>() { new DataExchangeInfo(listenExchange, listenExchangeType) }
                   , new List<DataExchangeInfo>() { new DataExchangeInfo(sendExchange, sendExchangeType) }
                   , null)
        {
            _ListenExchange = listenExchange;
            _SendExchange = sendExchange;
        }
        #endregion
    }
}
