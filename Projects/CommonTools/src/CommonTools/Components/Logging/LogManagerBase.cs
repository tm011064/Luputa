using System;
using System.Data.SqlClient;
using System.Data.Common;
using System.Web;
using CommonTools.Data;
using System.Data;
using System.Collections.Generic;

namespace CommonTools.Components.Logging
{

    /// <summary>
    /// This class handles event and exception logging.
    /// </summary>
    public class LogManagerBase
    {
        #region members
        /// <summary>
        /// 
        /// </summary>
        protected const string SP_EVENTLOG_GET = "EventLog_Get";
        /// <summary>
        /// 
        /// </summary>
        protected const string SP_EXCEPTIONS_GET = "Exceptions_Get";
        /// <summary>
        /// 
        /// </summary>
        protected const string SP_EXCEPTIONS_INSERT = "Exceptions_Insert";
        /// <summary>
        /// 
        /// </summary>
        protected const string SP_EXCEPTIONS_DELETE = "Exceptions_Delete";
        /// <summary>
        /// 
        /// </summary>
        protected const string SP_EVENTLOG_INSERT = "EventLog_Insert";
        /// <summary>
        /// 
        /// </summary>
        protected const string SP_EVENTLOG_UPDATE = "EventLog_Update";
        /// <summary>
        /// 
        /// </summary>
        protected const string SP_EVENTLOG_DELETE = "EventLog_Delete";
        /// <summary>
        /// 
        /// </summary>
        protected const string SP_EVENTLOG_GETPAGE = "EventLog_GetPage";
        /// <summary>
        /// 
        /// </summary>
        protected const string SP_EVENTLOG_GETPAGE_WITHFILTERS = "EventLog_GetPageWithAllFilters";
        /// <summary>
        /// 
        /// </summary>
        protected const string SP_EXCEPTIONS_UPDATE = "Exceptions_Update";
        /// <summary>
        /// 
        /// </summary>
        protected const string SP_EXCEPTIONS_GETPAGE = "Exceptions_GetPage";
        #endregion

        #region properties
        private static ILogController _LogController;
        /// <summary>
        /// Gets the log controller.
        /// </summary>
        /// <value>The log controller.</value>
        protected static ILogController LogController
        {
            get
            {
                if (_LogController == null)
                    _LogController = LogControllerFactory.CreateLogController();

                return _LogController;
            }
        }

        private static string _ConnectionString = string.Empty;
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_ConnectionString))
                    _ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[LogController.ConnectionStringName].ConnectionString;

                return _ConnectionString;
            }
            protected set
            {
                _ConnectionString = value;
            }
        }

        private static string _StoredProcedurePrefix = string.Empty;
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        protected static string StoredProcedurePrefix
        {
            get
            {
                if (string.IsNullOrEmpty(_StoredProcedurePrefix))
                    _StoredProcedurePrefix = LogController.StoredProcedurePrefix;

                return _StoredProcedurePrefix;
            }
            set
            {
                _StoredProcedurePrefix = value;
            }
        }


        private static string _LogBackupFilePath = string.Empty;
        /// <summary>
        /// Gets or sets the log backup file path. This path is used if for some reason the log manager class can't connect to the
        /// specified database and therefore logs to a file.
        /// </summary>
        /// <value>The log backup file path.</value>
        /// <remarks>The file log mechanism is not optimized for performance so it should not be used for normal log records. It is
        /// a mere backup mechanism in case the database crashes or the network connection fails.</remarks>
        public static string LogBackupFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_LogBackupFilePath))
                    _LogBackupFilePath = LogController.LogBackupFilePath;

                return _LogBackupFilePath;
            }
            set { _LogBackupFilePath = value; }
        }


        private static int? _LogLevel = null;
        /// <summary>
        /// Gets or sets the log level if you use a hirarchic eventlogtype mechanism. For example, if you define the eventlogtypes to
        /// be Error = 1, Warning = 2, Info = 3, then you can use this property to determine that all messages lower than 2 (Warning)
        /// should be logged to the database while Infos (3) will be ignored.
        /// </summary>
        /// <value>The log level.</value>
        public static int LogLevel
        {
            get
            {
                if (!_LogLevel.HasValue)
                    _LogLevel = LogController.LogLevel;

                return _LogLevel.Value;
            }
            set { _LogLevel = value; }
        }
        #endregion

        #region internal methods for testing
        /// <summary>
        /// Sets the connection string. This method should only be used for testing.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        internal protected static void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }
        /// <summary>
        /// Sets the stored procedure prefix. This method should only be used for testing.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        internal protected static void SetStoredProcedurePrefix(string prefix)
        {
            StoredProcedurePrefix = prefix;
        }
        #endregion

        #region private static methods
        /// <summary>
        /// Gets the name of the formatted stored procedure ( = prefix + stored procedure name).
        /// </summary>
        /// <param name="storedProcedure">The procedure</param>
        /// <returns></returns>
        protected static string GetFormattedStoredProcedureName(string storedProcedure)
        {
            return StoredProcedurePrefix + storedProcedure;
        }
        /// <summary>
        /// Deletes exception records older than a specific amount of days.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="exceptionId">The ExceptionId of the exception record to delete</param>
        /// <param name="olderThanInDays">The amount of days.</param>
        protected static void DeleteExceptionRecords(int? applicationId, long? exceptionId, int? olderThanInDays)
        {
            DataAccessManager dam = new DataAccessManager(ConnectionString);
            dam.AddInputParameter("@ApplicationId", (applicationId.HasValue ? applicationId.Value : -1));
            dam.AddInputParameter("@ExceptionId", (exceptionId.HasValue ? exceptionId.Value : -1));
            dam.AddInputParameter("@OlderThanInDays", (olderThanInDays.HasValue ? olderThanInDays.Value : -1));

            dam.ExecuteNonQuery(GetFormattedStoredProcedureName(SP_EXCEPTIONS_DELETE));
        }
        /// <summary>
        /// Deletes the event records.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="eventId">The EventId of the record to delete.</param>
        /// <param name="olderThanInDays">The amount of days.</param>
        /// <param name="eventTypeFilter">The event type filter.</param>
        protected static void DeleteEventRecords(int? applicationId, long? eventId, int? olderThanInDays, int? eventTypeFilter)
        {
            DataAccessManager dam = new DataAccessManager(ConnectionString);
            dam.AddInputParameter("@ApplicationId", (applicationId.HasValue ? applicationId.Value : -1));
            dam.AddInputParameter("@EventId", (eventId.HasValue ? eventId.Value : -1));
            dam.AddInputParameter("@OlderThanInDays", (olderThanInDays.HasValue ? olderThanInDays.Value : -1));
            dam.AddInputParameter("@EventType", (eventTypeFilter.HasValue ? eventTypeFilter.Value : -1));

            dam.ExecuteNonQuery(GetFormattedStoredProcedureName(SP_EVENTLOG_DELETE));
        }
        #endregion
    }
}
