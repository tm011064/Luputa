using System;
using CommonTools.Components.Threading;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using CommonTools.Components.Logging;
using CommonTools.TestSuite.Components;
using CommonTools.Extensions;
using System.Diagnostics;
using System.Threading;

namespace CommonTools.TestSuite.Jobs
{
    public class MockupFirstJob : IJob
    {
        public MockupFirstJob() { }

        #region IJob Members
        public void Execute(Dictionary<string, string> node)
        {
            Trace.WriteLine("Start MockupFirstJob on thread Id " + Thread.CurrentThread.ManagedThreadId + ".");
            int sleepMilliseconds = 2000;
            
            Trace.WriteLine("MockupFirstJob start sleeping for " + sleepMilliseconds + " ms.");

            Thread.Sleep(sleepMilliseconds);

            Trace.WriteLine("MockupFirstJob resumed at " + DateTime.UtcNow.ToString());

            Trace.WriteLine("MockupFirstJob executed at " + DateTime.UtcNow.ToString());
        }

        #endregion
    }
    public class MockupSecondJob : IJob
    {
        public MockupSecondJob() { }

        #region IJob Members

        public void Execute(Dictionary<string, string> node)
        {
            Trace.WriteLine("MockupFirstJob executed at " + DateTime.UtcNow.ToString());

            //LogManager.LogEvent(ApplicationLocation.Jobs, EventType.Information, "MockupSecondJob executed at " + DateTime.UtcNow.ToString() + ". Dictionary: <br/>"
            //    + ((node != null) ? (node.ToDebugString<string, string>(TextFormat.HTML)) : ("not provided")));
        }

        #endregion
    }
    public class MockupNoSingleJob : IJob
    {
        public MockupNoSingleJob() { }

        #region IJob Members

        public void Execute(Dictionary<string, string> node)
        {
            Trace.WriteLine("MockupNoSingleJob executed at " + DateTime.UtcNow.ToString());

            //LogManager.LogEvent(ApplicationLocation.Jobs, EventType.Information, "MockupNoSingleJob executed at " + DateTime.UtcNow.ToString() + ". Dictionary: <br/>"
            //    + ((node != null) ? (node.ToDebugString<string, string>(TextFormat.HTML)) : ("not provided")));
        }

        #endregion
    }

    public class MockupDailyJob : IJob
    {
        public MockupDailyJob() { }

        #region IJob Members

        public void Execute(Dictionary<string, string> node)
        {
            //LogManager.LogEvent(ApplicationLocation.Jobs, EventType.Information, "DailyJob executed at " + DateTime.UtcNow.ToString() + ". Dictionary: <br/>"
            //    + ((node != null) ? (node.ToDebugString<string, string>(TextFormat.HTML)) : ("not provided")));
        }

        #endregion
    }
}