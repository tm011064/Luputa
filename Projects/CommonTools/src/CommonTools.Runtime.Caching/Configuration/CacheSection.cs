using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Caching;


namespace CommonTools.Runtime.Caching.Configuration
{
    /// <summary>
    /// Provides the handler for the cache section of app.config.
    /// </summary>
    public class CacheSection : ConfigurationSection, ICacheController
    {
        private Dictionary<string, ICacheItem> _CacheItems;

        /// <summary>
        /// holds the collection of cache objects in the app.config.
        /// </summary>
        [ConfigurationProperty("objects", IsRequired = true)]
        public CacheElements CacheElementCollection
        {
            get { return (CacheElements)base["objects"]; }
        }

        /// <summary>
        /// The number of minutes to hold the cached object.
        /// </summary>
        [ConfigurationProperty("cacheControllerProviderType", IsRequired = false)]
        public string CacheControllerType
        {
            get
            {
                if (base["cacheControllerProviderType"] != null)
                    return (string)base["cacheControllerProviderType"];

                return null;
            }
        }

        #region ICacheController Members

        /// <summary>
        /// Iterates the over cache items.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ICacheItem> IterateOverCacheItems()
        {
            foreach (ICacheItem item in this.CacheItems.Values)
                yield return item;
        }

        /// <summary>
        /// Creates an instance of this object's default controller.
        /// </summary>
        /// <returns></returns>
        public ICacheController CreateCacheControllerInstance()
        {
            return CacheSectionManager.CacheSection;
        }

        /// <summary>
        /// Gets the ICacheItem collection associated with this ICacheController.
        /// </summary>
        /// <value>The cache items.</value>
        public Dictionary<string, ICacheItem> CacheItems
        {
            get
            {
                if (_CacheItems == null)
                {
                    _CacheItems = new Dictionary<string, ICacheItem>();
                    foreach (CacheElement item in CacheElementCollection)
                        _CacheItems.Add(item.Name, (ICacheItem)item);
                }
                return _CacheItems;
            }
        }

        /// <summary>
        /// Gets the cache item.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public ICacheItem GetCacheItem(string key)
        {
            if (CacheItems.ContainsKey(key))
                return CacheItems[key];

            return null;
        }

        /// <summary>
        /// Gets the timespan to cache all ICacheItems at this object's ICacheItem collection. This value can be overwritten
        /// by the ICacheItem itself.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        public TimeSpan LifeSpanInterval
        {
            get { return CacheElementCollection.LifeSpanInterval; }
        }

        /// <summary>
        /// Gets a value indicating whether to enable caching or not. If this is set to false, no ICacheItem at the ICacheItem collection
        /// can use the HttpRuntime cache.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return CacheElementCollection.Enabled; }
        }

        #endregion
    }

    /// <summary>
    /// Provides the handler for each individual cache node in the cache section of app.config.
    /// </summary>
    public class CacheElements : ConfigurationElementCollection
    {
        #region attributes

        /// <summary>
        /// The number of minutes to hold the cached object.
        /// </summary>
        [ConfigurationProperty("lifeSpan", DefaultValue = "00:15:00")]
        public TimeSpan LifeSpanInterval
        {
            get { return (TimeSpan)base["lifeSpan"]; }
        }

        /// <summary>
        /// Whether the cache object is cached.
        /// </summary>
        [ConfigurationProperty("enabled", DefaultValue = "true")]
        public bool Enabled
        {
            get { return (bool)base["enabled"]; }
        }
        #endregion

        /// <summary>
        /// Overridden. Creates a new CacheElement.
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CacheElement();
        }

        /// <summary>
        /// Overridden. Retrieves the specified cache element name for the given weak typed node.
        /// </summary>
        /// <param name="element">The node in app.config</param>
        /// <returns>The cache element name located at the configuration element.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CacheElement)element).Name;
        }
    }

    /// <summary>
    /// Provides the cache element object as specified through app.config.
    /// </summary>
    public class CacheElement : ConfigurationElement, ICacheItem
    {
        #region attributes
        /// <summary>
        /// Whether this cache element is enabled.
        /// </summary>
        [ConfigurationProperty("enabled", DefaultValue = "true")]
        public bool Enabled
        {
            get { return (bool)base["enabled"]; }
        }
        
        /// <summary>
        /// The name of the cache object.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base["name"]; }
        }

        /// <summary>
        /// The cache key the element depends on.
        /// </summary>
        [ConfigurationProperty("cacheKey", DefaultValue = "")]
        public string CacheKey
        {
            get { return (string)base["cacheKey"]; }
        }

        /// <summary>
        /// Priority of the cache item.
        /// </summary>
        [ConfigurationProperty("cacheItemPriority", DefaultValue = "Default")]
        public CacheItemPriority CacheItemPriority
        {
            get { return (CacheItemPriority)base["cacheItemPriority"]; }
        }

        private TimeSpan? _LifeSpan = null;
        /// <summary>
        /// Gets the life span.
        /// </summary>
        /// <value>The life span.</value>
        [ConfigurationProperty("lifeSpan", DefaultValue = "00:00:00")]
        public TimeSpan LifeSpan
        {
            get
            {
                if (!_LifeSpan.HasValue)
                {
                    _LifeSpan = (TimeSpan)base["lifeSpan"];
                }
                return _LifeSpan.Value;
            }
            set
            {
                _LifeSpan = value;
            }
        }

        /// <summary>
        /// Gets the time indicating for how long an object should linger in cache until the next call comes in which reloads the object
        /// on a separate thread. If an object has a lifespan of 10 minutes and the UseContinuousAccess flag is used, this property defines
        /// that the object should actually be cached for 10 minutes + ContinuousAccessExtendedLifeSpan. If a request comes in after the
        /// official expiry date (10 min), the object will be reloaded on a separate thread and then inserted into cache.
        /// While loading, the stale object which is still on its "extended lifespan" will be returned so no Dog Pile Effect can
        /// occur.
        /// </summary>
        /// <value>The continuous access extended life span.</value>
        [ConfigurationProperty("continuousAccessExtendedLifeSpan", IsRequired = false, DefaultValue = "01:00:00")]
        public TimeSpan ContinuousAccessExtendedLifeSpan
        {
            get { return (TimeSpan)base["continuousAccessExtendedLifeSpan"]; }
        }

        /// <summary>
        /// Gets a value indicating whether [use continuous access].
        /// </summary>
        /// <value><c>true</c> if [use continuous access]; otherwise, <c>false</c>.</value>
        [ConfigurationProperty("useContinuousAccess", IsRequired = false, DefaultValue = false)]
        public bool UseContinuousAccess
        {
            get { return (bool)base["useContinuousAccess"]; }
        }

        #endregion
    }
}