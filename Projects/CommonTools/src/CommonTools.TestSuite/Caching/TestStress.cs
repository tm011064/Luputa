using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using CommonTools.Components.Testing;
using System.Threading;

namespace CommonTools.Caching.Testing
{
    [TestFixture]
    public class TestStress : TestBase
    {
        enum TestCacheItem
        {
            slim_testitem_1,
            slim_testitem_2,
            slim_testitem_3,
            slim_testitem_4,
            slim_testitem_5
        }

        private int NUMBER_OF_ITEMS = 10000;

        private Dictionary<int, string> GetHugeDictionary(int numberOfItems)
        {
            Dictionary<int, string> records = new Dictionary<int, string>();
            for (int i = 0; i < numberOfItems; i++)
            {
                records.Add(i, "Huge Dictionary String " + i);
            }
            return records;
        }

        private List<int> GetHugeList(int numberOfItems)
        {
            List<int> records = new List<int>();
            for (int i = 0; i < numberOfItems; i++)
            {
                records.Add(i);
            }
            Thread.Sleep(100);
            return records;
        }

        private Dictionary<int, string> GetDictionaryFromCache(TestCacheItem testCacheItem, int numberOfItems)
        {
            Trace.WriteLine("Method call: GetDictionaryFromCache(TestCacheItem, int)");

            return SlimCacheManager.LoadFromCache<Dictionary<int, string>>(
                  testCacheItem.ToString()
                  , delegate { return GetHugeDictionary(numberOfItems); });
        }
        private Dictionary<int, string> TestCacheItem1CachedDictionary
        {
            get
            {
                return SlimCacheManager.LoadFromCache<Dictionary<int, string>>(
                      TestCacheItem.slim_testitem_1.ToString()
                      , delegate { return GetHugeDictionary(NUMBER_OF_ITEMS); });
            }
        }

        private void AddItemToCacheDictionary(int key)
        {
            if (!TestCacheItem1CachedDictionary.ContainsKey(key))
            {
                lock (TestCacheItem1CachedDictionary)
                {
                    if (!TestCacheItem1CachedDictionary.ContainsKey(key))
                    {
                        TestCacheItem1CachedDictionary.Add(key, "Huge Dictionary String " + key);
                    }
                }
            }
        }

        public void Test_ThreadAccess()
        {
            int numberOfThreads = 10;
            Random random = new Random();

            Thread[] threads = new Thread[numberOfThreads];

            for (int i = 0; i < numberOfThreads; i++)
            {
                    threads[i] = new Thread(Test_LoopThroughExpiredCachedListWithLocalCopyAndReinsertedValues);
            }

            for (int i = 0; i < numberOfThreads; i++)
            {
                threads[i].Start();
            }

            Thread.Sleep(10000);
        }



