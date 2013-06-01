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

namespace CommonTools.TestSuite
{
    [TestFixture]
    public class PreciseTimerTests
    {
        private int _Count = 0;
        private List<long> _Ticks = new List<long>();

        private void IncrementCount(object state)
        {
            int count = _Count + 1;
            Trace.WriteLine("TimerCallback: " + DateTime.Now.ToString("hh:MM:ss.fff") + ", Count: " + _Count);
            Interlocked.Exchange(ref _Count, count);
        }

        private void AddTicks(object state)
        {
            _Ticks.Add(DateTime.Now.Ticks);
        }

        [Test]
        public void Test_StartStop()
        {
            int ms = 200;
            int iterations = 5;
            using (PreciseTimer timer = new PreciseTimer(new TimerCallback(IncrementCount), TimeSpan.FromMilliseconds(ms), PreciseTimerCallbackMode.Async))
            {
                timer.Start();
                Thread.Sleep(ms * iterations);
                timer.Stop();
                Trace.WriteLine("Timer stopped");

                Assert.AreEqual(iterations, _Count);
            }
        }

        [Test]
        public void Test_Precision()
        {
            int ms = 1;
            int iterations = 100;

            using (PreciseTimer timer = new PreciseTimer(new TimerCallback(AddTicks), TimeSpan.FromMilliseconds(ms), PreciseTimerCallbackMode.Synchronized))
            {
                timer.Start();
                Thread.Sleep(ms * iterations);
                timer.Stop();
                Trace.WriteLine("Timer stopped");

                int length = _Ticks.Count;
                long difference;
                for (int i = 1; i < length; i++)
                {
                    difference = _Ticks[i] - _Ticks[i - 1];
                    Trace.WriteLine(difference);

                    Assert.GreaterOrEqual(difference, ms * 10000);
                    Assert.LessOrEqual(difference, ms * 10000 + 120); // we should have at least a 120 nano second precision                    
                }
            }
        }

    }
}
