using System;
using System.Web.Caching;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace CommonTools.Components.Caching
{
    /// <summary>
    /// Summary description for CacheManager
    /// </summary>
    public static class CacheManager
    {
        #region properties
        private static Cache Cache
        {
            get { return HttpRuntime.Cache; }
        }
        #endregion

        #region public static methods
        /// <summary>
        /// Remove from the ASP.NET cache all items whose key starts with the input prefix
        /// </summary>
        public static void PurgeCacheItemsByPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                return;

            prefix = prefix.ToLower();
            List<string> itemsToRemove = new List<string>();

            IDictionaryEnumerator enumerator = CacheManager.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString().ToLowerInvariant().StartsWith(prefix))
                    itemsToRemove.Add(enumerator.Key.ToString());
            }

            foreach (string itemToRemove in itemsToRemove)
                CacheManager.Cache.Remove(itemToRemove);
        }
        /// <summary>
        /// Remove from the ASP.NET cache all items whose key ends with the input suffix
        /// </summary>
        public static void PurgeCacheItemsBySuffix(string suffix)
        {
            if (string.IsNullOrEmpty(suffix))
                return;

            suffix = suffix.ToLower();
            List<string> itemsToRemove = new List<string>();

            IDictionaryEnumerator enumerator = CacheManager.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString().ToLowerInvariant().EndsWith(suffix))
                    itemsToRemove.Add(enumerator.Key.ToString());
            }

            foreach (string itemToRemove in itemsToRemove)
                CacheManager.Cache.Remove(itemToRemove);
        }

        /// <summary>
        /// Remove from the ASP.NET cache the item with the given key
        /// </summary>
        public static void PurgeCacheItemByKey(string key)
        {
            CacheManager.Cache.Remove(key);
        }

        /// <summary>
        /// Cache the input data, if caching is enabled
        /// </summary>
        public static void CacheData(string key, object data, DateTime expire)
        {
            if (CacheManager.Cache[key] != null)
                PurgeCacheItemByKey(key);

            CacheManager.Cache.Insert(key, data, null,
                expire, TimeSpan.Zero);
        }

        /// <summary>
        /// Cache the input data, if caching is enabled
        /// </summary>
        public static void CacheData(string key, object data, DateTime expire, CacheItemPriority pPriority)
        {
            if (CacheManager.Cache[key] != null)
                PurgeCacheItemByKey(key);

            CacheManager.Cache.Insert(
                key
                , data
                , null
                , expire
                , Cache.NoSlidingExpiration
                , pPriority
                , null);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
        public static string GetCacheElementSuffix(string name)
        {
            // get the section
            CacheSection section = CacheSectionManager.CacheSection;
            CacheElement item = null;
            // get the element from web.config
            IEnumerator enumerator = section.CacheElementCollection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (((CacheElement)enumerator.Current).Name.Equals(name))
                {
                    item = ((CacheElement)enumerator.Current);
                    break;
                }
            }

            // throw exception if the element was not found
            if (item == null)
                throw new CachingException("Cache object \"" + name + "\" not found at the CacheElementCollection collection.");

            return item.Suffix;
        }
        #endregion
    }
}
