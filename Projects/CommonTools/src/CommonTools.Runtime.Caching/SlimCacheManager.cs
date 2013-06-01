using System;
using CommonTools.Runtime.Caching.Configuration;
using System.Runtime.Caching;
#region debug
#if DEBUG
using System.Diagnostics;
using System.Runtime.Caching;
#endif
#endregion

namespace CommonTools.Runtime.Caching
{
    #region delegates
    /// <summary>
    /// This delegate is used to get a new instance of the BusinessObjectManager's business object if it can't be loaded
    /// from cache.
    /// </summary>
    /// <returns>A new business object instance.</returns>
    public delegate T LoadSerializedObjectDelegate<T>();
    #endregion

    /// <summary>
    /// This class controls all SlimCache related data. It makes use of the CommonTools.Runtime.Caching.Configuration.CacheControllerFactory. It does not
    /// support clusetered cache items. Use this class for optimized performance.
    /// </summary>
    public static class SlimCacheManager
    {
        #region members
        private static ICacheController _StaticCacheController;
        internal static readonly ObjectCache _Cache;
        #endregion

        #region properties

        #endregion

        #region constructors
        static SlimCacheManager()
        {
            _StaticCacheController = CacheControllerFactory.CreateCacheController();
            _Cache = MemoryCache.Default;

            CacheItemContainer container;
            foreach (ICacheItem item in _StaticCacheController.IterateOverCacheItems())
            {
                // get the global cachesettings
                if (item.LifeSpan.TotalMilliseconds <= 0)
                    item.LifeSpan = _StaticCacheController.LifeSpanInterval;

                if (item.LifeSpan == TimeSpan.Zero)
                    throw new CachingException("Cache Configuration item with key " + item.Name + " has no expiry time specified. Either set the Seconds or Minutes value. It is allowed to set both values at a time.");

                container = new CacheItemContainer(
                   item
                   , string.IsNullOrEmpty(item.CacheKey) ? item.Name : item.CacheKey);

                ContinuousCacheAccessSynchronizationManager.InitializeCacheItemSynchronizationController(container);
            }
        }
        #endregion

        #region cache controller methods

        #region private methods
        /// <summary>
        /// Creates the save item instance.
        /// </summary>
        /// <param name="cacheItemName">Name of the cache item.</param>
        /// <returns></returns>
        private static CacheItemContainer CreateSaveItemInstance(string cacheItemName)
        {
            // we don't check whether the specified name acutally exists but just throw a key not found exception
            return ContinuousCacheAccessSynchronizationManager.GetCacheItemContainer(cacheItemName);
        }
        #endregion

        #region internal methods


        #endregion

        #region public methods

        /// <summary>
        /// Determines whether [is object cached] [the specified cache item name].
        /// </summary>
        /// <param name="cacheItemName">Name of the cache item.</param>
        /// <returns>
        /// 	<c>true</c> if [is object cached] [the specified cache item name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsObjectCached(string cacheItemName)
        {
            return IsObjectCached(cacheItemName, null);
        }
        /// <summary>
        /// Determines whether [is object cached] [the specified cache item name].
        /// </summary>
        /// <param name="cacheItemName">Name of the cache item.</param>
        /// <param name="iterationKey">The iteration key.</param>
        /// <returns>
        /// 	<c>true</c> if [is object cached] [the specified cache item name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsObjectCached(string cacheItemName, string iterationKey)
        {
            CacheItemContainer cacheItemContainer = CreateSaveItemInstance(cacheItemName);
            if (cacheItemContainer != null)
            {
                return _Cache[cacheItemContainer.CacheKey + iterationKey ?? string.Empty] != null;
            }

            return false;
        }

        /// <summary>
        /// Purges the cache item.
        /// </summary>
        /// <param name="cacheItemName">Name of the cache item.</param>
        public static void PurgeCacheItem(string cacheItemName)
        {
            PurgeCacheItem(cacheItemName, null);
        }
        /// <summary>
        /// Purges the cache item.
        /// </summary>
        /// <param name="cacheItemName">Name of the cache item.</param>
        /// <param name="iterationKey">The iteration key.</param>
        public static void PurgeCacheItem(string cacheItemName, string iterationKey)
        {
            CacheItemContainer cacheItemContainer = CreateSaveItemInstance(cacheItemName);
            if (cacheItemContainer != null)
            {
                _Cache.Remove(cacheItemContainer.CacheKey + iterationKey ?? string.Empty);

                #region debug
#if DEBUG
                Trace.WriteLine(DebugConstants.DEBUG_PREFIX + "Removed item " + cacheItemContainer.CacheKey + " from cache");
#endif
                #endregion
            }
        }

