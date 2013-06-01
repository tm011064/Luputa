using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using CommonTools.Xml;
using CommonTools.Finance;

namespace CommonTools
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConversionHelper
    {
        #region members
        private static object _TypeNamesLock = new object();
        private static Dictionary<string, string> _TypeNamesLookup = new Dictionary<string, string>();
        #endregion

        #region Sytem.DateTime
        /// <summary>
        /// ConversionHelpers this datetime object to a canonical formated string that
        /// can be recognized by SQL. ()Format: MM.DD.YYYY hh:mm:ss.ms
        /// </summary>
        /// <param name="dt">The datetime object</param>
        /// <returns></returns>
        public static string ToCanonical(DateTime dt)
        {
            // Format: MM.DD.YYYY hh:mm:ss.ms
            return String.Format("{0}.{1}.{2} {3}:{4}:{5}.{6}",
                dt.Month, dt.Day, dt.Year, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
        }

        /// <summary>
        /// Converts this object to a SqlSmallDateTime (cuts off the seconds and milliseconds)
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static DateTime ToSqlSmallDateTime(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0);
        }
        #endregion

        #region System.String
        /// <summary>
        /// Formats a given text with some specified options.
        /// </summary>
        /// <param name="text">The text to format</param>
        /// <param name="options">The options to use in order to format the text.</param>
        /// <returns>A with the specified options formated string.</returns>
        public static string Format(string text, params TextFormatOptions[] options)
        {
            foreach (TextFormatOptions option in options)
            {
                switch (option)
                {
                    case TextFormatOptions.RemoveAllTags:
                        text = ConversionHelper.RemoveTags(text);
                        break;

                    case TextFormatOptions.RemoveBadSQLCharacters:
                        text = ConversionHelper.RemoveMaliciousSQLCharacters(text);
                        break;

                    case TextFormatOptions.RemoveBadTags:
                        text = ConversionHelper.RemoveMaliciousTags(text);
                        break;

                    case TextFormatOptions.RemoveLineBreaks:
                        text = ConversionHelper.RemoveLineBreaks(text);
                        break;

                    case TextFormatOptions.RemoveScriptTags:
                        text = ConversionHelper.RemoveScriptTags(text);
                        break;

                    case TextFormatOptions.SafeQuerystringParameter:
                        text = ConversionHelper.UrlEncode(text);
                        break;

                    case TextFormatOptions.DefuseScriptTags:
                        text = ConversionHelper.DefuseScriptTags(text);
                        break;
                }
            }

            return text;
        }

        /// <summary>
        /// Gets a string with a lower-case initial letter, e.g.: ThisIsMyString -> thisIsMyString
        /// </summary>
        /// <param name="text">The string to ConversionHelper</param>
        /// <returns>a string with a lower-case initial letter</returns>
        public static string ToLoweredInitalString(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            char[] returnValue = text.ToCharArray();
            returnValue[0] = Char.ToLower(text[0]);
            return new String(returnValue);
        }

        /// <summary>
        /// ConversionHelpers the given string to upper case only words, e.g.: this is My STRING -> This Is My String
        /// </summary>
        /// <param name="text">The string to ConversionHelper</param>
        /// <returns>to upper case only words ConversionHelpered string</returns>
        public static string ToUpperCaseWordsString(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            string[] splitted = text.Split(' ');
            string returnString = string.Empty;
            foreach (string split in splitted)
            {
                char[] c = split.ToLower().ToCharArray();
                if (c.Length > 0)
                {
                    c[0] = Char.ToUpper(split[0]);
                    returnString += new String(c) + " ";
                }
            }
            return returnString.Trim();
        }

        /// <summary>
        /// This method gets a special-character safe querystring parameter
        /// </summary>
        /// <param name="id">The ID of the parameter</param>
        /// <param name="text">The value of the parameter</param>
        /// <returns>a special-character safe querystring parameter in the format {id}={value}</returns>
        public static string ToQuerystringParameter(string text, string id)
        {
            return id + "=" + ConversionHelper.UrlEncode(text);
        }

        /// <summary>
        /// Transforms a given string to its querystring-safe representation.
        /// </summary>
        /// <param name="text">The string to ConversionHelper</param>
        /// <returns>The querystring-safe representation of a given string</returns>
        public static string UrlEncode(string text)
        {
            return HttpUtility.UrlEncode(text);
        }
        /// <summary>
        /// ConversionHelpers a string that has been encoded for transmission in a URL into a
        /// decoded string.
        /// </summary>
        /// <param name="text">The string to ConversionHelper</param>
        /// <returns>The querystring-safe representation of a given string</returns>
        public static string UrlDecode(string text)
        {
            return HttpUtility.UrlDecode(text);
        }


        /// <summary>
        /// Splits long words using the splitter supplied, and limiting word lengths to whatever is specified.
        /// </summary>
        /// <param name="text">the text to check</param>
        /// <param name="splitter">the splitter</param>
        /// <param name="maxWordLength">maximum word length</param>
        /// <returns></returns>
        public static string SplitLongWords(string text, string splitter, int maxWordLength)
        {
            string returnString = "";
            string[] words = text.Split(' ');

            foreach (string word in words)
            {
                if (word.Length > maxWordLength)
                {
                    string tempWord = word;
                    while (tempWord.Length > maxWordLength)
                    {
                        returnString += tempWord.Substring(0, maxWordLength - 1) + splitter;
                        tempWord = tempWord.Remove(0, maxWordLength - 1);
                    }
                    returnString += tempWord;
                }
                else
                {
                    returnString += word;
                }
                returnString += " ";
            }
            returnString = returnString.Remove(returnString.Length - 1);

            return returnString;
        }

        /// <summary>
        /// Gets a friendly mail address.
        /// </summary>
        /// <param name="mailAddress">The mail address</param>
        /// <param name="friendlyName">Name of the p friendly.</param>
        /// <returns>A string in the friendly address name format</returns>
        public static string ToFriendlyMailAddress(string mailAddress, string friendlyName)
        {
            return "\"" + friendlyName + "\" <" + mailAddress + ">";
        }

        /// <summary>
        /// ConversionHelpers all returns ( \r\n ) from an asp:TextBox to HTML &lt;br /&gt;. Additionally, words that
        /// contain more characters than defined in the maxWordLength parameter get splitted by a blank
        /// character.
        /// </summary>
        /// <param name="text">The text to ConversionHelper</param>
        /// <param name="maxWordLength">The maximum length of a word before it gets splitted by a blank character</param>
        /// <param name="removeBadTags">Set to true to also strip out invalid tags.</param>
        /// <returns></returns>
        public static string ConversionHelperTextBoxToHtml(string text, int maxWordLength, bool removeBadTags)
        {
            string returnString = "";

            if (maxWordLength > 0)
            {
                string x = "\r\n";
                char[] cutters = x.ToCharArray();
                string[] paragraph = text.Split(cutters);

                foreach (string para in paragraph)
                {
                    if (!string.IsNullOrEmpty(para))
                    {
                        string[] words = para.Split(' ');

                        foreach (string word in words)
                        {
                            if (word.Length > maxWordLength)
                            {
                                string tempWord = word;
                                while (tempWord.Length > maxWordLength)
                                {
                                    returnString += tempWord.Substring(0, maxWordLength - 1) + "<br/>";
                                    tempWord = tempWord.Remove(0, maxWordLength - 1);
                                }
                                returnString += tempWord;
                            }
                            else
                            {
                                returnString += word;
                            }
                            returnString += " ";
                        }
                        returnString = returnString.Remove(returnString.Length - 1);
                        returnString += "<br/>";
                    }
                }
            }
            else
            {
                returnString = text;
            }

            if (removeBadTags)
            {
                returnString = ConversionHelper.RemoveMaliciousTags(returnString);
            }

            return returnString.Replace("\r\n", "<br/>");
        }
        /// <summary>
        /// ConversionHelpers all returns ( \r\n ) from an asp:TextBox to HTML &lt;br /&gt;.
        /// </summary>
        /// <param name="text">The text to ConversionHelper</param>
        /// <returns></returns>
        public static string ConversionHelperTextBoxToHtml(string text)
        {
            return ConversionHelper.ConversionHelperTextBoxToHtml(text, -1, false);
        }

        /// <summary>
        /// ConversionHelpers all returns ( \r\n ) from an asp:TextBox to HTML &lt;br /&gt;.
        /// </summary>
        /// <param name="text">The text to ConversionHelper</param>
        /// <param name="maxWordLength">Cut off after this many bytes (-1 to use infinite).</param>
        /// <returns></returns>
        public static string ConversionHelperTextBoxToHtml(string text, int maxWordLength)
        {
            return ConversionHelper.ConversionHelperTextBoxToHtml(text, maxWordLength, false);
        }

        /// <summary>
        /// Removes bad tags from string.
        /// </summary>
        /// <param name="text">The string to replace bad tags from</param>
        /// <param name="maliciousTags">bad tags in the following format: "tagname|tagname|tagname|...|tagname"</param>
        /// <returns>the input string without the specified bad tags</returns>
        public static string RemoveMaliciousTags(string text, string maliciousTags)
        {
            Regex regex = new Regex("<[/]{0,1}(" + maliciousTags + "){1}[.]*>"
                    , RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regex.Replace(text, "");
        }

        /// <summary>
        /// Removes the following tags from string a string: 
        /// abbr|acronym|address|applet|area|base|basefont|bdo|big|body|button|
        /// caption|center|cite|code|col|colgroup|dd|del|dir|div|dfn|dl|dt|embed|
        /// fieldset|font|form|frame|frameset|head|html|iframe|img|input|ins|
        /// isindex|kbd|label|legend|link|map|menu|meta|noframes|noscript|object|
        /// optgroup|option|param|pre|q|s|samp|script|select|small|span|strike|
        /// style|table|tbody|td|textarea|tfoot|th|thead|title|tr|tt|var|xmp
        /// </summary>
        /// <param name="text">The string to replace bad tags from</param>
        /// <returns>A "bad-tag-free" string.</returns>
        public static string RemoveMaliciousTags(string text)
        {
            return ConversionHelper.RemoveMaliciousTags(text, "abbr|acronym|address|applet|area|base|basefont|bdo|big|body|button|caption|center|cite|code|col|colgroup|dd|del|dir|div|dfn|dl|dt|embed|fieldset|font|form|frame|frameset|head|html|iframe|img|input|ins|isindex|kbd|label|legend|link|map|menu|meta|noframes|noscript|object|optgroup|option|param|pre|q|s|samp|script|select|small|span|strike|style|table|tbody|td|textarea|tfoot|th|thead|title|tr|tt|var|xmp");
        }

        /// <summary>
        /// ConversionHelpers HTML markup to a string that can be displayed safely in a text input field or textarea
        /// </summary>
        /// <param name="text">The HTML markup text</param>
        /// <returns>A string that can be displayed safely in a text input field or textarea</returns>
        public static string ConversionHelperHtmlToTextBox(string text)
        {
            text = Regex.Replace(text, "<br>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            text = Regex.Replace(text, "<br/>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            text = Regex.Replace(text, "<br />", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            text = Regex.Replace(text, "<p>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            text = text.Replace("'", "&#39;");

            return ConversionHelper.RemoveTags(text);
        }

        /// <summary>
        /// Removes all line breaks inside a given string.
        /// </summary>
        /// <param name="text">The text to remove the line-breaks from</param>
        /// <returns>A text without linebreaks.</returns>
        public static string RemoveLineBreaks(string text)
        {
            text = Regex.Replace(text, "<br>", " ", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            text = Regex.Replace(text, "<br/>", " ", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            text = Regex.Replace(text, "<br />", " ", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            text = Regex.Replace(text, "<p>", " ", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            text = Regex.Replace(text, "</p>", " ", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            text = Regex.Replace(text, "<p/>", " ", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            text = Regex.Replace(text, "\r\n", " ", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            text = Regex.Replace(text, "\n", " ", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return text;
        }


        /// <summary>
        /// Highlights a specified text pattern inside a string by wrapping it with a span that has the associated
        /// Css Class.
        /// </summary>
        /// <param name="text">The text to highlight content from</param>
        /// <param name="wordToHighlight">The text to highlight</param>
        /// <param name="highlightCssClass">The CSS class responsible for highlighting</param>
        /// <returns></returns>
        public static string HighlightWord(string text, string wordToHighlight, string highlightCssClass)
        {
            return text.Replace(wordToHighlight, "<span class=\"" + highlightCssClass + "\">"
                + wordToHighlight + "</span>");
        }

        /// <summary>
        /// Removes all html/xml tags from a specified string.
        /// </summary>
        /// <param name="text">String to be cleaned from tags</param>
        /// <returns>The specified string without tags</returns>
        public static string RemoveTags(string text)
        {
            return Regex.Replace(text, "<[^>]+>", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        /// This methods removes all javascript script tags from the given string
        /// </summary>
        /// <param name="text">Text to be cleaned for script tags</param>
        /// <returns>Clean text with no script tags.</returns>
        public static string RemoveScriptTags(string text)
        {
            text = Regex.Replace(text, "<script((.|\n)*?)</script>", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            text = Regex.Replace(text, "\"javascript:", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return text;
        }

        /// <summary>
        /// This method defuses script tags by replacing the opening tag bracket with &lt;.
        /// </summary>
        /// <param name="text">Text to be cleaned for script tags</param>
        /// <returns>Clean text with no script tags.</returns>
        public static string DefuseScriptTags(string text)
        {
            text = Regex.Replace(text, "<script((.|\n)*?)</script>", "&lt;script$1&lt;/script&gt;", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            text = Regex.Replace(text, "\"javascript:", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return text;
        }


        /// <summary>
        /// Removes characters that can be used for SQL injections.
        /// </summary>
        /// <param name="text">The text to remove the bad characters from</param>
        /// <returns>A bad SQL character clean string</returns>
        public static string RemoveMaliciousSQLCharacters(string text)
        {
            text = text.Replace("'", "''");
            return Regex.Replace(text, "--", " ", RegexOptions.Compiled | RegexOptions.Multiline);
        }

        /// <summary>
        /// This method cuts a string if it exceeds a given maximum length. Optionally, you can define
        /// a suffix (e.g.: "...") to be attached when the string is to long.
        /// </summary>
        /// <param name="text">The string to format</param>
        /// <param name="maxLength">The maximum length of the string</param>
        /// <param name="suffix">The text to attach if the word exceeds the maximum length.</param>
        /// <example>
        /// <![CDATA[
        ///     string text = "abcdefghijklmn"; 
        ///     
        ///     // ConversionHelper text to "abcd..."
        ///     text = text.Abbreviate(7, "...");
        /// ]]>
        /// </example>
        /// <returns></returns>
        public static string Abbreviate(string text, int maxLength, string suffix)
        {
            if (text.Length > maxLength)
            {
                if (!string.IsNullOrEmpty(suffix))
                {
                    return text.Remove(maxLength - suffix.Length) + suffix;
                }
                else
                {
                    return text.Remove(maxLength);
                }
            }
            return text;
        }
        /// <summary>
        /// This method cuts a string if it exceeds a given maximum length. Optionally, you can define
        /// a suffix (e.g.: "...") to be attached when the string is to long.
        /// </summary>
        /// <param name="text">The string to format</param>
        /// <param name="maxLength">The maximum length of the string</param>
        /// <returns></returns>
        public static string Abbreviate(string text, int maxLength)
        {
            return ConversionHelper.Abbreviate(text, maxLength, null);
        }
        /// <summary>
        /// This method cuts a string if it exceeds a given maximum length. Optionally, you can define
        /// a suffix (e.g.: "...") to be attached when the string is to long.
        /// </summary>
        /// <param name="text">The string to format</param>
        /// <param name="maxLength">The maximum length of the string</param>
        /// <param name="attachPoints">if true, "..." is attached when the string is too long.</param>
        /// <returns></returns>
        public static string Abbreviate(string text, int maxLength, bool attachPoints)
        {
            return ConversionHelper.Abbreviate(text, maxLength, attachPoints ? "&hellip;" : null);
        }

        /// <summary>
        /// ConversionHelpers a Dictionary to a debug string
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string ToDebugString<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TextFormat format)
        {
            StringBuilder sb = new StringBuilder();

            if (format == TextFormat.ASCII)
            {
                foreach (TKey key in dictionary.Keys)
                {
                    sb.Append(key.ToString() + " -> " + dictionary[key].ToString() + "\n");
                }
            }
            else
            {
                sb.Append("<table><tr><th>key</th><th>value</th></tr>");
                foreach (TKey key in dictionary.Keys)
                {
                    sb.Append("<tr><td>" + key.ToString() + "</td><td>" + dictionary[key].ToString() + "</td></tr>");
                }
                sb.Append("</table>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// This method returns an integer extracted from a querystring parameter. 
        /// </summary>
        /// <example>
        /// You can use this extension in the following way ->
        /// 
        /// int? myPostID = HttpContext.Current.Request.QueryString["postId"].ToQueryStringInteger();
        /// if (myPostID != null)
        /// {
        ///     // ... do something
        /// }
        /// 
        /// </example>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static int? ToQueryStringInteger(string text)
        {
            if (text != null)
            {
                int returnValue = 0;
                if (int.TryParse(text, out returnValue))
                    return returnValue;
            }
            return null;
        }
        #endregion

        #region collections
        /// <summary>
        /// Gets all the enum names of a specified enum type as a human readable string.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string GetDebugString(Type enumType, string separator)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string name in Enum.GetNames(enumType))
                sb.Append(name + separator);
            return sb.ToString().Trim().TrimEnd(separator.ToCharArray());
        }

        /// <summary>
        /// Gets all the collection's values as a human readable string.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string GetDebugString(ICollection obj, string separator)
        {
            StringBuilder sb = new StringBuilder();
            IEnumerator enumerator = obj.GetEnumerator();
            while (enumerator.MoveNext())
            {
                sb.Append(enumerator.Current.ToString() + separator);
            }
            return sb.ToString().Trim().TrimEnd(separator.ToCharArray());
        }
        #endregion

        #region exceptions
        /// <summary>
        /// Returns an exception with all its inner exceptions as a formatted string.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="e">The exception.</param>
        /// <returns></returns>
        public static string GetFormattedException(string message, Exception e)
        {
            return GetFormattedException(message, e, TextFormat.HTML);
        }
        /// <summary>
        /// Returns an exception with all its inner exceptions as a formatted string.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="e">The exception.</param>
        /// <param name="textFormat">The text format.</param>
        /// <returns></returns>
        public static string GetFormattedException(string message, Exception e, TextFormat textFormat)
        {
            if (e == null)
                return message;
            else
            {
                string lineBreak = string.Empty
                        , boldOpen = string.Empty
                        , boldClose = string.Empty;

                switch (textFormat)
                {
                    case TextFormat.ASCII:
                        lineBreak = Environment.NewLine;
                        boldOpen = boldClose = string.Empty;
                        break;

                    case TextFormat.HTML:
                        lineBreak = "<br/>";
                        boldOpen = "<strong>";
                        boldClose = "</strong>";
                        break;
                }

                if (!string.IsNullOrEmpty(message))
                    message += lineBreak + lineBreak + boldOpen + "Inner Exception: " + boldClose;
                else
                    message += boldOpen + "Exception: " + boldClose;

                message += HttpUtility.HtmlEncode(e.Message) + (e.StackTrace == null ? "" : (lineBreak + boldOpen + "StackTrace: " + boldClose + HttpUtility.HtmlEncode(e.StackTrace)));
                return GetFormattedException(message, e.InnerException, textFormat);
            }
        }
        #endregion

        #region cascading
        /// <summary>
        /// Cascades the properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record">The record.</param>
        /// <param name="parent">The parent.</param>
        public static void CascadeProperties<T>(T record, T parent)
        {
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();

            object parentValue = null;
            object[] attributes;
            bool isNull;

            foreach (PropertyInfo info in propertyInfos)
            {
                isNull = false;
                attributes = (object[])info.GetCustomAttributes(typeof(CascadableAttribute), false);
                if (attributes.Length > 0)
                {
                    try
                    {
                        // try to get the value of this property. This method will throw a TargetInvocationException if no 
                        // value was provided.
                        isNull = info.GetValue(record, null) == null;
                    }
                    catch (TargetInvocationException)
                    {
                        // our value was null, so assign the parent value
                        isNull = true;
                    }

                    if (isNull)
                    {
                        parentValue = null;
                        try { parentValue = info.GetValue(parent, null); }
                        catch (TargetInvocationException) { isNull = true; }

                        info.SetValue(record, parentValue, null);
                    }
                }
            }
        }
        #endregion

        #region extrainfo serializer
        /// <summary>
        /// Extracts the extra info.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        public static XElement ExtractExtraInfo<T>(T record)
        {
            return ExtractExtraInfo<T>(record, "r", true);
        }
        /// <summary>
        /// Extracts extra info properties from a given interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record">The record.</param>
        /// <param name="returnNullIfEmpty">if set to <c>true</c> [return null if empty].</param>
        /// <returns></returns>
        public static XElement ExtractExtraInfo<T>(T record, bool returnNullIfEmpty)
        {
            return ExtractExtraInfo<T>(record, "r", true);
        }
        /// <summary>
        /// Extracts extra info properties from a given interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record">The record.</param>
        /// <param name="rootName">Name of the root node.</param>
        /// <param name="returnNullIfEmpty">if set to <c>true</c> [return null if empty].</param>
        /// <returns></returns>
        public static XElement ExtractExtraInfo<T>(T record, string rootName, bool returnNullIfEmpty)
        {
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();

            XElement xElement = new XElement(rootName);
            object value = null;
            object[] attributes;
            bool isEmpty = true;

            foreach (PropertyInfo info in propertyInfos)
            {
                value = null;
                try
                {
                    // try to get the value of this property. This method will throw a TargetInvocationException if no 
                    // value was provided.
                    value = info.GetValue(record, null);
                }
                catch (TargetInvocationException) { }

                if (value != null)
                {
                    attributes = (object[])info.GetCustomAttributes(typeof(XmlExtraInfoAttribute), false);
                    if (attributes.Length > 0)
                    {
                        XmlExtraInfoAttribute xmlExtraInfoAttribute = ((XmlExtraInfoAttribute)attributes[0]);

                        if (string.IsNullOrEmpty(xmlExtraInfoAttribute.TagName))
                            xElement.SetElementValue(info.Name, value);
                        else
                            xElement.SetElementValue(xmlExtraInfoAttribute.TagName, value);

                        isEmpty = false;
                    }
                }
            }

            if (returnNullIfEmpty && isEmpty)
                return null;

            return xElement;
        }
        #endregion

        #region xml

        #region private methods
        private const string XML_LIST_ITERATOR_NODENAME = "i";
        private const string XML_DICT_KEY_ITERATOR_ATTRIBUTENAME = "k";


        private static string GetFormattedTypeName(Type type)
        {
            return type.FullName
                       .Remove(0, type.FullName.IndexOf("[[") + 2)
                       .Replace("]]", "");
        }
        private static object GetParsedObject(Type type, string value, object defaultValue)
        {
            bool b = false;
            return GetParsedObject(type, value, defaultValue, out b);
        }
        private static object GetParsedObject(Type type, string value, object defaultValue, out bool wasParseSuccessful)
        {
            wasParseSuccessful = true;
            string typename;
            if (type.FullName == "System.String")
            {
                return ((object)value);
            }
            else if (type.BaseType == typeof(ValueType))
            {
                #region value types

                #region get typename from lookup table
                if (_TypeNamesLookup.ContainsKey(type.FullName))
                {
                    typename = _TypeNamesLookup[type.FullName];
                }
                else
                {
                    lock (_TypeNamesLock)
                    {
                        if (!_TypeNamesLookup.ContainsKey(type.FullName))
                        {
                            typename = type.FullName.Replace("System.Nullable`1[[", "");
                            if (typename.Contains(','))
                                typename = typename.Remove(typename.IndexOf(','));

                            _TypeNamesLookup.Add(type.FullName, typename);
                        }
                        else
                        {
                            typename = _TypeNamesLookup[type.FullName];
                        }
                    }
                }
                #endregion

                switch (typename)
                {
                    case "System.Boolean":
                        bool b;

                        wasParseSuccessful = bool.TryParse(value, out b);
                        if (!wasParseSuccessful)
                        {
                            return ((object)defaultValue);
                        }
                        else
                            return (object)b;

                    case "System.Byte":
                        byte by;
                        wasParseSuccessful = byte.TryParse(value, out by);
                        if (!wasParseSuccessful)
                        {
                            return ((object)defaultValue);
                        }
                        else
                            return (object)by;

                    case "System.Char":
                        char ch;
                        wasParseSuccessful = char.TryParse(value, out ch);
                        if (!wasParseSuccessful)
                        {
                            return ((object)defaultValue);
                        }
                        else
                            return (object)ch;

                    case "System.DateTime":
                        DateTime dt;
                        wasParseSuccessful = DateTime.TryParse(value, out dt);
                        if (!wasParseSuccessful)
                        {
                            return ((object)defaultValue);
                        }
                        else
                            return (object)dt;

                    case "System.Decimal":
                        decimal d;
                        wasParseSuccessful = decimal.TryParse(value, out d);
                        if (!wasParseSuccessful)
                        {
                            return ((object)defaultValue);
                        }
                        else
                            return (object)d;


                    case "System.Double":
                        double db;
                        wasParseSuccessful = double.TryParse(value, out db);
                        if (!wasParseSuccessful)
                        {
                            return ((object)defaultValue);
                        }
                        else
                            return (object)db;


                    case "System.Single":
                        float f;
                        wasParseSuccessful = float.TryParse(value, out f);
                        if (!wasParseSuccessful)
                        {
                            return ((object)defaultValue);
                        }
                        else
                            return (object)f;

                    case "System.Guid":
                        try { return (object)new Guid(value); }
                        catch
                        {
                            wasParseSuccessful = false;
                            return ((object)defaultValue);
                        }

                    case "System.Int16":
                        short s;
                        wasParseSuccessful = short.TryParse(value, out s);
                        if (!wasParseSuccessful)
                        {
                            return ((object)defaultValue);
                        }
                        else
                            return (object)s;

                    case "System.Int32":
                        int i;
                        wasParseSuccessful = int.TryParse(value, out i);
                        if (!wasParseSuccessful)
                        {
                            return ((object)defaultValue);
                        }
                        else
                            return (object)i;

                    case "System.Int64":
                        long l;
                        wasParseSuccessful = long.TryParse(value, out l);
                        if (!wasParseSuccessful)
                        {
                            return ((object)defaultValue);
                        }
                        else
                            return (object)l;

                    case "System.TimeSpan":
                        TimeSpan ts;
                        wasParseSuccessful = TimeSpan.TryParse(value, out ts);
                        if (!wasParseSuccessful)
                        {
                            return ((object)defaultValue);
                        }
                        else
                            return (object)ts;

                    default:

                        // we might have a nullable enum, so check for this scenario
                        string formattedTypeName = GetFormattedTypeName(type);
                        Type innerType = Type.GetType(formattedTypeName);
                        if (innerType != null)
                        {
                            if (innerType.BaseType.ToString() == "System.Enum")
                            {
                                try { return Enum.Parse(innerType, value); }
                                catch (ArgumentException) { }
                            }
                        }
                        break;
                }
                #endregion
            }
            else
            {
                switch (type.BaseType.ToString())
                {
                    case "System.Enum":

                        try { return Enum.Parse(type, value); }
                        catch (ArgumentException)
                        {
                            wasParseSuccessful = false;
                            /* fall through if the element value is wrong */
                        }

                        return defaultValue;
                }
            }
            throw new FormatException("The generic type was not recognized.");
        }
        #endregion

        /// <summary>
        /// Encodes an XML value
        /// </summary>
        /// <param name="text">The text to encode.</param>
        /// <returns></returns>
        public static string XmlEncode(string text)
        {
            return HttpUtility.HtmlEncode(text);
        }

        /// <summary>
        /// Parses the X element node.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="container">The container.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static object ParseXElementNode(Type type, XElement container, XName name, object defaultValue)
        {
            XElement element = container.Element(name);
            if (element == null || element.Value == null)
            {
                container.SetElementValue(name, defaultValue);
                return ((object)defaultValue);
            }
            else
            {
                bool wasSuccess = false;
                object obj = GetParsedObject(type, element.Value, defaultValue, out wasSuccess);
                if (!wasSuccess)
                    container.SetElementValue(name, obj);
                return obj;
            }

            throw new FormatException("The generic type was not recognized.");
        }

        /// <summary>
        /// Sets the X element node lookup.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="name">The name.</param>
        /// <param name="obj">The obj.</param>
        public static void SetXElementNodeLookup(XElement element, XName name, IDictionary obj)
        {
            element.SetElementValue(name, null);
            if (obj != null)
            {
                XElement list = new XElement(name);
                IDictionaryEnumerator enumerator = obj.GetEnumerator();
                XElement value;
                while (enumerator.MoveNext())
                {
                    value = new XElement(XML_LIST_ITERATOR_NODENAME);
                    value.SetAttributeValue(XML_DICT_KEY_ITERATOR_ATTRIBUTENAME, enumerator.Key.ToString());
                    value.SetValue(enumerator.Value);
                    list.Add(value);
                }
                element.Add(list);
            }
        }

        /// <summary>
        /// Parses the X element node lookup.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container">The container.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T ParseXElementNodeLookup<T>(XElement container, XName name, T defaultValue)
            where T : IDictionary, new()
        {
            Type type = typeof(T);

            XElement element = container.Element(name);
            if (element == null || element.Value == null)
            {
                return defaultValue;
            }
            else
            {
                T returnList = new T();

                string typename = GetFormattedTypeName(type);
                int splitIndex = typename.IndexOf("],[");

                var x = typename.Remove(splitIndex);
                var y = typename.Remove(0, splitIndex + 3);

                Type[] innerTypes = new Type[2];
                innerTypes[0] = Type.GetType(typename.Remove(splitIndex));
                innerTypes[1] = Type.GetType(typename.Remove(0, splitIndex + 3));

                IEnumerable<XElement> elements = element.Elements();
                bool wasSuccess = false;
                XElement value;
                XAttribute attribute;
                for (int i = 0; i < elements.Count(); i++)
                {
                    value = elements.ElementAt(i);
                    attribute = value.Attribute(XML_DICT_KEY_ITERATOR_ATTRIBUTENAME);

                    object dictValue = GetParsedObject(innerTypes[1], value.Value, null, out wasSuccess);
                    if (wasSuccess)
                    {
                        object dictKey = GetParsedObject(innerTypes[0], attribute.Value, null, out wasSuccess);
                        if (wasSuccess)
                        {
                            returnList.Add(dictKey, dictValue);
                        }
                    }
                }
                return returnList;
            }
        }

        /// <summary>
        /// Sets the X element node list.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="name">The name.</param>
        /// <param name="obj">The obj.</param>
        public static void SetXElementNodeList(XElement element, XName name, IEnumerable obj)
        {
            element.SetElementValue(name, null);
            if (obj != null)
            {
                XElement list = new XElement(name);
                IEnumerator enumerator = obj.GetEnumerator();
                while (enumerator.MoveNext())
                    list.Add(new XElement(XML_LIST_ITERATOR_NODENAME, enumerator.Current));
                element.Add(list);
            }
        }

        /// <summary>
        /// Parses the X element node list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container">The container.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T ParseXElementNodeList<T>(XElement container, XName name, T defaultValue)
            where T : IList, new()
        {
            Type type = typeof(T);

            XElement element = container.Element(name);
            if (element == null || element.Value == null)
            {
                return defaultValue;
            }
            else
            {
                T returnList = new T();

                string typename = GetFormattedTypeName(type);
                Type innerType = Type.GetType(typename);

                IEnumerable<XElement> elements = element.Elements();
                bool wasSuccess = false;
                object o;
                for (int i = 0; i < elements.Count(); i++)
                {
                    o = GetParsedObject(innerType, elements.ElementAt(i).Value, null, out wasSuccess);
                    if (wasSuccess)
                        returnList.Add(o);
                }
                return returnList;
            }
        }

        /// <summary>
        /// Parses the X element node to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container">The parent XElement that contains the tag.</param>
        /// <param name="name">The name of the tag.</param>
        /// <param name="defaultValue">The default value if the tag can't be found at the default element (can be null with
        /// nullable objects).</param>
        /// <returns>The object parsed from the tag, the default value if the tag was not found or invalid.</returns>
        /// <exception cref="System.FormatException">Throws a System.FormatException if the specified generic type could not be parsed.</exception>
        public static T ParseXElementNode<T>(XElement container, XName name, T defaultValue)
        {
            return (T)ParseXElementNode(typeof(T), container, name, defaultValue);
        }

        /// <summary>
        /// Generates the X element.
        /// </summary>
        /// <param name="rootTagName">Name of the root tag.</param>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        public static XElement GenerateXElement(string rootTagName, List<KeyValuePair<string, object>> objects)
        {
            XElement element = new XElement(rootTagName);

            XElement tag;
            foreach (KeyValuePair<string, object> obj in objects)
            {
                tag = new XElement(obj.Key);
                tag.SetValue(obj.Value.ToString());
                element.Add(tag);
            }

            return element;
        }

        #endregion

        #region general
        /// <summary>
        /// Gets all the enum names of a specified enum type as a human readable string.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string GetCollectionString(Type enumType, string separator)
        {
            if (enumType == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (string name in Enum.GetNames(enumType))
                sb.Append(name + separator);
            return sb.ToString().Trim().TrimEnd(separator.ToCharArray());
        }

        /// <summary>
        /// Gets all the enumerable object's values as a human readable string.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string GetCollectionString(IEnumerable obj, string separator)
        {
            if (obj == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            IEnumerator enumerator = obj.GetEnumerator();
            while (enumerator.MoveNext())
            {
                sb.Append(enumerator.Current.ToString() + separator);
            }
            return sb.ToString().Trim().TrimEnd(separator.ToCharArray());
        }
        /// <summary>
        /// Gets all the collection's values as a human readable string.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string GetCollectionString(ICollection obj, string separator)
        {
            if (obj == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            IEnumerator enumerator = obj.GetEnumerator();
            while (enumerator.MoveNext())
            {
                sb.Append(enumerator.Current.ToString() + separator);
            }
            return sb.ToString().Trim().TrimEnd(separator.ToCharArray());
        }

        /// <summary>
        /// Gets the collection string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string GetCollectionString<T>(IEnumerable<T> obj, string separator, string format)
            where T : IFormattable
        {
            if (obj == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            IEnumerator<T> enumerator = obj.GetEnumerator();
            while (enumerator.MoveNext())
            {
                sb.Append(enumerator.Current.ToString(format, null) + separator);
            }
            return sb.ToString().Trim().TrimEnd(separator.ToCharArray());
        }

        #endregion

        #region dictionaries

        /// <summary>
        /// Gets a generic field from a specified dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public static T GetField<T>(IDictionary<string, object> dictionary, string fieldName)
        {
            return GetField<T>(dictionary, fieldName, false, default(T));
        }

        /// <summary>
        /// Gets a generic field from a specified dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="upperCase">if set to <c>true</c> [upper case].</param>
        /// <returns></returns>
        public static T GetField<T>(IDictionary<string, object> dictionary, string fieldName, bool upperCase)
        {
            return GetField<T>(dictionary, fieldName, upperCase, default(T));
        }

        /// <summary>
        /// Gets a generic field from a specified dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="upperCase">if set to <c>true</c> [upper case].</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T GetField<T>(IDictionary<string, object> dictionary, string fieldName, bool upperCase, T defaultValue)
        {
            if (fieldName == null)
                return default(T);

            Type type = typeof(T);

            object rawField;
            if (dictionary.TryGetValue(fieldName, out rawField))
            {
                if (rawField == null)
                {
                    return defaultValue;
                }
                else
                {
                    if (rawField.GetType() == type)
                        return (T)rawField;

                    if (type.FullName == "System.String")
                    {
                        return (T)((object)(upperCase ? rawField.ToString().ToUpperInvariant() : rawField.ToString()));
                    }
                    else
                    {
                        string typename;

                        #region get typename from lookup table
                        if (_TypeNamesLookup.ContainsKey(type.FullName))
                        {
                            typename = _TypeNamesLookup[type.FullName];
                        }
                        else
                        {
                            lock (_TypeNamesLock)
                            {
                                if (!_TypeNamesLookup.ContainsKey(type.FullName))
                                {
                                    typename = type.FullName.Replace("System.Nullable`1[[", "");
                                    if (typename.Contains(','))
                                        typename = typename.Remove(typename.IndexOf(','));

                                    _TypeNamesLookup.Add(type.FullName, typename);
                                }
                                else
                                {
                                    typename = _TypeNamesLookup[type.FullName];
                                }
                            }
                        }
                        #endregion

                        switch (typename)
                        {
                            case "System.Boolean":
                                #region bool
                                if (rawField is string)
                                {
                                    bool b;
                                    if (bool.TryParse((string)rawField, out b))
                                        return (T)(object)b;
                                }

                                return defaultValue;
                                #endregion

                            case "System.Byte":
                                #region byte
                                if (rawField is string)
                                {
                                    byte b;
                                    if (byte.TryParse((string)rawField, out b))
                                        return (T)(object)b;
                                }
                                else
                                {
                                    try { return (T)(object)Convert.ToByte(rawField); }
                                    catch (OverflowException) { }
                                }

                                return defaultValue;
                                #endregion

                            case "System.Char":
                                #region char
                                if (rawField is string)
                                {
                                    char c;
                                    if (char.TryParse((string)rawField, out c))
                                        return (T)(object)c;
                                }
                                else
                                {
                                    try { return (T)(object)Convert.ToChar(rawField); }
                                    catch (OverflowException) { }
                                }

                                return defaultValue;
                                #endregion

                            case "System.DateTime":
                                #region datetime
                                if (rawField is string)
                                {
                                    DateTime dt;
                                    string rf = (string)rawField;

                                    if (DateTime.TryParseExact(rf, MagicFields.RENDEZVOUS_DATEFORMAT, null, DateTimeStyles.None, out dt)
                                        || DateTime.TryParseExact(rf, MagicFields.RENDEZVOUS_DATEFORMAT_SHORT, null, DateTimeStyles.None, out dt)
                                        || DateTime.TryParse(rf, out dt))
                                        return (T)(object)(dt);
                                }

                                return defaultValue;
                                #endregion

                            case "System.Decimal":
                                #region decimal
                                if (rawField is string)
                                {
                                    decimal d;
                                    if (decimal.TryParse((string)rawField, out d))
                                        return (T)(object)d;
                                }
                                else
                                {
                                    try { return (T)(object)Convert.ToDecimal(rawField); }
                                    catch (OverflowException) { }
                                }

                                return defaultValue;
                                #endregion

                            case "System.Double":
                                #region double
                                if (rawField is string)
                                {
                                    double db;
                                    if (double.TryParse((string)rawField, out db))
                                        return (T)(object)db;
                                }
                                else
                                {
                                    try { return (T)(object)Convert.ToDouble(rawField); }
                                    catch (OverflowException) { }
                                }

                                return defaultValue;
                                #endregion

                            case "System.Single":
                                #region float
                                if (rawField is string)
                                {
                                    float f;
                                    if (float.TryParse((string)rawField, out f))
                                        return (T)(object)f;
                                }
                                else
                                {
                                    try { return (T)(object)Convert.ToSingle(rawField); }
                                    catch (OverflowException) { }
                                }

                                return defaultValue;
                                #endregion

                            case "System.Guid":
                                #region guid
                                if (rawField is string)
                                {
                                    try { return (T)(object)new Guid((string)rawField); }
                                    catch { }
                                }

                                return defaultValue;
                                #endregion

                            case "System.Int16":
                                #region short
                                if (rawField is string)
                                {
                                    short s;
                                    if (short.TryParse((string)rawField, out s))
                                        return (T)(object)s;
                                }
                                else
                                {
                                    try { return (T)(object)Convert.ToInt16(rawField); }
                                    catch (OverflowException) { }
                                }

                                return defaultValue;
                                #endregion

                            case "System.Int32":
                                #region int
                                if (rawField is string)
                                {
                                    int i;
                                    if (int.TryParse((string)rawField, out i))
                                        return (T)(object)i;
                                }
                                else
                                {
                                    try { return (T)(object)Convert.ToInt32(rawField); }
                                    catch (OverflowException) { }
                                }

                                return defaultValue;
                                #endregion

                            case "System.Int64":
                                #region long
                                if (rawField is string)
                                {
                                    long l;
                                    if (long.TryParse((string)rawField, out l))
                                        return (T)(object)l;
                                }
                                else
                                {
                                    try { return (T)(object)Convert.ToInt64(rawField); }
                                    catch (OverflowException) { }
                                }

                                return defaultValue;
                                #endregion

                            default: throw new NotImplementedException("Type " + typename + " is not supported by this method");
                        }
                    }
                }
            }

            return default(T);
        }


        /// <summary>
        /// Gets a generic field from a specified dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public static T GetField<T>(IDictionary<string, string> dictionary, string fieldName)
        {
            return GetField<T>(dictionary, fieldName, false, default(T));
        }

        /// <summary>
        /// Gets a generic field from a specified dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="upperCase">if set to <c>true</c> [upper case].</param>
        /// <returns></returns>
        public static T GetField<T>(IDictionary<string, string> dictionary, string fieldName, bool upperCase)
        {
            return GetField<T>(dictionary, fieldName, upperCase, default(T));
        }

        /// <summary>
        /// Gets a generic field from a specified dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="upperCase">if set to <c>true</c> [upper case].</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T GetField<T>(IDictionary<string, string> dictionary, string fieldName, bool upperCase, T defaultValue)
        {
            if (fieldName == null)
                return default(T);

            Type type = typeof(T);

            string rawField;
            if (dictionary.TryGetValue(fieldName, out rawField))
            {
                if (rawField == null)
                {
                    return defaultValue;
                }
                else
                {
                    if (rawField.GetType() == type)
                        return (T)(object)rawField;

                    if (type.FullName == "System.String")
                    {
                        return (T)((object)(upperCase ? rawField.ToString().ToUpperInvariant() : rawField.ToString()));
                    }
                    else
                    {
                        string typename;

                        #region get typename from lookup table
                        if (_TypeNamesLookup.ContainsKey(type.FullName))
                        {
                            typename = _TypeNamesLookup[type.FullName];
                        }
                        else
                        {
                            lock (_TypeNamesLock)
                            {
                                if (!_TypeNamesLookup.ContainsKey(type.FullName))
                                {
                                    typename = type.FullName.Replace("System.Nullable`1[[", "");
                                    if (typename.Contains(','))
                                        typename = typename.Remove(typename.IndexOf(','));

                                    _TypeNamesLookup.Add(type.FullName, typename);
                                }
                                else
                                {
                                    typename = _TypeNamesLookup[type.FullName];
                                }
                            }
                        }
                        #endregion

                        switch (typename)
                        {
                            case "System.Boolean":
                                #region bool
                                bool b;
                                if (bool.TryParse((string)rawField, out b))
                                    return (T)(object)b;

                                return defaultValue;
                                #endregion

                            case "System.Byte":
                                #region byte
                                byte by;
                                if (byte.TryParse((string)rawField, out by))
                                    return (T)(object)by;

                                return defaultValue;
                                #endregion

                            case "System.Char":
                                #region char
                                char c;
                                if (char.TryParse((string)rawField, out c))
                                    return (T)(object)c;

                                return defaultValue;
                                #endregion

                            case "System.DateTime":
                                #region datetime
                                DateTime dt;
                                string rf = (string)rawField;

                                if (DateTime.TryParseExact(rf, MagicFields.RENDEZVOUS_DATEFORMAT, null, DateTimeStyles.None, out dt)
                                    || DateTime.TryParseExact(rf, MagicFields.RENDEZVOUS_DATEFORMAT_SHORT, null, DateTimeStyles.None, out dt)
                                    || DateTime.TryParse(rf, out dt))
                                    return (T)(object)(dt);

                                return defaultValue;
                                #endregion

                            case "System.Decimal":
                                #region decimal
                                decimal d;
                                if (decimal.TryParse((string)rawField, out d))
                                    return (T)(object)d;

                                return defaultValue;
                                #endregion

                            case "System.Double":
                                #region double
                                double db;
                                if (double.TryParse((string)rawField, out db))
                                    return (T)(object)db;

                                return defaultValue;
                                #endregion

                            case "System.Single":
                                #region float
                                float f;
                                if (float.TryParse((string)rawField, out f))
                                    return (T)(object)f;

                                return defaultValue;
                                #endregion

                            case "System.Guid":
                                #region guid
                                try { return (T)(object)new Guid((string)rawField); }
                                catch { }

                                return defaultValue;
                                #endregion

                            case "System.Int16":
                                #region short
                                short s;
                                if (short.TryParse((string)rawField, out s))
                                    return (T)(object)s;

                                return defaultValue;
                                #endregion

                            case "System.Int32":
                                #region int
                                int i;
                                if (int.TryParse((string)rawField, out i))
                                    return (T)(object)i;

                                return defaultValue;
                                #endregion

                            case "System.Int64":
                                #region long
                                long l;
                                if (long.TryParse((string)rawField, out l))
                                    return (T)(object)l;

                                return defaultValue;
                                #endregion

                            default: throw new NotImplementedException("Type " + typename + " is not supported by this method");
                        }
                    }
                }
            }

            return default(T);
        }

        #endregion

        #region parsing

        /// <summary>
        /// Tries the parse.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rawField">The raw field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool TryParse<T>(object rawField, out T value)
        {
            if (rawField == null)
            {
                value = default(T);
                return true;
            }

            Type type = typeof(T);
            if (rawField.GetType() == type)
            {
                value = (T)(object)rawField;
                return true;
            }

            if (type.FullName == "System.String")
            {
                value = (T)((object)(rawField.ToString()));
                return true;
            }
            else
            {
                string typename;

                #region get typename from lookup table
                if (_TypeNamesLookup.ContainsKey(type.FullName))
                {
                    typename = _TypeNamesLookup[type.FullName];
                }
                else
                {
                    lock (_TypeNamesLock)
                    {
                        if (!_TypeNamesLookup.ContainsKey(type.FullName))
                        {
                            typename = type.FullName.Replace("System.Nullable`1[[", "");
                            if (typename.Contains(','))
                                typename = typename.Remove(typename.IndexOf(','));

                            _TypeNamesLookup.Add(type.FullName, typename);
                        }
                        else
                        {
                            typename = _TypeNamesLookup[type.FullName];
                        }
                    }
                }
                #endregion

                switch (typename)
                {
                    case "System.Boolean":
                        #region bool
                        bool b;
                        if (bool.TryParse(rawField.ToString(), out b))
                        {
                            value = (T)(object)b;
                            return true;
                        }
                        else
                        {
                            value = default(T);
                            return false;
                        }
                        #endregion

                    case "System.Byte":
                        #region byte
                        byte by;
                        if (byte.TryParse(rawField.ToString(), out by))
                        {
                            value = (T)(object)by;
                            return true;
                        }
                        else
                        {
                            value = default(T);
                            return false;
                        }
                        #endregion

                    case "System.Char":
                        #region char
                        char c;
                        if (char.TryParse(rawField.ToString(), out c))
                        {
                            value = (T)(object)c;
                            return true;
                        }
                        else
                        {
                            value = default(T);
                            return false;
                        }
                        #endregion

                    case "System.DateTime":
                        #region datetime
                        DateTime dt;
                        string rf = rawField.ToString();

                        if (DateTime.TryParseExact(rf, MagicFields.RENDEZVOUS_DATEFORMAT, null, DateTimeStyles.None, out dt)
                            || DateTime.TryParseExact(rf, MagicFields.RENDEZVOUS_DATEFORMAT_SHORT, null, DateTimeStyles.None, out dt)
                            || DateTime.TryParse(rf, out dt))
                        {
                            value = (T)(object)(dt);
                            return true;
                        }
                        else
                        {
                            value = default(T);
                            return false;
                        }
                        #endregion

                    case "System.Decimal":
                        #region decimal
                        decimal d;
                        if (decimal.TryParse(rawField.ToString(), out d))
                        {
                            value = (T)(object)d;
                            return true;
                        }
                        else
                        {
                            value = default(T);
                            return false;
                        }
                        #endregion

                    case "System.Double":
                        #region double
                        double db;
                        if (double.TryParse(rawField.ToString(), out db))
                        {
                            value = (T)(object)db;
                            return true;
                        }
                        else
                        {
                            value = default(T);
                            return false;
                        }
                        #endregion

                    case "System.Single":
                        #region float
                        float f;
                        if (float.TryParse(rawField.ToString(), out f))
                        {
                            value = (T)(object)f;
                            return true;
                        }
                        else
                        {
                            value = default(T);
                            return false;
                        }
                        #endregion

                    case "System.Guid":
                        #region guid
                        try
                        {
                            value = (T)(object)new Guid(rawField.ToString());
                            return true;
                        }
                        catch { }

                        value = default(T);
                        return false;

                        #endregion

                    case "System.Int16":
                        #region short
                        short s;
                        if (short.TryParse(rawField.ToString(), out s))
                        {
                            value = (T)(object)s;
                            return true;
                        }
                        else
                        {
                            value = default(T);
                            return false;
                        }
                        #endregion

                    case "System.Int32":
                        #region int
                        int i;
                        if (int.TryParse(rawField.ToString(), out i))
                        {
                            value = (T)(object)i;
                            return true;
                        }
                        else
                        {
                            value = default(T);
                            return false;
                        }
                        #endregion

                    case "System.Int64":
                        #region long
                        long l;
                        if (long.TryParse(rawField.ToString(), out l))
                        {
                            value = (T)(object)l;
                            return true;
                        }
                        else
                        {
                            value = default(T);
                            return false;
                        }
                        #endregion

                    default: throw new NotImplementedException("Type " + typename + " is not supported by this method");
                }
            }
        }

        /// <summary>
        /// This method takes a string as input and tries to extract a value from a specified array index in the string after splitting it into
        /// an array using a given delimiter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text">The text.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if successful, otherwise false</returns>
        public static bool TryGetField<T>(string text, char delimiter, int index, out T value)
        {
            if (string.IsNullOrEmpty(text))
            {
                value = default(T);
                return false;
            }

            return TryGetField<T>(text.Split(delimiter), index, out value);
        }

        /// <summary>
        /// This method takes a string array as input and tries to extract a value from the specified index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="splitted">The splitted.</param>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if successful, otherwise false</returns>
        public static bool TryGetField<T>(string[] splitted, int index, out T value)
        {
            if (splitted == null)
            {
                value = default(T);
                return false;
            }

            return TryGetField<T>(splitted, splitted.Length, index, out value);

        }

        /// <summary>
        /// This method takes a string array as input and tries to extract a value from the specified index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="splitted">The splitted.</param>
        /// <param name="splittedLength">Length of the array. Use this parameter if you already calculated the length of the array</param>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool TryGetField<T>(string[] splitted, int splittedLength, int index, out T value)
        {
            if (splitted == null
                || splittedLength < index)
            {
                value = default(T);
                return false;
            }
            return TryParse<T>(splitted[index], out value);
        }
        #endregion
    }
}
