using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.TestApp.Components
{
    public static class Extensions
    {
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

        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int pageIndex, int pageSize)
        {
            return source.Skip(pageIndex * pageSize).Take(pageSize);
        }

        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int pageIndex, int pageSize)
        {
            return source.Skip(pageIndex * pageSize).Take(pageSize);
        }
    }
}
