using System;
using System.Web;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// This static class includes methods that are helpful for web navigation.
    /// </summary>
    public static class UrlManager
    {
        /// <summary>
        /// This method returns an integer that holds the value of a given querystring key. If
        /// the querystring-key was not provided or malicious, this methods returns null
        /// </summary>
        /// <param name="key">The QueryString key</param>
        /// <returns>
        /// An integer that holds the value of a given querystring key. If the querystring-key
        /// was not provided or malicious, this methods returns null
        /// </returns>
        public static int? GetIntegerFromQueryString(string key)
        {
            string queryValue = HttpContext.Current.Request.QueryString[key];
            if (queryValue != null)
            {
                int returnValue = -1;
                if (int.TryParse(queryValue, out returnValue))
                    return returnValue;
            }
            return null;
        }
    }
}
