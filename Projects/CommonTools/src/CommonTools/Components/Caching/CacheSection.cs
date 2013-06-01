using System;
using System.Configuration;
using System.Web.Caching;
using System.Collections;
using System.Collections.Generic;


namespace CommonTools.Components.Caching
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
        /// Gets the amount of minutes to cache all ICacheItems at this object's ICacheItem collection. This value can be overwritten
        /// by the ICacheItem itself.
        /// </summary>
        /// <value>The minutes.</value>
        public int Minutes
        {
            get { return CacheElementCollection.Minutes; }
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


        /// <summary>
        /// Gets the continuous access stale key suffix for memcached.
        /// </summary>
        /// <value>The continuous access stale key suffix for memcached.</value>
        public string ContinuousAccessStaleKeySuffixForMemcached
        {
            get { return CacheElementCollection.ContinuousAccessStaleKeySuffixForMemcached; }
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
        /// Gets the continuous access stale key suffix for memcached.
        /// </summary>
        /// <value>The continuous access stale key suffix for memcached.</value>
        [ConfigurationProperty("continuousAccessStaleKeySuffixForMemcached", DefaultValue = "~")]
        public string ContinuousAccessStaleKeySuffixForMemcached
        {
            get { return (string)base["continuousAccessStaleKeySuffixForMemcached"]; }
        }

        /// <summary>
        /// The number of minutes to hold the cached object.
        /// </summary>
        [ConfigurationProperty("minutes", DefaultValue = "15")]
        public int Minutes
        {
            get { return (int)base["minutes"]; }
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

        private int _Minutes = -1;
        /// <summary>
        /// The number of minutes before the cache item is expired. A default value of -1 means that
        /// the object never expires.
        /// </summary>
        [ConfigurationProperty("minutes", DefaultValue = "-1")]
        public int Minutes
        {
            get
            {
                if (_Minutes >= 0)
                    return _Minutes;

                return (int)base["minutes"];
            }
            set
            {
                _Minutes = value;
                _LifeSpan = null;
            }
        }

        private int _Seconds = -1;
        /// <summary>
        /// The number of minutes before the cache item is expired. A default value of -1 means that
        /// the object never expires.
        /// </summary>
        [ConfigurationProperty("seconds", DefaultValue = "-1")]
        public int Seconds
        {
            get
            {
                if (_Seconds >= 0)
                    return _Seconds;

                return (int)base["seconds"];
            }
            set
            {
                _Seconds = value;
                _LifeSpan = null;
            }
        }

        /// <summary>
        /// A suffix to uniquely identify the cache type.
        /// </summary>
        [ConfigurationProperty("suffix")]
        public string Suffix
        {
            get { return (string)base["suffix"]; }
        }

        /// <summary>
        /// Determines whether the cache element is an enumerable cache object.
        /// </summary>
        [ConfigurationProperty("isIterating")]
        public bool IsIterating
        {
            get { return (bool)base["isIterating"]; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is clustered.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is clustered; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("isClustered", IsRequired = false, DefaultValue = false)]
        public bool IsClustered
        {
            get { return (bool)base["isClustered"]; }
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
                    if (_LifeSpan == TimeSpan.Zero)
                    {
                        int totalSeconds = 0;
                        if (this.Seconds > 0)
                            totalSeconds += this.Seconds;
                        if (this.Minutes > 0)
                            totalSeconds += this.Minutes * 60;
                        _LifeSpan = TimeSpan.FromSeconds(totalSeconds);
                    }
                }
                return _LifeSpan.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is memcached.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is memcached; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("isMemcached", IsRequired = false, DefaultValue = false)]
        public bool IsMemcached
        {
            get { return (bool)base["isMemcached"]; }
        }

        /// <summary>
        /// Gets a value indicating whether to use protocol buffer serialization for memcached objects. This serialization technique
        /// has an average performance increase of about 20%, but the specified object must be decorated with ProtoContract attricutes.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [use protocol buffer serialization]; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("useProtocolBufferSerialization", IsRequired = false, DefaultValue = false)]
        public bool UseProtocolBufferSerialization
        {
            get { return (bool)base["useProtocolBufferSerialization"]; }
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