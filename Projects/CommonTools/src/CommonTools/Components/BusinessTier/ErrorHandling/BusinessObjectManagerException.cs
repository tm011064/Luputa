using System;

namespace CommonTools.Components.Caching
{
    /// <summary>
    /// Exception that gets thrown by the CommonTools.Components.BusinessTier namespace
    /// </summary>
    public class BusinessObjectManagerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachingException"/> class.
        /// </summary>
        /// <param name="message">The message</param>
        public BusinessObjectManagerException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingException"/> class.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="inner">The inner exception</param>
        public BusinessObjectManagerException(string message, Exception inner)
            : base(message, inner) { }
    }
}
