using System;
using System.Reflection;
using System.Xml.Serialization;

namespace CommonTools.Web.JavaScript
{
	/// <summary>
	/// Specifies the naming to use for a property or field when serializing
	/// </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple=false)]
	public class JsonNameAttribute : Attribute
	{
		#region Fields

		private string jsonName = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public JsonNameAttribute()
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="jsonName"></param>
		public JsonNameAttribute(string jsonName)
		{
			this.jsonName = jsonName;
		}

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets and sets the name to be used in JSON
		/// </summary>
		public string Name
		{
			get { return this.jsonName; }
			set { this.jsonName = value; }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Gets the name specified for use in Json serialization.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetJsonName(object value)
		{
			if (value == null)
			{
				return null;
			}

			Type type = value.GetType();
			MemberInfo memberInfo = null;

			if (type.IsEnum)
			{
				string name = Enum.GetName(type, value);
				if (string.IsNullOrEmpty(name))
				{
					return null;
				}
				memberInfo = type.GetField(name);
			}
			else
			{
				memberInfo = value as MemberInfo;
			}

			if (memberInfo == null)
			{
				throw new ArgumentException();
			}

			if (!Attribute.IsDefined(memberInfo, typeof(JsonNameAttribute)))
			{
				return null;
			}
			JsonNameAttribute attribute = (JsonNameAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(JsonNameAttribute));

			return attribute.Name;
		}

		///// <summary>
		///// Gets the name specified for use in Json serialization.
		///// </summary>
		///// <param name="value"></param>
		///// <returns></returns>
		//public static string GetXmlName(object value)
		//{
		//    if (value == null)
		//    {
		//        return null;
		//    }

		//    Type type = value.GetType();
		//    ICustomAttributeProvider memberInfo = null;

		//    if (type.IsEnum)
		//    {
		//        string name = Enum.GetName(type, value);
		//        if (string.IsNullOrEmpty(name))
		//        {
		//            return null;
		//        }
		//        memberInfo = type.GetField(name);
		//    }
		//    else
		//    {
		//        memberInfo = value as ICustomAttributeProvider;
		//    }

		//    if (memberInfo == null)
		//    {
		//        throw new ArgumentException();
		//    }

		//    XmlAttributes xmlAttributes = new XmlAttributes(memberInfo);
		//    if (xmlAttributes.XmlElements.Count == 1 &&
		//        !string.IsNullOrEmpty(xmlAttributes.XmlElements[0].ElementName))
		//    {
		//        return xmlAttributes.XmlElements[0].ElementName;
		//    }
		//    if (xmlAttributes.XmlAttribute != null &&
		//        !string.IsNullOrEmpty(xmlAttributes.XmlAttribute.AttributeName))
		//    {
		//        return xmlAttributes.XmlAttribute.AttributeName;
		//    }

		//    return null;
		//}

		#endregion Methods
	}
}
