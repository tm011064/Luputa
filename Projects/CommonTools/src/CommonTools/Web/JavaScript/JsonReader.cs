using System;
using System.ComponentModel;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace CommonTools.Web.JavaScript
{
	/// <summary>
	/// Reader for consuming JSON data
	/// </summary>
	public class JsonReader
	{
		#region Constants

		internal const string LiteralFalse = "false";
		internal const string LiteralTrue = "true";
		internal const string LiteralNull = "null";
		internal const string LiteralNotANumber = "NaN";
		internal const string LiteralPositiveInfinity = "Infinity";
		internal const string LiteralNegativeInfinity = "-Infinity";

		internal const char OperatorNegate = '-';
		internal const char OperatorUnaryPlus = '+';
		internal const char OperatorArrayStart = '[';
		internal const char OperatorArrayEnd = ']';
		internal const char OperatorObjectStart = '{';
		internal const char OperatorObjectEnd = '}';
		internal const char OperatorStringDelim = '"';
		internal const char OperatorStringDelimAlt = '\'';
		internal const char OperatorValueDelim = ',';
		internal const char OperatorNameDelim = ':';
		internal const char OperatorCharEscape = '\\';

		private const string CommentStart = "/*";
		private const string CommentEnd = "*/";
		private const string CommentLine = "//";
		private const string LineEndings = "\r\n";

		private const string ErrorUnrecognizedToken = "Illegal JSON sequence.";
		private const string ErrorUnterminatedComment = "Unterminated comment block.";
		private const string ErrorUnterminatedObject = "Unterminated JSON object.";
		private const string ErrorUnterminatedArray = "Unterminated JSON array.";
		private const string ErrorUnterminatedString = "Unterminated JSON string.";
		private const string ErrorIllegalNumber = "Illegal JSON number.";
		private const string ErrorExpectedString = "Expected JSON string.";
		private const string ErrorExpectedObject = "Expected JSON object.";
		private const string ErrorExpectedArray = "Expected JSON array.";
		private const string ErrorExpectedPropertyName = "Expected JSON object property name.";
		private const string ErrorExpectedPropertyNameDelim = "Expected JSON object property name delimiter.";
		private const string ErrorNullValueType = "{0} does not accept null as a value";
		private const string ErrorDefaultCtor = "Only objects with default constructors can be deserialized.";
		private const string ErrorGenericIDictionary = "Types which implement Generic IDictionary<TKey, TValue> also need to implement IDictionary to be deserialized.";

		#endregion Constants
		
		#region Fields

		private Dictionary<Type, Dictionary<string, MemberInfo>> MemberMapCache = null;
		private readonly string Source = null;
		private readonly int SourceLength = 0;
		private int index = 0;
		private bool allowNullValueTypes = false;
		private string typeHintName = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor.
		/// </summary>
		/// <param name="input">TextReader containing source</param>
		public JsonReader(TextReader input)
		{
			this.Source = input.ReadToEnd();
			this.SourceLength = this.Source.Length;
		}

		/// <summary>
		/// Ctor.
		/// </summary>
		/// <param name="input">Stream containing source</param>
		public JsonReader(Stream input)
		{
			using (StreamReader reader = new StreamReader(input, true))
			{
				this.Source = reader.ReadToEnd();
			}
			this.SourceLength = this.Source.Length;
		}

		/// <summary>
		/// Ctor.
		/// </summary>
		/// <param name="input">string containing source</param>
		public JsonReader(string input)
		{
			this.Source = input;
			this.SourceLength = this.Source.Length;
		}

		/// <summary>
		/// Ctor.
		/// </summary>
		/// <param name="input">StringBuilder containing source</param>
		public JsonReader(StringBuilder input)
		{
			this.Source = input.ToString();
			this.SourceLength = this.Source.Length;
		}

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets and sets if ValueTypes can accept values of null
		/// </summary>
		/// <remarks>
		/// Only affects deserialization: if a ValueType is assigned the
		/// value of null, it will receive the value default(TheType).
		/// Setting this to false, throws an exception if null is
		/// specified for a ValueType member.
		/// </remarks>
		public bool AllowNullValueTypes
		{
			get { return this.allowNullValueTypes; }
			set { this.allowNullValueTypes = value; }
		}

		/// <summary>
		/// Gets and sets the property name used for type hinting.
		/// </summary>
		public string TypeHintName
		{
			get { return this.typeHintName; }
			set { this.typeHintName = value; }
		}

		#endregion Properties

		#region Parsing Methods

		/// <summary>
		/// Convert from JSON string to Object graph
		/// </summary>
		/// <returns></returns>
		public object Deserialize()
		{
			return this.Deserialize(null);
		}

		/// <summary>
		/// Convert from JSON string to Object graph of specific Type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public object Deserialize(Type type)
		{
			// should this run through a preliminary test here?
			return this.Read(type, false);
		}

		private object Read(Type expectedType, bool typeIsHint)
		{
			if (expectedType == typeof(Object))
			{
				expectedType = null;
			}

			JsonToken token = this.Tokenize();

			switch (token)
			{
				case JsonToken.ObjectStart:
				{
					return this.ReadObject(typeIsHint ? null : expectedType);
				}
				case JsonToken.ArrayStart:
				{
					return this.ReadArray(typeIsHint ? null : expectedType);
				}
				case JsonToken.String:
				{
					return this.ReadString(typeIsHint ? null : expectedType);
				}
				case JsonToken.Number:
				{
					return this.ReadNumber(typeIsHint ? null : expectedType);
				}
				case JsonToken.False:
				{
					this.index += JsonReader.LiteralFalse.Length;
					return false;
				}
				case JsonToken.True:
				{
					this.index += JsonReader.LiteralTrue.Length;
					return true;
				}
				case JsonToken.Null:
				{
					this.index += JsonReader.LiteralNull.Length;
					return null;
				}
				case JsonToken.NaN:
				{
					this.index += JsonReader.LiteralNotANumber.Length;
					return Double.NaN;
				}
				case JsonToken.PositiveInfinity:
				{
					this.index += JsonReader.LiteralPositiveInfinity.Length;
					return Double.PositiveInfinity;
				}
				case JsonToken.NegativeInfinity:
				{
					this.index += JsonReader.LiteralNegativeInfinity.Length;
					return Double.NegativeInfinity;
				}
				case JsonToken.End:
				default:
				{
					return null;
				}
			}
		}

		private object ReadObject(Type objectType)
		{
			if (this.Source[this.index] != JsonReader.OperatorObjectStart)
			{
				throw new JsonDeserializationException(JsonReader.ErrorExpectedObject, this.index);
			}

			Dictionary<string, MemberInfo> memberMap = null;
			Object result;
			if (objectType != null)
			{
				result = this.InstantiateObject(objectType, ref memberMap);
			}
			else
			{
				result = new Dictionary<String, Object>();
			}

			JsonToken token;
			do
			{
				Type memberType;
				MemberInfo memberInfo;

				// consume opening brace or delim
				this.index++;
				if (this.index >= this.SourceLength)
				{
					throw new JsonDeserializationException(JsonReader.ErrorUnterminatedObject, this.index);
				}

				// get next token
				token = this.Tokenize();
				if (token == JsonToken.ObjectEnd)
				{
					break;
				}

				if (token != JsonToken.String)
				{
					throw new JsonDeserializationException(JsonReader.ErrorExpectedPropertyName, this.index);
				}

				// parse object member value
				string memberName = (String)this.ReadString(null);

				// determine the type of the property/field
				JsonReader.GetMemberInfo(memberMap, memberName, out memberType, out memberInfo);

				// get next token
				token = this.Tokenize();
				if (token != JsonToken.NameDelim)
				{
					throw new JsonDeserializationException(JsonReader.ErrorExpectedPropertyNameDelim, this.index);
				}

				// consume delim
				index++;
				if (this.index >= this.SourceLength)
				{
					throw new JsonDeserializationException(JsonReader.ErrorUnterminatedObject, this.index);
				}

				// parse object member value
				object value = this.Read(memberType, false);

				if (result is IDictionary)
				{
					if (objectType == null &&
						!string.IsNullOrEmpty(this.typeHintName) &&
						this.typeHintName.Equals(memberName, StringComparison.InvariantCulture))
					{
						result = this.ProcessTypeHint(ref objectType, ref memberMap, (IDictionary)result, value as string);
					}
					else
					{
						((IDictionary)result)[memberName] = value;
					}
				}
				else if (objectType.GetInterface(JsonWriter.TypeGenericIDictionary) != null)
				{
					throw new JsonDeserializationException(JsonReader.ErrorGenericIDictionary, this.index);
				}
				else
				{
					this.SetMemberValue(result, memberType, memberInfo, value);
				}

				// get next token
				token = this.Tokenize();
			} while (token == JsonToken.ValueDelim);

			if (token != JsonToken.ObjectEnd)
			{
				throw new JsonDeserializationException(JsonReader.ErrorUnterminatedObject, this.index);
			}

			// consume closing brace
			this.index++;

			return result;
		}

		private Array ReadArray(Type arrayType)
		{
			if (this.Source[this.index] != JsonReader.OperatorArrayStart)
			{
				throw new JsonDeserializationException(JsonReader.ErrorExpectedArray, this.index);
			}

			bool isArrayTypeSet = (arrayType != null);
			bool isArrayTypeAHint = !isArrayTypeSet;

			if (isArrayTypeSet)
			{
				if (arrayType.HasElementType)
				{
					arrayType = arrayType.GetElementType();
				}
				else if (arrayType.IsGenericType)
				{
					Type[] generics = arrayType.GetGenericArguments();
					if (generics.Length != 1)
					{
						// could use the first or last, but this more correct
						arrayType = null;
					}
					else
					{
						arrayType = generics[0];
					}
				}
				else
				{
					arrayType = null;
				}
			}

			ArrayList jsArray = new ArrayList();

			JsonToken token;
			do
			{
				// consume opening bracket or delim
				this.index++;
				if (this.index >= this.SourceLength)
				{
					throw new JsonDeserializationException(JsonReader.ErrorUnterminatedArray, this.index);
				}

				// get next token
				token = this.Tokenize();
				if (token == JsonToken.ArrayEnd)
				{
					break;
				}

				// parse array item
				object value = this.Read(arrayType, isArrayTypeAHint);
				jsArray.Add(value);

				// establish if array is of common type
				if (value == null)
				{
					if (arrayType != null && arrayType.IsValueType)
					{
						// use plain object to hold null
						arrayType = null;
					}
					isArrayTypeSet = true;
				}
				else if (arrayType != null && !arrayType.IsAssignableFrom(value.GetType()))
				{
					// use plain object to hold value
					arrayType = null;
					isArrayTypeSet = true;
				}
				else if (!isArrayTypeSet)
				{
					// try out special type
					// if hasn't been set before
					arrayType = value.GetType();
					isArrayTypeSet = true;
				}

				// get next token
				token = this.Tokenize();
			} while (token == JsonToken.ValueDelim);

			if (token != JsonToken.ArrayEnd)
			{
				throw new JsonDeserializationException(JsonReader.ErrorUnterminatedArray, this.index);
			}

			// consume closing bracket
			this.index++;

			// check to see if all the same type and convert to that
			if (arrayType != null && arrayType != typeof(object))
			{
				return jsArray.ToArray(arrayType);
			}

			return jsArray.ToArray();
		}

		private object ReadString(Type expectedType)
		{
			if (this.Source[this.index] != JsonReader.OperatorStringDelim &&
				this.Source[this.index] != JsonReader.OperatorStringDelimAlt)
			{
				throw new JsonDeserializationException(JsonReader.ErrorExpectedString, this.index);
			}

			char startStringDelim = this.Source[this.index];

			// consume opening quote
			this.index++;
			if (this.index >= this.SourceLength)
			{
				throw new JsonDeserializationException(JsonReader.ErrorUnterminatedString, this.index);
			}

			int start = this.index;
			StringBuilder builder = new StringBuilder();

			while (this.Source[this.index] != startStringDelim)
			{
				if (this.Source[this.index] == JsonReader.OperatorCharEscape)
				{
					// copy chunk before decoding
					builder.Append(this.Source, start, this.index - start);

					// consume escape char
					this.index++;
					if (this.index >= this.SourceLength)
					{
						throw new JsonDeserializationException(JsonReader.ErrorUnterminatedString, this.index);
					}

					// decode
					switch (this.Source[this.index])
					{
						case '0':
						{
							// don't allow NULL char '\0'
							// causes CStrings to terminate
							break;
						}
						case 'b':
						{
							// backspace
							builder.Append('\b');
							break;
						}
						case 'f':
						{
							// formfeed
							builder.Append('\f');
							break;
						}
						case 'n':
						{
							// newline
							builder.Append('\n');
							break;
						}
						case 'r':
						{
							// carriage return
							builder.Append('\r');
							break;
						}
						case 't':
						{
							// tab
							builder.Append('\t');
							break;
						}
						case 'u':
						{
							// Unicode escape sequence
							// e.g. Copyright: "\u00A9"

							// unicode ordinal
							int utf16;
							if (this.index+4 < this.SourceLength &&
								Int32.TryParse(
									this.Source.Substring(this.index+1, 4),
									NumberStyles.AllowHexSpecifier,
									NumberFormatInfo.InvariantInfo,
									out utf16))
							{
								builder.Append(Char.ConvertFromUtf32(utf16));
								this.index += 4;
							}
							else
							{
								// using FireFox style recovery, if not a valid hex
								// escape sequence then treat as single escaped 'u'
								// followed by rest of string
								builder.Append(this.Source[this.index]);
							}
							break;
						}
						default:
						{
							builder.Append(Source[this.index]);
							break;
						}
					}

					this.index++;
					if (this.index >= this.SourceLength)
					{
						throw new JsonDeserializationException(JsonReader.ErrorUnterminatedString, this.index);
					}

					start = this.index;
				}
				else
				{
					// next char
					index++;
					if (this.index >= this.SourceLength)
					{
						throw new JsonDeserializationException(JsonReader.ErrorUnterminatedString, this.index);
					}
				}
			}

			// copy rest of string
			builder.Append(this.Source, start, this.index-start);

			// consume closing quote
			this.index++;

			if (expectedType != null && expectedType != typeof(String))
			{
				return JsonReader.CoerceType(expectedType, builder.ToString(), this.index, this.AllowNullValueTypes);
			}

			return builder.ToString();
		}

		private object ReadNumber(Type expectedType)
		{
			bool hasDecimal = false;
			bool hasExponent = false;
			int start = this.index;
			int precision = 0;
			int exponent = 0;

			// optional minus part
			if (this.Source[this.index] == JsonReader.OperatorNegate)
			{
				// consume sign
				this.index++;
				if (this.index >= this.SourceLength || !Char.IsDigit(this.Source[this.index]))
					throw new JsonDeserializationException(JsonReader.ErrorIllegalNumber, this.index);
			}

			// integer part
			while ((this.index < this.SourceLength) && Char.IsDigit(this.Source[this.index]))
			{
				// consume digit
				this.index++;
			}

			// optional decimal part
			if ((this.index < this.SourceLength) && (this.Source[this.index] == '.'))
			{
				hasDecimal = true;

				// consume decimal
				this.index++;
				if (this.index >= this.SourceLength || !Char.IsDigit(this.Source[this.index]))
				{
					throw new JsonDeserializationException(JsonReader.ErrorIllegalNumber, this.index);
				}

				// fraction part
				while (this.index < this.SourceLength && Char.IsDigit(this.Source[this.index]))
				{
					// consume digit
					this.index++;
				}
			}

			// note the number of significant digits
			precision = this.index-start - (hasDecimal ? 1 : 0);

			// optional exponent part
			if (this.index < this.SourceLength && (this.Source[this.index] == 'e' || this.Source[this.index] == 'E'))
			{
				hasExponent = true;

				// consume 'e'
				this.index++;
				if (this.index >= this.SourceLength)
				{
					throw new JsonDeserializationException(JsonReader.ErrorIllegalNumber, this.index);
				}

				int expStart = this.index;

				// optional minus/plus part
				if (this.Source[this.index] == JsonReader.OperatorNegate || this.Source[this.index] == JsonReader.OperatorUnaryPlus)
				{
					// consume sign
					this.index++;
					if (this.index >= this.SourceLength || !Char.IsDigit(this.Source[this.index]))
					{
						throw new JsonDeserializationException(JsonReader.ErrorIllegalNumber, this.index);
					}
				}
				else
				{
					if (!Char.IsDigit(this.Source[this.index]))
					{
						throw new JsonDeserializationException(JsonReader.ErrorIllegalNumber, this.index);
					}
				}

				// exp part
				while (this.index < this.SourceLength && Char.IsDigit(this.Source[this.index]))
				{
					// consume digit
					this.index++;
				}

				Int32.TryParse(this.Source.Substring(expStart, this.index-expStart), NumberStyles.Integer,
					NumberFormatInfo.InvariantInfo, out exponent);
			}

			// at this point, we have the full number string and know its characteristics
			string numberString = this.Source.Substring(start, this.index - start);

			if (!hasDecimal && !hasExponent && precision < 19)
			{
				// is Integer value

				// parse as most flexible
				decimal number = Decimal.Parse(
					numberString,
					NumberStyles.Integer,
					NumberFormatInfo.InvariantInfo);

				if (expectedType != null)
				{
					return JsonReader.CoerceType(expectedType, number, this.index, this.AllowNullValueTypes);
				}

				if (number >= Int32.MinValue && number <= Int32.MaxValue)
				{
					// use most common
					return (int)number;
				}
				if (number >= Int64.MinValue && number <= Int64.MaxValue)
				{
					// use more flexible
					return (long)number;
				}

				// use most flexible
				return number;
			}
			else
			{
				// is Floating Point value

				if (expectedType == typeof(Decimal))
				{
					// special case since Double does not convert to Decimal
					return Decimal.Parse(
						numberString,
						NumberStyles.Float,
						NumberFormatInfo.InvariantInfo);
				}

				// use native EcmaScript number (IEEE 754)
				double number = Double.Parse(
					numberString,
					NumberStyles.Float,
					NumberFormatInfo.InvariantInfo);

				if (expectedType != null)
				{
					return JsonReader.CoerceType(expectedType, number, this.index, this.AllowNullValueTypes);
				}

				return number;
			}
		}

		#endregion Parsing Methods

		#region Object Methods

		/// <summary>
		/// If a Type Hint is present then this method attempts to
		/// use it and move any previously parsed data over.
		/// </summary>
		/// <param name="objectType">reference to the objectType</param>
		/// <param name="memberMap">reference to the memberMap</param>
		/// <param name="result">the previous result</param>
		/// <param name="typeInfo">the type info string to use</param>
		/// <returns></returns>
		private Object ProcessTypeHint(
			ref Type objectType,
			ref Dictionary<string, MemberInfo> memberMap,
			IDictionary result,
			string typeInfo)
		{
			if (string.IsNullOrEmpty(typeInfo))
			{
				return result;
			}

			Type hintedType = Type.GetType(typeInfo, false);
			if (hintedType == null)
			{
				return result;
			}
			objectType = hintedType;

			object newResult = this.InstantiateObject(hintedType, ref memberMap);
			if (memberMap != null)
			{
				Type memberType;
				MemberInfo memberInfo;

				// copy any values into new object
				foreach (object key in result.Keys)
				{
					JsonReader.GetMemberInfo(memberMap, key as String, out memberType, out memberInfo);
					this.SetMemberValue(newResult, memberType, memberInfo, result[key]);
				}
			}

			return newResult;
		}

		private Object InstantiateObject(Type objectType, ref Dictionary<string, MemberInfo> memberMap)
		{
			Object result;
			ConstructorInfo ctor = objectType.GetConstructor(Type.EmptyTypes);
			if (ctor == null)
			{
				throw new JsonDeserializationException(JsonReader.ErrorDefaultCtor, this.index);
			}
			result = ctor.Invoke(null);

			// don't incurr the cost of member map if a dictionary
			if (!typeof(IDictionary).IsAssignableFrom(objectType))
			{
				memberMap = this.CreateMemberMap(objectType);
			}
			return result;
		}

		private Dictionary<string, MemberInfo> CreateMemberMap(Type objectType)
		{
			if (this.MemberMapCache == null)
			{
				// instantiate space for cache
				this.MemberMapCache = new Dictionary<Type, Dictionary<string, MemberInfo>>();
			}
			else if (this.MemberMapCache.ContainsKey(objectType))
			{
				// map was stored in cache
				return this.MemberMapCache[objectType];
			}

			// create a new map
			Dictionary<string, MemberInfo> memberMap = new Dictionary<string, MemberInfo>();

			// load properties into property map
			PropertyInfo[] properties = objectType.GetProperties();
			foreach (PropertyInfo info in properties)
			{
				if (!info.CanRead || !info.CanWrite)
				{
					continue;
				}

				if (JsonIgnoreAttribute.IsJsonIgnore(info))
				{
					continue;
				}

				string jsonName = JsonNameAttribute.GetJsonName(info);
				if (string.IsNullOrEmpty(jsonName))
				{
					memberMap[info.Name] = info;
				}
				else
				{
					memberMap[jsonName] = info;
				}
			}

			// load public fields into property map
			FieldInfo[] fields = objectType.GetFields();
			foreach (FieldInfo info in fields)
			{
				if (!info.IsPublic)
				{
					continue;
				}

				if (JsonIgnoreAttribute.IsJsonIgnore(info))
				{
					continue;
				}

				string jsonName = JsonNameAttribute.GetJsonName(info);
				if (string.IsNullOrEmpty(jsonName))
				{
					memberMap[info.Name] = info;
				}
				else
				{
					memberMap[jsonName] = info;
				}
			}

			// store in cache for repeated usage
			this.MemberMapCache[objectType] = memberMap;

			return memberMap;
		}

		private static void GetMemberInfo(
			Dictionary<string, MemberInfo> memberMap,
			string memberName,
			out Type memberType,
			out MemberInfo memberInfo)
		{
			memberType = null;
			memberInfo = null;

			if (memberMap != null &&
				memberMap.ContainsKey(memberName))
			{
				// Check properties for object member
				memberInfo = memberMap[memberName];

				if (memberInfo is PropertyInfo)
				{
					// maps to public property
					memberType = ((PropertyInfo)memberInfo).PropertyType;
				}
				else if (memberInfo is FieldInfo)
				{
					// maps to public field
					memberType = ((FieldInfo)memberInfo).FieldType;
				}
				else
				{
					// none found
					memberType = null;
				}
			}
			else
			{
				// none found
				memberType = null;
			}
		}

		/// <summary>
		/// Helper method to set value of either property or field
		/// </summary>
		/// <param name="result"></param>
		/// <param name="memberType"></param>
		/// <param name="memberInfo"></param>
		/// <param name="value"></param>
		private void SetMemberValue(Object result, Type memberType, MemberInfo memberInfo, object value)
		{
			if (memberInfo is PropertyInfo)
			{
				// set value of public property
				((PropertyInfo)memberInfo).SetValue(
					result,
					JsonReader.CoerceType(memberType, value, this.index, this.AllowNullValueTypes),
					null);
			}
			else if (memberInfo is FieldInfo)
			{
				// set value of public field
				((FieldInfo)memberInfo).SetValue(
					result,
					JsonReader.CoerceType(memberType, value, this.index, this.AllowNullValueTypes));
			}

			// all other values are ignored
		}

		#endregion Object Methods

		#region Type Methods

        /// <summary>
        /// Coerces the type.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
		public static object CoerceType(Type targetType, object value)
		{
			return JsonReader.CoerceType(targetType, value, -1, false);
		}

		private static object CoerceType(Type targetType, object value, int index, bool allowNullValueTypes)
		{
			bool isNullable = JsonReader.IsNullable(targetType);
			if (value == null)
			{
				if (allowNullValueTypes &&
					targetType.IsValueType &&
					!isNullable)
				{
					throw new JsonDeserializationException(String.Format(JsonReader.ErrorNullValueType, targetType.FullName), index);
				}
				return value;
			}

			if (isNullable)
			{
				// nullable types have a real underlying struct
				Type[] genericArgs = targetType.GetGenericArguments();
				if (genericArgs.Length == 1)
				{
					targetType = genericArgs[0];
				}
			}

			Type actualType = value.GetType();
			if (targetType.IsAssignableFrom(actualType))
			{
				return value;
			}

			if (targetType.IsEnum)
			{
				if (!Enum.IsDefined(targetType, value))
				{
					// if isn't a defined value perhaps it is the JsonName
					foreach (FieldInfo field in targetType.GetFields())
					{
						string jsonName = JsonNameAttribute.GetJsonName(field);
						if (((string)value).Equals(jsonName))
						{
							value = field.Name;
							break;
						}
					}
				}

				if (value is String)
				{
					return Enum.Parse(targetType, (string)value);
				}
				else
				{
					return Convert.ChangeType(value, targetType);
				}
			}

			if (actualType.IsArray && !targetType.IsArray)
			{
				return JsonReader.CoerceArray(targetType, actualType, value, index, allowNullValueTypes);
			}

			if (value is String)
			{
				if (targetType == typeof(DateTime))
				{
					DateTime date;
					if (DateTime.TryParse(
						(string)value,
						DateTimeFormatInfo.InvariantInfo,
						DateTimeStyles.RoundtripKind|DateTimeStyles.AllowWhiteSpaces|DateTimeStyles.NoCurrentDateDefault,
						out date))
					{
						return date;
					}
				}
				else if (targetType == typeof(Guid))
				{
					// try-catch is pointless since will throw upon generic conversion
					return new Guid((string)value);
				}
				else if (targetType == typeof(Uri))
				{
					Uri uri;
					if (Uri.TryCreate((string)value, UriKind.RelativeOrAbsolute, out uri))
					{
						return uri;
					}
				}
				else if (targetType == typeof(Version))
				{
					// try-catch is pointless since will throw upon generic conversion
					return new Version((string)value);
				}
			}

			else if (targetType == typeof(TimeSpan))
			{
				return new TimeSpan((long)JsonReader.CoerceType(typeof(Int64), value, index, allowNullValueTypes));
			}

			TypeConverter converter = TypeDescriptor.GetConverter(targetType);
			if (converter.CanConvertFrom(actualType))
			{
				return converter.ConvertFrom(value);
			}

			converter = TypeDescriptor.GetConverter(actualType);
			if (converter.CanConvertTo(targetType))
			{
				return converter.ConvertTo(value, targetType);
			}

			return Convert.ChangeType(value, targetType);
		}

		private static object CoerceArray(Type targetType, Type arrayType, object value, int index, bool allowNullValueTypes)
		{
			// targetType serializes as a JSON array but is not an array
			// assume is an ICollection / IEnumerable with AddRange, Add,
			// or custom Constructor with which we can populate it

			ConstructorInfo ctor = targetType.GetConstructor(Type.EmptyTypes);
			if (ctor == null)
			{
				throw new JsonDeserializationException(JsonReader.ErrorDefaultCtor, index);
			}
			object collection = ctor.Invoke(null);

			Array arrayValue = (Array)value;

			// many ICollection types have an AddRange method
			// which adds all items at once
			MethodInfo method = targetType.GetMethod("AddRange");
			ParameterInfo[] parameters = (method == null) ?
					null : method.GetParameters();
			Type paramType = (parameters == null || parameters.Length != 1) ?
					null : parameters[0].ParameterType;
			if (paramType != null &&
					paramType.IsAssignableFrom(arrayType))
			{
				// add all members in one method
				method.Invoke(
					collection,
					new object[] { arrayValue });
				return collection;
			}
			else
			{
				// many ICollection types have an Add method
				// which adds items one at a time
				method = targetType.GetMethod("Add");
				parameters = (method == null) ?
						null : method.GetParameters();
				paramType = (parameters == null || parameters.Length != 1) ?
						null : parameters[0].ParameterType;
				if (paramType != null)
				{
					// loop through adding items to collection
					foreach (object item in arrayValue)
					{
						method.Invoke(
							collection,
							new object[] {
									JsonReader.CoerceType(paramType, item, index, allowNullValueTypes)
								});
					}
					return collection;
				}
			}

			// many ICollection types take an IEnumerable or ICollection
			// as a constructor argument.  look through constructors for
			// a compatible match.
			ConstructorInfo[] ctors = targetType.GetConstructors();
			foreach (ConstructorInfo ctor2 in ctors)
			{
				ParameterInfo[] paramList = ctor2.GetParameters();
				if (paramList.Length == 1 &&
						paramList[0].ParameterType.IsAssignableFrom(arrayType))
				{
					try
					{
						// invoke first constructor that can take this value as an argument
						return ctor2.Invoke(
								new object[] { value }
							);
					}
					catch
					{
						// there might exist a better match
						continue;
					}
				}
			}

			return Convert.ChangeType(value, targetType);
		}

		private static bool IsNullable(Type type)
		{
			return type.IsGenericType && (typeof(Nullable<>) == type.GetGenericTypeDefinition());
		}

		#endregion Type Methods

		#region Tokenizing Methods

		private JsonToken Tokenize()
		{
			if (this.index >= this.SourceLength)
			{
				return JsonToken.End;
			}

			// skip whitespace
			while (Char.IsWhiteSpace(this.Source[this.index]))
			{
				this.index++;
				if (this.index >= this.SourceLength)
				{
					return JsonToken.End;
				}
			}

			#region Skip Comments

			// skip block and line comments
			if (this.Source[this.index] == JsonReader.CommentStart[0])
			{
				if (this.index+1 >= this.SourceLength)
				{
					throw new JsonDeserializationException(JsonReader.ErrorUnrecognizedToken, this.index);
				}

				// skip over first char of comment start
				this.index++;

				bool isBlockComment = false;
				if (this.Source[this.index] == JsonReader.CommentStart[1])
				{
					isBlockComment = true;
				}
				else if (this.Source[this.index] != JsonReader.CommentLine[1])
				{
					throw new JsonDeserializationException(JsonReader.ErrorUnrecognizedToken, this.index);
				}
				// skip over second char of comment start
				this.index++;

				if (isBlockComment)
				{
					// store index for unterminated case
					int commentStart = this.index-2;

					if (this.index+1 >= this.SourceLength)
					{
						throw new JsonDeserializationException(JsonReader.ErrorUnterminatedComment, commentStart);
					}

					// skip over everything until reach block comment ending
					while (this.Source[this.index] != JsonReader.CommentEnd[0] ||
						this.Source[this.index+1] != JsonReader.CommentEnd[1])
					{
						this.index++;
						if (this.index+1 >= this.SourceLength)
						{
							throw new JsonDeserializationException(JsonReader.ErrorUnterminatedComment, commentStart);
						}
					}

					// skip block comment end token
					this.index += 2;
					if (this.index >= this.SourceLength)
					{
						return JsonToken.End;
					}
				}
				else
				{
					// skip over everything until reach line ending
					while (JsonReader.LineEndings.IndexOf(this.Source[this.index]) < 0)
					{
						this.index++;
						if (this.index >= this.SourceLength)
						{
							return JsonToken.End;
						}
					}
				}

				// skip whitespace again
				while (Char.IsWhiteSpace(this.Source[this.index]))
				{
					this.index++;
					if (this.index >= this.SourceLength)
					{
						return JsonToken.End;
					}
				}
			}

			#endregion Skip Comments

			// consume positive signing (as is extraneous)
			if (this.Source[this.index] == JsonReader.OperatorUnaryPlus)
			{
				this.index++;
				if (this.index >= this.SourceLength)
				{
					return JsonToken.End;
				}
			}

			switch (this.Source[this.index])
			{
				case JsonReader.OperatorArrayStart:
				{
					return JsonToken.ArrayStart;
				}
				case JsonReader.OperatorArrayEnd:
				{
					return JsonToken.ArrayEnd;
				}
				case JsonReader.OperatorObjectStart:
				{
					return JsonToken.ObjectStart;
				}
				case JsonReader.OperatorObjectEnd:
				{
					return JsonToken.ObjectEnd;
				}
				case JsonReader.OperatorStringDelim:
				case JsonReader.OperatorStringDelimAlt:
				{
					return JsonToken.String;
				}
				case JsonReader.OperatorValueDelim:
				{
					return JsonToken.ValueDelim;
				}
				case JsonReader.OperatorNameDelim:
				{
					return JsonToken.NameDelim;
				}
				default:
				{
					break;
				}
			}

			// number
			if (Char.IsDigit(this.Source[this.index]) ||
				((this.Source[this.index] == JsonReader.OperatorNegate) && (this.index+1 < this.SourceLength) && Char.IsDigit(this.Source[this.index+1])))
			{
				return JsonToken.Number;
			}

			// "false" literal
			if (this.MatchLiteral(JsonReader.LiteralFalse))
			{
				return JsonToken.False;
			}

			// "true" literal
			if (this.MatchLiteral(JsonReader.LiteralTrue))
			{
				return JsonToken.True;
			}

			// "null" literal
			if (this.MatchLiteral(JsonReader.LiteralNull))
			{
				return JsonToken.Null;
			}

			// "NaN" literal
			if (this.MatchLiteral(JsonReader.LiteralNotANumber))
			{
				return JsonToken.NaN;
			}

			// "Infinity" literal
			if (this.MatchLiteral(JsonReader.LiteralPositiveInfinity))
			{
				return JsonToken.PositiveInfinity;
			}

			// "-Infinity" literal
			if (this.MatchLiteral(JsonReader.LiteralNegativeInfinity))
			{
				return JsonToken.NegativeInfinity;
			}

			throw new JsonDeserializationException(JsonReader.ErrorUnrecognizedToken, this.index);
		}

		/// <summary>
		/// Determines if the next token is the given literal
		/// </summary>
		/// <param name="literal"></param>
		/// <returns></returns>
		private bool MatchLiteral(string literal)
		{
			int literalLength = literal.Length;
			for (int i=0, j=this.index; i<literalLength && j<this.SourceLength; i++, j++)
			{
				if (literal[i] != this.Source[j])
				{
					return false;
				}
			}

			return true;
		}

		#endregion Tokenizing Methods
	}
}
