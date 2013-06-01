using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading;
using System.Diagnostics;
using CommonTools.Caching.Testing.Mockups;

namespace CommonTools.Caching.Testing
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
        private SimpleObject GetMemcachedCacheItem(int index)
        {
            Trace.WriteLine("*************** GetMemcachedCacheItem call at: " + DateTime.UtcNow.ToString("HH:mm:ss.fff") + " ******************");
            return SlimCacheManager.LoadFromCache<SimpleObject>(
                  "TestItemMemcached"
                  , delegate
                  {
                      Trace.WriteLine("DELEGATE CALL TO: ___START___  GetMemcachedCacheItem, INDEX: " + index);
                      Thread.Sleep(200);
                      Trace.WriteLine("DELEGATE CALL TO: ___FINISH___  GetMemcachedCacheItem, INDEX: " + index);
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


        public void Test2()
        {
            int index = 0;

            Trace.WriteLine(GetMemcachedCacheItem(++index).MyString + "\n");
            Trace.WriteLine(GetMemcachedCacheItem(++index).MyString + "\n");

            Sleep(2700);

            Trace.WriteLine(GetMemcachedCacheItem(++index).MyString + "\n");
            Trace.WriteLine(GetMemcachedCacheItem(++index).MyString + "\n");

            Sleep(2100);

            Trace.WriteLine(GetMemcachedCacheItem(++index).MyString + "\n");

            Sleep(400);

            Trace.WriteLine(GetMemcachedCacheItem(++index).MyString + "\n");
            Trace.WriteLine(GetMemcachedCacheItem(++index).MyString + "\n");

            Sleep(250);

            Trace.WriteLine(GetMemcachedCacheItem(++index).MyString + "\n");
            Trace.WriteLine(GetMemcachedCacheItem(++index).MyString + "\n");

            Sleep(2500);

            Trace.WriteLine(GetMemcachedCacheItem(++index).MyString + "\n");
            Trace.WriteLine(GetMemcachedCacheItem(++index).MyString + "\n");

            Thread.Sleep(2000);
        }
    }
}
