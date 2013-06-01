using System;
using CommonTools.Components.Threading;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using CommonTools.Components.Logging;
using CommonTools.TestApp.Components;
using CommonTools.Extensions;

namespace CommonTools.TestApp.Jobs
{
    public class FirstJob : IJob
    {
        public FirstJob() { }

        #region IJob Members
        public void Execute(Dictionary<string, string> node)
        {
            StringBuilder sb = new StringBuilder("Name value collection");

            LogManager.LogEvent(ApplicationLocation.Jobs, EventType.Information, "FirstJob executed at " + DateTime.UtcNow.ToString() + ". Dictionary: <br/>"
                + ((node != null) ? (node.ToDebugString<string, string>(TextFormat.HTML)) : ("not provided")));
        }

        #endregion
    }
    public class SecondJob : IJob
    {
        public SecondJob() { }

        #region IJob Members

        public void Execute(Dictionary<string, string> node)
        {
            LogManager.LogEvent(ApplicationLocation.Jobs, EventType.Information, "SecondJob executed at " + DateTime.UtcNow.ToString() + ". Dictionary: <br/>"
                + ((node != null) ? (node.ToDebugString<string, string>(TextFormat.HTML)) : ("not provided")));
        }

        #endregion
    }
    public class NoSingleJob : IJob
    {
        public NoSingleJob() { }

        #region IJob Members

        public void Execute(Dictionary<string, string> node)
        {
            LogManager.LogEvent(ApplicationLocation.Jobs, EventType.Information, "NoSingleJob executed at " + DateTime.UtcNow.ToString() + ". Dictionary: <br/>"
                + ((node != null) ? (node.ToDebugString<string, string>(TextFormat.HTML)) : ("not provided")));
        }

        #endregion
    }
    public class DailyJob : IJob
    {
        public DailyJob() { }

        #region IJob Members

        public void Execute(Dictionary<string, string> node)
        {
            LogManager.LogEvent(ApplicationLocation.Jobs, EventType.Information, "DailyJob executed at " + DateTime.UtcNow.ToString() + ". Dictionary: <br/>"
                + ((node != null) ? (node.ToDebugString<string, string>(TextFormat.HTML)) : ("not provided")));
        }

        #endregion
    }
}