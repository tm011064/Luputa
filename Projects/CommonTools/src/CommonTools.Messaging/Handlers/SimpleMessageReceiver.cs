using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Threading;

namespace CommonTools.Messaging.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TIncomingMessageType">The type of the incoming message type.</typeparam>
    public class SimpleMessageReceiver<TIncomingMessageType> : IBasicConsumer, IDisposable
    {
        #region members
        private Type _Type = typeof(SimpleMessageReceiver<TIncomingMessageType>);
        private ILogManager _LogManager;

        private ConnectionFactory _ConnectionFactory;
        private IConnection _Connection;
        private IModel _Model;

        private string _Exchange
                       , _ExchangeType
                       , _ReceiveQ;

        private bool _IsDurable
                     , _DoListenToAnyTopic
                     , _IsDisposed;

        /// <summary>
        /// 
        /// </summary>
        protected readonly object _SubscriptionLock = new object();
        /// <summary>
        /// 
        /// </summary>
        protected readonly object _OpenCloseLock = new object();

        Dictionary<string, TIncomingMessageType> _IncomingMessageTypeLookup;
        private HashSet<string> _BoundRoutingKeys = new HashSet<string>();
        #endregion

        #region events
        /// <summary>
        /// Occurs when [message received].
        /// </summary>
        public event EventHandler<MessageEventArgs<TIncomingMessageType>> MessageReceived;
        /// <summary>
        /// Occurs when [connection status changed].
        /// </summary>
        public event EventHandler<ConnectionStatusChangedEventArgs> ConnectionStatusChanged;

        private void SetStatus(BaseMessageHandlerStatus baseMessageHandlerStatus)
        {
            this.Status = baseMessageHandlerStatus;
            if (this.ConnectionStatusChanged != null)
                this.ConnectionStatusChanged(this, new ConnectionStatusChangedEventArgs(baseMessageHandlerStatus));
        }
        #endregion

        #region private methods
        
        private void RebindRoutingKeys()
        {
            foreach (string routingKey in _BoundRoutingKeys)
                BindRoutingKey(routingKey);
        }

        private void Log(string message, Exception err, LogLevel logLevel)
        {
            if (_LogManager != null)
                _LogManager.Log(_Type, message, err, logLevel);
        }

        #endregion

        #region properties
        /// <summary>
        /// Gets the status.
        /// </summary>
        public BaseMessageHandlerStatus Status { get; private set; }
        #endregion

        #region public methods

        #region subscribe...

        /// <summary>
        /// Binds the routing key.
        /// </summary>
        /// <param name="routingKey">The routing key.</param>
        /// <returns></returns>
        public bool BindRoutingKey(string routingKey)
        {
            if (!string.IsNullOrEmpty(routingKey)
                && this.Status == BaseMessageHandlerStatus.Connected)
            {
                lock (_SubscriptionLock)
                {
                    try { _Model.QueueBind(_ReceiveQ, _Exchange, routingKey, true, null); }
                    catch (Exception err)
                    {
                        Log("Error binding to routingKey " + routingKey, err, LogLevel.Warning);
                        return false;
                    }
                    _BoundRoutingKeys.Add(routingKey);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Unbinds the routing key.
        /// </summary>
        /// <param name="routingKey">The routing key.</param>
        public void UnbindRoutingKey(string routingKey)
        {
            if (!string.IsNullOrEmpty(routingKey)
                && _BoundRoutingKeys.Contains(routingKey)
                && this.Status == BaseMessageHandlerStatus.Connected)
            {
                lock (_SubscriptionLock)
                {
                    try
                    {
                        _Model.QueueUnbind(_ReceiveQ, _Exchange, routingKey, null);
                        _BoundRoutingKeys.Remove(routingKey);
                    }
                    catch (Exception err)
                    {
                        Log("Error binding to routingKey " + routingKey, err, LogLevel.Warning);
                    }
                }
            }
        }        
        #endregion

        #region open/close logic
        /// <summary>
        /// Closes this connection
        /// </summary>
        public void Close()
        {
            lock (_OpenCloseLock)
            {
                if (_Model != null)
                {
                    try { _Model.Close(); }
                    catch { }
                }

                if (_Connection != null
                    && _Connection.IsOpen)
                {
                    try { _Connection.Close(); }
                    catch (AlreadyClosedException) { } // that's fine, this exception will be thrown when the connection gets closed just before this call...
                }
                SetStatus(BaseMessageHandlerStatus.Disconnected);
            }
        }
        /// <summary>
        /// Opens this instance.
        /// </summary>
        /// <returns></returns>
        public BaseMessageHandlerStatus Open()
        {
            return Open(false);
        }
        /// <summary>
        /// Opens this connection.
        /// </summary>
        /// <param name="retry">if set to <c>true</c> [retry].</param>
        /// <returns></returns>
        public BaseMessageHandlerStatus Open(bool retry)
        {
            Close();

            lock (_OpenCloseLock)
            {
                while (this.Status == BaseMessageHandlerStatus.Disconnected)
                {
                    try
                    {
                        Log("Start connecting process for exchange " + _Exchange + " (" + _ExchangeType + ") ...", null, LogLevel.Debug);
                        SetStatus(BaseMessageHandlerStatus.Connecting);

                        _Connection = _ConnectionFactory.CreateConnection();

                        _Model = _Connection.CreateModel();

                        _Connection.AutoClose = true;

                        _Model.ExchangeDeclare(_Exchange, _ExchangeType, this._IsDurable);

                        _ReceiveQ = _Model.QueueDeclare();

                        if (this._ExchangeType == ExchangeType.Topic
                            && this._DoListenToAnyTopic)
                        {
                            _Model.QueueBind(_ReceiveQ, _Exchange, "#", true, null);
                        }
                        else
                        {
                            _Model.QueueBind(_ReceiveQ, _Exchange, string.Empty, true, null);
                        }

                        _Model.BasicConsume(_ReceiveQ, true, null, this);

                        Log("Connection to exchange  " + _Exchange + " (" + _ExchangeType + ") established!", null, LogLevel.Debug);
                        SetStatus(BaseMessageHandlerStatus.Connected);

                        RebindRoutingKeys();

                        break;
                    }
                    catch (Exception ex)
                    {
                        Log("Error connecting to RabbitMQ broker", ex, LogLevel.Warning);

                        if (_Model != null)
                        {
                            try { _Model.Close(); }
                            catch { }
                        }

                        if (_Connection != null
                            && _Connection.IsOpen)
                        {
                            try { _Connection.Close(); }
                            catch (AlreadyClosedException) { } // that's fine, this exception will be thrown when the connection gets closed just before this call...
                        }
                        SetStatus(BaseMessageHandlerStatus.Disconnected);

                        if (!retry)
                            return BaseMessageHandlerStatus.Disconnected;
                    }
                    Log("Connection attempt failed, next reconnect attempt in 10000 ms", null, LogLevel.Warning);
                    Thread.Sleep(10000);
                }
            }

            return this.Status;
        }
        #endregion

        #endregion

        #region IBasicConsumer Members

        /// <summary>
        /// Handles the basic cancel ok.
        /// </summary>
        /// <param name="consumerTag">The consumer tag.</param>
        public void HandleBasicCancelOk(string consumerTag) { }
        /// <summary>
        /// Handles the basic consume ok.
        /// </summary>
        /// <param name="consumerTag">The consumer tag.</param>
        public void HandleBasicConsumeOk(string consumerTag) { }

        /// <summary>
        /// Handles the basic deliver.
        /// </summary>
        /// <param name="consumerTag">The consumer tag.</param>
        /// <param name="deliveryTag">The delivery tag.</param>
        /// <param name="redelivered">if set to <c>true</c> [redelivered].</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="body">The body.</param>
        public void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            if (properties != null
                && !string.IsNullOrEmpty(properties.Type)
                && _IncomingMessageTypeLookup.ContainsKey(properties.Type))
            {
                if (MessageReceived != null)
                    MessageReceived(this, new MessageEventArgs<TIncomingMessageType>(_IncomingMessageTypeLookup[properties.Type], body, properties.ReplyTo));
            }
        }

        /// <summary>
        /// Handles the model shutdown.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="reason">The <see cref="RabbitMQ.Client.ShutdownEventArgs"/> instance containing the event data.</param>
        public void HandleModelShutdown(IModel model, ShutdownEventArgs reason)
        {
            if (!_IsDisposed)
            {
                SetStatus(BaseMessageHandlerStatus.Disconnected);

                Log("Error, rabbitmq model shutdown " + reason.Cause ?? string.Empty, null, LogLevel.Warning);
                ThreadPool.QueueUserWorkItem(p =>
                {
                    int timeout = 15000;
                    Log("Next reconnect attempt in " + timeout + " ms", null, LogLevel.Warning);
                    Thread.Sleep(timeout);
                    Open(true);
                });
            }
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        public IModel Model
        {
            get { return _Model; }
        }

        #endregion
        
        #region private methods

        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMessageReceiver&lt;TIncomingMessageType&gt;"/> class.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="exchangeType">Type of the exchange.</param>
        /// <param name="isDurable">if set to <c>true</c> [is durable].</param>
        /// <param name="logManager">The log manager.</param>
        public SimpleMessageReceiver(string hostName, string exchange, string exchangeType, bool isDurable, ILogManager logManager)
            : this(hostName, exchange, exchangeType, isDurable, false, logManager) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMessageReceiver&lt;TIncomingMessageType&gt;"/> class.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="exchangeType">Type of the exchange.</param>
        /// <param name="isDurable">if set to <c>true</c> [is durable].</param>
        /// <param name="doListenToAnyTopic">if set to <c>true</c> [do listen to any topic].</param>
        /// <param name="logManager">The log manager.</param>
        public SimpleMessageReceiver(string hostName, string exchange, string exchangeType, bool isDurable, bool doListenToAnyTopic, ILogManager logManager)
        {
            Type type = typeof(TIncomingMessageType);

            if (!type.IsEnum)
                throw new ArgumentException("TIncomingMessageType must be an enumerated type");

            _IncomingMessageTypeLookup = new Dictionary<string, TIncomingMessageType>();
            foreach (int value in Enum.GetValues(type))
                _IncomingMessageTypeLookup.Add(value.ToString(), (TIncomingMessageType)Enum.Parse(type, value.ToString()));

            _ConnectionFactory = new ConnectionFactory();
            _ConnectionFactory.HostName = hostName;

            this._Exchange = exchange;
            this._ExchangeType = exchangeType;
            this._IsDurable = isDurable;
            this._LogManager = logManager;
            this._DoListenToAnyTopic = doListenToAnyTopic;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (!_IsDisposed)
            {
                _IsDisposed = true;
                
                Close();
                SetStatus(BaseMessageHandlerStatus.Disposed);
            }
        }
        #endregion
    }
}
