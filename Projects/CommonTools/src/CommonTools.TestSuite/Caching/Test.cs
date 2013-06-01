using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Caching.Testing.Mockups;
using NUnit.Framework;
using System.Diagnostics;
using System.Threading;

namespace CommonTools.Caching.Testing
{
    public class Test : TestBase
    {
        public void ValueTest()
        {
            int x = 123;

            DistributedCache.Insert("ValueTest", x, DateTime.UtcNow.AddSeconds(30));
            int y = DistributedCache.Get<int>("ValueTest");
        }


        public void ObjectTest()
        {
            SimpleProtoObject simple = new SimpleProtoObject() { MyString = "Hello Franz", MyValue = 100 };

            DistributedCache.Insert("SimpleObject", simple, DateTime.UtcNow.AddSeconds(30));
            SimpleProtoObject cachedObject = DistributedCache.Get<SimpleProtoObject>("SimpleObject");
        }

        public void SerializeProtoObjects(int iterations)
        {
            ResetStopwatch("String Serialize");
            ResetStopwatch("String Deserialize");

            SimpleProtoObject simple;
            for (int i = 0; i < iterations; i++)
            {
                simple = new SimpleProtoObject() { MyString = "Hello Franz", MyValue = i };

                StartStopwatch("String Serialize");
                DistributedCache.Insert("SerializationPerformanceTest" + i, simple, DateTime.UtcNow.AddSeconds(60));
                StopStopwatch("String Serialize");
            }

            for (int i = 0; i < iterations; i++)
            {
                StartStopwatch("String Deserialize");
                simple = DistributedCache.Get<SimpleProtoObject>("SerializationPerformanceTest" + i);
                StopStopwatch("String Deserialize");

                Assert.IsNotNull(simple);
                Assert.AreEqual(simple.MyValue, i);
                Assert.AreEqual(simple.MyString, "Hello Franz");
            }

            StopAndTraceStopwatch("String Serialize");
            StopAndTraceStopwatch("String Deserialize");
        }

        public void SerializeObjects(int iterations)
        {
            ResetStopwatch("String Serialize");
            ResetStopwatch("String Deserialize");

            SimpleObject simple;
            for (int i = 0; i < iterations; i++)
            {
                simple = new SimpleObject() { MyString = "Hello Franz", MyValue = i };

                StartStopwatch("String Serialize");
                DistributedCache.Insert("SerializationPerformanceTest" + i, simple, DateTime.UtcNow.AddSeconds(60));
                StopStopwatch("String Serialize");
            }

            for (int i = 0; i < iterations; i++)
            {
                StartStopwatch("String Deserialize");
                simple = DistributedCache.Get<SimpleObject>("SerializationPerformanceTest" + i);
                StopStopwatch("String Deserialize");

                Assert.IsNotNull(simple);
                Assert.AreEqual(simple.MyValue, i);
                Assert.AreEqual(simple.MyString, "Hello Franz");
            }

            StopAndTraceStopwatch("String Serialize");
            StopAndTraceStopwatch("String Deserialize");
        }

        public void SerializeObjectsInProcess(int iterations)
        {
            ResetStopwatch("String Serialize");
            ResetStopwatch("String Deserialize");

            SimpleObject simple;
            for (int i = 0; i < iterations; i++)
            {
                simple = new SimpleObject() { MyString = "Hello Franz", MyValue = i };

                StartStopwatch("String Serialize");
                System.Web.HttpRuntime.Cache.Insert("SerializationPerformanceTest" + i, simple, null, DateTime.UtcNow.AddSeconds(60), System.Web.Caching.Cache.NoSlidingExpiration);
                StopStopwatch("String Serialize");
            }

            for (int i = 0; i < iterations; i++)
            {
                StartStopwatch("String Deserialize");
                simple = System.Web.HttpRuntime.Cache.Get("SerializationPerformanceTest" + i) as SimpleObject;
                StopStopwatch("String Deserialize");

                Assert.IsNotNull(simple);
                Assert.AreEqual(simple.MyValue, i);
                Assert.AreEqual(simple.MyString, "Hello Franz");
            }

            StopAndTraceStopwatch("String Serialize");
            StopAndTraceStopwatch("String Deserialize");
        }

