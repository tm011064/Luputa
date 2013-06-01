using System;
using System.Web.Caching;
using System.Web;
using System.Web.Configuration;
using System.Collections;

namespace CommonTools.Components.Caching
{
    /// <summary>
    /// Provides the base class for objects which are to be cached.
    /// </summary>
    /// <typeparam name="T">The type of the object to be cached.</typeparam>
    public abstract class CacheObject<T>
    {
        #region abstracts
        /// <summary>
        /// Gets the object to be cached.
        /// </summary>
        /// <returns></returns>
        protected abstract object GetObject();
        #endregion

        #region globals
        private CacheElement _Item;
        private Cache _Cache = HttpRuntime.Cache;
        private CacheSection _Section;
        private bool _UseCache = false;
        private bool _IsInitialized = false;
        #endregion

        #region properties
        private string _EnumerationKey = string.Empty;
        private string Suffix { get { return (string.IsNullOrEmpty(_Item.Suffix) ? _Item.Name : _Item.Suffix); } }

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <value>The cache key.</value>
        protected string CacheKey
        {
            get
            {
                // get the cache key                
                if (_Item.IsIterating)
                {// cachekey is in the format { identifier }{ Suffix }
                    return _EnumerationKey + this.Suffix;
                }
                else
                {// cachekey is in the format { identifier }
                    return string.IsNullOrEmpty(_Item.CacheKey) ? _Item.Name : _Item.CacheKey;
                }
            }
        }
        #endregion

