using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// The IUrlRewriteController provides a collection of IUrlRewriteItems.
    /// </summary>
    public interface IUrlRewriteController
    {
        /// <summary>
        /// Creates an instance of this object's default controller.
        /// </summary>
        /// <returns></returns>
        IUrlRewriteController CreateUrlRewriteControllerInstance();
        /// <summary>
        /// Gets the IUrlRewriteItem collection associated with this IUrlRewriteController.
        /// </summary>
        /// <value>The UrlRewrite items.</value>
        List<IUrlRewriteItem> UrlRewriteItems { get; }
        /// <summary>
        /// Gets an IUrlRewriteItem from the IUrlRewriteItem collection by name
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns></returns>
        IUrlRewriteItem GetUrlRewriteItem(string name);
        /// <summary>
        /// Gets the sitemap urls cache duration in seconds.
        /// </summary>
        /// <value>The sitemap urls cache duration in seconds.</value>
        int SitemapUrlsCacheDurationInSeconds { get; }
        /// <summary>
        /// Gets the sitemap urls cache key.
        /// </summary>
        /// <value>The sitemap urls cache key.</value>
        string SitemapUrlsCacheKey { get; }
        /// <summary>
        /// Gets the sitemap urls cache item priority.
        /// </summary>
        /// <value>The sitemap urls cache item priority.</value>
        CacheItemPriority SitemapUrlsCacheItemPriority { get; }
    }
}
