using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data;

namespace CommonTools.Extensions
{
  /// <summary>
  /// 
  /// </summary>
  public static class Extensions
  {
    #region Lists

    /// <summary>
    /// Nexts the random.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <param name="random">The random.</param>
    /// <returns></returns>
    public static T NextRandom<T>(this IEnumerable<T> source, Random random)
    {
      int count = source.Count();
      if (count > 0)
      {
        return source.Skip(random.Next(0, count)).Take(1).SingleOrDefault();
      }
      return default(T);
    }

    /// <summary>
    /// Randomizes the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputList">The input list.</param>
    /// <param name="random">The random.</param>
    /// <returns></returns>
    public static T RandomizeList<T>(this T inputList, Random random) where T : IList, new()
    {
      T randomList = new T();
      if (inputList.Count == 0)
        return randomList;

      int randomIndex = 0;
      while (inputList.Count > 0)
      {
        randomIndex = random.Next(0, inputList.Count); //Choose a random object in the list
        randomList.Add(inputList[randomIndex]); //add it to the new, random list<
        inputList.RemoveAt(randomIndex); //remove to avoid duplicates
      }

      return randomList; //return the new random list
    }
    #endregion

    #region Sytem.DateTime
    /// <summary>
    /// Converts this datetime object to a canonical formated string that
    /// can be recognized by SQL. ()Format: MM.DD.YYYY hh:mm:ss.ms
    /// </summary>
    /// <param name="dt">The datetime object</param>
    /// <returns></returns>
    public static string ToCanonical(this DateTime dt)
    {
      string returnValue = string.Format("{0}.{1}.{2} {3}:{4}:{5}.{6}",
          dt.Day, dt.Month, dt.Year, dt.Hour, dt.Minute, dt.Second
          , (dt.Millisecond < 100 ? "0" : "") + (dt.Millisecond < 10 ? "0" : "") + dt.Millisecond);

      // Format: MM.DD.YYYY hh:mm:ss.ms
      return returnValue;
    }

    /// <summary>
    /// Converts this object to a SqlSmallDateTime (cuts off the seconds and milliseconds)
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns></returns>
    public static DateTime ToSqlSmallDateTime(this DateTime dateTime)
    {
      return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0);
    }