        [Test]
        public void Test_LoopThroughExpiredCachedList()
        {
            int numberOfItems = 10;
            string value;
            int half = numberOfItems / 2;

            SlimCacheManager.PurgeCacheItem(TestCacheItem.slim_testitem_1.ToString());
            Trace.WriteLine("\n\nStart Loop through direct access");
            foreach (int key in GetDictionaryFromCache(TestCacheItem.slim_testitem_1, numberOfItems).Keys)
            {
                value = GetDictionaryFromCache(TestCacheItem.slim_testitem_1, numberOfItems)[key];
                Trace.WriteLine("Successfully retrieved value: " + value);

                if (half == key)
                {
                    Trace.WriteLine("Remove object from cache");
                    SlimCacheManager.PurgeCacheItem(TestCacheItem.slim_testitem_1.ToString());
                    Assert.AreEqual(false, SlimCacheManager.IsObjectCached(TestCacheItem.slim_testitem_1.ToString()));
                }
            }
        }
        [Test]
        public void Test_LoopThroughExpiredCachedListWithLocalCopy()
        {
            int numberOfItems = 10;

            Dictionary<int, string> records = GetDictionaryFromCache(TestCacheItem.slim_testitem_1, numberOfItems);

            string value;
            int half = numberOfItems / 2;

            Trace.WriteLine("Start Loop through local copy");
            foreach (int key in records.Keys)
            {
                value = records[key];
                Trace.WriteLine("Successfully retrieved value: " + value);

                if (half == key)
                {
                    Trace.WriteLine("Remove object from cache");
                    SlimCacheManager.PurgeCacheItem(TestCacheItem.slim_testitem_1.ToString());
                    Assert.AreEqual(false, SlimCacheManager.IsObjectCached(TestCacheItem.slim_testitem_1.ToString()));
                }
            }
        }
        [Test]
        public void Test_LoopThroughExpiredCachedListWithLocalCopyAndReinsertedValues()
        {
            int numberOfItems = 10;

            Dictionary<int, string> records = GetDictionaryFromCache(TestCacheItem.slim_testitem_1, numberOfItems);

            string value;
            int half = numberOfItems / 2;


            SlimCacheManager.PurgeCacheItem(TestCacheItem.slim_testitem_1.ToString());
            records = GetDictionaryFromCache(TestCacheItem.slim_testitem_1, numberOfItems);
            Trace.WriteLine("\n\nStart Loop through local copy");
            foreach (int key in records.Keys)
            {
                if (half == key)
                {
                    Trace.WriteLine("Remove object from cache");
                    SlimCacheManager.PurgeCacheItem(TestCacheItem.slim_testitem_1.ToString());
                    Assert.AreEqual(false, SlimCacheManager.IsObjectCached(TestCacheItem.slim_testitem_1.ToString()));

                    Trace.WriteLine("Insert object into cache with half amount of items");
                    records = GetDictionaryFromCache(TestCacheItem.slim_testitem_1, half);
                    DebugUtility.GetDebugString(records);
                }

                //if (key >= half)
                //    Assert.Throws<KeyNotFoundException>(delegate { value = records[key]; });
                //else
                //{
                    value = records[key];
                    Trace.WriteLine("Successfully retrieved value: " + value);
                //}
            }
        }
        [Test]
        public void Test_LoopThroughExpiredCachedListWithReinsertedValues()
        {
            int numberOfItems = 10;
            string value;
            int half = numberOfItems / 2;

            SlimCacheManager.PurgeCacheItem(TestCacheItem.slim_testitem_1.ToString());
            Trace.WriteLine("\n\nStart Loop through direct access with change");
            foreach (int key in GetDictionaryFromCache(TestCacheItem.slim_testitem_1, numberOfItems).Keys)
            {
                if (half == key)
                {
                    Trace.WriteLine("Remove object from cache");
                    SlimCacheManager.PurgeCacheItem(TestCacheItem.slim_testitem_1.ToString());
                    Assert.AreEqual(false, SlimCacheManager.IsObjectCached(TestCacheItem.slim_testitem_1.ToString()));

                    Trace.WriteLine("Insert object into cache with half amount of items");
                    var records = GetDictionaryFromCache(TestCacheItem.slim_testitem_1, half);
                    DebugUtility.GetDebugString(records);
                }
                
                if (key >= half)
                    Assert.Throws<KeyNotFoundException>(delegate { value = GetDictionaryFromCache(TestCacheItem.slim_testitem_1, numberOfItems)[key]; });
                else
                {
                    value = GetDictionaryFromCache(TestCacheItem.slim_testitem_1, numberOfItems)[key];
                    Trace.WriteLine("Successfully retrieved value: " + value);
                }
            }
        }


        [Test]
        public void Test_LoopThroughExpiredCachedListWithLocalCopyAndNullCheck()
        {
            int numberOfItems = 10;

            Dictionary<int, string> records = GetDictionaryFromCache(TestCacheItem.slim_testitem_1, numberOfItems);

            string value;
            int half = numberOfItems / 2;


            SlimCacheManager.PurgeCacheItem(TestCacheItem.slim_testitem_1.ToString());
            records = GetDictionaryFromCache(TestCacheItem.slim_testitem_1, numberOfItems);
            Trace.WriteLine("\n\nStart Loop through local copy");
            foreach (int key in records.Keys)
            {
                if (half == key)
                {
                    Trace.WriteLine("Remove object from cache");
                    SlimCacheManager.PurgeCacheItem(TestCacheItem.slim_testitem_1.ToString());
                    Assert.AreEqual(false, SlimCacheManager.IsObjectCached(TestCacheItem.slim_testitem_1.ToString()));

                    Trace.WriteLine("Insert object into cache with half amount of items");
                    records = GetDictionaryFromCache(TestCacheItem.slim_testitem_1, half);
                    DebugUtility.GetDebugString(records);
                }

                if (records.ContainsKey(key))
                {
                    value = records[key];
                    Trace.WriteLine("Successfully retrieved value: " + value);
                }
            }
        }
    }
}
