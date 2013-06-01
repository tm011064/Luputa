using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Caching;
using CommonTools.Runtime.Caching.Configuration;
using CommonTools.TestSuite.RuntimeCaching;
using CommonTools.Runtime.Caching;

namespace CommonTools.TestSuite.RuntimeCaching.Tests
{
    [TestFixture]
    public class TestThreadSafety
    {
        class ThreadHelper
        {
            private SimpleObject GetCacheItem(int index)
            {
                return SlimCacheManager.LoadFromCache<SimpleObject>(
                      "TestItem"
                      , delegate
                      {
                          Trace.WriteLine("DELEGATE CALL TO: ___START___  GetCacheItem, INDEX: " + index);
                          Thread.Sleep(200);
                          Trace.WriteLine("DELEGATE CALL TO: ___FINISH___  GetCacheItem, INDEX: " + index);
                          return new SimpleObject()
                          {
                              MyString = "________" + index + "_________________________________________________ " + DateTime.UtcNow.ToString("HH:mm:ss.fff"),
                              MyValue = index
                          };
                      });
            }


            public void LoadData()
            {
                using (CommonTools.Components.Testing.DisposableStopwatch sw = new Components.Testing.DisposableStopwatch(_Name, Components.Testing.DisposableStopwatch.TraceMode.Milliseconds))
                {
                    for (int i = 0; i < 100000; i++)
                    {
                        sw.Start();
                        SimpleObject simpleObject = GetCacheItem(i);
                        sw.Stop();
                    }
                }
            }

            string _Name;

            public ThreadHelper(string name)
            {
                _Name = name;
            }
        }


        [Test]
        public void Test_SimultaneousAccess()
        {
            int count = 10;
            Thread[] threads = new Thread[count];

            for (int i = 0; i < count; i++)
            {
               ThreadHelper threadHelper = new ThreadHelper(i.ToString());
               threads[i] = new Thread(new ThreadStart(threadHelper.LoadData));
            }

            for (int i = 0; i < count; i++)
            {
                threads[i].Start();
            }

            for (int i = 0; i < count; i++)
            {
                threads[i].Join();
            }          
        }
    }


 
}
