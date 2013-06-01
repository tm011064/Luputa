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

namespace CommonTools.TestSuite.Tests
{
    [TestFixture]
    public class Components_Logging
    {
        #region const values
        protected const double _ExpectedLogInsertTimeInMilliseconds = 5;
        protected TimeSpan _ExpectedLogInsertTime = TimeSpan.FromMilliseconds(_ExpectedLogInsertTimeInMilliseconds);
        protected int _TotalLogInsertsForSpeedTests = 1000;
        #endregion

        #region const values
        private string GetTestMessage(Guid id)
        {
            return "**** TESTMESSAGE - ID: " + id.ToString() + " ****";
        }
        #endregion

        #region basic tests

        [Test]
        public void Test_LogEventAsynchronously()
        {
            Trace.WriteLine(Configuration.GetGenericHeader());
            Guid testId = Guid.NewGuid();

            ILogController logController = LogControllerFactory.CreateLogController();

            int? initialRowCount = -1
                 , rowCount = -1;
            LogManager.GetEventPage(1, 0, out initialRowCount);

            for (int i = 0; i < logController.AsynchronousBatchSize; i++)
            {
                testId = Guid.NewGuid();

                LogManager.GetEventPage(1, 0, out rowCount);
                // the events must no be logged yet...
                Assert.AreEqual(initialRowCount, rowCount);

                AsynchronousLogManager.LogEvent(ApplicationLocation.Application, EventType.Verbose, GetTestMessage(testId));
            }
            
            // since the for-loop reached the batch size limit, the last LogEvent call must have saved the events at the db
            LogManager.GetEventPage(1, 0, out rowCount);
            Assert.Greater(rowCount, initialRowCount);

            Trace.WriteLine(Configuration.GetGenericFooter());
        }


        [Test]
        public void Test_LogEvent()
        {
            Trace.WriteLine(Configuration.GetGenericHeader());

            int? oldRowCount = 0;
            Guid testId = Guid.NewGuid();
            
            LogManager.GetEventPage(10, 0, out oldRowCount);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            LogManager.LogEvent(ApplicationLocation.Application, EventType.Verbose, GetTestMessage(testId));
            stopwatch.Stop();
            Trace.WriteLine("Event Log insert time: " + stopwatch.Elapsed.ToString());

            int? newRowCount = 0;
            LogDatasets.EventLogDataTable dt = LogManager.GetEventPage(10, 0, out newRowCount);
            Assert.IsTrue(newRowCount > oldRowCount);
            Assert.IsTrue(dt[0].Message == GetTestMessage(testId));
            Trace.WriteLine(Configuration.GetGenericFooter());
        }

        [Test]
        public void Test_LogException()
        {
            Trace.WriteLine(Configuration.GetGenericHeader());

            int? oldRowCount = 0;
            Guid testId = Guid.NewGuid();

            LogManager.GetExceptionPage(10, 0, out oldRowCount);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            LogManager.LogException(ApplicationLocation.Application, ExceptionHandlingStatus.Unhandled, new Exception(GetTestMessage(testId)));
            stopwatch.Stop();
            Trace.WriteLine("Exception Log insert time: " + stopwatch.Elapsed.ToString());

            int? newRowCount = 0;
            LogDatasets.ExceptionLogDataTable dt = LogManager.GetExceptionPage(10, 0, out newRowCount);
            Assert.IsTrue(newRowCount > oldRowCount);
            Assert.IsTrue(dt[0].ExceptionMessage.IndexOf(GetTestMessage(testId)) > 0);
            Trace.WriteLine(Configuration.GetGenericFooter());
        }

        [Test]
        public void Test_LogEventSpeedTest()
        {
            Trace.WriteLine(Configuration.GetGenericHeader());

            Guid testId = Guid.NewGuid();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < _TotalLogInsertsForSpeedTests; i++)
                LogManager.LogEvent(ApplicationLocation.Application, EventType.Verbose, GetTestMessage(testId));
            stopwatch.Stop();

