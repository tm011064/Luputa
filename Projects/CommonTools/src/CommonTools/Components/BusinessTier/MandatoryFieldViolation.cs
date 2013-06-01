using System;

namespace CommonTools.Components.BusinessTier
{
    /// <summary>
    /// This class exposes properties indicating a mandatory field violation.
    /// </summary>
    public class MandatoryFieldViolation
    {
        #region properties
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
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MandatoryFieldViolation"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public MandatoryFieldViolation(string propertyName)
        {
            _PropertyName = propertyName;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MandatoryFieldViolation"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="errorMessage">The message</param>
        public MandatoryFieldViolation(string propertyName, string errorMessage)
        {
            _PropertyName = propertyName;
            _ErrorMessage = errorMessage;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MandatoryFieldViolation"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="errorMessage">The message</param>
        /// <param name="errorMessageResourceKey">The message resource key</param>
        public MandatoryFieldViolation(string propertyName, string errorMessage, string errorMessageResourceKey)
        {
            _PropertyName = propertyName;
            _ErrorMessage = errorMessage;
            _ErrorMessageResourceKey = errorMessageResourceKey;
        }
        #endregion
    }
}
