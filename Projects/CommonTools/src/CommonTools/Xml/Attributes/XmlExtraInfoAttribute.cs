using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.Xml
{
    /// <summary>
    /// This class contains all XML extra info attribute related data
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class XmlExtraInfoAttribute : System.Configuration.ConfigurationValidatorAttribute
    {
        #region properties
        private string _TagName;
        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        /// <value>The name of the tag.</value>
        public string TagName
        {
            get { return _TagName; }
            set { _TagName = value; }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlExtraInfoAttribute"/> class.
        /// </summary>
        public XmlExtraInfoAttribute() { }
        #endregion

        #region overrides
        /// <summary>
        /// Gets the validator attribute instance.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The current <see cref="T:System.Configuration.ConfigurationValidatorBase"/>.
        /// </returns>
        public override System.Configuration.ConfigurationValidatorBase ValidatorInstance
        {
            get { return base.ValidatorInstance; }
        }
        #endregion
    }
}
