using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Components.Caching
{
    /// <summary>
    /// The ICacheController provides a collection of ICacheItems.
    /// </summary>
    public interface ICacheController
    {
        /// <summary>
        /// Gets the continuous access stale key suffix for memcached.
        /// </summary>
        /// <value>The continuous access stale key suffix for memcached.</value>
        string ContinuousAccessStaleKeySuffixForMemcached { get; }
        /// <summary>
        /// Gets the amount of minutes to cache all ICacheItems at this object's ICacheItem collection. This value can be overwritten
        /// by the ICacheItem itself.
        /// </summary>
        /// <value>The minutes.</value>
        int Minutes { get; }
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
    }
}
