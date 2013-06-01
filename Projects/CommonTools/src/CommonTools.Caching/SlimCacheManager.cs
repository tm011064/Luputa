using System;
using System.Web;
using CommonTools.Components.Caching;
#region debug
#if DEBUG
using System.Diagnostics;
#endif
#endregion

namespace CommonTools.Caching
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
    /// This class controls all SlimCache related data. It makes use of the CommonTools.Components.Caching.CacheControllerFactory. It does not
    /// support clusetered cache items. Use this class for optimized performance.
    /// </summary>
    public static class SlimCacheManager
    {
        #region members
        private static ICacheController _StaticCacheController = CacheControllerFactory.CreateCacheController();
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
            // we try to get the item from a 
            if (!ContinuousCacheAccessSynchronizationManager.IsCacheItemContainerInitialized(cacheItemName))
            {
                ICacheItem item = _StaticCacheController.GetCacheItem(cacheItemName);

                // throw exception if the element was not found
                if (item == null)
                    throw new CachingException("Cache object \"" + cacheItemName + "\" not found at the CacheElementCollection.");
                if (item.IsClustered)
                    throw new CachingException("Cache Configuration item with key " + cacheItemName + " is configured as clustered. SlimCacheManager does not support clustered cache items. Use CacheManager instead.");

                // get the global cachesettings
                if (item.Minutes < 0 && item.Seconds < 0 && item.LifeSpan.TotalMilliseconds <= 0)
                    item.Minutes = _StaticCacheController.Minutes;

                if (item.LifeSpan == TimeSpan.Zero)
                    throw new CachingException("Cache Configuration item with key " + cacheItemName + " has no expiry time specified. Either set the Seconds or Minutes value. It is allowed to set both values at a time.");

                CacheMode cacheMode;
                if (item.IsMemcached)
                {
                    if (item.UseProtocolBufferSerialization)
                        cacheMode = CacheMode.MemcachedProtocolBufferSerialization;
                    else
                        cacheMode = CacheMode.Memcached;
                }
                else
                    cacheMode = CacheMode.InProcess;

                CacheItemContainer container = new CacheItemContainer(
                    item
                    , string.IsNullOrEmpty(item.CacheKey) ? item.Name : item.CacheKey
                    , cacheMode
                    , _StaticCacheController.ContinuousAccessStaleKeySuffixForMemcached);

                ContinuousCacheAccessSynchronizationManager.InitializeCacheItemSynchronizationController(container);
            }

            return ContinuousCacheAccessSynchronizationManager.GetCacheItemContainer(cacheItemName);
        }
        #endregion

        #region internal methods

        /// <summary>
        /// Gets the memcached object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cacheMode">The cache mode.</param>
        /// <returns></returns>
        internal static T GetMemcachedObject<T>(string cacheKey, CacheMode cacheMode) where T : class
        {
            return GetMemcachedObject<T>(cacheKey, null, cacheMode);
        }
        /// <summary>
        /// Gets the memcached object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="iterationKey">The iteration key.</param>
        /// <param name="cacheMode">The cache mode.</param>
        /// <returns></returns>
        internal static T GetMemcachedObject<T>(string cacheKey, string iterationKey, CacheMode cacheMode) where T : class
        {
            switch (cacheMode)
            {
                case CacheMode.Memcached: return DistributedCache.Get<T>(cacheKey + iterationKey ?? string.Empty);
                case CacheMode.MemcachedProtocolBufferSerialization: return DistributedCache.GetProtocolBufferObject<T>(cacheKey + iterationKey ?? string.Empty);

                default: throw new ArgumentException("CacheMode " + cacheMode + " not allowed");
            }
        }

        /// <summary>
        /// Inserts the memcached object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheItemContainer">The cache item container.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        internal static bool InsertMemcachedObject<T>(CacheItemContainer cacheItemContainer, T value) where T : class
        {
            return InsertMemcachedObject<T>(cacheItemContainer, null, value);
        }
        /// <summary>
        /// Inserts the memcached object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheItemContainer">The cache item container.</param>
        /// <param name="iterationKey">The iteration key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        internal static bool InsertMemcachedObject<T>(CacheItemContainer cacheItemContainer, string iterationKey, T value) where T : class
        {
            bool success;
            cacheItemContainer.LatestFetch = DateTime.UtcNow;

            switch (cacheItemContainer.CacheMode)
            {
                case CacheMode.Memcached:
                    success = DistributedCache.Insert(cacheItemContainer.CacheKey + iterationKey ?? string.Empty, value, cacheItemContainer.ActualExpiryDate);
                    break;

                case CacheMode.MemcachedProtocolBufferSerialization:
                    success = DistributedCache.InsertProtocolBufferObject<T>(cacheItemContainer.CacheKey + iterationKey ?? string.Empty, value, cacheItemContainer.ActualExpiryDate);
                    break;

                default: throw new ArgumentException("CacheMode " + cacheItemContainer.CacheMode + " not allowed");
            }
            if (cacheItemContainer.CacheItem.UseContinuousAccess
                && success)
            {// add the stale key...                        
                DistributedCache.Insert(cacheItemContainer.MemcachedStaleCacheKey + iterationKey ?? string.Empty, new object(), cacheItemContainer.SpecifiedExpiryDate);
            }

            #region debug
#if DEBUG
            Trace.WriteLine("\n" + DebugConstants.DEBUG_PREFIX + "\n*MEMCACHED INSERT:\n*\t\t\tInserted new object into cache at " + cacheItemContainer.LatestFetch.ToString("HH:mm:ss.fff")
                + "\n*\t\t\tActual expiry date: " + cacheItemContainer.ActualExpiryDate.ToString("HH:mm:ss.fff")
                + "\n*\t\t\tInserted new stale key with expiry date: " + cacheItemContainer.SpecifiedExpiryDate.ToString("HH:mm:ss.fff")
                + "\n*\t\t\tKey: " + cacheItemContainer.CacheKey
                + "\n************************************************************************************************\n");
#endif
            #endregion

            return success;
        }
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
                switch (cacheItemContainer.CacheMode)
                {
                    case CacheMode.InProcess:
                        return HttpRuntime.Cache[cacheItemContainer.CacheKey + iterationKey ?? string.Empty] != null;

                    case CacheMode.Memcached:
                    case CacheMode.MemcachedProtocolBufferSerialization:
                        return DistributedCache.Get(cacheItemContainer.CacheKey + iterationKey ?? string.Empty) != null;

                    default: throw new ArgumentException("CacheMode " + cacheItemContainer.CacheMode + " not allowed");
                }
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
                switch (cacheItemContainer.CacheMode)
                {
                    case CacheMode.InProcess:
                        HttpRuntime.Cache.Remove(cacheItemContainer.CacheKey + iterationKey ?? string.Empty);

                        #region debug
#if DEBUG
                        Trace.WriteLine(DebugConstants.DEBUG_PREFIX + "Removed item " + cacheItemContainer.CacheKey + " from cache");
#endif
                        #endregion

                        break;

                    case CacheMode.Memcached:
                    case CacheMode.MemcachedProtocolBufferSerialization:
                        DistributedCache.Remove(cacheItemContainer.CacheKey + iterationKey ?? string.Empty);

                        #region debug
#if DEBUG
                        Trace.WriteLine(DebugConstants.DEBUG_PREFIX + "Removed item " + cacheItemContainer.CacheKey + " from cache");
#endif
                        #endregion

                        break;

                    default: throw new ArgumentException("CacheMode " + cacheItemContainer.CacheMode + " not allowed");
                }
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

                switch (cacheItemContainer.CacheMode)
                {
                    case CacheMode.InProcess:

                        #region InProcess
                        returnObject = HttpRuntime.Cache[cacheKey] as T;
                        if (returnObject == null)
                        {
                            lock (ContinuousCacheAccessSynchronizationManager.CacheItemNameLocks[cacheItemName])
                            {
                                // check again in case the object was inserted into cache after the conditional statement but before the Monitor lock
                                returnObject = HttpRuntime.Cache[cacheKey] as T;

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

                                        HttpRuntime.Cache.Insert(
                                            cacheKey
                                            , returnObject
                                            , null
                                            , cacheItemContainer.ActualExpiryDate
                                            , System.Web.Caching.Cache.NoSlidingExpiration
                                            , cacheItemContainer.CacheItem.CacheItemPriority
                                            , null);

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

                        break;

                    case CacheMode.Memcached:
                    case CacheMode.MemcachedProtocolBufferSerialization:

                        #region Memcached

                        returnObject = GetMemcachedObject<T>(cacheKey, cacheItemContainer.CacheMode);
                        if (returnObject == null)
                        {
                            lock (ContinuousCacheAccessSynchronizationManager.CacheItemNameLocks[cacheItemName])
                            {
                                // check again in case the object was inserted into cache after the conditional statement but before the Monitor lock
                                returnObject = GetMemcachedObject<T>(cacheKey, cacheItemContainer.CacheMode);

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
                                        InsertMemcachedObject<T>(cacheItemContainer, returnObject);
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
                            #region Note on Memcached locking and synchronization
                            /* Important note on memcached caching:
                             
                             trying to accomplish thread safety via Memcached.Add is not thread safe and may not work as expected. Memcached.Add should only 
                             add an object if it does not exist yet, however, you never know when memcached actually inserts the data. If you write
                             code like ->
                              
                                  int i = 5;
                                  DistributedCache.Insert("MyKey", 5);
                                  int cachedValue = DistributedCache.Get<int>("MyKey");
                            
                             it is not guaranteed that "cachedValue" will be 5. During testing against a dev machine with memcache running as a
                             Windows XP service, it took up to 600ms until a value was actually retrievable after the insert. Here s a table with
                             tests which were run against the dev machine:
                            
        ,--.       ,---.            Add Test success percentage with 1 milliseconds: 0.3942
       /    '.    /     \           Add Test success percentage with 26 milliseconds: 0.416
              \  ;                  Add Test success percentage with 51 milliseconds: 0.4408
               \-|                  Add Test success percentage with 76 milliseconds: 0.4804
              (o o)      (/         Add Test success percentage with 101 milliseconds: 0.555
              /'v'     ,-'          Add Test success percentage with 126 milliseconds: 0.5548
      ,------/ >< \---'             Add Test success percentage with 151 milliseconds: 0.5556
     /)     ;  --  :                Add Test success percentage with 176 milliseconds: 0.5562
        ,---| ---- |--.             Add Test success percentage with 201 milliseconds: 0.6018
       ;    | ---- |   :            Add Test success percentage with 226 milliseconds: 0.6232
      (|  ,-| ---- |-. |)           Add Test success percentage with 251 milliseconds: 0.6
         | /| ---- |\ |             Add Test success percentage with 276 milliseconds: 0.6132
         |/ | ---- | \|             Add Test success percentage with 301 milliseconds: 0.6906
         \  : ---- ;  |             Add Test success percentage with 326 milliseconds: 0.738
          \  \ -- /  /              Add Test success percentage with 351 milliseconds: 0.7476
          ;   \  /  :               Add Test success percentage with 376 milliseconds: 0.76
         /   / \/ \  \              Add Test success percentage with 401 milliseconds: 0.7996
        /)           (\             Add Test success percentage with 426 milliseconds: 0.8068
                                    Add Test success percentage with 451 milliseconds: 0.848
                                    Add Test success percentage with 476 milliseconds: 0.8486                                    
                                    Add Test success percentage with 501 milliseconds: 0.86
                                    Add Test success percentage with 526 milliseconds: 0.9284
                                    Add Test success percentage with 551 milliseconds: 0.981
                                    Add Test success percentage with 576 milliseconds: 0.9554
                                    Add Test success percentage with 601 milliseconds: 1
                                    Add Test success percentage with 626 milliseconds: 1
                                    Add Test success percentage with 651 milliseconds: 1
                                    Add Test success percentage with 676 milliseconds: 1
                                    Add Test success percentage with 701 milliseconds: 1
                                    Add Test success percentage with 726 milliseconds: 1
                                    Add Test success percentage with 751 milliseconds: 1
                                    Add Test success percentage with 776 milliseconds: 1
                                    Add Test success percentage with 801 milliseconds: 1
                                    Add Test success percentage with 826 milliseconds: 1
                                    Add Test success percentage with 851 milliseconds: 1
                                    Add Test success percentage with 876 milliseconds: 1
                                    Add Test success percentage with 901 milliseconds: 1
                                    Add Test success percentage with 926 milliseconds: 1
                                    Add Test success percentage with 951 milliseconds: 1
                                    Add Test success percentage with 976 milliseconds: 1
                             
                             However, this is still better than aquiring a lock from a database table which would be thread safe but cause an enormous
                             amount of SQL traffic. Using a windows service on a dedicated machine which handles the locking would be best but it also
                             eats a lot of network bandwidth.                             
                                                                    
                            */
                            #endregion

                            if (cacheItemContainer.CacheItem.UseContinuousAccess
                                && DistributedCache.Get<object>(cacheItemContainer.MemcachedStaleCacheKey) == null)
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

                        break;
                }
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
