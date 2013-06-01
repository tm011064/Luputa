using System;
using CommonTools.Components.Caching;

namespace CommonTools.Caching
{
    /// <summary>
    /// This class contains all cache item container related data
    /// </summary>
    class CacheItemContainer
    {
        #region properties
        /// <summary>
        /// Gets or sets the cache item.
        /// </summary>
        /// <value>The cache item.</value>
        public ICacheItem CacheItem { get; set; }

        /// <summary>
        /// Gets or sets the cache key.
        /// </summary>
        /// <value>The cache key.</value>
        public string CacheKey { get; set; }

        /// <summary>
        /// Gets or sets the memcached stale cache key.
        /// </summary>
        /// <value>The memcached stale cache key.</value>
        public string MemcachedStaleCacheKey { get; private set; }

        /// <summary>
        /// Gets or sets the cache mode.
        /// </summary>
        /// <value>The cache mode.</value>
        public CacheMode CacheMode { get; set; }

        /// <summary>
        /// Gets or sets the specified expiry date.
        /// </summary>
        /// <value>The specified expiry date.</value>
        public DateTime SpecifiedExpiryDate { get; private set; }

        /// <summary>
        /// Gets or sets the actual expiry date.
        /// </summary>
        /// <value>The actual expiry date.</value>
        public DateTime ActualExpiryDate { get; private set; }

        private DateTime _LatestFetch = DateTime.MinValue;
        /// <summary>
        /// Gets or sets the latest fetch.
        /// </summary>
        /// <value>The latest fetch.</value>
        public DateTime LatestFetch
        {
            get { return _LatestFetch; }
            set
            {
                _LatestFetch = value;
                this.SpecifiedExpiryDate = _LatestFetch.Add(CacheItem.LifeSpan);
                this.ActualExpiryDate = CacheItem.UseContinuousAccess ? this.SpecifiedExpiryDate.Add(this.CacheItem.ContinuousAccessExtendedLifeSpan)
                                                                      : this.SpecifiedExpiryDate;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Determines whether [is in extended life span] [the specified date time].
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        /// 	<c>true</c> if [is in extended life span] [the specified date time]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInExtendedLifeSpan(DateTime dateTime)
        {
            return dateTime > SpecifiedExpiryDate && dateTime <= this.ActualExpiryDate;
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheItemContainer"/> class.
        /// </summary>
        /// <param name="cacheItem">The cache item.</param>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cacheMode">The cache mode.</param>
        /// <param name="continuousAccessStaleKeySuffixForMemcached">The continuous access stale key suffix for memcached.</param>
        public CacheItemContainer(ICacheItem cacheItem, string cacheKey, CacheMode cacheMode
            , string continuousAccessStaleKeySuffixForMemcached)
        {
            this.CacheItem = cacheItem;
            this.CacheKey = cacheKey;
            this.CacheMode = cacheMode;
            this.MemcachedStaleCacheKey = this.CacheKey + continuousAccessStaleKeySuffixForMemcached;
            
            this.ActualExpiryDate = DateTime.MinValue;
            this.SpecifiedExpiryDate = DateTime.MinValue;
        }
        #endregion
    }
}