    /// <summary>
    /// Rounds the specified date time.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns></returns>
    public static DateTime Round(this DateTime dateTime)
    {
      return Round(dateTime, DateTimeRoundingAccuracy.Second, MidpointRounding.AwayFromZero);
    }
    /// <summary>
    /// Rounds the specified date time.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <param name="dateTimeRoundingAccuracy">The date time rounding accuracy.</param>
    /// <returns></returns>
    public static DateTime Round(this DateTime dateTime, DateTimeRoundingAccuracy dateTimeRoundingAccuracy)
    {
      return Round(dateTime, dateTimeRoundingAccuracy, MidpointRounding.AwayFromZero);
    }
    /// <summary>
    /// Rounds the specified date time.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <param name="dateTimeRoundingAccuracy">The date time rounding accuracy.</param>
    /// <param name="midpointRounding">The midpoint rounding.</param>
    /// <returns></returns>
    public static DateTime Round(this DateTime dateTime, DateTimeRoundingAccuracy dateTimeRoundingAccuracy, MidpointRounding midpointRounding)
    {
      switch (midpointRounding)
      {
        case MidpointRounding.AwayFromZero:

          switch (dateTimeRoundingAccuracy)
          {
            case DateTimeRoundingAccuracy.Millisecond: return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, 0, dateTime.Kind);
            case DateTimeRoundingAccuracy.Second: return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0, dateTime.Kind);
            case DateTimeRoundingAccuracy.Minute: return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0, 0, dateTime.Kind);
            case DateTimeRoundingAccuracy.Hour: return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0, dateTime.Kind);
            case DateTimeRoundingAccuracy.Day: return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0, dateTime.Kind);
            case DateTimeRoundingAccuracy.Month: return new DateTime(dateTime.Year, 1, 1, 0, 0, 0, 0, dateTime.Kind);

            default: throw new NotSupportedException();
          }

        case MidpointRounding.ToEven:

          int s, m, h, d, mm, daysInMonth;
          double ms = 0;

          switch (dateTimeRoundingAccuracy)
          {
            case DateTimeRoundingAccuracy.Millisecond:
              #region Millisecond

              return dateTime.AddMilliseconds(
                  dateTime.Millisecond >= 500 ? 1000 - dateTime.Millisecond
                                              : dateTime.Millisecond * -1);
              #endregion

            case DateTimeRoundingAccuracy.Second:
              #region Second
              if (dateTime.Millisecond >= 500)
              {
                s = dateTime.Second + 1;
                ms += 1000 - dateTime.Millisecond;
              }
              else
              {
                s = dateTime.Second;
                ms -= dateTime.Millisecond;
              }

              if (s >= 30 && s < 60)
              {
                ms += 1000;
              }

              return dateTime.AddMilliseconds(ms);
              #endregion

            case DateTimeRoundingAccuracy.Minute:
              #region Minute
              if (dateTime.Millisecond >= 500)
              {
                s = dateTime.Second + 1;
                ms += 1000 - dateTime.Millisecond;
              }
              else
              {
                s = dateTime.Second;
                ms -= dateTime.Millisecond;
              }

              if (s >= 60)
              {
                m = dateTime.Minute + 1;
              }
              else if (s >= 30)
              {
                m = dateTime.Minute + 1;
                ms += 1000;
              }
              else
                m = dateTime.Minute;

              if (m >= 30 && m < 60)
              {
                ms += 60000;
              }

              return dateTime.AddMilliseconds(ms);
              #endregion

            case DateTimeRoundingAccuracy.Hour:
              #region Hour
              if (dateTime.Millisecond >= 500)
              {
                s = dateTime.Second + 1;
                ms += 1000 - dateTime.Millisecond;
              }
              else
              {
                s = dateTime.Second;
                ms -= dateTime.Millisecond;
              }

              if (s >= 60)
              {
                m = dateTime.Minute + 1;
              }
              else if (s >= 30)
              {
                m = dateTime.Minute + 1;
                ms += 1000;
              }
              else
                m = dateTime.Minute;

              if (m >= 60)
              {
                h = dateTime.Hour + 1;
              }
              else if (m >= 30)
              {
                h = dateTime.Hour + 1;
                ms += 60000;
              }
              else
                h = dateTime.Hour;

              if (h >= 12 && h < 24)
              {
                ms += 3600000;
              }

              return dateTime.AddMilliseconds(ms);
              #endregion

            case DateTimeRoundingAccuracy.Day:
              #region Day

              if (dateTime.Millisecond >= 500)
              {
                s = dateTime.Second + 1;
                ms += 1000 - dateTime.Millisecond;
              }
              else
              {
                s = dateTime.Second;
                ms -= dateTime.Millisecond;
              }

              if (s >= 60)
              {
                m = dateTime.Minute + 1;
              }
              else if (s >= 30)
              {
                m = dateTime.Minute + 1;
                ms += 1000;
              }
              else
                m = dateTime.Minute;

              if (m >= 60)
              {
                h = dateTime.Hour + 1;
              }
              else if (m >= 30)
              {
                h = dateTime.Hour + 1;
                ms += 60000;
              }
              else
                h = dateTime.Hour;

              if (h >= 24)
              {
                d = dateTime.Day + 1;
              }
              else if (h >= 12)
              {
                d = dateTime.Day + 1;
                ms += 3600000;
              }
              else
                d = dateTime.Day;

              daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
              if (d >= daysInMonth / 2 && d < daysInMonth)
              {
                ms += (daysInMonth - d) * 86400000;
              }

              return dateTime.AddMilliseconds(ms);
              #endregion

            case DateTimeRoundingAccuracy.Month:
              #region month
              if (dateTime.Millisecond >= 500)
              {
                s = dateTime.Second + 1;
              }
              else
              {
                s = dateTime.Second;
                ms -= dateTime.Millisecond;
              }

              if (s >= 60)
              {
                m = dateTime.Minute + 1;
              }
              else if (s >= 30)
              {
                m = dateTime.Minute + 1;
                ms += 1000;
              }
              else
                m = dateTime.Minute;

              if (m >= 60)
              {
                h = dateTime.Hour + 1;
              }
              else if (m >= 30)
              {
                h = dateTime.Hour + 1;
                ms += 60000;
              }
              else
                h = dateTime.Hour;

              if (h >= 24)
              {
                d = dateTime.Day + 1;
              }
              else if (h >= 12)
              {
                d = dateTime.Day + 1;
                ms += 3600000;
              }
              else
                d = dateTime.Day;

              daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
              if (d >= daysInMonth / 2)
              {
                mm = dateTime.Month + 1;
              }
              else
                mm = dateTime.Month;

              if (mm >= 6)
                return new DateTime(dateTime.Year + 1, 1, 1, 0, 0, 0, 0, dateTime.Kind);
              else
                return new DateTime(dateTime.Year, 1, 1, 0, 0, 0, 0, dateTime.Kind);
              #endregion

            default: throw new NotSupportedException();
          }

        default: throw new NotSupportedException();
      }
    }
    #endregion

    #region System.String
    /// <summary>
    /// Gets the quoted search keywords.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    public static List<string> GetQuotedSearchKeywords(this string text)
    {
      List<string> quotedSections = new List<string>();
      List<string> unquotedSections = new List<string>();

      string[] splitted = text.Split('"');

      if (text.IndexOf(@"""") >= 0)
      {
        if (!text.StartsWith(@""""))
        {
          // we don't start with a quote, so add it immediately
          unquotedSections.Add(text.Remove(text.IndexOf(@"""")));
        }
        bool flag = false;
        for (int i = 1; i < splitted.Length; i++)
        {
          if (i + 1 < splitted.Length && !flag)
            quotedSections.Add(splitted[i].Trim());
          else
            unquotedSections.Add(splitted[i].Trim());

          flag = !flag;
        }
      }
      else
      {
        unquotedSections.Add(text);
      }

      List<string> returnList = new List<string>();

      foreach (string junk in unquotedSections)
      {
        foreach (string splittedJunk in junk.Split(' '))
        {
          if (!string.IsNullOrEmpty(splittedJunk))
            returnList.Add(splittedJunk);
        }
      }
      foreach (string junk in quotedSections)
      {
        if (!string.IsNullOrEmpty(junk))
          returnList.Add(junk);
      }

      return returnList;
    }

    /// <summary>
    /// Formats a given text with some specified options.
    /// </summary>
    /// <param name="text">The text to format</param>
    /// <param name="options">The options to use in order to format the text.</param>
    /// <returns>A with the specified options formated string.</returns>
    public static string Format(this string text, params TextFormatOptions[] options)
    {
      foreach (TextFormatOptions option in options)
      {
        switch (option)
        {
          case TextFormatOptions.RemoveAllTags:
            text = text.RemoveTags();
            break;

          case TextFormatOptions.RemoveBadSQLCharacters:
            text = text.RemoveMaliciousSQLCharacters();
            break;

          case TextFormatOptions.RemoveBadTags:
            text = text.RemoveMaliciousTags();
            break;

          case TextFormatOptions.RemoveLineBreaks:
            text = text.RemoveLineBreaks();
            break;

          case TextFormatOptions.RemoveScriptTags:
            text = text.RemoveScriptTags();
            break;

          case TextFormatOptions.SafeQuerystringParameter:
            text = text.UrlEncode();
            break;

          case TextFormatOptions.DefuseScriptTags:
            text = text.DefuseScriptTags();
            break;
        }
      }

      return text;
    }

    /// <summary>
    /// Gets a string with a lower-case initial letter, e.g.: ThisIsMyString -> thisIsMyString
    /// </summary>
    /// <param name="text">The string to convert</param>
    /// <returns>a string with a lower-case initial letter</returns>
    public static string ToLoweredInitalString(this string text)
    {
      if (string.IsNullOrEmpty(text))
        return string.Empty;

      char[] returnValue = text.ToCharArray();
      returnValue[0] = Char.ToLower(text[0]);
      return new String(returnValue);
    }

    /// <summary>
    /// Converts the given string to upper case only words, e.g.: this is My STRING -> This Is My String
    /// </summary>
    /// <param name="text">The string to convert</param>
    /// <returns>to upper case only words converted string</returns>
    public static string ToUpperCaseWordsString(this string text)
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
    public static string ToQuerystringParameter(this string text, string id)
    {
      return id + "=" + text.UrlEncode();
    }

    /// <summary>
    /// Transforms a given string to its querystring-safe representation.
    /// </summary>
    /// <param name="text">The string to convert</param>
    /// <returns>The querystring-safe representation of a given string</returns>
    public static string UrlEncode(this string text)
    {
      return HttpUtility.UrlEncode(text);
    }
    /// <summary>
    /// Converts a string that has been encoded for transmission in a URL into a
    /// decoded string.
    /// </summary>
    /// <param name="text">The string to convert</param>
    /// <returns>The querystring-safe representation of a given string</returns>
    public static string UrlDecode(this string text)
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
    public static string SplitLongWords(this string text, string splitter, int maxWordLength)
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
    public static string ToFriendlyMailAddress(this string mailAddress, string friendlyName)
    {
      return "\"" + friendlyName + "\" <" + mailAddress + ">";
    }

    /// <summary>
    /// Converts all returns ( \r\n ) from an asp:TextBox to HTML &lt;br /&gt;. Additionally, words that
    /// contain more characters than defined in the maxWordLength parameter get splitted by a blank
    /// character.
    /// </summary>
    /// <param name="text">The text to convert</param>
    /// <param name="maxWordLength">The maximum length of a word before it gets splitted by a blank character</param>
    /// <param name="removeBadTags">Set to true to also strip out invalid tags.</param>
    /// <returns></returns>
    public static string ConvertTextBoxToHtml(this string text, int maxWordLength, bool removeBadTags)
    {
      StringBuilder sb = new StringBuilder();
      if (maxWordLength > 0)
      {
        string x = "\r\n";
        char[] cutters = x.ToCharArray();
        string[] paragraph = text.Split(cutters);

        foreach (string para in paragraph)
        {
          if (!string.IsNullOrEmpty(para.Trim()))
          {
            string[] words = para.Trim().Split(' ');

            foreach (string word in words)
            {
              if (word.Length > maxWordLength)
              {
                string tempWord = word;
                while (tempWord.Length > maxWordLength)
                {
                  sb.Append(tempWord.Substring(0, maxWordLength - 1) + " ");
                  tempWord = tempWord.Remove(0, maxWordLength - 1);
                }
                sb.Append(tempWord);
              }
              else
              {
                sb.Append(word);
              }
              sb.Append(" ");
            }
            sb = sb.Remove(sb.Length - 1, 1);
            sb.Append("<br/>");
          }
        }
      }
      else
      {
        sb.Append(text);
      }

      string returnString = sb.ToString();
      if (returnString.EndsWith("<br/>"))
        returnString = returnString.Remove(returnString.Length - 5, 5);

      if (removeBadTags)
      {
        returnString = returnString.RemoveMaliciousTags();
      }

      return returnString;
    }
    /// <summary>
    /// Converts all returns ( \r\n ) from an asp:TextBox to HTML &lt;br /&gt;.
    /// </summary>
    /// <param name="text">The text to convert</param>
    /// <returns></returns>
    public static string ConvertTextBoxToHtml(this string text)
    {
      return text.ConvertTextBoxToHtml(-1, false);
    }

    /// <summary>
    /// Converts all returns ( \r\n ) from an asp:TextBox to HTML &lt;br /&gt;.
    /// </summary>
    /// <param name="text">The text to convert</param>
    /// <param name="maxWordLength">Cut off after this many bytes (-1 to use infinite).</param>
    /// <returns></returns>
    public static string ConvertTextBoxToHtml(this string text, int maxWordLength)
    {
      return text.ConvertTextBoxToHtml(maxWordLength, false);
    }

    /// <summary>
    /// Removes bad tags from string.
    /// </summary>
    /// <param name="text">The string to replace bad tags from</param>
    /// <param name="maliciousTags">bad tags in the following format: "tagname|tagname|tagname|...|tagname"</param>
    /// <returns>the input string without the specified bad tags</returns>
    public static string RemoveMaliciousTags(this string text, string maliciousTags)
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
    public static string RemoveMaliciousTags(this string text)
    {
      return text.RemoveMaliciousTags("abbr|acronym|address|applet|area|base|basefont|bdo|big|body|button|caption|center|cite|code|col|colgroup|dd|del|dir|div|dfn|dl|dt|embed|fieldset|font|form|frame|frameset|head|html|iframe|img|input|ins|isindex|kbd|label|legend|link|map|menu|meta|noframes|noscript|object|optgroup|option|param|pre|q|s|samp|script|select|small|span|strike|style|table|tbody|td|textarea|tfoot|th|thead|title|tr|tt|var|xmp");
    }

    /// <summary>
    /// Converts HTML markup to a string that can be displayed safely in a text input field or textarea
    /// </summary>
    /// <param name="text">The HTML markup text</param>
    /// <returns>A string that can be displayed safely in a text input field or textarea</returns>
    public static string ConvertHtmlToTextBox(this string text)
    {
      text = Regex.Replace(text, "<br>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      text = Regex.Replace(text, "<br/>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      text = Regex.Replace(text, "<br />", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      text = Regex.Replace(text, "<p>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      text = text.Replace("'", "&#39;");

      return text.RemoveTags();
    }

    /// <summary>
    /// Removes all line breaks inside a given string.
    /// </summary>
    /// <param name="text">The text to remove the line-breaks from</param>
    /// <returns>A text without linebreaks.</returns>
    public static string RemoveLineBreaks(this string text)
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
    public static string HighlightWord(this string text, string wordToHighlight, string highlightCssClass)
    {
      return text.Replace(wordToHighlight, "<span class=\"" + highlightCssClass + "\">"
          + wordToHighlight + "</span>");
    }

    /// <summary>
    /// Removes all html/xml tags from a specified string.
    /// </summary>
    /// <param name="text">String to be cleaned from tags</param>
    /// <returns>The specified string without tags</returns>
    public static string RemoveTags(this string text)
    {
      return Regex.Replace(text, "<[^>]+>", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    /// <summary>
    /// This methods removes all javascript script tags from the given string
    /// </summary>
    /// <param name="text">Text to be cleaned for script tags</param>
    /// <returns>Clean text with no script tags.</returns>
    public static string RemoveScriptTags(this string text)
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
    public static string DefuseScriptTags(this string text)
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
    public static string RemoveMaliciousSQLCharacters(this string text)
    {
      text = text.Replace("'", "''");
      return Regex.Replace(text, "--", " ", RegexOptions.Compiled | RegexOptions.Multiline);
    }

    /// <summary>
    /// Removes non-numeric characters from a string.
    /// </summary>
    /// <param name="text">The text to remove the non-numerics from</param>
    /// <returns>A numeric string</returns>
    public static string RemoveNonNumericCharacters(this string text)
    {
      return Regex.Replace(text, @"[^\d]", "");
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
    ///     // convert text to "abcd..."
    ///     text = text.Abbreviate(7, "...");
    /// ]]>
    /// </example>
    /// <returns></returns>
    public static string Abbreviate(this string text, int maxLength, string suffix)
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
    public static string Abbreviate(this string text, int maxLength)
    {
      return text.Abbreviate(maxLength, null);
    }
    /// <summary>
    /// This method cuts a string if it exceeds a given maximum length. Optionally, you can define
    /// a suffix (e.g.: "...") to be attached when the string is to long.
    /// </summary>
    /// <param name="text">The string to format</param>
    /// <param name="maxLength">The maximum length of the string</param>
    /// <param name="attachPoints">if true, "..." is attached when the string is too long.</param>
    /// <returns></returns>
    public static string Abbreviate(this string text, int maxLength, bool attachPoints)
    {
      return text.Abbreviate(maxLength, attachPoints ? "&hellip;" : null);
    }

    /// <summary>
    /// Converts a Dictionary to a debug string
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="format">The format.</param>
    /// <returns></returns>
    public static string ToDebugString<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TextFormat format)
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
    public static int? ToQueryStringInteger(this string text)
    {
      if (text != null)
      {
        int returnValue = 0;
        if (int.TryParse(text, out returnValue))
          return returnValue;
      }
      return null;
    }

    /// <summary>
    /// This method returns a short extracted from a querystring parameter. 
    /// </summary>
    /// <example>
    /// You can use this extension in the following way ->
    /// 
    /// short? myPostID = HttpContext.Current.Request.QueryString["postId"].ToQueryStringShort();
    /// if (myPostID != null)
    /// {
    ///     // ... do something
    /// }
    /// 
    /// </example>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    public static short? ToQueryStringShort(this string text)
    {
      if (text != null)
      {
        short returnValue = 0;
        if (short.TryParse(text, out returnValue))
          return returnValue;
      }
      return null;
    }

    /// <summary>
    /// This method returns a datetime extracted from a querystring parameter.
    /// </summary>
    /// <example>
    /// You can use this extension in the following way ->
    /// 
    /// DateTime? postdate = HttpContext.Current.Request.QueryString["postdate"].ToQueryStringDate();
    /// if (postdate != null)
    /// {
    ///     // ... do something
    /// }
    /// 
    /// </example>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    public static DateTime? ToQueryStringDate(this string text)
    {
      if (text != null)
      {
        DateTime result = DateTime.Now;
        if (DateTime.TryParse(text, out result))
          return result;
      }

      return null;
    }

    /// <summary>
    /// Converts this text into a splitted camel case string
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// Text: ThisIsMyExample
    /// Outcome: This Is My Example
    /// ]]>
    /// </example>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    public static string ToSplittedCamelCase(this string text)
    {
      string result = "";
      bool firstWord = true;
      string[] splitWords = Regex.Split(text, @"(?<!^)(?=[A-Z])");
      foreach (string word in splitWords)
      {
        if (firstWord)
        {
          firstWord = false;
          result += word;
        }
        else
        {
          result += " " + word;
        }
      }
      return result;
    }

    #endregion

    #region xml

    /// <summary>
    /// Encodes an XML value
    /// </summary>
    /// <param name="text">The text to encode.</param>
    /// <returns></returns>
    public static string XmlEncode(this string text)
    {
      return ConversionHelper.XmlEncode(text);
    }
    /// <summary>
    /// Sets the X element node list.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="name">The name.</param>
    /// <param name="obj">The obj.</param>
    public static void SetXElementNodeList(this XElement container, XName name, IList obj)
    {
      ConversionHelper.SetXElementNodeList(container, name, obj);
    }
    /// <summary>
    /// Sets the X element node lookup.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="name">The name.</param>
    /// <param name="obj">The obj.</param>
    public static void SetXElementNodeLookup(this XElement container, XName name, IDictionary obj)
    {
      ConversionHelper.SetXElementNodeLookup(container, name, obj);
    }
    /// <summary>
    /// Parses the X element node lookup.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="container">The container.</param>
    /// <param name="name">The name.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static T ParseXElementNodeLookup<T>(this XElement container, XName name, T defaultValue)
       where T : IDictionary, new()
    {
      return ConversionHelper.ParseXElementNodeLookup<T>(container, name, defaultValue);
    }
    /// <summary>
    /// Parses the X element node list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="container">The container.</param>
    /// <param name="name">The name.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static T ParseXElementNodeList<T>(this XElement container, XName name, T defaultValue)
        where T : IList, new()
    {
      return ConversionHelper.ParseXElementNodeList<T>(container, name, defaultValue);
    }
    /// <summary>
    /// Parses the X element node to the specified type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="container">The parent XElement that contains the tag.</param>
    /// <param name="name">The name of the tag.</param>
    /// <param name="defaultValue">The default value if the tag can't be found at the default element (can be null with
    /// nullable objects).</param>
    /// <returns>
    /// The object parsed from the tag, the default value if the tag was not found or invalid.
    /// </returns>
    /// <exception cref="System.FormatException">Throws a System.FormatException if the specified generic type could not be parsed.</exception>
    public static T ParseXElementNode<T>(this XElement container, XName name, T defaultValue)
    {
      return ConversionHelper.ParseXElementNode<T>(container, name, defaultValue);
    }
    #endregion

    #region dictionaries

    private static object _TypeNamesLock = new object();
    private static Dictionary<string, string> _TypeNamesLookup = new Dictionary<string, string>();

    /// <summary>
    /// Gets a generic field from a specified dictionary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns></returns>
    public static T GetField<T>(this IDictionary<string, object> dictionary, string fieldName)
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
    public static T GetField<T>(this IDictionary<string, object> dictionary, string fieldName, bool upperCase)
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
    public static T GetField<T>(this IDictionary<string, object> dictionary, string fieldName, bool upperCase, T defaultValue)
    {
      return ConversionHelper.GetField<T>(dictionary, fieldName, upperCase, defaultValue);
    }


    /// <summary>
    /// Gets a generic field from a specified dictionary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns></returns>
    public static T GetField<T>(this IDictionary<string, string> dictionary, string fieldName)
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
    public static T GetField<T>(this IDictionary<string, string> dictionary, string fieldName, bool upperCase)
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
    public static T GetField<T>(this IDictionary<string, string> dictionary, string fieldName, bool upperCase, T defaultValue)
    {
      return ConversionHelper.GetField<T>(dictionary, fieldName, upperCase, defaultValue);
    }

    #endregion

    #region SqlCommand

    /// <summary>
    /// Creates a Sql EXEC statement for the specified command.
    /// </summary>
    /// <param name="cmd">The command.</param>
    /// <returns></returns>
    public static string ToSqlExecStatement(this SqlCommand cmd)
    {
      StringBuilder sb = new StringBuilder("EXEC " + cmd.CommandText + Environment.NewLine);
      string comma = "  ";
      string value;
      foreach (SqlParameter parameter in cmd.Parameters)
      {
        if (parameter.ParameterName == "@RETURN_VALUE")
        {
          continue;
        }
        else if (parameter.Value == null || parameter.Value == DBNull.Value)
        {
          value = "NULL";
        }
        else
        {
          switch (parameter.SqlDbType)
          {
            case SqlDbType.Bit:
              value = ((bool)parameter.Value == false ? 0 : 1).ToString();
              break;

            case SqlDbType.Char:
            case SqlDbType.VarChar:
            case SqlDbType.NChar:
            case SqlDbType.NVarChar:
            case SqlDbType.DateTime:
              value = "'" + parameter.Value.ToString().Replace("'", "''") + "'";
              break;

            default:
              value = parameter.Value.ToString();
              break;
          }
        }

        sb.AppendLine(string.Format("{0}{1} = {2}"
          , comma
          , parameter.ParameterName
          , value));
        comma = ", ";
      }
      return sb.ToString();
    }
    #endregion
  }
}