            decimal avg = (decimal)stopwatch.ElapsedMilliseconds / (decimal)_TotalLogInsertsForSpeedTests;
            Trace.WriteLine("Elapsed Milliseconds: " + stopwatch.ElapsedMilliseconds.ToString() + " for " + _TotalLogInsertsForSpeedTests.ToString() + " inserts");
            Trace.WriteLine("Average Log insert time: " + avg.ToString() + " ms.");

            Assert.IsTrue(avg < (decimal)_ExpectedLogInsertTimeInMilliseconds,
                string.Format("Average Event Log insert time ({0}) exceeds the expected time ({1}).",
                TimeSpan.FromMilliseconds((double)avg).ToString(), _ExpectedLogInsertTime.ToString()));

            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < _TotalLogInsertsForSpeedTests; i++)
                AsynchronousLogManager.LogEvent(ApplicationLocation.Application, EventType.Verbose, GetTestMessage(testId));
            AsynchronousLogManager.SafeAllPendingEvents();
            stopwatch.Stop();
            avg = (decimal)stopwatch.ElapsedMilliseconds / (decimal)_TotalLogInsertsForSpeedTests;
            Trace.WriteLine("Elapsed Milliseconds: " + stopwatch.ElapsedMilliseconds.ToString() + " for " + _TotalLogInsertsForSpeedTests.ToString() + " inserts");
            Trace.WriteLine("Average Log insert time: " + avg.ToString() + " ms.");
            Assert.IsTrue(avg < (decimal)_ExpectedLogInsertTimeInMilliseconds,
               string.Format("Average Event Log insert time ({0}) exceeds the expected time ({1}).",
               TimeSpan.FromMilliseconds((double)avg).ToString(), _ExpectedLogInsertTime.ToString()));

            Trace.WriteLine(Configuration.GetGenericFooter());
        }

        [Test]
        public void Test_LogExceptionSpeedTest()
        {
            Trace.WriteLine(Configuration.GetGenericHeader());

            Guid testId = Guid.NewGuid();
            Exception exception = new Exception(GetTestMessage(testId));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < _TotalLogInsertsForSpeedTests; i++)
                LogManager.LogException(ApplicationLocation.Application, ExceptionHandlingStatus.HandledInCode, exception);
            stopwatch.Stop();

            decimal avg = (decimal)stopwatch.ElapsedMilliseconds / (decimal)_TotalLogInsertsForSpeedTests;
            Trace.WriteLine("Elapsed Milliseconds: " + stopwatch.ElapsedMilliseconds.ToString() + " for " + _TotalLogInsertsForSpeedTests.ToString() + " inserts");
            Trace.WriteLine("Average Log insert time: " + avg.ToString() + " ms.");

            Assert.IsTrue(avg < (decimal)_ExpectedLogInsertTimeInMilliseconds,
                string.Format("Average Event Log insert time ({0}) exceeds the expected time ({1}).",
                TimeSpan.FromMilliseconds((double)avg).ToString(), _ExpectedLogInsertTime.ToString()));
            Trace.WriteLine(Configuration.GetGenericFooter());
        }

        #endregion

        #region exception tests
        [Test]
        [ExpectedException(ExpectedException = typeof(CommonTools.Data.DataAccessManagerConnectionStringException))]
        public void TestException_WrongConnectionString()
        {
            Trace.WriteLine(Configuration.GetGenericHeader());
            LogManager.SetConnectionString(Configuration.WrongConnectionString);
            try
            {
                LogManager.LogEvent(ApplicationLocation.Application, EventType.Verbose, "test");
            }
            catch (Exception err)
            {
                Trace.WriteLine(err.Message);
                throw;
            } 
            Trace.WriteLine(Configuration.GetGenericFooter());
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(CommonTools.Data.DataAccessManagerStoredProcedureException))]
        public void TestException_CantFindStoredProcedure()
        {
            Trace.WriteLine(Configuration.GetGenericHeader());
            LogManager.SetStoredProcedurePrefix("Some_Prefix");
            try
            {
                LogManager.LogEvent(ApplicationLocation.Application, EventType.Verbose, "test");
            }
            catch (Exception err)
            {
                Trace.WriteLine(err.Message);
                throw;
            } 
            Trace.WriteLine(Configuration.GetGenericFooter());
        }
        #endregion
    }
}
