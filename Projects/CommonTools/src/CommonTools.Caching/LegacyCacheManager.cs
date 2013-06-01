using System;
using System.Web;
using CommonTools.Components.Caching;

namespace CommonTools.Caching
{
    /// <summary>
    /// This class contains all cache controller related data
    /// </summary>
    public static class LegacyCacheManager
    {
        #region static members

        private static ICacheController _CacheController;
        private static ICacheController CacheControllerSection
        {
            get
            {
                if (_CacheController == null)
                    _CacheController = CacheControllerFactory.CreateCacheController();

                return _CacheController;
            }
        }
        #endregion
        
        #region cache controller methods

        #region private methods
        private static string GetCacheSuffix(ICacheItem item)
        {
            return (string.IsNullOrEmpty(item.Suffix) ?
                            (string.IsNullOrEmpty(item.CacheKey) ?
                                    item.Name
                                : item.CacheKey)
                        : item.Suffix);
        }
        private static string GetCacheKey(ICacheItem item)
        {
            return GetCacheKey(item, null);
        }
        private static string GetCacheKey(ICacheItem item, string iterationKey)
        {
            if (item.IsIterating && String.IsNullOrEmpty(iterationKey))
                throw new CachingException("Iteration key not provided for an iterating cache item");

            // get the cache key                
            if (item.IsIterating)
            {// cachekey is in the format { identifier }{ Suffix }
                return (iterationKey + GetCacheSuffix(item));
            }
            else
            {// cachekey is in the format { identifier }
                return (string.IsNullOrEmpty(item.CacheKey) ? item.Name : item.CacheKey);
            }
        }
        private static ICacheItem CreateSaveItemInstance(ICacheController controller, string configSectionKey, string iterationKey)
        {
            ICacheItem item = controller.GetCacheItem(configSectionKey);

            // throw exception if the element was not found
            if (item == null)
                throw new CachingException("Cache object \"" + configSectionKey + "\" not found at the CacheElementCollection.");
            if (item.IsIterating && string.IsNullOrEmpty(iterationKey))
                throw new CachingException("Iteration key not provided for an iterating cache item");

            // get the global cachesettings
            if (item.Minutes < 0 && item.Seconds < 0)
                item.Minutes = controller.Minutes;

            return item;
        }
        #endregion

        #region delegates
        /// <summary>
        /// This delegate is used to get a new instance of the BusinessObjectManager's business object if it can't be loaded
        /// from cache.
        /// </summary>
        /// <returns>A new business object instance.</returns>
        public delegate T LoadSerializedObjectDelegate<T>();
        #endregion

        #region members
        private static object _CacheLock = new object();
        #endregion

        #region public methods

        #region purge

        /// <summary>
        /// Purges the cache item.
        /// </summary>
        /// <param name="name">The name.</param>
        public static void PurgeCacheItem(string name)
        {
            PurgeCacheItem(name, null);
        }

        /// <summary>
        /// Purges the cache item.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="iterationKey">The iteration key.</param>
        public static void PurgeCacheItem(string name, string iterationKey)
        {
            PurgeItemFromCache(name, iterationKey);
        }

        private static void PurgeItemFromCache(string name, string iterationKey)
        {
            ICacheItem item = CreateSaveItemInstance(CacheControllerSection, name, iterationKey);

            if (item.IsClustered)
                ClusteredCacheManager.PurgeCacheItem(GetCacheKey(item, iterationKey));
            else
                HttpRuntime.Cache.Remove(GetCacheKey(item, iterationKey));
        }
        #endregion

        #region cache item
        /// <summary>
        /// Inserts the cache item.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="iterationKey">The iteration key.</param>
        /// <param name="item">The item.</param>
        public static void InsertCacheItem(string name, string iterationKey, object item)
        {
            ICacheItem cacheitem = CreateSaveItemInstance(CacheControllerSection, name, iterationKey);

            string cacheKey = GetCacheKey(cacheitem, iterationKey);

            lock (_CacheLock)
            {
                HttpRuntime.Cache.Remove(cacheKey);

                if (cacheitem.Minutes < 0 && cacheitem.Seconds < 0)
                    throw new CachingException("You must specify either a seconds or minutes value for a non clusterd cache item.");

                DateTime expire = cacheitem.Seconds < 0 ? DateTime.UtcNow.AddMinutes(cacheitem.Minutes) : DateTime.UtcNow.AddSeconds(cacheitem.Seconds);
                CacheManager.CacheData(cacheKey, item, expire, cacheitem.CacheItemPriority);
            }
        }
        #endregion

