using System;
using System.Collections.Generic;
using System.Text;
using CommonTools.Core;

namespace CommonTools.Components.BusinessTier
{
    /// <summary>
    /// This class acts as a validation report that can be used for examining the validation result of a BusinessObjectManager.ValidateAction call.
    /// </summary>
    public class BusinessObjectValidationResult
    {
        #region properties
        private ValidationStatus _Status = ValidationStatus.Valid;
        /// <summary>
        /// Gets or sets the validation status.
        /// </summary>
        /// <value>The validation status.</value>
        public ValidationStatus ValidationStatus
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private List<PropertyError> _PropertyErrors = new List<PropertyError>();
        /// <summary>
        /// Gets the property errors.
        /// </summary>
        /// <value>The property errors.</value>
        public List<PropertyError> PropertyErrors
        {
            get
            {
                return _PropertyErrors;
            }
        }

        private List<MandatoryFieldViolation> _MandatoryFieldViolations = new List<MandatoryFieldViolation>();
        /// <summary>
        /// Gets the mandatory field violations.
        /// </summary>
        /// <value>The mandatory field violations.</value>
        public List<MandatoryFieldViolation> MandatoryFieldViolations
        {
            get
            {
                return _MandatoryFieldViolations;
            }
        }
        /// <summary>
        /// Gets a value indicating whether this report is valid or not (it also test for property errors and mandatory 
        /// field violations).
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid
        {
            get
            {
                if (_Status == ValidationStatus.Valid)
                {
                    if (HasMandatoryFieldViolations || HasPropertyErrors)
                        return false;

                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Gets a value indicating whether this instance has property errors.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has property errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasPropertyErrors
        {
            get
            {
                if (PropertyErrors.Count == 0)
                    return false;

                return true;
            }
        }
        /// <summary>
        /// Gets a value indicating whether this instance has mandatory field violations.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has mandatory field violations; otherwise, <c>false</c>.
        /// </value>
        public bool HasMandatoryFieldViolations
        {
            get
            {
                if (MandatoryFieldViolations.Count == 0)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Gets or sets the custom error messages.
        /// </summary>
        /// <value>The custom error messages.</value>
        public List<string> CustomErrorMessages { get; set; }
        #endregion

        #region public methods
        /// <summary>
        /// Gets the mandatory field error message.
        /// </summary>
        /// <returns></returns>
        public string GetMandatoryFieldErrorMessage()
        {
            if (!HasMandatoryFieldViolations)
                return string.Empty;

            int errorCount = MandatoryFieldViolations.Count;
            StringBuilder sb = new StringBuilder();
            foreach (MandatoryFieldViolation violation in MandatoryFieldViolations)
            {
                sb.Append(violation.PropertyName + ", ");
            }

            string s = sb.ToString().Remove(sb.ToString().Length - 2);
            if (errorCount > 1)
            {
                int lastCommaIndex = s.LastIndexOf(",");
                s = s.Remove(lastCommaIndex, 1);
                s = s.Insert(lastCommaIndex, " and");
            }

            return "Field" + (errorCount > 1 ? "s " : " ") + s
                    + (errorCount > 1 ? " are " : " is ") + "mandatory when creating/updating database records.";
        }

        /// <summary>
        /// Gets the property errors message.
        /// </summary>
        /// <param name="format">The format to use.</param>
        /// <returns></returns>
        public string GetPropertyErrorsMessage(TextFormat format)
        {
            if (!HasPropertyErrors)
                return string.Empty;

            string lineBreak = Text.GetLineBreak(format);
            StringBuilder sb = new StringBuilder();
            foreach (PropertyError error in PropertyErrors)
            {
                sb.Append(error.ToString(format) + lineBreak);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Converts this report to a string string.
        /// </summary>
        /// <param name="format">The format to use.</param>
        /// <returns></returns>
        public string ToString(TextFormat format)
        {
            string lineBreak = Text.GetLineBreak(format);
            StringBuilder sb = new StringBuilder();

            sb.Append("Validation status: " + this.ValidationStatus.ToString());

            sb.Append(lineBreak + lineBreak);

            if (HasMandatoryFieldViolations)
            {
                sb.Append("Mandatory field violations: ");
                sb.Append(lineBreak + lineBreak + GetMandatoryFieldErrorMessage());
                sb.Append(lineBreak + lineBreak);
            }

            if (HasPropertyErrors)
            {
                sb.Append("Property errors: ");
                sb.Append(lineBreak + lineBreak + GetPropertyErrorsMessage(format));
                sb.Append(lineBreak + lineBreak);
            }

            if (this.CustomErrorMessages.Count > 0)
            {
                sb.Append("Errors: ");
                sb.Append(lineBreak + lineBreak);
                foreach (string errorMessage in this.CustomErrorMessages)
                    sb.Append(lineBreak + errorMessage);
            }

            return sb.ToString();
        }
        #endregion

        #region overrides
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return ToString(TextFormat.HTML);
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessObjectValidationResult"/> class.
        /// </summary>
        public BusinessObjectValidationResult()
        {
            this.CustomErrorMessages = new List<string>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessObjectValidationResult"/> class.
        /// </summary>
        /// <param name="status">The status to initialize this object with.</param>
        public BusinessObjectValidationResult(ValidationStatus status)
        {
            this.CustomErrorMessages = new List<string>();
            _Status = status;
        }
        #endregion
    }
}
