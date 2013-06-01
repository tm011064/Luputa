using System;
using System.Reflection;
using System.Xml.Serialization;

namespace CommonTools.Web.JavaScript
{
	/// <summary>
	/// Designates a property or field to not be serialized.
	/// </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple=false)]
	public sealed class JsonIgnoreAttribute : Attribute
	{
		#region Methods

		/// <summary>
		/// Gets a value which indicates if should be ignored in Json serialization.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsJsonIgnore(object value)
		{
			if (value == null)
			{
				return false;
			}

			Type type = value.GetType();

			ICustomAttributeProvider provider = null;
			if (type.IsEnum)
			{
				provider = type.GetField(Enum.GetName(type, value));
			}
			else
			{
				provider = value as ICustomAttributeProvider;
			}

			if (provider == null)
			{
				throw new ArgumentException();
			}

			return provider.IsDefined(typeof(JsonIgnoreAttribute), true);
		}

		/// <summary>
		/// Gets a value which indicates if should be ignored in Json serialization.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsXmlIgnore(object value)
		{
			if (value == null)
			{
				return false;
			}

			Type type = value.GetType();

			ICustomAttributeProvider provider = null;
			if (type.IsEnum)
			{
				provider = type.GetField(Enum.GetName(type, value));
			}
			else
			{
				provider = value as ICustomAttributeProvider;
			}

			if (provider == null)
			{
				throw new ArgumentException();
			}

			return provider.IsDefined(typeof(XmlIgnoreAttribute), true);
		}

		#endregion Methods
	}
}