        #region private methods
        private bool TryRequestObject(out T requestedObject)
        {
            try
            {
                requestedObject = (T)this.GetObject();
                return true;
            }
            catch
            {
                requestedObject = default(T);
                return false;
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Sets the enumeration key. The enumeration key can only be set if the CacheObject was instanciated with the 'waitForInitialization'
        /// flag set to false. 
        /// </summary>
        /// <param name="key">
        /// The enumeration key is necessary for objects that must be instanciated with an identifier, e.g.:
        /// 
        ///     myApplicationUser user1 = new myApplicationUser(15);    // where the passed parameter '15' is the ID of the application user.
        ///     myApplicationUser user2 = new myApplicationUser(367);   // ...
        /// 
        /// Such object's cache keys are stored in the following format: { enumeration key } + { suffix }
        ///     -> e.g.: '15_MySuffix' and '367_MySuffix' (where the string '_MySuffix' is used as the suffix)
        /// 
        /// A benefit from this is that all cached objects of a specific type can be removed from cache by calling 
        /// the 'PurgeAllEnumeratingObjects();' method. 
        /// </param>
        public void SetEnumerationKey(string key)
        {
            // set the enumeration key
            _EnumerationKey = key;
            // init this object
            Init();
        }


        /// <summary>
        /// this method gets the requested object from cache. If the object can't be found at the Cache,
        /// a new object gets instanciated and then inserted into the system cache.
        /// </summary>
        /// <returns>the requested object</returns>
        public T Fetch()
        {
            return Fetch(true, false);
        }

        /// <summary>
        /// this method gets the requested object from cache. If the object can't be found at the Cache,
        /// a new object gets instanciated and then inserted into the system cache.
        /// </summary>
        /// <param name="useCache">
        /// Determines whether the requested object should be fetched from cache or newly instanciated.
        /// If this value is false, the requested object won't be inserted into the system cache either. 
        /// </param>
        /// <returns>the requested object</returns>
        public T Fetch(bool useCache)
        {
            return Fetch(useCache, false);
        }

        /// <summary>
        /// this method gets the requested object from cache. If the object can't be found at the Cache,
        /// a new object gets instanciated and then inserted into the system cache.
        /// </summary>
        /// <param name="useCache">
        /// Determines whether the requested object should be fetched from cache or newly instanciated.
        /// If this value is false, the requested object won't be inserted into the system cache either. 
        /// </param>
        /// <param name="flushAfterFetch">
        /// Determines whether the fetched object should be removed from the system cache after it was 
        /// fetched from cache.
        /// </param>
        /// <returns>the requested object</returns>
        public T Fetch(bool useCache, bool flushAfterFetch)
        {
            if (!_IsInitialized)
                throw new CachingException(
"Runtime Error at CacheObject<T>: The instanced CacheObject was not initialized. If a CacheObject is " +
"instanciated with the 'waitForInitialization' flag, this error will be thrown unless the EnumerationKey" +
" value is set via the SetEnumerationKey(string) method.");

            T returnObject;

            if (_UseCache && useCache && _Item.Enabled)
            {// use cache

                if (_Cache[this.CacheKey] == null)
                {// get new object and cache it

                    // get the object
                    if (this.TryRequestObject(out returnObject))
                    {
                        // cache the object  
                        SetCache(returnObject);
                    }
                                          
                }
                else
                {// get object from cache
                    returnObject = (T)_Cache[this.CacheKey];
                }
            }
            else
            {// don't use cache
                this.TryRequestObject(out returnObject);
            }

            // flush if enabled
            if (flushAfterFetch)
                PurgeItemFromCache();

            return returnObject;
        }

        /// <summary>
        /// This method removes this instanciated object from cache.
        /// </summary>
        public void PurgeItemFromCache()
        {
            CacheManager.PurgeCacheItemByKey(CacheKey);
        }

        /// <summary>
        /// This method removes all objects of this object's type from cache.
        /// </summary>
        public void PurgeAllEnumeratingObjects()
        {
            if (_Item.IsIterating)
                CacheManager.PurgeCacheItemsBySuffix(this.Suffix);
        }

        /// <summary>
        /// sets the current cache object
        /// </summary>
        /// <param name="cacheObject">object to cache</param>
        protected void SetCache(T cacheObject)
        {
            if (_UseCache && _Item.Enabled)
            {
                CacheManager.CacheData(
                            this.CacheKey
                            , cacheObject
                            , DateTime.UtcNow.AddMinutes(_Item.Minutes < 0 ? _Section.CacheElementCollection.Minutes : _Item.Minutes)
                            , _Item.CacheItemPriority);
            }
        }
        #endregion

        #region Constructors
        private void Init()
        {
            // get the name of this object
            string name = this.GetType().Name;

            // get the section
            _Section = CacheSectionManager.CacheSection;

            // get the element from web.config
            IEnumerator enumerator = _Section.CacheElementCollection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (((CacheElement)enumerator.Current).Name.Equals(name))
                {
                    _Item = ((CacheElement)enumerator.Current);
                    break;
                }
            }

            // throw exception if the element was not found
            if (_Item == null)
                throw new CachingException("Cache object \"" + name + "\" not found at the CacheElementCollection collection.");
            
            if (_Item.IsIterating && string.IsNullOrEmpty(_EnumerationKey))
                throw new CachingException("Enumeration key not set on an enumerating object. Please define the enumeration key at the constructor.");

            // get the global cachesettings
            if (_Item.Minutes <= 0 && _Item.Seconds <= 0)
                _Item.Minutes = _Section.CacheElementCollection.Minutes;

            _UseCache = _Section.CacheElementCollection.Enabled;

            _IsInitialized = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheObject&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="waitForEnumerationKey">if set to true, the enumeration key is not set. In order to use
        /// this class, the enumeration key must be set via the SetEnumerationKey(string) method, otherwise an exception will be thrown.</param>
        public CacheObject(bool waitForEnumerationKey)
        {
            if (!waitForEnumerationKey)
                Init();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheObject&lt;T&gt;"/> class.
        /// </summary>
        public CacheObject()
        {
            Init();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheObject&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="enumerationKey">
        /// The enumeration key is necessary for objects that must be instanciated with an identifier, e.g.:
        /// 
        ///     myApplicationUser user1 = new myApplicationUser(15);    // where the passed parameter '15' is the ID of the application user.
        ///     myApplicationUser user2 = new myApplicationUser(367);   // ...
        /// 
        /// Such object's cache keys are stored in the following format: { enumeration key } + { suffix }
        ///     -> e.g.: '15_MySuffix' and '367_MySuffix' (where the string '_MySuffix' is used as the suffix)
        /// 
        /// A benefit from this is that all cached objects of a specific type can be removed from cache by calling 
        /// the 'PurgeAllEnumeratingObjects();' method.     
        /// </param>
        public CacheObject(string enumerationKey)
        {
            _EnumerationKey = enumerationKey;
            Init();
        }

        #endregion
    }
}
