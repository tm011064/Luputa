using System;

namespace CommonTools.Components.TextResources
{
    /// <summary>
    /// Exceptions that can be thrown by the CommonTools.Components.TextResources.TextResourceManagerException class.
    /// </summary>
    public class TextResourceManagerException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TextResourceManagerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public TextResourceManagerException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextResourceManagerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public TextResourceManagerException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}