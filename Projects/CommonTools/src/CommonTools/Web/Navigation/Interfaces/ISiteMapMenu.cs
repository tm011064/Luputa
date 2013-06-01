using System;
using System.Collections.Generic;
using System.Web.Caching;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// This interface enforces all properties needed for 
    /// </summary>
    public interface ISiteMapMenu
    {
        /// <summary>
        /// Gets the name of the <see cref="IUrlRewriteItem"/>.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }
        /// <summary>
        /// Gets the child nodes.
        /// </summary>
        /// <value>The child nodes.</value>
        List<ISiteMapMenuItem> MenuNodes { get; set; }
        /// <summary>
        /// Gets the matching site map menu item.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        ISiteMapMenuItem GetMatchingSiteMapMenuItem(string name);
        /// <summary>
        /// Gets or sets a value indicating whether [enable caching].
        /// </summary>
        /// <value><c>true</c> if [enable caching]; otherwise, <c>false</c>.</value>
        bool EnableCaching { get; set; }
        /// <summary>
        /// Gets or sets the cache duration in seconds.
        /// </summary>
        /// <value>The cache duration in seconds.</value>
        int CacheDurationInSeconds { get; set; }
        /// <summary>
        /// Gets or sets the cache item priority.
        /// </summary>
        /// <value>The cache item priority.</value>
        CacheItemPriority CacheItemPriority { get; set; }
    }
}
