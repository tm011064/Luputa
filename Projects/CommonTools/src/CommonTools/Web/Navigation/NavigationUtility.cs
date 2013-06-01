using System;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using CommonTools.Extensions;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// This is a general helper class for manipulating navigation items.
    /// </summary>
    public static class NavigationUtility
    {
        /// <summary>
        /// Inserts or updates the query string of a specified path with a new value.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="queryStringKey">The query string key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string InsertOrUpdateQueryStringParamter(string path, string queryStringKey, string value)
        {
            int index = path.IndexOf("?");
            if (index >= 0)
            {// we have a query string
                string url = path.Remove(index).Trim();
                string query = path.Remove(0, index + 1).Trim();

                if (query.Contains(queryStringKey + "="))
                {// we might have a message parameter
                    foreach (string parameter in query.Split('&'))
                    {
                        if (parameter.StartsWith(queryStringKey + "="))
                        {// we found a message parameter
                            path = url + "?" + query.Replace(parameter, queryStringKey + "=" + value.UrlEncode());
                            return path;
                        }
                    }
                }

                // we don't have an existing message parameter to replace, so simply attach...
                if (query.Length > 0 && !query.EndsWith("&"))
                {
                    return path + "&" + queryStringKey + "=" + value.UrlEncode();
                }
                else
                {
                    return path + queryStringKey + "=" + value.UrlEncode();
                }
            }
            else
            {
                return path + "?" + queryStringKey + "=" + value.UrlEncode();
            }
        }

        /// <summary>
        /// Gets the current query string with the specified substitutions/additions. For example, a query string NameValueCollection
        /// that looks [A] with substitutions [B] will result in the returning string [C]:
        /// [A] -&gt; { {a,1}, {b,2}, {c,3} }
        /// [B] -&gt; key=a, value=9
        /// [C] -&gt; "?a=9&amp;b=2&amp;c=3"
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetSubstitutedQueryString(NameValueCollection queryString, string key, string value)
        {
            return GetSubstitutedQueryString(queryString, new Dictionary<string, string>() { { key, value } });
        }
        /// <summary>
        /// Gets the current query string with the specified substitutions/additions. For example, a query string NameValueCollection
        /// that looks [A] with substitutions [B] will result in the returning string [C]:
        /// 
        ///     [A] -> { {a,1}, {b,2}, {c,3} }
        ///     [B] -> { {a,5}, {b,1}, {xyz,99} }
        ///     [C] -> "?a=5&amp;b=1&amp;c=3&amp;xyz=99"
        ///     
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <param name="substitutes">The substitutes.</param>
        /// <returns></returns>
        public static string GetSubstitutedQueryString(NameValueCollection queryString, Dictionary<string, string> substitutes)
        {
            return GetSubstitutedQueryString(queryString, substitutes, new List<string>());
        }
        /// <summary>
        /// Gets the current query string with the specified substitutions/additions. For example, a query string NameValueCollection
        /// that looks [A] with substitutions [B] will result in the returning string [C]:
        /// [A] -&gt; { {a,1}, {b,2}, {c,3} }
        /// [B] -&gt; { {a,5}, {b,1}, {xyz,99} }
        /// [C] -&gt; "?a=5&amp;b=1&amp;c=3&amp;xyz=99"
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <param name="substitutes">The substitutes.</param>
        /// <param name="parametersToRemove">The parameters to remove from the original querystring.</param>
        /// <returns></returns>
        public static string GetSubstitutedQueryString(NameValueCollection queryString, Dictionary<string, string> substitutes, List<string> parametersToRemove)
        {
            StringBuilder sb = new StringBuilder("?");
            foreach (string key in queryString.Keys)
            {
                if (!String.IsNullOrEmpty(key) && !parametersToRemove.Contains(key))
                {
                    if (substitutes.ContainsKey(key))
                    {
                        sb.Append(key + "=" + substitutes[key].UrlEncode() + "&");
                        substitutes.Remove(key);
                    }
                    else
                        sb.Append(key + "=" + queryString[key].UrlEncode() + "&");
                }
            }
            foreach (string key in substitutes.Keys)
                sb.Append(key + "=" + substitutes[key].UrlEncode() + "&");

            if (sb.Length > 1)
                sb = sb.Remove(sb.Length - 1, 1);

            return sb.ToString();

        }
        /// <summary>
        /// Gets the mapped application path.
        /// </summary>
        /// <param name="absoluteApplicationPath">The absolute application path.</param>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns></returns>
        public static string GetMappedApplicationPath(string absoluteApplicationPath, string virtualPath)
        {
            return (absoluteApplicationPath + virtualPath.Replace("~/", "/").Replace("/", @"\")).Replace(@"\\", @"\");
        }
        /// <summary>
        /// Gets the absolute path.
        /// </summary>
        /// <param name="applicationPath">The application path.</param>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns></returns>
        public static string GetAbsolutePath(string applicationPath, string virtualPath)
        {
            string path = (applicationPath + virtualPath.Replace("~/", "/"));
            return path
                .Replace("http://", "##http##")
                .Replace("https://", "##https##")
                .Replace(@"//", @"/")
                .Replace("##http##", "http://")
                .Replace("##https##", "https://");
        }
    }
}
