using System;

namespace CommonTools.Data
{
    /// <summary>
    /// Exception that gets thrown by the CommonTools.Components.Caching namespace
    /// </summary>
    public class DataAccessManagerConnectionStringException : DataAccessManagerException
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessManagerConnectionStringException"/> class.
        /// </summary>
        /// <param name="message">The message</param>
        public DataAccessManagerConnectionStringException(string message)
            : base(message) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessManagerConnectionStringException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public DataAccessManagerConnectionStringException(string message, Exception inner)
            : base(message, inner) { }
    }
}