        /// <summary>
        /// Loads from cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheItemName">Name of the cache item.</param>
        /// <param name="loadObject">The load object.</param>
        /// <returns></returns>
        public static T LoadFromCache<T>(string cacheItemName, LoadSerializedObjectDelegate<T> loadObject) where T : class
        {
            return LoadFromCache<T>(cacheItemName, null, loadObject);
        }
        /// <summary>
        /// Loads from cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheItemName">Name of the cache item.</param>
        /// <param name="iterationKey">The iteration key.</param>
        /// <param name="loadObject">The load object.</param>
        /// <returns></returns>
        public static T LoadFromCache<T>(string cacheItemName, string iterationKey, LoadSerializedObjectDelegate<T> loadObject) where T : class
        {
            T returnObject = default(T);

            // get the section                
            CacheItemContainer cacheItemContainer = CreateSaveItemInstance(cacheItemName);
            string cacheKey = cacheItemContainer.CacheKey + iterationKey ?? string.Empty;

            if (_StaticCacheController.Enabled && cacheItemContainer.CacheItem.Enabled)
            {// use cache

                #region InProcess
                returnObject = _Cache[cacheKey] as T;
                if (returnObject == null)
                {
                    lock (ContinuousCacheAccessSynchronizationManager.CacheItemNameLocks[cacheItemName])
                    {
                        // check again in case the object was inserted into cache after the conditional statement but before the Monitor lock
                        returnObject = _Cache[cacheKey] as T;

                        if (returnObject == null)
                        {// get new object and cache it

                            #region debug
#if DEBUG
                            Trace.WriteLine(DebugConstants.DEBUG_PREFIX + "Item not found at cache");
#endif
                            #endregion

                            try { returnObject = loadObject.Invoke(); }
                            catch (Exception e) { throw new CachingException("An error occurred during the execution of the loadObject delegate. See the inner exception for further details.", e); }

                            if (returnObject != null)
                            {
                                cacheItemContainer.LatestFetch = DateTime.UtcNow;

                                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                                cacheItemPolicy.Priority = cacheItemContainer.CacheItem.CacheItemPriority;
                                cacheItemPolicy.AbsoluteExpiration = cacheItemContainer.ActualExpiryDate;

                                SlimCacheManager._Cache.Set(
                                    cacheKey
                                    , returnObject
                                    , cacheItemPolicy);

                                #region debug
#if DEBUG
                                Trace.WriteLine("\n" + DebugConstants.DEBUG_PREFIX + "\n*INPROCESS CACHE INSERT:\n*\t\t\tInserted new object into cache at " + cacheItemContainer.LatestFetch.ToString("HH:mm:ss.fff")
                                    + "\n*\t\t\tActual expiry date: " + cacheItemContainer.ActualExpiryDate.ToString("HH:mm:ss.fff")
                                    + "\n*\t\t\tInserted new stale key with expiry date: " + cacheItemContainer.SpecifiedExpiryDate.ToString("HH:mm:ss.fff")
                                    + "\n*\t\t\tKey: " + cacheItemContainer.CacheKey
                                    + "\n************************************************************************************************\n");
#endif
                                #endregion
                            }

                            #region debug
#if DEBUG
                            Trace.WriteLine(DebugConstants.DEBUG_PREFIX + "Item inserted at normal cache call");
#endif
                            #endregion

                        }
                    }
                }
                else
                {
                    if (cacheItemContainer.CacheItem.UseContinuousAccess &&
                        cacheItemContainer.IsInExtendedLifeSpan(DateTime.UtcNow))
                    {
                        #region debug
#if DEBUG
                        Trace.WriteLine(DebugConstants.DEBUG_PREFIX + "IsInExtendedLifeSpan at " + DateTime.UtcNow.ToString("HH:mm:ss.fff"));
#endif
                        #endregion

                        // we are in the extended lifespan, so we need to check whether we have to reload the object
                        ContinuousCacheAccessSynchronizationManager.TriggerAsynchronousFetch<T>(loadObject, cacheItemName, iterationKey, cacheItemContainer);
                    }
                }
                #endregion
            }
            else
            {// don't use cache
                try { returnObject = loadObject.Invoke(); }
                catch (Exception e) { throw new CachingException("An error occurred during the execution of the loadObject delegate. See the inner exception for further details.", e); }
            }

            return returnObject;
        }
        #endregion

        #endregion
    }
}
