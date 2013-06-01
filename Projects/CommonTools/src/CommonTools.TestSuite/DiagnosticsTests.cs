using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CommonTools.Components.Testing;
using System.Threading;
using System.Diagnostics;

namespace CommonTools.TestSuite
{
    [TestFixture]
    public class DiagnosticsTests
    {
        [Test]
        public void Test_GlobalStopwatch()
        {
            Random random = new Random();

            for (int i = 0; i < 100; i++)
            {
                GlobalStopwatchSingleThread.Start("Test");
                Thread.Sleep(random.Next(10, 21));
                GlobalStopwatchSingleThread.Stop("Test");
            }

            Trace.WriteLine(GlobalStopwatchSingleThread.GetElapsedTimeFormatted("Test"));
        }
    }
}
