using System;

namespace CommonTools.Components.Threading
{
    /// <summary>
    /// Exception that gets thrown by the CommonTools.Components.Caching namespace
    /// </summary>
    public class JobsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public JobsException(string message)
            : base(message) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="JobsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public JobsException(string message, Exception inner)
            : base(message, inner) { }
    }
}
