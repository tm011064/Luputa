using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CommonTools.Components.Localization;
using CommonTools.TestSuite.Components;
using System.Diagnostics;
using CommonTools.Components.Threading;
using System.Threading;
using CommonTools.Components.Mathematics;

namespace CommonTools.TestSuite
{
    [TestFixture]
    public class MathTests
    {

        [Test]
        public void Test_GetMedian()
        {
            Assert.AreEqual(2, Statistics.GetMedian(new List<int>() { 1, 1, 1, 2, 2, 2, 2 }));
            Assert.AreEqual(2, Statistics.GetMedian(new List<int>() { 2, 2, 2, 2, 1, 1, 1 }));

            Assert.AreEqual(1, Statistics.GetMedian(new List<int>() { 1, 1, 1, 1, 2, 2, 2 }));
            Assert.AreEqual(1, Statistics.GetMedian(new List<int>() { 2, 2, 2, 1, 1, 1, 1 }));

            Assert.AreEqual(1.5, Statistics.GetMedian(new List<int>() { 1, 1, 1, 1, 2, 2, 2, 2 }));
            Assert.AreEqual(1.5, Statistics.GetMedian(new List<int>() { 2, 2, 2, 2, 1, 1, 1, 1 }));

            Assert.AreEqual(0, Statistics.GetMedian(new List<int>()));
        }
        
        [Test]
        public void Test_StandardDeviation()
        {
            Assert.AreEqual(1.0302, Math.Round(Statistics.CalculateStandardDeviation(new List<double>() { 10, 10, 9, 9, 8, 11, 11 }), 4));
        }
        [Test]
        public void Test_MedianDeviation()
        {
            Assert.AreEqual(1.0690, Math.Round(Statistics.CalculateMedianAbsoluteDeviation(new List<double>() { 10, 10, 9, 9, 8, 11, 11 }), 4));
        }

        [Test]
        public void Test_Covariance()
        {
            Assert.AreEqual(-2.5, Statistics.CalculateCovariance(
                new List<double>() { 1, 3, 3, 5 }
                , new List<double>() { 12, 12, 11, 7 }));

            Assert.AreEqual(-2.5, Statistics.CalculateCovariance(
                new double[] { 1, 3, 3, 5 }
                , new double[] { 12, 12, 11, 7 }));
        }
    }
}
