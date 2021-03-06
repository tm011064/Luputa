﻿using System;
using System.Web;
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
    /// <summary>
    /// This class contains all continuous cache access thread helper related data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class ContinuousCacheAccessThreadHelper<T> where T : class
    {
        #region members
        private LoadSerializedObjectDelegate<T> _LoadObjectDelegate;
        private string _CacheItemName;
        private string _IterationKey;
        #endregion

        #region methods
        /// <summary>
        /// Fetches data and inserts it into cache
        /// </summary>
        public void FetchAndInsertData()
        {
            CacheItemContainer cacheItemContainer = ContinuousCacheAccessSynchronizationManager.GetCacheItemContainer(_CacheItemName);
            string cacheKey = cacheItemContainer.CacheKey + _IterationKey;

            ContinuousCacheAccessSynchronizationManager.SetIsFetchingDataFlag(cacheKey, true);

            lock (ContinuousCacheAccessSynchronizationManager.CacheItemNameLocks[cacheItemContainer.CacheItem.Name])
            {
                #region debug
#if DEBUG
                Trace.WriteLine(DebugConstants.DEBUG_PREFIX + "SEPARATE THREAD: Starting FetchAndInsertData at " + DateTime.UtcNow.ToString("HH:mm:ss.fff"));
#endif
                #endregion

                T item = default(T);

                try { item = _LoadObjectDelegate.Invoke(); }
                catch (Exception e) { throw new CachingException("An error occurred during the execution of the loadObject delegate. See the inner exception for further details.", e); }

                if (item != null)
                {
                    cacheItemContainer.LatestFetch = DateTime.UtcNow;

                    CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                    cacheItemPolicy.Priority = cacheItemContainer.CacheItem.CacheItemPriority;
                    cacheItemPolicy.AbsoluteExpiration = cacheItemContainer.ActualExpiryDate;

                    SlimCacheManager._Cache.Set(
                        cacheKey
                        , item
                        , cacheItemPolicy);
                         
                    #region debug
#if DEBUG
                    Trace.WriteLine(DebugConstants.DEBUG_PREFIX + "\n***\n*INPROCESS CACHE INSERT:\n*\t\t\tInserted new object into cache at " + cacheItemContainer.LatestFetch.ToString("HH:mm:ss.fff")
                        + "\n*\t\t\tActual expiry date: " + cacheItemContainer.ActualExpiryDate.ToString("HH:mm:ss.fff")
                        + "\n*\t\t\tInserted new stale key with expiry date: " + cacheItemContainer.SpecifiedExpiryDate.ToString("HH:mm:ss.fff")
                        + "\n************************************************************************************************\n");
#endif
                    #endregion
                }
            }

            ContinuousCacheAccessSynchronizationManager.SetIsFetchingDataFlag(cacheKey, false);
        }
        #endregion

        #region constructors
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="loadObject">The load object.</param>
        /// <param name="cacheItemName">Name of the cache item.</param>
        /// <param name="iterationKey">The iteration key.</param>
        /// <returns></returns>
        internal static ContinuousCacheAccessThreadHelper<T> GetInstance(LoadSerializedObjectDelegate<T> loadObject, string cacheItemName, string iterationKey)
        {
            ContinuousCacheAccessThreadHelper<T> record = new ContinuousCacheAccessThreadHelper<T>();

            record._LoadObjectDelegate = loadObject;
            record._CacheItemName = cacheItemName;
            record._IterationKey = iterationKey ?? string.Empty;

            return record;
        }

        private ContinuousCacheAccessThreadHelper() { }
        #endregion
    }
}
