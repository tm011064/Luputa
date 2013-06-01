using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using System.Threading;

namespace CommonTools.Messaging
{
    /// <summary>
    /// This is an abstract base class used for the BaseMessageHandler classes. You can't instanciate this class, use 
    /// BaseMessageHandler or BaseMessageHandler&lt;T&gt; instead.
    /// </summary>
    public abstract class AbstractMessageHandlerBase
        : IBasicConsumer, IDisposable
    {
        #region members
        private ILogManager _LogManager;

        private bool _IsDisposed;

        private IModel _Receiver;
        private IModel _Sender;

        private IConnection _Connection;

        private string _IP;
        private string _ProviderName;

        /// <summary>
        /// 
        /// </summary>
        protected readonly object _SubscriptionLock = new object();
        /// <summary>
        /// 
        /// </summary>
        protected readonly object _ListenLock = new object();
        /// <summary>
        /// 
        /// </summary>
        protected readonly object _SendLock = new object();

        private int _TotalBytesReceived;
        private int _TotalMessagesreceived;

        private List<DataExchangeInfo> _ReceiverExchanges;
        private List<DataExchangeInfo> _SenderExchanges;

        /// <summary>
        /// 
        /// </summary>
        protected string _ReceiveQ;
        /// <summary>
        /// 
        /// </summary>
        protected readonly object _OpenCloseLock = new object();
        #endregion

        #region abstracts

        #region properties
        /// <summary>
        /// Gets the type of the log.
        /// </summary>
        /// <value>The type of the log.</value>
        protected abstract Type LogType { get; }
        #endregion

        #region abstract methods
        /// <summary>
        /// Called when subscriptions get resent.
        /// </summary>
        protected abstract void DoResendSubscriptions();
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
        protected abstract void OnHandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body);
        #endregion

        #endregion

        #region properties
        /// <summary>
        /// Gets the status.
        /// </summary>
        public BaseMessageHandlerStatus Status { get; private set; }
        #endregion

        #region protected methods

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="err">The err.</param>
        /// <param name="logLevel">The log level.</param>
        protected void Log(string message, Exception err, LogLevel logLevel)
        {
            if (_LogManager != null)
                _LogManager.Log(LogType, message, err, logLevel);
        }

        /// <summary>
        /// Resends the subscriptions.
        /// </summary>
        protected void ResendSubscriptions()
        {
            lock (_SubscriptionLock)
            {
                DoResendSubscriptions();
            }
        }

