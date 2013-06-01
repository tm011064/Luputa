using System;
using System.Reflection;

namespace CommonTools.Components.BusinessTier
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BusinessObjectCacheAttribute : Attribute
    {
        private string _ConfigSectionKey;
        /// <summary>
        /// Gets the config section key.
        /// </summary>
        /// <value>The config section key.</value>
        public string ConfigSectionKey
        {
            get { return _ConfigSectionKey; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessObjectCacheAttribute"/> class.
        /// </summary>
        /// <param name="configSectionKey">The config section key.</param>
        public BusinessObjectCacheAttribute(string configSectionKey)
        {
            this._ConfigSectionKey = configSectionKey;
        }
    }
}