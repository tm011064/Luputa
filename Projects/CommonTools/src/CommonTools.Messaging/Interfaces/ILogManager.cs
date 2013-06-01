using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogManager
    {
        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        /// <param name="logLevel">The log level.</param>
        void Log(Type type, string message, LogLevel logLevel);
        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="logLevel">The log level.</param>
        void Log(Type type, string message, Exception exception, LogLevel logLevel);
    }
}
