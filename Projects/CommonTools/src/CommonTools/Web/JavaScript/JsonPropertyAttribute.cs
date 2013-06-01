using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Web.JavaScript
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple=false)]
    public class JsonPropertyAttribute:JsonNameAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonPropertyAttribute"/> class.
        /// </summary>
        public JsonPropertyAttribute():base()
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonPropertyAttribute"/> class.
        /// </summary>
        /// <param name="jsonname">The jsonname.</param>
        public JsonPropertyAttribute(string jsonname):base(jsonname)
        {
            
        }
    }
}
