using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CommonTools.Components.Testing;
using CommonTools.Components.BusinessTier;
using CommonTools;
using System.Diagnostics;
using CommonTools.TestSuite.Components;
using CommonTools.Components.Caching;
using CommonTools.Components.Logging;
using System.Threading;
using CommonTools.Components.Threading;
using CommonTools.Components.Localization;

namespace CommonTools.TestSuite.Tests
{
    [TestFixture]
    public class Components_Jobs //: TestBase
    {
        #region const values
        protected const double _ExpectedLogInsertTimeInMilliseconds = 5;
        protected TimeSpan _ExpectedLogInsertTime = TimeSpan.FromMilliseconds(_ExpectedLogInsertTimeInMilliseconds);
        protected int _TotalLogInsertsForSpeedTests = 500;
        #endregion

        #region members
        internal static List<long> _Ticks = new List<long>();
        internal static AutoResetEvent[] _JobTestResetEvent;
        internal static int[] _JobTestSynchronizationCounter;
        #endregion

        #region basic tests

        [Test]
        public void Test_DailyJobExecution()
        {
            DateTime now = DateTime.Now;

            int ms = 1000;

            JobSection section = new JobSection();
            section.JobItems.Add(new JobElement()
            {
                Enabled = true,
                EnableShutDown = false,
                ExecuteDaily = true,
                ExecuteOnOwnThread = true,
                FirstRunAtInitialization = false,
                DailyLocalizedExecutionTimeZoneName = "GMT Standard Time",
                DailyLocalizedExecutionTime = now.AddMilliseconds(ms),
                Type = "CommonTools.TestSuite.Tests.ResetEventJob, CommonTools.TestSuite",
                Options = new Dictionary<string, string>() { { "Index", "0" }, { "Name", "Test_DailyJobExecution" } }
            });

            _JobTestResetEvent = new AutoResetEvent[] { new AutoResetEvent(false), new AutoResetEvent(false), new AutoResetEvent(false) };
            _JobTestSynchronizationCounter = new int[] { 0, 0, 0 };

            CommonTools.Components.Threading.Jobs jobs = CommonTools.Components.Threading.Jobs.Instance();

            jobs.Start(section);

            Stopwatch sw = Stopwatch.StartNew();

            _JobTestResetEvent[0].WaitOne(10000);

            sw.Stop();

            Assert.LessOrEqual(sw.ElapsedMilliseconds, ms + 1000);

            Assert.AreEqual(1, _JobTestSynchronizationCounter[0]);

            jobs.Stop();
        }

        [Test]
        public void Test_SeveralJobsOnOwnThread()
        {
            JobSection section = new JobSection();

            double msJobShort = 1000
                   , msJobMedium = 3000
                   , msJobLong = 7000;

            section.JobItems.Add(new JobElement()
            {
                Enabled = true,
                EnableShutDown = false,
                ExecuteDaily = false,
                ExecuteOnOwnThread = true,
                FirstRunAtInitialization = false,
                Period = TimeSpan.FromMilliseconds(msJobShort),
                Type = "CommonTools.TestSuite.Tests.ResetEventJob, CommonTools.TestSuite",
                Options = new Dictionary<string, string>() { { "Index", "0" }, { "Name", "msJobShort" } }
            });
            section.JobItems.Add(new JobElement()
            {
                Enabled = true,
                EnableShutDown = false,
                ExecuteDaily = false,
                ExecuteOnOwnThread = true,
                FirstRunAtInitialization = false,
                Period = TimeSpan.FromMilliseconds(msJobMedium),
                Type = "CommonTools.TestSuite.Tests.ResetEventJob, CommonTools.TestSuite",
                Options = new Dictionary<string, string>() { { "Index", "1" }, { "Name", "msJobMedium" } }
            });
            section.JobItems.Add(new JobElement()
            {
                Enabled = true,
                EnableShutDown = false,
                ExecuteDaily = false,
                ExecuteOnOwnThread = true,
                FirstRunAtInitialization = false,
                Period = TimeSpan.FromMilliseconds(msJobLong),
                Type = "CommonTools.TestSuite.Tests.ResetEventJob, CommonTools.TestSuite",
                Options = new Dictionary<string, string>() { { "Index", "2" }, { "Name", "msJobLong" } }
            });

            _JobTestResetEvent = new AutoResetEvent[] { new AutoResetEvent(false), new AutoResetEvent(false), new AutoResetEvent(false) };
            _JobTestSynchronizationCounter = new int[] { 0, 0, 0 };

            CommonTools.Components.Threading.Jobs jobs = CommonTools.Components.Threading.Jobs.Instance();

            jobs.Start(section);

            Stopwatch sw = Stopwatch.StartNew();

            _JobTestResetEvent[0].WaitOne(10000);

            _JobTestResetEvent[1].WaitOne(10000);

            _JobTestResetEvent[2].WaitOne(10000);

            sw.Stop();

            Assert.LessOrEqual(sw.ElapsedMilliseconds, msJobLong + 1000); // the reset events should not take longer than the 

            Assert.GreaterOrEqual(_JobTestSynchronizationCounter[0], 5);
            Assert.GreaterOrEqual(_JobTestSynchronizationCounter[1], 2);
            Assert.GreaterOrEqual(_JobTestSynchronizationCounter[2], 1);

            jobs.Stop();
        }

