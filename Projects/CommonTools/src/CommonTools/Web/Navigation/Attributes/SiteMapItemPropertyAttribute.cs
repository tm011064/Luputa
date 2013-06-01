using System;
using System.Reflection;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// This class contains all site map item property attribute related data
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SiteMapItemPropertyAttribute : Attribute
    {
        private string _PropertyName;
        /// <summary>
        /// Gets or sets the name of the attribute.
        /// </summary>
        /// <value>The name of the attribute.</value>
        public string PropertyName
        {
            get { return _PropertyName; }
            private set { _PropertyName = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapItemPropertyAttribute"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public SiteMapItemPropertyAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }
    }
}