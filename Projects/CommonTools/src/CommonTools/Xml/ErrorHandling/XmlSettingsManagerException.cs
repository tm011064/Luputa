using System;

namespace CommonTools.Xml
{
    /// <summary>
    /// Exceptions that can be thrown by the CommonTools.Xml.XmlResourceFileManager class.
    /// </summary>
    public class XmlSettingsManagerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonTools.Xml.XmlResourceFileManagerException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public XmlSettingsManagerException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonTools.Xml.XmlResourceFileManagerException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The original inner exception.</param>
        public XmlSettingsManagerException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}