using System;
using System.Web.Caching;

namespace CommonTools.Components.Caching
{
    /// <summary>
    /// This interface enforces all properties needed for 
    /// </summary>
    public interface ICacheItem
    {
        /// <summary>
        /// Gets a value indicating whether to use protocol buffer serialization for memcached objects. This serialization technique
        /// has an average performance increase of about 20%, but the specified object must be decorated with ProtoContract attricutes.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [use protocol buffer serialization]; otherwise, <c>false</c>.
        /// </value>
        bool UseProtocolBufferSerialization { get; }
        /// <summary>
        /// Gets a value indicating whether this instance is memcached.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is memcached; otherwise, <c>false</c>.
        /// </value>
        bool IsMemcached { get; }
        /// <summary>
        /// Gets the time indicating for how long an object should linger in cache until the next call comes in which reloads the object
        /// on a separate thread. If an object has a lifespan of 10 minutes and the UseContinuousAccess flag is used, this property defines
        /// that the object should actually be cached for 10 minutes + ContinuousAccessExtendedLifeSpan. If a request comes in after the
        /// official expiry date (10 min), the object will be reloaded on a separate thread and then inserted into cache.
        /// While loading, the old object which is still on its "extended lifespan" will be returned so the reload thread doesn't block
        /// all other requests for that object.
        /// </summary>
        /// <value>The continuous access extended life span.</value>
        TimeSpan ContinuousAccessExtendedLifeSpan { get; }
        /// <summary>
        /// Gets a value indicating whether to use continuous access to this cached item. Continuous access means that a cached item gets reloaded
        /// on a background thread after it "officially" expires and gracefully switches to the new object once it is instanziated. While the new object
        /// is loading, the old object will be returned. This mechanism should be used if generating an object takes a long time and threads
        /// must not wait/lock during its initialization.
        /// </summary>
        /// <value><c>true</c> if [use continuous access]; otherwise, <c>false</c>.</value>
        bool UseContinuousAccess { get; }
        /// <summary>
        /// Gets the life span.
        /// </summary>
        /// <value>The life span.</value>
        TimeSpan LifeSpan { get; }
        /// <summary>
        /// Gets a value indicating whether caching is enabled for this <see cref="ICacheItem"/>.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        bool Enabled { get; }
        /// <summary>
        /// Gets or sets the minutes the <see cref="ICacheItem"/> should be cached.
        /// </summary>
        /// <value>The minutes.</value>
        int Minutes { get; set; }
        /// <summary>
        /// Gets or sets the seconds the <see cref="ICacheItem"/> should be cached.
        /// </summary>
        /// <value>The minutes.</value>
        int Seconds { get; set; }
        /// <summary>
        /// Gets the suffix if this <see cref="ICacheItem"/> is in iterating format {CacheKey}{Suffix}.
        /// </summary>
        /// <value>The suffix, can be null.</value>
        string Suffix { get; }
        /// <summary>
        /// Gets a value indicating whether the <see cref="ICacheItem"/> can iterate (object has a primary key).
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is iterating; otherwise, <c>false</c>.
        /// </value>
        bool IsIterating { get; }
        /// <summary>
        /// Gets a value indicating whether this instance is clustered.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is clustered; otherwise, <c>false</c>.
        /// </value>
        bool IsClustered { get; }
        /// <summary>
        /// Gets the name of the <see cref="ICacheItem"/>.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }
        /// <summary>
        /// Gets the cache key used at HttpRuntime.Cache for this <see cref="ICacheItem"/>. If this value is null, the <see cref="ICacheItem"/>.Name 
        /// property is used as key in the HttpRuntime.Cache collection.
        /// </summary>
        /// <value>The cache key.</value>
        string CacheKey { get; }
        /// <summary>
        /// Gets the cache item priority of this <see cref="ICacheItem"/>.
        /// </summary>
        /// <value>The cache item priority.</value>
        CacheItemPriority CacheItemPriority { get; }
    }
}
