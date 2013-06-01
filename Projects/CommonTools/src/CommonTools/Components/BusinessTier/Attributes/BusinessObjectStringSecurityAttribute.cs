using System;
using System.Configuration;

namespace CommonTools.Components.BusinessTier
{
    /// <summary>
    /// This attribute can be used for BaseBusinessObject properties in order to enforce constraints.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class BusinessObjectStringSecurityAttribute : ConfigurationValidatorAttribute
    {
        #region properties
        private bool _RemoveScriptTags;
        /// <summary>
        /// Gets or sets a value indicating whether script tags should be removed prior to SQL insert/update.
        /// </summary>
        /// <value><c>true</c> if [remove script tags]; otherwise, <c>false</c>.</value>
        public bool RemoveScriptTags
        {
            get { return _RemoveScriptTags; }
            set { _RemoveScriptTags = value; }
        }

        private bool _DefuseScriptTags;
        /// <summary>
        /// Gets or sets a value indicating whether script tags should be disarmed via tag bracket replacement.
        /// </summary>
        /// <value><c>true</c> if [remove script tags]; otherwise, <c>false</c>.</value>
        public bool DefuseScriptTags
        {
            get { return _DefuseScriptTags; }
            set { _DefuseScriptTags = value; }
        }

        private bool _RemoveBadSQLCharacters;
        /// <summary>
        /// Gets or sets a value indicating whether bad SQL Characters should be removed prior to SQL insert/update.
        /// </summary>
        /// <value><c>true</c> if [remove script tags]; otherwise, <c>false</c>.</value>
        public bool RemoveBadSQLCharacters
        {
            get { return _RemoveBadSQLCharacters; }
            set { _RemoveBadSQLCharacters = value; }
        }

        private bool _RemoveBadHtmlTags;
        /// <summary>
        /// Gets or sets a value indicating whether bad HTML tags should be removed prior to SQL insert/update.
        /// </summary>
        /// <value><c>true</c> if [remove script tags]; otherwise, <c>false</c>.</value>
        public bool RemoveBadHtmlTags
        {
            get { return _RemoveBadHtmlTags; }
            set { _RemoveBadHtmlTags = value; }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessObjectPropertyAttribute"/> class.
        /// </summary>
        public BusinessObjectStringSecurityAttribute() { }
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