        /// <summary>
        /// Sends the subscription.
        /// </summary>
        /// <param name="receiverRoutingKey">The receiver routing key.</param>
        /// <param name="senderRoutingKey">The sender routing key.</param>
        /// <param name="message">The message.</param>
        protected void SendSubscription(string receiverRoutingKey, string senderRoutingKey, byte[] message)
        {
            try
            {
                if (this.Status == BaseMessageHandlerStatus.Connected)
                {
                    lock (this._ListenLock)
                    {
                        foreach (var receiver in _ReceiverExchanges)
                            _Receiver.QueueBind(_ReceiveQ, receiver.ExchangeName, receiverRoutingKey, true, null);
                    }

                    lock (_SendLock)
                    {
                        IBasicProperties prop = _Sender.CreateBasicProperties();
                        prop.ReplyTo = _ReceiveQ;

                        foreach (var sender in _SenderExchanges)
                            _Sender.BasicPublish(sender.ExchangeName, senderRoutingKey, prop, message);
                    }
                }
            }
            catch (Exception ex)
            {
                Log(string.Format("Error subscribing to " + receiverRoutingKey), ex, LogLevel.Warning);
            }
        }
        private void DisposeOnOwnThread()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    if (_Receiver != null)
                        _Receiver.Dispose();
                }
                catch { }
                finally { _Receiver = null; }

                try
                {
                    if (_Sender != null)
                    {
                        _Sender.Dispose();
                        _Sender = null;
                    }
                }
                catch { }
                finally { _Sender = null; }

                try
                {
                    if (_Connection != null)
                    {
                        _Connection.Abort();
                    }
                }
                catch { }
                finally { _Connection = null; }
            });
        }

        private void Disconnect(BaseMessageHandlerStatus statusAfterDisconnect)
        {
            lock (_OpenCloseLock)
            {
                try
                {
                    if (_Receiver != null)
                    {
                        _Receiver.Close();
                        _Receiver = null;
                    }
                }
                catch { }

                try
                {
                    if (_Sender != null)
                    {
                        _Sender.Close();
                        _Sender = null;
                    }
                }
                catch { }

                try
                {
                    if (_Connection != null
                        && _Connection.IsOpen)
                    {
                        _Connection.Close();
                    }
                    _Connection = null;
                }
                catch { }

                SetStatus(statusAfterDisconnect);
            }
        }
        #endregion

        #region events
        /// <summary>
        /// Occurs when [connection status changed].
        /// </summary>
        public event EventHandler<ConnectionStatusChangedEventArgs> ConnectionStatusChanged;

        /// <summary>
        /// Sets the status.
        /// </summary>
        /// <param name="baseMessageHandlerStatus">The base message handler status.</param>
        private void SetStatus(BaseMessageHandlerStatus baseMessageHandlerStatus)
        {
            this.Status = baseMessageHandlerStatus;
            if (this.ConnectionStatusChanged != null)
                this.ConnectionStatusChanged(this, new ConnectionStatusChangedEventArgs(baseMessageHandlerStatus));
        }
        #endregion

        #region public methods

        /// <summary>
        /// Connects to a specified currency provider
        /// </summary>
        /// <param name="retry">if set to <c>true</c>, this provider will keep trying to connect indefinitely if the connection process fails</param>
        /// <returns></returns>
        public BaseMessageHandlerStatus Connect(bool retry)
        {
            while (this.Status == BaseMessageHandlerStatus.Disconnected)
            {
                lock (_OpenCloseLock)
                {
                    //  we could be disposing/disposed while waiting for the lock
                    if (this.Status == BaseMessageHandlerStatus.Connected // could have happened while waiting for the lock...
                        || this.Status == BaseMessageHandlerStatus.Disposed
                        || this.Status == BaseMessageHandlerStatus.Disposing)
                        return this.Status;

                    try
                    {
                        Log("Start connecting process...", null, LogLevel.Debug);
                        SetStatus(BaseMessageHandlerStatus.Connecting);

                        ConnectionFactory factory = new ConnectionFactory();
                        factory.Protocol = Protocols.DefaultProtocol;
                        factory.HostName = _IP;
                        factory.RequestedHeartbeat = 60;

                        _Connection = factory.CreateConnection();

                        _Receiver = _Connection.CreateModel();

                        _Sender = _Connection.CreateModel();

                        _Connection.AutoClose = true;

                        foreach (DataExchangeInfo sender in this._SenderExchanges)
                            _Sender.ExchangeDeclare(sender.ExchangeName, sender.ExchangeType, false, false, false, false, true, null);

                        foreach (DataExchangeInfo receiver in this._ReceiverExchanges)
                            _Receiver.ExchangeDeclare(receiver.ExchangeName, receiver.ExchangeType, false, false, false, false, true, null);

                        _ReceiveQ = _Receiver.QueueDeclare();

                        foreach (DataExchangeInfo receiver in this._ReceiverExchanges)
                        {
                            if (receiver.DoListenToAnyTopic)
                                _Receiver.QueueBind(_ReceiveQ, receiver.ExchangeName, "#", true, null);
                        }

                        _Receiver.BasicConsume(_ReceiveQ, true, null, this);

                        Log(_ProviderName + " receive Queue " + _ReceiveQ, null, LogLevel.Debug);

                        SetStatus(BaseMessageHandlerStatus.Connected);

                        #region bind to any startup routing keys
                        foreach (DataExchangeInfo receiver in this._ReceiverExchanges)
                        {
                            foreach (DataExchangeInfoSpecialBinding dataExchangeInfoSpecialBinding in receiver.SpecialBindings)
                            {
                                _Receiver.QueueBind(
                                    _ReceiveQ
                                    , dataExchangeInfoSpecialBinding.Exchange
                                    , dataExchangeInfoSpecialBinding.RoutingKey
                                    , dataExchangeInfoSpecialBinding.NoWait
                                    , dataExchangeInfoSpecialBinding.Arguments);
                            }
                        }
                        #endregion

                        ResendSubscriptions();

                        Log("Connection established!", null, LogLevel.Debug);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Log("Error connecting to RabbitMQ broker", ex, LogLevel.Warning);

                        DisposeOnOwnThread();

                        SetStatus(BaseMessageHandlerStatus.Disconnected);

                        if (!retry)
                            return BaseMessageHandlerStatus.Disconnected;
                    }
                }

                Log("Connection attempt failed, next reconnect attempt in 15000 ms", null, LogLevel.Warning);
                Thread.Sleep(15000);
            }
            return this.Status;
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        public void Disconnect()
        {
            Disconnect(BaseMessageHandlerStatus.Disconnected);
        }
        #region IBasicConsumer Members

        /// <summary>
        /// Handles the basic cancel ok.
        /// </summary>
        /// <param name="consumerTag">The consumer tag.</param>
        public virtual void HandleBasicCancelOk(string consumerTag) { }
        /// <summary>
        /// Handles the basic consume ok.
        /// </summary>
        /// <param name="consumerTag">The consumer tag.</param>
        public virtual void HandleBasicConsumeOk(string consumerTag) { }

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
            _TotalBytesReceived += body.Length;
            _TotalMessagesreceived++;

            OnHandleBasicDeliver(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties, body);
        }


        /// <summary>
        /// Handles the model shutdown.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="reason">The RabbitMQ.Client.ShutdownEventArgs instance containing the event data.</param>
        public virtual void HandleModelShutdown(IModel model, ShutdownEventArgs reason)
        {
            if (!_IsDisposed
                && this.Status != BaseMessageHandlerStatus.Disposing)
            {
                DisposeOnOwnThread();

                SetStatus(BaseMessageHandlerStatus.Disconnected);

                Log("Error, rabbitmq model shutdown " + reason.Cause ?? string.Empty, null, LogLevel.Warning);
                ThreadPool.QueueUserWorkItem(p =>
                {
                    int timeout = 15000;
                    Log("Next reconnect attempt in " + timeout + " ms", null, LogLevel.Warning);
                    Thread.Sleep(timeout);
                    Connect(true);
                });
            }
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>The model.</value>
        public IModel Model
        {
            get { return _Receiver; }
        }

        /// <summary>
        /// Gets the sender.
        /// </summary>
        /// <value>The sender.</value>
        public IModel Sender
        {
            get { return _Receiver; }
        }
        #endregion

        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMessageHandler&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="ip">The ip.</param>
        /// <param name="receiverExchanges">The receiver exchanges.</param>
        /// <param name="senderExchanges">The sender exchanges.</param>
        internal protected AbstractMessageHandlerBase(string providerName, string ip
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
        internal protected AbstractMessageHandlerBase(string providerName, string ip
            , List<DataExchangeInfo> receiverExchanges, List<DataExchangeInfo> senderExchanges
            , ILogManager logManager)
        {
            _ProviderName = providerName;
            _IP = ip;

            _ReceiverExchanges = receiverExchanges ?? new List<DataExchangeInfo>();
            _SenderExchanges = senderExchanges ?? new List<DataExchangeInfo>();
            _LogManager = logManager;

            this.Status = BaseMessageHandlerStatus.Disconnected;
        }

        private AbstractMessageHandlerBase() { }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (!_IsDisposed)
            {
                SetStatus(BaseMessageHandlerStatus.Disposing);

                Disconnect(BaseMessageHandlerStatus.Disposed);

                _IsDisposed = true;
            }
        }
        #endregion
    }
}
