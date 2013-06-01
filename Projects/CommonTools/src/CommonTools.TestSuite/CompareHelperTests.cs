using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CommonTools.Core;
using System.Diagnostics;

namespace CommonTools.TestSuite
{
    [TestFixture]
    public class CompareHelperTests
    {
        [Test]
        public void Test_CompareElements()
        {
            Random random = new Random();
            int numberOfItems = 1000000;

            List<int> listA = new List<int>()
                , listB = new List<int>();

            int value;
            for (int i = 0; i < numberOfItems; i++)
            {
                value = random.Next(0, 10);
                listA.Add(value);
                listB.Add(value);
            }

            Assert.IsTrue(CompareHelper.HaveEqualItems<int>(listA, listB));

            int firstValue = listA[0];
            listB.RemoveAt(0);
            listB.Add(firstValue == 0 ? 1 : 0);

            Assert.IsFalse(CompareHelper.HaveEqualItems<int>(listA, listB));

            listB.RemoveAt(listA.Count - 1);
            listB.Add(firstValue);

            Assert.IsTrue(CompareHelper.HaveEqualItems<int>(listA, listB));
        }
    }
}
