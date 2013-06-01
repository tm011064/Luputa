using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace CommonTools.Core
{
    /// <summary>
    /// Various string utility functions.
    /// </summary>
    public static class Text
    {
        /// <summary>
        /// Gets a line break in the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string GetLineBreak(TextFormat format)
        {
            string lineBreak = string.Empty;
            switch (format)
            {
                case TextFormat.ASCII:
                    lineBreak = Environment.NewLine;
                    break;
                case TextFormat.HTML:
                    lineBreak = "<br/>";
                    break;
            }
            return lineBreak;
        }
    }
}
