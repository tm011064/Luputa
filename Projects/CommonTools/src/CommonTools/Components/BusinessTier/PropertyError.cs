using System;
using System.Text;
using CommonTools.Extensions;
using CommonTools.Core;

namespace CommonTools.Components.BusinessTier
{
    /// <summary>
    /// This class exposes properties indicating a property error.
    /// </summary>
    public class PropertyError
    {
        #region properties
        private PropertyValidationError _PropertyValidationError;
        /// <summary>
        /// Gets or sets the property validation error.
        /// </summary>
        /// <value>The property validation error.</value>
        public PropertyValidationError PropertyValidationError
        {
            get { return _PropertyValidationError; }
            set { _PropertyValidationError = value; }
        }
        private string _ErrorMessage = string.Empty;
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
        private string _ErrorMessageResourceKey = string.Empty;
        /// <summary>
        /// Gets or sets the error message resource key.
        /// </summary>
        /// <value>The error message resource key.</value>
        public string ErrorMessageResourceKey
        {
            get { return _ErrorMessageResourceKey; }
            set { _ErrorMessageResourceKey = value; }
        }
        private string _PropertyName = string.Empty;
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName
        {
            get { return _PropertyName; }
            set { _PropertyName = value; }
        }
        /// <summary>
        /// Gets a value indicating whether this instance has error messsage.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has error messsage; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrorMesssage
        {
            get { return string.IsNullOrEmpty(ErrorMessage); }
        }
        /// <summary>
        /// Gets a value indicating whether this instance has error messsage resource key.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has error messsage resource key; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrorMesssageResourceKey
        {
            get { return string.IsNullOrEmpty(ErrorMessageResourceKey); }
        }
        #endregion

        #region public methods
        /// <summary>
        /// This method returns a string containing all necessary information about this object
        /// </summary>
        /// <param name="format">The format of the string to return.</param>
        /// <returns></returns>
        public string ToString(TextFormat format)
        {
            string lineBreak = Text.GetLineBreak(format);

            StringBuilder sb = new StringBuilder();
            sb.Append("Property: " + this.PropertyName + lineBreak);
            sb.Append("ErrorType: " + this.PropertyValidationError.ToString() + lineBreak);
            sb.Append("ErrorMessage: " + this.ErrorMessage + lineBreak);

            return sb.ToString();
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyError"/> class.
        /// </summary>
        /// <param name="error">The error</param>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyError(PropertyValidationError error, string propertyName)
        {
            _PropertyName = propertyName;
            _PropertyValidationError = error;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyError"/> class.
        /// </summary>
        /// <param name="error">The error</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="errorMessage">The message</param>
        public PropertyError(PropertyValidationError error, string propertyName, string errorMessage)
        {
            _PropertyName = propertyName;
            _PropertyValidationError = error;
            _ErrorMessage = errorMessage;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyError"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="errorMessage">The message</param>
        /// <param name="errorMessageResourceKey">The message resource key</param>
        public PropertyError(PropertyValidationError error, string propertyName, string errorMessage, string errorMessageResourceKey)
        {
            _PropertyName = propertyName;
            _PropertyValidationError = error;
            _ErrorMessage = errorMessage;
            _ErrorMessageResourceKey = errorMessageResourceKey;
        }
        #endregion
    }
}
