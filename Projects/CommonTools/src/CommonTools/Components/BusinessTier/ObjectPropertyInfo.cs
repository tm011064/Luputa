using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CommonTools.Components.BusinessTier
{
    /// <summary>
    /// This class contains all object property info related data
    /// </summary>
    class ObjectPropertyInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public enum PropertyTypeDefinition
        {
            /// <summary>
            /// 
            /// </summary>
            String,
            /// <summary>
            /// 
            /// </summary>
            Int64,
            /// <summary>
            /// 
            /// </summary>
            Int32,
            /// <summary>
            /// 
            /// </summary>
            Int16,
            /// <summary>
            /// 
            /// </summary>
            Decimal,
            /// <summary>
            /// 
            /// </summary>
            Double
        }

        /// <summary>
        /// Gets or sets the property info.
        /// </summary>
        /// <value>The property info.</value>
        public PropertyInfo PropertyInfo { get; private set; }
        /// <summary>
        /// Gets or sets the business object property attribute.
        /// </summary>
        /// <value>The business object property attribute.</value>
        public BusinessObjectPropertyAttribute BusinessObjectPropertyAttribute { get; private set; }
        /// <summary>
        /// Gets or sets the business object validation attribute.
        /// </summary>
        /// <value>The business object validation attribute.</value>
        public BusinessObjectValidationAttribute BusinessObjectValidationAttribute { get; private set; }
        /// <summary>
        /// Gets or sets the business object string security attribute.
        /// </summary>
        /// <value>The business object string security attribute.</value>
        public BusinessObjectStringSecurityAttribute BusinessObjectStringSecurityAttribute { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has business object property attribute.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has business object property attribute; otherwise, <c>false</c>.
        /// </value>
        public bool HasBusinessObjectPropertyAttribute { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance has business object validation attribute.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has business object validation attribute; otherwise, <c>false</c>.
        /// </value>
        public bool HasBusinessObjectValidationAttribute { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance has business object string security attribute.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has business object string security attribute; otherwise, <c>false</c>.
        /// </value>
        public bool HasBusinessObjectStringSecurityAttribute { get; private set; }

        /// <summary>
        /// Gets or sets the type of the property.
        /// </summary>
        /// <value>The type of the property.</value>
        public PropertyTypeDefinition PropertyType { get; private set; }

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPropertyInfo"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        public ObjectPropertyInfo(PropertyInfo propertyInfo)
        {
            this.PropertyInfo = propertyInfo;

            object[] attributes = (object[])propertyInfo.GetCustomAttributes(typeof(BusinessObjectPropertyAttribute), false);
            if (attributes.Length > 0)
            {// attribute exists

                this.BusinessObjectPropertyAttribute = ((BusinessObjectPropertyAttribute)attributes[0]);
                this.HasBusinessObjectPropertyAttribute = true;
            }

            attributes = (object[])propertyInfo.GetCustomAttributes(typeof(BusinessObjectValidationAttribute), false);
            if (attributes.Length > 0)
            {// found attribute
                this.BusinessObjectValidationAttribute = attributes[0] as BusinessObjectValidationAttribute;
                this.HasBusinessObjectValidationAttribute = true;
            }

            if (propertyInfo.PropertyType == typeof(string))
                this.PropertyType = PropertyTypeDefinition.String;
            else if (propertyInfo.PropertyType == typeof(long))
                this.PropertyType = PropertyTypeDefinition.Int64;
            else if (propertyInfo.PropertyType == typeof(int))
                this.PropertyType = PropertyTypeDefinition.Int32;
            else if (propertyInfo.PropertyType == typeof(short))
                this.PropertyType = PropertyTypeDefinition.Int16;
            else if (propertyInfo.PropertyType == typeof(decimal))
                this.PropertyType = PropertyTypeDefinition.Decimal;
            else if (propertyInfo.PropertyType == typeof(double) || propertyInfo.PropertyType == typeof(Single))
                this.PropertyType = PropertyTypeDefinition.Double;

            if (this.PropertyType == PropertyTypeDefinition.String)
            {
                attributes = (object[])propertyInfo.GetCustomAttributes(typeof(BusinessObjectStringSecurityAttribute), false);
                if (attributes.Length > 0)
                {// found attribute 
                    this.BusinessObjectStringSecurityAttribute = attributes[0] as BusinessObjectStringSecurityAttribute;
                    this.HasBusinessObjectStringSecurityAttribute = true;
                }
            }

        }
        #endregion
    }
}
