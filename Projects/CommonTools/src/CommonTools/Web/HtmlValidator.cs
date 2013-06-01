using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CommonTools.Web
{
    /// <summary>
    /// This code was nicked from some guy at the internet - don't know how well it behaves, but it should be sufficient for internal
    /// admin projects...
    /// </summary>
    public static class HtmlValidator
    {
        private const int PROBLEMZONE_LENGTH = 26;
        private static string GetProblemZone(int index, int maxLength, string text)
        {
            if (text.Length <= maxLength)
                return text; int startIndex = (index - maxLength / 2) < 0 ? 0 : index - maxLength / 2;
            int endIndex = startIndex + maxLength >= text.Length ? (text.Length - startIndex) : maxLength;
            return text.Substring(startIndex, endIndex).Replace("<", "&lt;");
        }

        /// <summary>
        /// Validates the HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public static bool ValidateHtml(string html, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrEmpty(html))
                return true;

            Random random = new Random();
            string root = "root" + random.Next(10000000, 99999999);

            try { new XmlDocument().LoadXml("<" + root + ">" + html + "</" + root + ">"); }
            catch (XmlException err)
            {
                string[] lines = html.Split('\n');
                if (lines.Length >= err.LineNumber)
                {
                    error = err.Message + " Problem zone-&gt; '..." + GetProblemZone(err.LinePosition, PROBLEMZONE_LENGTH, lines[err.LineNumber - 1]) + "...'";
                }
                return false;
            }

            return true;
        }
    }
}