        #region is cached query

        /// <summary>
        /// Determines whether [is object cached] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// 	<c>true</c> if [is object cached] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsObjectCached(string name)
        {
            return IsObjectCached(name, null);
        }

        /// <summary>
        /// Determines whether [is object cached] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="iterationKey">The iteration key.</param>
        /// <returns>
        /// 	<c>true</c> if [is object cached] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsObjectCached(string name, string iterationKey)
        {
            return IsInCache(name, iterationKey);
        }
        private static bool IsInCache(string name, string iterationKey)
        {
            ICacheItem item = CreateSaveItemInstance(CacheControllerSection, name, iterationKey);

            return HttpRuntime.Cache[GetCacheKey(item, iterationKey)] != null;
        }

        #endregion

        /// <summary>
        /// Loads from cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="loadObject">The load object.</param>
        /// <returns></returns>
        public static T LoadFromCache<T>(string name, LoadSerializedObjectDelegate<T> loadObject)
        {
            return LoadObjectFromCache<T>(name, null, loadObject);
        }
        /// <summary>
        /// Loads from cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="iterationKey">The iteration key.</param>
        /// <param name="loadObject">The load object.</param>
        /// <returns></returns>
        public static T LoadFromCache<T>(string name, string iterationKey, LoadSerializedObjectDelegate<T> loadObject)
        {
            return LoadObjectFromCache<T>(name, iterationKey, loadObject);
        }

        /// <summary>
        /// Loads the object from cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="iterationKey">The iteration key.</param>
        /// <param name="loadObject">The load object.</param>
        /// <returns></returns>
        private static T LoadObjectFromCache<T>(string name, string iterationKey, LoadSerializedObjectDelegate<T> loadObject)
        {
            T returnObject = default(T);

            // get the section                
            ICacheController section = CacheControllerSection;
            ICacheItem item = CreateSaveItemInstance(section, name, iterationKey);

            string cacheKey = GetCacheKey(item, iterationKey);
            if (section.Enabled && item.Enabled)
            {// use cache

                if (item.IsClustered)
                {
                    try
                    {
                        returnObject = (T)ClusteredCacheManager.GetCacheItem(
                            cacheKey
                            , delegate { return loadObject(); }
                            , item.CacheItemPriority
                            , true);
                    }
                    catch (Exception e)
                    {
                        throw new CachingException("An error occurred during the retrieval of the business object from the cache collection. See the inner exception for further details.", e);
                    }
                }
                else
                {
                    lock (_CacheLock)
                    {
                        if (HttpRuntime.Cache[cacheKey] == null)
                        {// get new object and cache it

                            try
                            {
                                if (item.Minutes < 0 && item.Seconds < 0)
                                    throw new CachingException("You must specify either a seconds or minutes value for a non clusterd cache item.");

                                DateTime expire = item.Seconds < 0 ? DateTime.UtcNow.AddMinutes(item.Minutes) : DateTime.UtcNow.AddSeconds(item.Seconds);

                                returnObject = loadObject();
                                if (returnObject != null)
                                {
                                    HttpRuntime.Cache.Remove(cacheKey);
                                    HttpRuntime.Cache.Insert(cacheKey, returnObject, null, expire, System.Web.Caching.Cache.NoSlidingExpiration, item.CacheItemPriority, null);
                                }
                            }
                            catch (Exception e)
                            {
                                throw new CachingException("An error occurred during the execution of the loadObject delegate. See the inner exception for further details.", e);
                            }
                        }
                        else
                        {// get object from cache
                            try
                            {
                                returnObject = (T)HttpRuntime.Cache[cacheKey];
                            }
                            catch (Exception e)
                            {
                                throw new CachingException("An error occurred during the retrieval of the business object from the cache collection. See the inner exception for further details.", e);
                            }
                        }
                    }
                }
            }
            else
            {// don't use cache
                try
                {
                    returnObject = loadObject();
                }
                catch (Exception e)
                {
                    throw new CachingException("An error occurred during the execution of the loadObject delegate. See the inner exception for further details.", e);
                }
            }

            return returnObject;
        }
        #endregion

        #endregion
    }
}