        public void SerializeObjectsInProcessCustomSerialization(int iterations)
        {
            ResetStopwatch("String Serialize");
            ResetStopwatch("String Deserialize");

            SimpleProtoObject simple;
            for (int i = 0; i < iterations; i++)
            {
                simple = new SimpleProtoObject() { MyString = "Hello Franz", MyValue = i };

                StartStopwatch("String Serialize");
                System.Web.HttpRuntime.Cache.Insert("SerializationPerformanceTest" + i, ProtocolBufferSerialize<SimpleProtoObject>(simple), null, DateTime.UtcNow.AddSeconds(60), System.Web.Caching.Cache.NoSlidingExpiration);
                StopStopwatch("String Serialize");
            }

            for (int i = 0; i < iterations; i++)
            {
                StartStopwatch("String Deserialize");
                simple = ProtocolBufferDeserialize<SimpleProtoObject>((byte[])System.Web.HttpRuntime.Cache.Get("SerializationPerformanceTest" + i));
                StopStopwatch("String Deserialize");

                Assert.IsNotNull(simple);
                Assert.AreEqual(simple.MyValue, i);
                Assert.AreEqual(simple.MyString, "Hello Franz");
            }

            StopAndTraceStopwatch("String Serialize");
            StopAndTraceStopwatch("String Deserialize");
        }

        public void SerializationPerformanceTest()
        {
            int iterations = 10000;

            Trace.WriteLine("\n\nSerializeProtoObjects\n");
            SerializeProtoObjects(iterations);
            SerializeProtoObjects(iterations);
            SerializeProtoObjects(iterations);

            Trace.WriteLine("\n\nSerializeObjects\n");
            SerializeObjects(iterations);
            SerializeObjects(iterations);
            SerializeObjects(iterations);

            Trace.WriteLine("\n\nSerializeObjectsInProcess\n");
            SerializeObjectsInProcess(iterations);
            SerializeObjectsInProcess(iterations);
            SerializeObjectsInProcess(iterations);

            Trace.WriteLine("\n\nSerializeObjectsInProcessCustomSerialization\n");
            SerializeObjectsInProcessCustomSerialization(iterations);
            SerializeObjectsInProcessCustomSerialization(iterations);
            SerializeObjectsInProcessCustomSerialization(iterations);
        }

        public bool TestAdd(int sleepTimeInMilliseconds)
        {
            DistributedCache.Remove("mykey");
            
            DistributedCache.Add("mykey", new object(), DateTime.UtcNow.AddMilliseconds(sleepTimeInMilliseconds));

            //if (DistributedCache.Get<object>("mykey") == null)
            //    Trace.WriteLine("NOT INSERTED!!");

            bool worked = true;
            if (DistributedCache.Add("mykey", new object(), DateTime.UtcNow.AddMilliseconds(sleepTimeInMilliseconds)))
            {
                worked = false;
            }

            return worked;
        }

        public void TestTestAdd(Dictionary<int, List<int>> records)
        {
            int millisecondsSleep = 1;
            for (int i = 0; i < 40; i++)
            {
                if (!records.ContainsKey(millisecondsSleep))
                    records.Add(millisecondsSleep, new List<int>());

                for (int j = 0; j < 100; j++)
                {
                    records[millisecondsSleep].Add(TestAdd(millisecondsSleep) ? 1 : 0);
                }

                millisecondsSleep += 25;
            }
        }

        public void TestBehaviour()
        {
            Dictionary<int, List<int>> records = new Dictionary<int, List<int>>();

            for (int i = 0; i < 50; i++)
            {
                TestTestAdd(records);
                Trace.WriteLine("TEST CYCLE " + i);
            }

            foreach (int key in records.Keys)
            {
                Trace.WriteLine(string.Format("Add Test success percentage with {0} milliseconds: " + records[key].Average().ToString(), key));
            }
        }
    }
}
