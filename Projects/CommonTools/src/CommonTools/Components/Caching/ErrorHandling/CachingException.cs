using System;

namespace CommonTools.Components.Caching
{
    /// <summary>
    /// Exception that gets thrown by the CommonTools.Components.Caching namespace
    /// </summary>
    public class CachingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachingException"/> class.
        /// </summary>
        /// <param name="message">The message</param>
        public CachingException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingException"/> class.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="inner">The inner exception</param>
        public CachingException(string message, Exception inner)
            : base(message, inner) { }
    }
}
