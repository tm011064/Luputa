using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CommonTools.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public class DataExchangeInfoSpecialBinding
    {
        /// <summary>
        /// Gets or sets the exchange.
        /// </summary>
        /// <value>The exchange.</value>
        public string Exchange { get; private set; }
        /// <summary>
        /// Gets or sets the routing key.
        /// </summary>
        /// <value>The routing key.</value>
        public string RoutingKey { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether [no wait].
        /// </summary>
        /// <value><c>true</c> if [no wait]; otherwise, <c>false</c>.</value>
        public bool NoWait { get; private set; }
        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public IDictionary Arguments { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataExchangeInfoSpecialBinding"/> class.
        /// </summary>
        /// <param name="exchange">The exchange.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="nowait">if set to <c>true</c> [nowait].</param>
        /// <param name="arguments">The arguments.</param>
        public DataExchangeInfoSpecialBinding(string exchange, string routingKey, bool nowait, IDictionary arguments)
        {
            this.Exchange = exchange;
            this.RoutingKey = routingKey;
            this.NoWait = nowait;
            this.Arguments = arguments;
        }
    }
}
