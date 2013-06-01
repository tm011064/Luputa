using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public class DataExchangeInfo
    {
        /// <summary>
        /// Gets or sets the special bindings.
        /// </summary>
        /// <value>The special bindings.</value>
        public List<DataExchangeInfoSpecialBinding> SpecialBindings { get; private set; }

        /// <summary>
        /// Gets or sets the name of the exchange.
        /// </summary>
        /// <value>The name of the exchange.</value>
        public string ExchangeName { get; private set; }
        /// <summary>
        /// Gets or sets the type of the exchange.
        /// </summary>
        /// <value>The type of the exchange.</value>
        public string ExchangeType { get; private set; }
        /// <summary>
        /// Gets the do listen to any topic property. If this is set to true and the receiver's exchange type is "topic", then the receiver
        /// will listen to all routing keys
        /// </summary>
        public bool DoListenToAnyTopic { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataExchangeInfo"/> class.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="exchangeType">Type of the exchange.</param>
        public DataExchangeInfo(string exchangeName, string exchangeType) : this(exchangeName, new List<DataExchangeInfoSpecialBinding>(), exchangeType) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataExchangeInfo"/> class.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="specialBindings">The special bindings.</param>
        /// <param name="exchangeType">Type of the exchange.</param>
        public DataExchangeInfo(string exchangeName, List<DataExchangeInfoSpecialBinding> specialBindings, string exchangeType)
            : this(exchangeName, specialBindings, exchangeType, false) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataExchangeInfo"/> class.
        /// </summary>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="specialBindings">Any special binding that needs to be used with this queue, e.g. startup messages.</param>
        /// <param name="exchangeType">Type of the exchange.</param>
        /// <param name="doListenToAnyTopic">if set to <c>true</c> [do listen to any topic].</param>
        public DataExchangeInfo(string exchangeName, List<DataExchangeInfoSpecialBinding> specialBindings, string exchangeType, bool doListenToAnyTopic)
        {
            this.ExchangeName = exchangeName;
            this.SpecialBindings = specialBindings;

            this.ExchangeType = exchangeType;
            this.DoListenToAnyTopic = doListenToAnyTopic && exchangeType == RabbitMQ.Client.ExchangeType.Topic;
        }
    }
}
