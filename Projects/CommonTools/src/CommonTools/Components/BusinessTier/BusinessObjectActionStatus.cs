using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Components.BusinessTier
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BusinessObjectActionReport<T>
    {
        #region properties
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        private T _Status;
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public T Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private BusinessObjectValidationResult _ValidationResult = null;
        /// <summary>
        /// Gets or sets the validation result.
        /// </summary>
        /// <value>The validation result.</value>
        public BusinessObjectValidationResult ValidationResult
        {
            get { return _ValidationResult; }
            set { _ValidationResult = value; }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessObjectActionReport&lt;T&gt;"/> [ERROR: invalid expression DeclaringTypeKind].
        /// </summary>
        /// <param name="status">The status.</param>
        public BusinessObjectActionReport(T status)
        {
            _Status = status;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessObjectActionReport&lt;T&gt;"/> [ERROR: invalid expression DeclaringTypeKind].
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="validationResult">The validation result.</param>
        public BusinessObjectActionReport(T status, BusinessObjectValidationResult validationResult)
        {
            _Status = status;
            _ValidationResult = validationResult;
        }
        #endregion
    }
}
