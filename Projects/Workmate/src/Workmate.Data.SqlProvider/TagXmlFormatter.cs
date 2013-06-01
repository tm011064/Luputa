using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Workmate.Data.SqlProvider
{
  public class TagXmlFormatter
  {
    internal const string XML_TAGS_ROOT = "tags";
    internal const string XML_TAGS_FILETYPES_ROOT = "filetypes";
    internal const string XML_TAGS_NODE = "<r t=\"{0}\" />";

    internal static string GetTagsXml(IEnumerable<string> tags)
    {
      return GetTagsXml(tags, XML_TAGS_ROOT, true);
    }
    internal static string GetTagsXml(IEnumerable<string> tags, bool doLowerCase)
    {
      return GetTagsXml(tags, XML_TAGS_ROOT);
    }
    internal static string GetTagsXml(IEnumerable<string> tags, string root)
    {
      return GetTagsXml(tags, root, true);
    }
    internal static string GetTagsXml(IEnumerable<string> tags, string root, bool doLowerCase)
    {
      if (tags == null)
        return "<" + root + "></" + root + ">";

      StringBuilder stringBuilder = new StringBuilder();

      stringBuilder.Append("<" + root + ">");

      foreach (string current in tags.Distinct<string>())
      {
        if (!string.IsNullOrEmpty(current.Trim()))
        {
          if (doLowerCase)
          {
            stringBuilder.Append(string.Format(XML_TAGS_NODE, HttpUtility.HtmlEncode(current.Trim().ToLower())));
          }
          else
          {
            stringBuilder.Append(string.Format(XML_TAGS_NODE, HttpUtility.HtmlEncode(current.Trim())));
          }
        }
      }

      stringBuilder.Append("</" + root + ">");
      return stringBuilder.ToString();
    }
  }
}
