using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonTools.Messaging.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMessageType">The type of the message type.</typeparam>
    public class MessageEventArgs<TMessageType> : EventArgs
    {
        #region members
        private byte[] _Body;
        #endregion

        #region methods
        /// <summary>
        /// Gets the embedded object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetEmbeddedObject<T>()
        {
            if (_Body != null
                && _Body.Length > 0)
            {
                try
                {
                    using (MemoryStream stream = new MemoryStream(_Body))
                        return ProtoBuf.Serializer.Deserialize<T>(stream);
                }
                catch { }
            }
            return default(T);
        }
        #endregion

        #region properties
        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public TMessageType MessageType { get; private set; }
        /// <summary>
        /// Gets the reply to queue.
        /// </summary>
        public string ReplyToQueue { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance has reply queue.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has reply queue; otherwise, <c>false</c>.
        /// </value>
        public bool HasReplyQueue { get; private set; }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEventArgs&lt;TMessageType&gt;"/> class.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="body">The body.</param>
        /// <param name="replyToQueue">The reply to queue.</param>
        public MessageEventArgs(TMessageType messageType, byte[] body, string replyToQueue)
        {
            this._Body = body;
            this.MessageType = messageType;
            this.ReplyToQueue = replyToQueue;
            this.HasReplyQueue = !string.IsNullOrEmpty(replyToQueue);
        }
        #endregion
    }
}
