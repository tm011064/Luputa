using System;
using System.Configuration;

namespace CommonTools.Components.BusinessTier
{
    /// <summary>
    /// This attribute can be used for BaseBusinessObject properties in order to enforce constraints.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class BusinessObjectPropertyAttribute : ConfigurationValidatorAttribute
    {
        #region properties
        private bool _PropagateValidation;
        /// <summary>
        /// Gets or sets a value indicating whether [propagate validation].
        /// </summary>
        /// <value><c>true</c> if [propagate validation]; otherwise, <c>false</c>.</value>
        public bool PropagateValidation
        {
            get { return _PropagateValidation; }
            set { _PropagateValidation = value; }
        }

        private bool _IsMandatoryForInstance;
        /// <summary>
        /// Gets or sets a value indicating whether this property needs to be set before it can be
        /// created at the database.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this property is mandatory for instance; otherwise, <c>false</c>.
        /// </value>
        public bool IsMandatoryForInstance
        {
            get { return _IsMandatoryForInstance; }
            set { _IsMandatoryForInstance = value; }
        }

        private object _DefaultValue;
        /// <summary>
        /// Gets or sets the default value for this property if it doesn not get set before database creation.
        /// </summary>
        /// <value>The default value.</value>
        public object DefaultValue
        {
            get { return _DefaultValue; }
            set { _DefaultValue = value; }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessObjectPropertyAttribute"/> class.
        /// </summary>
        public BusinessObjectPropertyAttribute() { }
        #endregion

        #region overrides
        /// <summary>
        /// Gets the validator attribute instance.
        /// </summary>
        /// <value></value>
        /// <returns>The current <see cref="T:System.Configuration.ConfigurationValidatorBase"></see>.</returns>
        public override ConfigurationValidatorBase ValidatorInstance
        {
            get { return base.ValidatorInstance; }
        }
        #endregion
    }
}