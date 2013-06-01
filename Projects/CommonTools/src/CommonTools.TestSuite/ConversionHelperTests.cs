using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Xml.Linq;
using CommonTools.Extensions;
using System.Diagnostics;
using CommonTools.Components.Testing;
using System.Xml.Serialization;

namespace CommonTools.TestSuite
{
    [TestFixture]
    public class ConversionHelperTests
    {
        public enum TestEnum
        {
            Value1 = 1,
            Value2 = 2,
            Undefined = 3
        }

        [Test]
        public void Test_ParseXElementNode()
        {
            XElement element = new XElement("r");

            TimeSpan ts1 = new TimeSpan(12, 5, 3);
            TimeSpan ts2 = TimeSpan.FromDays(2);
            Trace.WriteLine(ts1.ToString());
            Trace.WriteLine(ts2.ToString());

            Trace.WriteLine(string.Format("{0}.{1:00}:{2:00}:{3:00}", ts1.Days, (int)ts1.Hours, ts1.Minutes, ts1.Seconds));

            #region enums
            element.SetElementValue("TimeSpan", string.Format("{0}.{1:00}:{2:00}:{3:00}", ts1.Days, (int)ts1.Hours, ts1.Minutes, ts1.Seconds));
            var testTimeSpan = element.ParseXElementNode<TimeSpan>("TimeSpan", TimeSpan.FromDays(1));

            Assert.AreEqual(ts1, testTimeSpan);
            #endregion

            #region enums
            element.SetElementValue("Enum", TestEnum.Value1);
            var testEnum = element.ParseXElementNode<TestEnum>("Enum", TestEnum.Undefined);

            Assert.AreEqual(TestEnum.Value1, testEnum);

            TestEnum? testEnum2;
            element.SetElementValue("Enum2", null);
            testEnum2 = element.ParseXElementNode<TestEnum?>("Enum2", null);

            Assert.AreEqual(null, testEnum2);
            element.SetElementValue("Enum2", TestEnum.Value1);
            testEnum2 = element.ParseXElementNode<TestEnum?>("Enum2", null);

            Assert.AreEqual(TestEnum.Value1, testEnum2);
            #endregion

            #region lists
            List<int> testList = new List<int>() { 1, 2, 3, 4, 5 };
            List<bool> testList2 = new List<bool>() { false, true, false };
            List<string> testList3 = new List<string>() { "false", "true", "false" };
            List<DayOfWeek> testList4 = new List<DayOfWeek>() { DayOfWeek.Friday, DayOfWeek.Monday };
            List<TestEnum> testList5 = new List<TestEnum>() { TestEnum.Value1, TestEnum.Undefined };

            element.SetXElementNodeList("MyList1", testList);
            element.SetXElementNodeList("MyList2", testList2);
            element.SetXElementNodeList("MyList3", testList3);
            element.SetXElementNodeList("MyList4", testList4);
            element.SetXElementNodeList("MyList5", testList5);


            Trace.WriteLine(DebugUtility.GetDebugString(element.ParseXElementNodeList<List<int>>("MyList1", new List<int>())));
            Trace.WriteLine(DebugUtility.GetDebugString(element.ParseXElementNodeList<List<bool>>("MyList2", new List<bool>())));
            Trace.WriteLine(DebugUtility.GetDebugString(element.ParseXElementNodeList<List<string>>("MyList3", new List<string>())));
            Trace.WriteLine(DebugUtility.GetDebugString(element.ParseXElementNodeList<List<DayOfWeek>>("MyList4", new List<DayOfWeek>())));
            Trace.WriteLine(DebugUtility.GetDebugString(element.ParseXElementNodeList<List<TestEnum>>("MyList5", new List<TestEnum>())));
            #endregion

            #region dictionaries
            Dictionary<int, string> dict1 = new Dictionary<int, string>() { { 1, "test1" }, { 2, "test2" } };

            element.SetXElementNodeLookup("MyDict1", dict1);

            Trace.WriteLine(DebugUtility.GetDebugString(element.ParseXElementNodeLookup<Dictionary<int, string>>("MyDict1", new Dictionary<int, string>())));

            #endregion

            Trace.WriteLine(element.ToString(SaveOptions.None));
        }
    }
}
