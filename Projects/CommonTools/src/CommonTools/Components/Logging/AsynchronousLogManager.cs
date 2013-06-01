using System;
using System.Data.SqlClient;
using System.Data.Common;
using System.Web;
using CommonTools.Data;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Components.Logging
{

    /// <summary>
    /// This class handles asynchronous event logging. Events are not logged one-by-one, but they are queued
    /// and batch inserted when the queue reaches a specified size.
    /// </summary>
    public class AsynchronousLogManager : LogManagerBase
    {
        #region members
        private const string SP_EVENTLOG_INSERTBATCH = "EventLog_InsertBatch";
        private const string INSERT_NODE = @"insert";
        private const string INSERT_EVENT_TAG = @"<e i=""{6}"" a=""{0}"" d=""{1}"" e=""{2}"" m=""{3}"" n=""{4}"" u=""{5}"" />";
        #endregion

        #region properties
        private static object _EventLogTableLock = new object();
        private static LogDatasets.EventLogDataTable _EventLogTable;
        /// <summary>
        /// Gets the event log table.
        /// </summary>
        /// <value>The event log table.</value>
        public static LogDatasets.EventLogDataTable EventLogTable
        {
            get
            {
                if (_EventLogTable == null)
                    _EventLogTable = new LogDatasets.EventLogDataTable();

                return _EventLogTable;
            }
        }

        #endregion

        #region public static methods

        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="eventType">Type of the event. This parameter should represent the type of your event, e.g. MyEnum.Information</param>
        /// <param name="message">The event message.</param>
        public static void LogEvent(Enum appLocation, Enum eventType, string message)
        {
            LogEvent(LogController.ApplicationId, int.Parse(appLocation.ToString("d")), int.Parse(eventType.ToString("d")), message);
        }
        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="eventType">Type of the event. This parameter should represent the type of your event, e.g. MyEnum.Information</param>
        /// <param name="message">The event message.</param>
        /// <param name="authenticatedUserId">The authenticated user id.</param>
        public static void LogEvent(Enum appLocation, Enum eventType, string message, string authenticatedUserId)
        {
            LogEvent(LogController.ApplicationId, int.Parse(appLocation.ToString("d")), int.Parse(eventType.ToString("d")), message, authenticatedUserId);
        }
        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="eventType">Type of the event. This parameter should represent the type of your event, e.g. MyEnum.Information</param>
        /// <param name="message">The event message.</param>
        public static void LogEvent(int appLocation, int eventType, string message)
        {
            LogEvent(LogController.ApplicationId, appLocation, eventType, message, null);
        }
        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="eventType">Type of the event. This parameter should represent the type of your event, e.g. MyEnum.Information</param>
        /// <param name="message">The event message.</param>
        /// <param name="authenticatedUserId">The authenticated user id.</param>
        public static void LogEvent(int appLocation, int eventType, string message, string authenticatedUserId)
        {
            LogEvent(LogController.ApplicationId, appLocation, eventType, message, authenticatedUserId);
        }

        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="eventType">Type of the event. This parameter should represent the type of your event, e.g. MyEnum.Information</param>
        /// <param name="message">The event message.</param>
        public static void LogEvent(int applicationId, Enum appLocation, Enum eventType, string message)
        {
            LogEvent(applicationId, int.Parse(appLocation.ToString("d")), int.Parse(eventType.ToString("d")), message);
        }
        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="eventType">Type of the event. This parameter should represent the type of your event, e.g. MyEnum.Information</param>
        /// <param name="message">The event message.</param>
        /// <param name="authenticatedUserId">The authenticated user id.</param>
        public static void LogEvent(int applicationId, Enum appLocation, Enum eventType, string message, string authenticatedUserId)
        {
            LogEvent(applicationId, int.Parse(appLocation.ToString("d")), int.Parse(eventType.ToString("d")), message, authenticatedUserId);
        }
        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="eventType">Type of the event. This parameter should represent the type of your event, e.g. MyEnum.Information</param>
        /// <param name="message">The event message.</param>
        public static void LogEvent(int applicationId, int appLocation, int eventType, string message)
        {
            LogEvent(applicationId, appLocation, eventType, message, null);
        }
        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="eventType">Type of the event. This parameter should represent the type of your event, e.g. MyEnum.Information</param>
        /// <param name="message">The event message.</param>
        /// <param name="authenticatedUserId">The authenticated user id.</param>
        public static void LogEvent(int applicationId, int appLocation, int eventType, string message, string authenticatedUserId)
        {
            lock (_EventLogTableLock)
            {
                EventLogTable.AddEventLogRow(_EventLogTable.Count + 1, appLocation, DateTime.UtcNow, eventType, message, System.Environment.MachineName, authenticatedUserId, applicationId);
                if (EventLogTable.Count >= LogController.AsynchronousBatchSize)
                {// we are over the batch size level, so let's write to the database
                    SafeAllPendingEvents();                    
                }
            }
        }

        /// <summary>
        /// This methods writes all queued events to the database. Implement this method at the global.asax at web projects to ensure
        /// that all the queued events in memory get written to the database at a system crash.
        /// </summary>
        public static void SafeAllPendingEvents()
        {
            if (EventLogTable.Count == 0)
                return;

            // first generate the xml string
            StringBuilder sb = new StringBuilder("<" + INSERT_NODE + ">");

            foreach (LogDatasets.EventLogRow row in EventLogTable.Rows)
            {
                sb.Append(string.Format(INSERT_EVENT_TAG
                    , row.AppLocation
                    , row.EventDate.ToString("MM.dd.yyyy HH:mm:ss")
                    , row.EventType
                    , row.Message.Replace(@"""", @"&quot;").Replace("<", "&lt;").Replace(">", "&gt;")
                    , row.MachineName
                    , row.AuthenticatedUserId
                    , row.ApplicationId));
            }
            sb.Append("</" + INSERT_NODE + ">");

            // now send to the db
            DataAccessManager dam = new DataAccessManager(ConnectionString);
            dam.AddInputParameter("@EventXml", sb.ToString());
            dam.ExecuteNonQuery(GetFormattedStoredProcedureName(SP_EVENTLOG_INSERTBATCH));

            // we are finished, clear the table...
            EventLogTable.Clear();
        }
        #endregion
    }
}
