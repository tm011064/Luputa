using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CommonTools.Components.Caching;
using System.Threading;
using System.Diagnostics;

namespace CommonTools.Caching.Testing
{
    [TestFixture]
    public class TestThreadSafety
    {
        class MockupCacheItem : ICacheItem
        {
            #region ICacheItem Members

            public System.Web.Caching.CacheItemPriority CacheItemPriority
            {
                get { throw new NotImplementedException(); }
            }

            public string CacheKey
            {
                get { throw new NotImplementedException(); }
            }

            public TimeSpan ContinuousAccessExtendedLifeSpan
            {
                get { throw new NotImplementedException(); }
            }

            public bool Enabled
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsClustered
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsIterating
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsMemcached
            {
                get { throw new NotImplementedException(); }
            }

            public TimeSpan LifeSpan
            {
                get { throw new NotImplementedException(); }
            }

            public int Minutes
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public string Name { get; set; }

            public int Seconds
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public string Suffix
            {
                get { throw new NotImplementedException(); }
            }

            public bool UseContinuousAccess
            {
                get { throw new NotImplementedException(); }
            }

            public bool UseProtocolBufferSerialization
            {
                get { throw new NotImplementedException(); }
            }

            #endregion
        }

        class ThreadHelper
        {
            List<CacheItemContainer> CacheItemContainers { get; set; }
            Random Random { get; set; }
            string Name { get; set; }
            int Count { get; set; }

            public void InitializeController()
            {
                CacheItemContainer container = CacheItemContainers[Random.Next(0, Count)];
                ContinuousCacheAccessSynchronizationManager.InitializeCacheItemSynchronizationController(container);

                Trace.WriteLine(Name + "Initialized    " + container.CacheKey);
            }

            public void IsFetchingData()
            {
                CacheItemContainer container = CacheItemContainers[Random.Next(0, Count)];
                try
                {
                    ContinuousCacheAccessSynchronizationManager.IsFetchingData(container.CacheKey);
                }
                catch (Exception err)
                {
                    Trace.WriteLine(Name + ": NULL REFERENCE FOR    " + container.CacheKey + err.Message);
                }
            }

            public ThreadHelper(List<CacheItemContainer> cacheItemContainers, Random random)
            {
                this.CacheItemContainers = cacheItemContainers;
                this.Random = random;
                this.Count = cacheItemContainers.Count;
            }
        }


        [Test]
        public void Test_SimultaneousAccess()
        {
            int numberOfContainers = 4;
            List<CacheItemContainer> cacheItemContainers = new List<CacheItemContainer>();
            for (int i = 0; i < numberOfContainers; i++)
            {
                cacheItemContainers.Add(new CacheItemContainer(
                    new MockupCacheItem() { Name = "CacheItem " + i }
                    , "CacheKey " + i
                    , CacheMode.InProcess
                    , null));
            }

            Random random = new Random();
            int numberOfThreads = 100;
            Thread[] threads = new Thread[numberOfThreads];

            for (int i = 0; i < numberOfThreads; i++)
            {
                ThreadHelper threadHelper = new ThreadHelper(cacheItemContainers, random);
                switch (random.Next(0, 2))
                {
                    case 0: threads[i] = new Thread(threadHelper.InitializeController); break;
                    case 1: threads[i] = new Thread(threadHelper.IsFetchingData); break;
                }
            }

            for (int i = 0; i < numberOfThreads; i++)
            {
                threads[i].Start();
            }

            Thread.Sleep(1000);
        }
    }
}
