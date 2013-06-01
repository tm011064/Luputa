using System;

namespace CommonTools.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class WebException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WebException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="WebException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebException(string message, Exception innerException) : base(message, innerException) { }
    }
}