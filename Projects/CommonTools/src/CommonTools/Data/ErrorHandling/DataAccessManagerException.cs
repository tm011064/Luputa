using System;

namespace CommonTools.Data
{
    /// <summary>
    /// Exception that gets thrown by the CommonTools.Components.Caching namespace
    /// </summary>
    public class DataAccessManagerException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessManagerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DataAccessManagerException(string message)
            : base(message) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessManagerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public DataAccessManagerException(string message, Exception inner)
            : base(message, inner) { }
    }
}
