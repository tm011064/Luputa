using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionStatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the status.
        /// </summary>
        public BaseMessageHandlerStatus Status { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStatusChangedEventArgs"/> class.
        /// </summary>
        /// <param name="baseMessageHandlerStatus">The base message handler status.</param>
        public ConnectionStatusChangedEventArgs(BaseMessageHandlerStatus baseMessageHandlerStatus)
        {
            this.Status = baseMessageHandlerStatus;
        }
    }
}
