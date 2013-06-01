using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Runtime.Caching.Configuration
{
    /// <summary>
    /// The ICacheController provides a collection of ICacheItems.
    /// </summary>
    public interface ICacheController
    {
        /// <summary>
        /// Gets the timespan to cache all ICacheItems at this object's ICacheItem collection. This value can be overwritten
        /// by the ICacheItem itself.
        /// </summary>
        /// <value>The interval.</value>
        TimeSpan LifeSpanInterval { get; }
        /// <summary>
        /// Gets a value indicating whether to enable caching or not. If this is set to false, no ICacheItem at the ICacheItem collection
        /// can use the HttpRuntime cache.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        bool Enabled { get; }
        /// <summary>
        /// Creates an instance of this object's default controller.
        /// </summary>
        /// <returns></returns>
        ICacheController CreateCacheControllerInstance();
        /// <summary>
        /// Gets the ICacheItem collection associated with this ICacheController.
        /// </summary>
        /// <value>The cache items.</value>
        Dictionary<string, ICacheItem> CacheItems { get; }
        /// <summary>
        /// Gets an ICacheItem from the ICacheItem collection by name
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns></returns>
        ICacheItem GetCacheItem(string name);
        /// <summary>
        /// Iterates the over cache items.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ICacheItem> IterateOverCacheItems();
    }
}
