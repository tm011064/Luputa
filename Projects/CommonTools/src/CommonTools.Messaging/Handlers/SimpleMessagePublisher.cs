using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.IO;
using System.Threading;

namespace CommonTools.Messaging.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TOutgoingMessageType">The type of the outgoing message type.</typeparam>
    public class SimpleMessagePublisher<TOutgoingMessageType> : IDisposable
    {
        #region members
        private Type _Type = typeof(SimpleMessagePublisher<TOutgoingMessageType>);
        private ILogManager _LogManager;

        private ConnectionFactory _ConnectionFactory;
        private IConnection _Connection;
        private IModel _Model;

        private string _Exchange
                       , _ExchangeType;

        private bool _IsDurable
                     , _IsDisposed;
        /// <summary>
        /// 
        /// </summary>
        protected readonly object _OpenCloseLock = new object();
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

        void _Model_ModelShutdown(IModel model, ShutdownEventArgs reason)
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
        #endregion

        #region properties
        /// <summary>
        /// Gets the status.
        /// </summary>
        public BaseMessageHandlerStatus Status { get; private set; }
        #endregion

        #region private methods

        private void Log(string message, Exception err, LogLevel logLevel)
        {
            if (_LogManager != null)
                _LogManager.Log(_Type, message, err, logLevel);
        }

        private byte[] Serialize<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                try { ProtoBuf.Serializer.Serialize<T>(stream, obj); }
                catch (Exception e)
                {
                    Log("Error serializing message, see inner exception for further details.", e, LogLevel.Warning);
                    return null;
                }
                return stream.ToArray();
            }
        }

        private bool Send(TOutgoingMessageType messageType, string exchange, string routingKey, byte[] body)
        {
            if (this.Status != BaseMessageHandlerStatus.Connected)
                return false;

            try
            {
                IBasicProperties basicProperties = _Model.CreateBasicProperties();
                basicProperties.Type = (messageType as Enum).ToString("d");

                _Model.BasicPublish(_Exchange, routingKey, false, false, basicProperties, body);
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

        #region public methods

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
                        _Model.ModelShutdown += new RabbitMQ.Client.Events.ModelShutdownEventHandler(_Model_ModelShutdown);

                        _Connection.AutoClose = true;

                        _Model.ExchangeDeclare(_Exchange, _ExchangeType, this._IsDurable);

                        string queue = _Model.QueueDeclare();
                        _Model.QueueBind(queue, _Exchange, string.Empty, false, null);

                        Log("Connection to exchange  " + _Exchange + " (" + _ExchangeType + ") established!", null, LogLevel.Debug);
                        SetStatus(BaseMessageHandlerStatus.Connected);
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

        #region send methods
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <returns></returns>
        public bool SendMessage(TOutgoingMessageType messageType)
        {
            return Send(messageType, _Exchange, string.Empty, null);
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
            byte[] serializedMessage = Serialize<T>(messageBody);
            if (serializedMessage != null)
                return Send(messageType, _Exchange, string.Empty, serializedMessage);

            return false;
        }
        /// <summary>
        /// Sends the routed message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <returns></returns>
        public bool SendRoutedMessage(TOutgoingMessageType messageType, string routingKey)
        {
            return Send(messageType, _Exchange, routingKey, null);
        }
        /// <summary>
        /// Sends the routed message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageBody">The message body.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <returns></returns>
        public bool SendRoutedMessage<T>(TOutgoingMessageType messageType, T messageBody, string routingKey)
        {
            byte[] serializedMessage = Serialize<T>(messageBody);
            if (serializedMessage != null)
                return Send(messageType, _Exchange, routingKey, serializedMessage);

            return false;
        }
        #endregion

        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMessagePublisher&lt;TOutgoingMessageType&gt;"/> class.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="exchangeType">Type of the exchange.</param>
        /// <param name="durable">if set to <c>true</c> [durable].</param>
        public SimpleMessagePublisher(string hostName, string exchange, string exchangeType, bool durable)
            : this(hostName, exchange, exchangeType, durable, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMessagePublisher&lt;TOutgoingMessageType&gt;"/> class.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="exchangeType">Type of the exchange.</param>
        /// <param name="durable">if set to <c>true</c> [durable].</param>
        /// <param name="logManager">The log manager.</param>
        public SimpleMessagePublisher(string hostName, string exchange, string exchangeType, bool durable, ILogManager logManager)
        {
            Type type = typeof(TOutgoingMessageType);

            if (!type.IsEnum)
                throw new ArgumentException("TIncomingMessageType must be an enumerated type");

            _ConnectionFactory = new ConnectionFactory();
            _ConnectionFactory.HostName = hostName;

            this._Exchange = exchange;
            this._ExchangeType = exchangeType;
            this._IsDurable = durable;
            this._LogManager = logManager;
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
