using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Web.Components.Application;
using System.Text.RegularExpressions;

namespace Workmate.Web.Components
{
  /// <summary>
  /// 
  /// </summary>
  public class ContentFormatUtility
  {
    #region nested 
    /// <summary>
    /// Format code used to format string quotes
    /// </summary>
    public enum FormatMode
    {
      None = 0,
      EscapeSingleQuotes = 1,
      EscapeDoubleQuotes = 2
    }
    #endregion

    #region members
    private static Regex _ContentRegex = new Regex(@"##content\(([0-9a-zA-Z_-]*)\)##");
    private static Regex _UrlRegex = new Regex(@"##url\(([-0-9a-zA-Z_\.\\\/]*)\)##");
    #endregion

    #region public static methods
    /// <summary>
    /// Formats a specified string replacing placeholders with content
    /// </summary>
    /// <param name="theme"></param>
    /// <param name="content"></param>
    /// <param name="formatMode"></param>
    /// <param name="staticContentLookup"></param>
    /// <param name="themeFolderLookup"></param>
    /// <returns></returns>
    public static string Format(
      string theme
      , string content
      , IStaticContentLookup staticContentLookup
      , IThemeFolderLookup themeFolderLookup
      , FormatMode formatMode)
    {
      if (string.IsNullOrWhiteSpace(content))
        return content;
      // TODO (Roman): implement URL.RESOLVE to be able to resolve url root folders, not only theme paths...
      string value;
      foreach (Match match in _ContentRegex.Matches(content))
      {
        value = staticContentLookup.GetContent(theme, match.Groups[1].Value);
        if ((formatMode & FormatMode.EscapeSingleQuotes) != 0)
          value = value.Replace("'", @"\'");
        if ((formatMode & FormatMode.EscapeDoubleQuotes) != 0)
          value = value.Replace("\"", @"\""");

        content = content.Replace(match.Value, value);
      }
      foreach (Match match in _UrlRegex.Matches(content))
      {
        value = themeFolderLookup.GetAbsoluteThemePath(theme, match.Groups[1].Value);
        if ((formatMode & FormatMode.EscapeSingleQuotes) != 0)
          value = value.Replace("'", @"\'");
        if ((formatMode & FormatMode.EscapeDoubleQuotes) != 0)
          value = value.Replace("\"", @"\""");

        content = content.Replace(match.Value, value);
      }
      
      return content;
    }
    #endregion
  }
}