        [Test]
        public void Test_DailyExecutionTimeWithDaylightSavingAdjustment()
        {
            TimeZoneInfo timeZoneInfo = TimeZoneUtility.GetTimeZoneInfo("GMT Standard Time");

            List<DateTime> futureDates = new List<DateTime>(){
                new DateTime(2011, 3, 24, 23, 0, 0, DateTimeKind.Utc),
                new DateTime(2011, 3, 25, 23, 0, 0, DateTimeKind.Utc),
                new DateTime(2011, 3, 26, 23, 0, 0, DateTimeKind.Utc),
                new DateTime(2011, 3, 27, 23, 0, 0, DateTimeKind.Utc),
                new DateTime(2011, 3, 28, 23, 0, 0, DateTimeKind.Utc),
                new DateTime(2011, 3, 29, 23, 0, 0, DateTimeKind.Utc)
            };

            DateTime nowLocal = TimeZoneUtility.ConvertUTCDateToLocalizedDate(futureDates[0], timeZoneInfo).AddDays(-1);
            foreach (DateTime fd in futureDates)
            {
                Trace.WriteLine((TimeZoneUtility.ConvertUTCDateToLocalizedDate(fd, timeZoneInfo) - nowLocal).ToString() + "\n");
                nowLocal = (TimeZoneUtility.ConvertUTCDateToLocalizedDate(fd, timeZoneInfo));
            }
        }

        private void RunPreciseTimerTest(CommonTools.Components.Threading.Jobs jobs, JobSection section, int sleepTimeInMs)
        {
            _Ticks = new List<long>();

            jobs.Start(section);
            Thread.Sleep(sleepTimeInMs);
            jobs.Stop();
        }

        [Test]
        public void Test_PreciseTimer()
        {
            int ms = 1;

            JobSection section = new JobSection();
            section.JobItems.Add(
                new JobElement()
                {
                    Enabled = true,
                    EnableShutDown = false,
                    ExecuteDaily = false,
                    ExecuteOnOwnThread = true,
                    FirstRunAtInitialization = true,
                    Period = TimeSpan.FromMilliseconds(ms),
                    PreciseTimerCallbackMode = PreciseTimerCallbackMode.Async,
                    UsePreciseTimer = true,
                    Type = "CommonTools.TestSuite.Tests.PreciseTimerJob, CommonTools.TestSuite"
                });

            CommonTools.Components.Threading.Jobs jobs = CommonTools.Components.Threading.Jobs.Instance();

            int iterations = 100;

            // run first time to compile the C# code
            RunPreciseTimerTest(jobs, section, ms * iterations);
            Thread.Sleep(1000); // wait a bit so the timer gets its stuff sorted

            // now run the proper test
            RunPreciseTimerTest(jobs, section, ms * iterations);

            Thread.Sleep(ms * 10);
            int length = _Ticks.Count;
            long difference;
            for (int i = 20; i < length; i++)
            {// start at the 10th iteration to give some time for the compiler to optimize...
                difference = _Ticks[i] - _Ticks[i - 1];
                Trace.WriteLine(difference);

                Assert.GreaterOrEqual(difference, ms * 10000);
                Assert.LessOrEqual(difference, ms * 10000 + 120); // we should have at least a 120 nano second precision                       
            }

            Assert.AreEqual(length, _Ticks.Count);
        }
        #endregion
    }

    public class PreciseTimerJob : IJob
    {
        #region IJob Members

        public void Execute(Dictionary<string, string> node)
        {
            Components_Jobs._Ticks.Add(DateTime.Now.Ticks);
            Trace.WriteLine("Added ticks at " + DateTime.Now);
        }

        #endregion
    }

    public class ResetEventJob : IJob
    {
        #region IJob Members

        public void Execute(Dictionary<string, string> node)
        {
            int index = int.Parse(node["Index"]);

            Components_Jobs._JobTestSynchronizationCounter[index]++;
            Components_Jobs._JobTestResetEvent[index].Set();

            Trace.WriteLine("ResetEventJob  " + node["Name"] + " for index " + index + " called at " + DateTime.Now);
        }

        #endregion
    }
}
