using System;

namespace CommonTools.Data
{
    /// <summary>
    /// Exception that gets thrown by the CommonTools.Components.Caching namespace
    /// </summary>
    public class DataAccessManagerStoredProcedureException : DataAccessManagerException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessManagerStoredProcedureException"/> class.
        /// </summary>
        /// <param name="message">The message</param>
        public DataAccessManagerStoredProcedureException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessManagerStoredProcedureException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public DataAccessManagerStoredProcedureException(string message, Exception inner)
            : base(message, inner) { }
    }
}
