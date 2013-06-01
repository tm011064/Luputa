using System;
using System.Configuration;

namespace CommonTools.Components.BusinessTier
{
    /// <summary>
    /// This attribute can be used for BaseBusinessObject properties in order to enforce validation constraints.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class BusinessObjectValidationAttribute : ConfigurationValidatorAttribute
    {
        #region properties
        private string _MinimumValue = string.Empty;
        /// <summary>
        /// Gets or sets the minimum value of the validation range.
        /// </summary>
        /// <value>The minimum value.</value>
        public string MinimumValue
        {
            get { return _MinimumValue; }
            set { _MinimumValue = value; }
        }

        private string _MaximumValue = string.Empty;
        /// <summary>
        /// Gets or sets the maximum value of the validation range.
        /// </summary>
        /// <value>The maximum value.</value>
        public string MaximumValue
        {
            get { return _MaximumValue; }
            set { _MaximumValue = value; }
        }

        private int _MinLength = -1;
        /// <summary>
        /// Gets or sets the minimum amount of characters a string must have.
        /// </summary>
        /// <value>The length of the min.</value>
        public int MinLength
        {
            get { return _MinLength; }
            set { _MinLength = value; }
        }

        private int _MaxLength = -1;
        /// <summary>
        /// Gets or sets the maximum amount of characters a string is allowed have.
        /// </summary>
        /// <value>The length of the max.</value>
        public int MaxLength
        {
            get { return _MaxLength; }
            set { _MaxLength = value; }
        }

        private string _Regex = string.Empty;
        /// <summary>
        /// Gets or sets the regular expression that determines the pattern used to validate a field.
        /// </summary>
        /// <value>The regex.</value>
        public string Regex
        {
            get { return _Regex; }
            set { _Regex = value; }
        }

        private bool _IsRequired = false;
        /// <summary>
        /// Gets or sets a value indicating this property's input field is a required form field.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance this property's input field is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequired
        {
            get { return _IsRequired; }
            set { _IsRequired = value; }
        }

        private string _IsRequiredMessage = string.Empty;
        /// <summary>
        /// Gets or sets the text for the error message displayed in a CommonTools.BusinessObjects.BaseBusinessObjectValidationControl
        /// BaseBusinessObjectValidationControl when required field validation fails.
        /// </summary>
        /// <value>The is required message.</value>
        public string IsRequiredMessage
        {
            get { return _IsRequiredMessage; }
            set { _IsRequiredMessage = value; }
        }
        private string _IsRequiredMessageResourceKey = string.Empty;
        /// <summary>
        /// Gets or sets the text for the error message displayed in a CommonTools.BusinessObjects.BaseBusinessObjectValidationControl
        /// BaseBusinessObjectValidationControl when required field validation fails.
        /// </summary>
        /// <value>The is required message.</value>
        public string IsRequiredMessageResourceKey
        {
            get { return _IsRequiredMessageResourceKey; }
            set { _IsRequiredMessageResourceKey = value; }
        }

        private string _RegexMessage = string.Empty;
        /// <summary>
        /// Gets or sets the text for the error message displayed in a CommonTools.BusinessObjects.BaseBusinessObjectValidationControl
        /// BaseBusinessObjectValidationControl when regular expression validation fails.
        /// </summary>
        /// <value>The regex message.</value>
        public string RegexMessage
        {
            get { return _RegexMessage; }
            set { _RegexMessage = value; }
        }
        private string _RegexMessageResourceKey = string.Empty;
        /// <summary>
        /// Gets or sets the text for the error message displayed in a CommonTools.BusinessObjects.BaseBusinessObjectValidationControl
        /// BaseBusinessObjectValidationControl when regular expression validation fails.
        /// </summary>
        /// <value>The regex message.</value>
        public string RegexMessageResourceKey
        {
            get { return _RegexMessageResourceKey; }
            set { _RegexMessageResourceKey = value; }
        }

        private string _OutOfRangeErrorMessage = string.Empty;
        /// <summary>
        /// Gets or sets the text for the error message displayed in a CommonTools.BusinessObjects.BaseBusinessObjectValidationControl
        /// BaseBusinessObjectValidationControl out of bounce validation fails. This can be either a MinimumValue/MaximumValue or MinLength/MaxLength
        /// validation failure.
        /// </summary>
        /// <value>The out of range error message.</value>
        public string OutOfRangeErrorMessage
        {
            get { return _OutOfRangeErrorMessage; }
            set { _OutOfRangeErrorMessage = value; }
        }

        private string _OutOfRangeErrorMessageResourceKey = string.Empty;
        /// <summary>
        /// Gets or sets the text for the error message displayed in a CommonTools.BusinessObjects.BaseBusinessObjectValidationControl
        /// BaseBusinessObjectValidationControl out of bounce validation fails. This can be either a MinimumValue/MaximumValue or MinLength/MaxLength
        /// validation failure.
        /// </summary>
        /// <value>The out of range error message.</value>
        public string OutOfRangeErrorMessageResourceKey
        {
            get { return _OutOfRangeErrorMessageResourceKey; }
            set { _OutOfRangeErrorMessageResourceKey = value; }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessObjectValidationAttribute"/> class.
        /// </summary>
        public BusinessObjectValidationAttribute() { }
        #endregion

        #region overrides
        /// <summary>
        /// Gets the validator attribute instance.
        /// </summary>
        /// <value></value>
        /// <returns>The current <see cref="T:System.Configuration.ConfigurationValidatorBase"></see>.</returns>
        public override ConfigurationValidatorBase ValidatorInstance
        {
            get
            {
                return base.ValidatorInstance;
            }
        }
        #endregion
    }
}