using System;

namespace CommonTools.Components.Logging
{
    /// <summary>
    /// Exception that gets thrown by the CommonTools.Components.Caching namespace
    /// </summary>
    public class LoggingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public LoggingException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public LoggingException(string message, Exception inner)
            : base(message, inner) { }
    }
}
