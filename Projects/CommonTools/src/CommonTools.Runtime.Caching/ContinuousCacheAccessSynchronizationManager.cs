using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
#region debug
#if DEBUG
using System.Diagnostics;
#endif
#endregion

namespace CommonTools.Runtime.Caching
{
    static class ContinuousCacheAccessSynchronizationManager
    {
        #region members
        private static Hashtable _FetchDataLockTable = new Hashtable();
        private static Dictionary<string, bool> _IsFetchingDataLookup = new Dictionary<string, bool>();
        private static Dictionary<string, CacheItemContainer> _CacheItems = new Dictionary<string, CacheItemContainer>();
        #endregion

        #region properties
        private static Dictionary<string, object> _CacheItemNameLocks = new Dictionary<string, object>();
        public static Dictionary<string, object> CacheItemNameLocks
        {
            get { return _CacheItemNameLocks; }
        }
        #endregion

        #region methods
        public static CacheItemContainer GetCacheItemContainer(string cacheItemName)
        {
            try { return _CacheItems[cacheItemName]; }
            catch (KeyNotFoundException) { throw new NullReferenceException("CacheItem with name " + cacheItemName + " was not initialized and therefore can't be used."); }
        }
        public static bool IsCacheItemContainerInitialized(string cacheItemName)
        {
            return _CacheItems.ContainsKey(cacheItemName);
        }

        public static void InitializeCacheItemSynchronizationController(CacheItemContainer cacheItemContainer)
        {
            if (!_FetchDataLockTable.ContainsKey(cacheItemContainer.CacheKey))
            {
                lock (_FetchDataLockTable)
                {
                    if (!_FetchDataLockTable.ContainsKey(cacheItemContainer.CacheKey))
                    {
                        _FetchDataLockTable.Add(cacheItemContainer.CacheKey, new object());
                        _IsFetchingDataLookup.Add(cacheItemContainer.CacheKey, false);

                        _CacheItemNameLocks.Add(cacheItemContainer.CacheItem.Name, new object());
                        _CacheItems.Add(cacheItemContainer.CacheItem.Name, cacheItemContainer);
                    }
                }
            }
        }

        public static bool IsFetchingData(string cacheKey)
        {
            try { lock (_FetchDataLockTable[cacheKey]) { return _IsFetchingDataLookup[cacheKey]; } }
            catch (ArgumentNullException) { throw new NullReferenceException("CacheItem with key " + cacheKey + " was not initialized and therefore can't be used."); }
        }
        public static void SetIsFetchingDataFlag(string cacheKey, bool isFetchingData)
        {
            try { lock (_FetchDataLockTable[cacheKey]) { _IsFetchingDataLookup[cacheKey] = isFetchingData; } }
            catch (ArgumentNullException) { throw new NullReferenceException("CacheItem with key " + cacheKey + " was not initialized and therefore can't be used."); }
        }

        public static TriggerAsynchronousFetchStatus TriggerAsynchronousFetch<T>(LoadSerializedObjectDelegate<T> loadObject, string cacheItemName
            , string iterationKey, CacheItemContainer cacheItemContainer)
            where T : class
        {
            ContinuousCacheAccessThreadHelper<T> threadHelper;
            Thread thread;

            // we are in the extended lifespan, so we need to check whether we have to reload the object
            if (ContinuousCacheAccessSynchronizationManager.SetIsFetchingDataFlagToTrue(cacheItemContainer.CacheKey))
            {
                #region debug
#if DEBUG
                Trace.WriteLine(DebugConstants.DEBUG_PREFIX + "TRIGGER ASYNCHRONOUS FETCH: Initialize asynchronous fetch");
#endif
                #endregion

                threadHelper = ContinuousCacheAccessThreadHelper<T>.GetInstance(loadObject, cacheItemName, iterationKey);
                thread = new Thread(threadHelper.FetchAndInsertData);
                thread.Start();
                return TriggerAsynchronousFetchStatus.SucessfullyInitialized;
            }
            else
            {
                #region debug
#if DEBUG
                Trace.WriteLine(DebugConstants.DEBUG_PREFIX + "TRIGGER ASYNCHRONOUS FETCH: Already in process...");
#endif
                #endregion

                return TriggerAsynchronousFetchStatus.AlreadyFetching;
            }

        }
        #endregion

        #region private methods
        private static bool SetIsFetchingDataFlagToTrue(string cacheKey)
        {
            lock (_FetchDataLockTable[cacheKey])
            {
                try
                {
                    if (!_IsFetchingDataLookup[cacheKey])
                    {
                        _IsFetchingDataLookup[cacheKey] = true;
                        return true;
                    }
                    return false;
                }
                catch (KeyNotFoundException) { throw new NullReferenceException("CacheItem with key " + cacheKey + " was not initialized and therefore can't be used."); }
            }
        }
        #endregion
    }
}