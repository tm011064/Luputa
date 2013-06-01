using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using CommonTools.TestSuite.Components;
using CommonTools.Components.Testing;

namespace CommonTools.TestSuite
{
    [TestFixture]
    public class DebugTests
    {
        private enum DummyEnum
        {
            One,
            Two,
            THREE
        }

        public class DummyClass
        {
            public List<string> Names { get; set; }
            public string Name { get; set; }
        }

        [Test]
        public void Test_EnumDebugString()
        {
            Trace.WriteLine(Configuration.GetGenericHeader());

            Assert.AreEqual(DebugUtility.GetDebugString(typeof(DummyEnum)), "One, Two, THREE");

            Trace.WriteLine(Configuration.GetGenericFooter());
        }

        [Test]
        public void Test_EnumerableDebugString()
        {
            Trace.WriteLine(Configuration.GetGenericHeader());

            List<string> strings = new List<string>() { "One", "Two", "THREE" };
            Assert.AreEqual(DebugUtility.GetDebugString(strings), "One, Two, THREE");

            List<int> numbers = new List<int>() { 1, 2, 3 };
            Assert.AreEqual(DebugUtility.GetDebugString(numbers), "1, 2, 3");

            Trace.WriteLine(Configuration.GetGenericFooter());
        }

        [Test]
        public void Test_ObjectDebugString()
        {
            Trace.WriteLine(Configuration.GetGenericHeader());

            DummyDataManager dtm = new DummyDataManager(Configuration.DummyDataXmlPath);
            DummyUser user = dtm.GetDummy();

            Trace.WriteLine(DebugUtility.GetObjectString(user));

            DummyClass c = new DummyClass()
            {
                Names = new List<string>() { "Huey", "Dewey", "Louie" },
                Name = "Duckburg"
            };
            Trace.WriteLine(DebugUtility.GetObjectString(c));

            Trace.WriteLine(Configuration.GetGenericFooter());
        }
    }
}
