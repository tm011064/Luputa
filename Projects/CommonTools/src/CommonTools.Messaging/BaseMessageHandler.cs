using System.Collections.Generic;

namespace CommonTools.Messaging
{
    /// <summary>
    /// This is an abstract class for handling (send/receive) RabbitMQ messages
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseMessageHandler<T> : AbstractMessageHandlerBase
    {
        #region abstracts

        #region abstract methods
        /// <summary>
        /// Called when subscribe method is executed
        /// </summary>
        /// <param name="obj">The obj.</param>
        protected abstract void DoSubscribe(T obj);
        /// <summary>
        /// Called when [unsubscribe] method is executed.
        /// </summary>
        /// <param name="obj">The obj.</param>
        protected abstract void DoUnsubscribe(T obj);
        #endregion

        #endregion

        #region public methods

        /// <summary>
        /// Subscribes the specified currency.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public void Subscribe(T obj)
        {
            lock (_SubscriptionLock)
            {
                DoSubscribe(obj);
            }
        }

        /// <summary>
        /// Unsubscribes the specified currency.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public void Unsubscribe(T obj)
        {
            lock (_SubscriptionLock)
            {
                DoUnsubscribe(obj);
            }
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
        protected BaseMessageHandler(string providerName, string ip
            , List<DataExchangeInfo> receiverExchanges, List<DataExchangeInfo> senderExchanges)
            : base(providerName, ip, receiverExchanges, senderExchanges, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMessageHandler&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="ip">The ip.</param>
        /// <param name="receiverExchanges">The receiver exchanges.</param>
        /// <param name="senderExchanges">The sender exchanges.</param>
        /// <param name="logManager">The log manager.</param>
        protected BaseMessageHandler(string providerName, string ip
            , List<DataExchangeInfo> receiverExchanges, List<DataExchangeInfo> senderExchanges
            , ILogManager logManager)
            : base(providerName, ip, receiverExchanges, senderExchanges, logManager) { }

        #endregion
    }

    /// <summary>
    /// This is an abstract class for handling (send/receive) RabbitMQ messages
    /// </summary>
    public abstract class BaseMessageHandler : AbstractMessageHandlerBase
    {
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

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMessageHandler&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="ip">The ip.</param>
        /// <param name="receiverExchanges">The receiver exchanges.</param>
        /// <param name="senderExchanges">The sender exchanges.</param>
        protected BaseMessageHandler(string providerName, string ip
            , List<DataExchangeInfo> receiverExchanges, List<DataExchangeInfo> senderExchanges)
            : base(providerName, ip, receiverExchanges, senderExchanges, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMessageHandler&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="ip">The ip.</param>
        /// <param name="receiverExchanges">The receiver exchanges.</param>
        /// <param name="senderExchanges">The sender exchanges.</param>
        /// <param name="logManager">The log manager.</param>
        protected BaseMessageHandler(string providerName, string ip
            , List<DataExchangeInfo> receiverExchanges, List<DataExchangeInfo> senderExchanges
            , ILogManager logManager)
            : base(providerName, ip, receiverExchanges, senderExchanges, logManager) { }

        #endregion
    }
}
