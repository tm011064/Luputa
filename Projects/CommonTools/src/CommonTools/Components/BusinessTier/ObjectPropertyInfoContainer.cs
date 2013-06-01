using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CommonTools.Components.BusinessTier
{
    /// <summary>
    /// This class contains all object property info container related data
    /// </summary>
    class ObjectPropertyInfoContainer
    {
        /// <summary>
        /// Gets or sets the object property infos.
        /// </summary>
        /// <value>The object property infos.</value>
        public ObjectPropertyInfo[] ObjectPropertyInfos { get; private set; }
        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        public int Length { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPropertyInfoContainer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ObjectPropertyInfoContainer(Type type)
        {
            string fullName = type.FullName;

            PropertyInfo[] propertyInfos = type.GetProperties();
            this.Length = propertyInfos.Length;
            this.ObjectPropertyInfos = new ObjectPropertyInfo[this.Length];

            for (int i = 0; i < this.Length; i++)
                this.ObjectPropertyInfos[i] = new ObjectPropertyInfo(propertyInfos[i]);
        }
    }
}
