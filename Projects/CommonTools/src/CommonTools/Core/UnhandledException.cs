using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class UnhandledException : System.Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public UnhandledException(Exception innerException)
            : base("An unhandled exception was thrown at your application. See inner exception for further details.", innerException)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public UnhandledException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UnhandledException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
