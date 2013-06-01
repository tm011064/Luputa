using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading;
using System.Diagnostics;
using CommonTools.TestSuite.RuntimeCaching;
using CommonTools.Runtime.Caching;

namespace CommonTools.TestSuite.RuntimeCaching.Tests
{
    [TestFixture]
    public class TestContinuousAccess
    {
        private SimpleObject GetCacheItem(int index)
        {
            Trace.WriteLine("*************** GetCacheItem call at: " + DateTime.UtcNow.ToString("HH:mm:ss.fff") + " ******************");
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

        private void Sleep(int milliseconds)
        {
            Trace.WriteLine("\n\nSleep for " + milliseconds + " ms at " + DateTime.UtcNow.ToString("HH:mm:ss.fff") + "...");
            Thread.Sleep(milliseconds);
            Trace.WriteLine("... Awake from sleeping " + milliseconds + " ms at " + DateTime.UtcNow.ToString("HH:mm:ss.fff") + "\n\n");
        }

        public void Test1()
        {
            int index = 0;

            Trace.WriteLine(GetCacheItem(++index).MyString + "\n");
            Trace.WriteLine(GetCacheItem(++index).MyString + "\n");

            Sleep(2100);

            Trace.WriteLine(GetCacheItem(++index).MyString + "\n");
            Trace.WriteLine(GetCacheItem(++index).MyString + "\n");

            Sleep(2100);

            Trace.WriteLine(GetCacheItem(++index).MyString + "\n");

            Sleep(400);

            Trace.WriteLine(GetCacheItem(++index).MyString + "\n");
            Trace.WriteLine(GetCacheItem(++index).MyString + "\n");

            Sleep(250);

            Trace.WriteLine(GetCacheItem(++index).MyString + "\n");
            Trace.WriteLine(GetCacheItem(++index).MyString + "\n");

            Sleep(2500);

            Trace.WriteLine(GetCacheItem(++index).MyString + "\n");
            Trace.WriteLine(GetCacheItem(++index).MyString + "\n");

            Thread.Sleep(2000);
        }


        private SimpleObject GetCacheItem2()
        {
            Trace.WriteLine("*************** GetCacheItem2 call at: " + DateTime.UtcNow.ToString("HH:mm:ss.fff") + " ******************");
            return SlimCacheManager.LoadFromCache<SimpleObject>(
                  "TestItem2"
                  , delegate
                  {
                      Trace.WriteLine("DELEGATE CALL TO: ___FINISH___  GetCacheItem2");
                      return new SimpleObject()
                      {
                          MyString = "_________________________________________________________ " + DateTime.UtcNow.ToString("HH:mm:ss.fff")
                          ,
                          MyValue = 0
                      };
                  });
        }

        public void Test2()
        {
            var result1 = GetCacheItem2();
            Trace.WriteLine(result1.ToString());
            Thread.Sleep(22000);
            var result2 = GetCacheItem2();
            Trace.WriteLine(result2.ToString());
            Thread.Sleep(1000);
            var result3 = GetCacheItem2();
            Trace.WriteLine(result3.ToString());
        }




        private List<int> ListWithRandomNumberOfElements
        {
            get
            {
                Trace.WriteLine("*************** GetCacheItem2 call at: " + DateTime.UtcNow.ToString("HH:mm:ss.fff") + " ******************");
                return SlimCacheManager.LoadFromCache<List<int>>(
                      "ListWithRandomNumberOfElements"
                      , delegate
                      {
                          Random random = new Random();
                          List<int> list = new List<int>();
                          int count = random.Next(10, 30);

                          for (int i = 0; i < count; i++)
                              list.Add(i);

                          Trace.WriteLine("Reloaded cached list with " + count + " elements.");
                          return list;
                      });
            }
        }
        [Test]
        public void Test_CrossThreadAccess()
        {
            List<int> items = this.ListWithRandomNumberOfElements;

            int count = items.Count;
            Trace.WriteLine("Initial number of elements: " + count);

            for (int i = 0; i < count; i++)
            {
                Trace.WriteLine("Accessing item index " + i + ": " + items[i].ToString());
                Thread.Sleep(200);
                var itemAccess = this.ListWithRandomNumberOfElements;
            }

        }
    }
}
