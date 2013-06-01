using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Workmate.Data.SqlProvider
{
  public class ContenLevelNodeXmlFormatter
  {
    internal const string XML_ROOT = "r";
    internal const string XML_NODE = "<i n=\"{0}\" l=\"{1}\" />";

    internal static string GetXml(IEnumerable<string> contentLevelNodeNames)
    {
      if (contentLevelNodeNames == null)
        return null;

      StringBuilder stringBuilder = new StringBuilder();

      stringBuilder.Append("<" + XML_ROOT + ">");

      int counter = 0;
      bool hasTags = false;
      foreach (string current in contentLevelNodeNames)
      {
        stringBuilder.Append(string.Format(XML_NODE, HttpUtility.HtmlEncode(current.Trim()), counter));
        counter++;
        hasTags = true;
      }

      if (!hasTags)
        return null;

      stringBuilder.Append("</" + XML_ROOT + ">");
      return stringBuilder.ToString();
    }
  }
}
