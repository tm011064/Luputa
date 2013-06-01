using System;
using System.Collections.Generic;
using System.Web.Caching;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// This interface enforces all properties needed for 
    /// </summary>
    public interface ISiteMapUrls
    {
        /// <summary>
        /// Finds the node by resource key.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        IUrlRewriteItem FindNodeByResourceKey(string name);
        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <param name="key">The resourceKey of the sitemapnode</param>
        /// <returns></returns>
        string GetPath(string key);
        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <param name="key">The resourceKey of the sitemapnode</param>
        /// <param name="args">The arguments to replace the path of the SitemapNode with.</param>
        /// <returns></returns>
        string GetPath(string key, params string[] args);
        /// <summary>
        /// Gets the matching rewrite.
        /// </summary>
        /// <param name="pattern">The pattern to match</param>
        /// <returns></returns>
        string GetMatchingRewrite(string pattern);
        /// <summary>
        /// Gets the matching rewrite item. This method is intended for testing, don't use it in production because it is
        /// inefficient.
        /// </summary>
        /// <param name="pathAndQuery">The path and query url.</param>
        /// <returns></returns>
        IUrlRewriteItem GetMatchingRewriteItem(string pathAndQuery);
        /// <summary>
        /// Reloads the site map.
        /// </summary>
        void Reload();
    }
}
