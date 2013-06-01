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
    public class LogManager : LogManagerBase
    {
        #region members
        /// <summary>
        /// 
        /// </summary>
        public enum ExceptionsOrderBy : int
        {
            /// <summary>
            /// 
            /// </summary>
            DateLastOccurredDesc = 0,
            /// <summary>
            /// 
            /// </summary>
            DateCreatedDesc = 1,
            /// <summary>
            /// 
            /// </summary>
            TotalOccurrencesDesc = 2,
            /// <summary>
            /// 
            /// </summary>
            DateLastOccurredAsc = 10,
            /// <summary>
            /// 
            /// </summary>
            DateCreatedAsc = 11,
            /// <summary>
            /// 
            /// </summary>
            TotalOccurrencesAsc = 12
        }
        #endregion

        #region properties

        #endregion

        #region private static methods

        #endregion

        #region public static methods

        #region log events
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
        public static void LogEvent(Enum appLocation, Enum eventType, string message)
        {
            LogEvent(LogController.ApplicationId, int.Parse(appLocation.ToString("d")), int.Parse(eventType.ToString("d")), message, null);
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
        public static void LogEvent(int applicationId, Enum appLocation, Enum eventType, string message)
        {
            LogEvent(applicationId, int.Parse(appLocation.ToString("d")), int.Parse(eventType.ToString("d")), message, null);
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
        /// <param name="authenticatedUserId">The authenticated user id.</param>
        public static void LogEvent(int applicationId, int appLocation, int eventType, string message, string authenticatedUserId)
        {
            DataAccessManager dam = new DataAccessManager(ConnectionString);

            dam.AddInputParameter("@ApplicationId", applicationId);
            dam.AddInputParameter("@AppLocation", appLocation);
            dam.AddInputParameter("@EventType", eventType);
            dam.AddInputParameter("@Message", message);
            dam.AddInputParameter("@MachineName", System.Environment.MachineName);
            dam.AddInputParameter("@AuthenticatedUserId", authenticatedUserId);

            dam.ExecuteNonQuery(GetFormattedStoredProcedureName(SP_EVENTLOG_INSERT));
        }
        #endregion

        #region log exceptions
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="e">The exception to log.</param>
        public static void LogException(Enum appLocation, Exception e)
        {
            LogException(LogController.ApplicationId, int.Parse(appLocation.ToString("d")), ExceptionHandlingStatus.Unhandled, e);
        }
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="e">The exception to log.</param>
        public static void LogException(int appLocation, Exception e)
        {
            LogException(LogController.ApplicationId, appLocation, ExceptionHandlingStatus.Unhandled, e);
        }
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="e">The exception to log.</param>
        /// <param name="authenticatedUserId">The authenticated user id.</param>
        public static void LogException(Enum appLocation, Exception e, string authenticatedUserId)
        {
            LogException(LogController.ApplicationId, int.Parse(appLocation.ToString("d")), ExceptionHandlingStatus.Unhandled, e, authenticatedUserId);
        }
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="e">The exception to log.</param>
        /// <param name="authenticatedUserId">The authenticated user id.</param>
        public static void LogException(int appLocation, Exception e, string authenticatedUserId)
        {
            LogException(LogController.ApplicationId, appLocation, ExceptionHandlingStatus.Unhandled, e, authenticatedUserId);
        }
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="handlindStatus">The handling status of this exception.</param>
        /// <param name="e">The exception to log.</param>
        public static void LogException(Enum appLocation, ExceptionHandlingStatus handlindStatus, Exception e)
        {
            LogException(LogController.ApplicationId, int.Parse(appLocation.ToString("d")), handlindStatus, e, null);
        }
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="handlindStatus">The handling status of this exception.</param>
        /// <param name="e">The exception to log.</param>
        /// <param name="authenticatedUserId">The authenticated user id.</param>
        public static void LogException(Enum appLocation, ExceptionHandlingStatus handlindStatus, Exception e, string authenticatedUserId)
        {
            LogException(LogController.ApplicationId, int.Parse(appLocation.ToString("d")), handlindStatus, e, authenticatedUserId);
        }
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="handlindStatus">The handling status of this exception.</param>
        /// <param name="e">The exception to log.</param>
        public static void LogException(int appLocation, ExceptionHandlingStatus handlindStatus, Exception e)
        {
            LogException(LogController.ApplicationId, appLocation, handlindStatus, e, null);
        }
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="e">The exception to log.</param>
        public static void LogException(int applicationId, Enum appLocation, Exception e)
        {
            LogException(applicationId, int.Parse(appLocation.ToString("d")), ExceptionHandlingStatus.Unhandled, e);
        }
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="e">The exception to log.</param>
        public static void LogException(int applicationId, int appLocation, Exception e)
        {
            LogException(applicationId, appLocation, ExceptionHandlingStatus.Unhandled, e);
        }
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="e">The exception to log.</param>
        /// <param name="authenticatedUserId">The authenticated user id.</param>
        public static void LogException(int applicationId, Enum appLocation, Exception e, string authenticatedUserId)
        {
            LogException(applicationId, int.Parse(appLocation.ToString("d")), ExceptionHandlingStatus.Unhandled, e, authenticatedUserId);
        }
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="e">The exception to log.</param>
        /// <param name="authenticatedUserId">The authenticated user id.</param>
        public static void LogException(int applicationId, int appLocation, Exception e, string authenticatedUserId)
        {
            LogException(applicationId, appLocation, ExceptionHandlingStatus.Unhandled, e, authenticatedUserId);
        }
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="handlindStatus">The handling status of this exception.</param>
        /// <param name="e">The exception to log.</param>
        public static void LogException(int applicationId, Enum appLocation, ExceptionHandlingStatus handlindStatus, Exception e)
        {
            LogException(applicationId, int.Parse(appLocation.ToString("d")), handlindStatus, e, null);
        }
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="handlindStatus">The handling status of this exception.</param>
        /// <param name="e">The exception to log.</param>
        /// <param name="authenticatedUserId">The authenticated user id.</param>
        public static void LogException(int applicationId, Enum appLocation, ExceptionHandlingStatus handlindStatus, Exception e, string authenticatedUserId)
        {
            LogException(applicationId, int.Parse(appLocation.ToString("d")), handlindStatus, e, authenticatedUserId);
        }
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="handlindStatus">The handling status of this exception.</param>
        /// <param name="e">The exception to log.</param>
        public static void LogException(int applicationId, int appLocation, ExceptionHandlingStatus handlindStatus, Exception e)
        {
            LogException(applicationId, appLocation, handlindStatus, e, null);
        }
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="appLocation">The Application location. This parameter should represent where in your application the event occurred, e.g. MyEnum.BusinessLayer or MyEnum.UserCreation</param>
        /// <param name="handlindStatus">The handling status of this exception.</param>
        /// <param name="e">The exception to log.</param>
        /// <param name="authenticatedUserId">The authenticated user id.</param>
        public static void LogException(int applicationId, int appLocation, ExceptionHandlingStatus handlindStatus, Exception e, string authenticatedUserId)
        {
            DataAccessManager dam = new DataAccessManager(ConnectionString);
            string hashCode = string.Empty;

            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                hashCode = (appLocation.ToString() + context.Request.Url.PathAndQuery + e.ToString());

                dam.AddInputParameter("@ApplicationId", applicationId);
                dam.AddInputParameter("@AppLocation", appLocation);
                dam.AddInputParameter("@Exception", e.GetType().ToString());
                dam.AddInputParameter("@ExceptionMessage", ConversionHelper.GetFormattedException(string.Empty, e, TextFormat.HTML));
                dam.AddInputParameter("@Method", e.TargetSite == null ? string.Empty : e.TargetSite.ToString());
                dam.AddInputParameter("@IPAddress", context.Request.UserHostAddress);
                dam.AddInputParameter("@UserAgent", context.Request.UserAgent);
                dam.AddInputParameter("@HttpReferrer", context.Request.UrlReferrer == null ? null : context.Request.UrlReferrer.ToString());
                dam.AddInputParameter("@HttpVerb", context.Request.HttpMethod);
                dam.AddInputParameter("@Url", context.Request.Url.ToString());
                dam.AddInputParameter("@HashCode", hashCode.GetHashCode());
                dam.AddInputParameter("@HandlingStatus", (byte)handlindStatus);
                dam.AddInputParameter("@AuthenticatedUserId", authenticatedUserId);
                dam.AddInputParameter("@MachineName", System.Environment.MachineName);
            }
            else
            {
                hashCode = (appLocation.ToString() + e.ToString());

                dam.AddInputParameter("@ApplicationId", applicationId);
                dam.AddInputParameter("@AppLocation", appLocation);
                dam.AddInputParameter("@Exception", e.GetType().ToString());
                dam.AddInputParameter("@ExceptionMessage", ConversionHelper.GetFormattedException(string.Empty, e, TextFormat.HTML));
                dam.AddInputParameter("@Method", e.TargetSite == null ? string.Empty : e.TargetSite.ToString());
                dam.AddInputParameter("@IPAddress", null);
                dam.AddInputParameter("@UserAgent", null);
                dam.AddInputParameter("@HttpReferrer", null);
                dam.AddInputParameter("@HttpVerb", null);
                dam.AddInputParameter("@Url", null);
                dam.AddInputParameter("@HashCode", hashCode.GetHashCode());
                dam.AddInputParameter("@HandlingStatus", (byte)handlindStatus);
                dam.AddInputParameter("@AuthenticatedUserId", authenticatedUserId);
                dam.AddInputParameter("@MachineName", System.Environment.MachineName);
            }

            dam.ExecuteNonQuery(GetFormattedStoredProcedureName(SP_EXCEPTIONS_INSERT));
        }
        #endregion

        #region delete records
        /// <summary>
        /// Deletes an exception.
        /// </summary>
        /// <param name="exceptionId">The ExceptionId of the exception record to delete</param>
        public static void DeleteException(long exceptionId)
        {
            DeleteExceptionRecords(null, exceptionId, null);
        }
        /// <summary>
        /// Deletes exception records older than a specific amount of days.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="olderThanInDays">The amount of days.</param>
        public static void DeleteExceptionByDate(int applicationId, int olderThanInDays)
        {
            DeleteExceptionRecords(applicationId, null, olderThanInDays);
        }
        /// <summary>
        /// Deletes the exception by date.
        /// </summary>
        /// <param name="olderThanInDays">The older than in days.</param>
        public static void DeleteExceptionByDate(int olderThanInDays)
        {
            DeleteExceptionRecords(LogController.ApplicationId, null, olderThanInDays);
        }
        /// <summary>
        /// Deletes an event record.
        /// </summary>
        /// <param name="eventId">The EventId of the record to delete.</param>
        public static void DeleteEvent(long eventId)
        {
            DeleteEventRecords(null, eventId, -1, null);
        }
        /// <summary>
        /// Deletes event records older than a specific amount of days.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="olderThanInDays">The amount of days.</param>
        public static void DeleteEventByDate(int applicationId, int olderThanInDays)
        {
            DeleteEventRecords(applicationId, null, olderThanInDays, null);
        }
        /// <summary>
        /// Deletes event records older than a specific amount of days.
        /// </summary>
        /// <param name="olderThanInDays">The amount of days.</param>
        public static void DeleteEventByDate(int olderThanInDays)
        {
            DeleteEventRecords(LogController.ApplicationId, null, olderThanInDays, null);
        }
        #endregion

        #region get pages
        /// <summary>
        /// Gets a record page of all events.
        /// </summary>
        /// <param name="pageSize">The amount of record rows to retrieve.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="rowCount">The total amount of rows of the event table.</param>
        /// <returns></returns>
        public static LogDatasets.EventLogDataTable GetEventPage(int pageSize, int pageIndex, out int? rowCount)
        {
            return GetEventPage(LogController.ApplicationId, pageSize, pageIndex, out rowCount, -1, -1);
        }
        /// <summary>
        /// Gets a record page of all events.
        /// </summary>
        /// <param name="pageSize">The amount of record rows to retrieve.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="rowCount">The total amount of rows of the event table.</param>
        /// <param name="appLocationFilter">The ApplicationLocation to filter. Set this value to -1 if you don't want to filter by application locations.</param>
        /// <param name="eventTypeFilter">The EventType to filter. Set this value to -1 if you don't want to filter by event types.</param>
        /// <returns></returns>
        public static LogDatasets.EventLogDataTable GetEventPage(int pageSize, int pageIndex, out int? rowCount, Enum appLocationFilter, Enum eventTypeFilter)
        {
            return GetEventPage(LogController.ApplicationId, pageSize, pageIndex, out rowCount, int.Parse(appLocationFilter.ToString("d")), int.Parse(eventTypeFilter.ToString("d")));
        }
        /// <summary>
        /// Gets the event page.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="rowCount">The row count.</param>
        /// <param name="appLocationFilter">The app location filter.</param>
        /// <param name="eventTypeFilter">The event type filter.</param>
        /// <returns></returns>
        public static LogDatasets.EventLogDataTable GetEventPage(int pageSize, int pageIndex, out int? rowCount, int appLocationFilter, int eventTypeFilter)
        {
            return GetEventPage(LogController.ApplicationId, pageSize, pageIndex, out rowCount, appLocationFilter, eventTypeFilter);
        }
        /// <summary>
        /// Gets a record page of all events.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="pageSize">The amount of record rows to retrieve.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="rowCount">The total amount of rows of the event table.</param>
        /// <returns></returns>
        public static LogDatasets.EventLogDataTable GetEventPage(int applicationId, int pageSize, int pageIndex, out int? rowCount)
        {
            return GetEventPage(applicationId, pageSize, pageIndex, out rowCount, -1, -1);
        }
        /// <summary>
        /// Gets a record page of all events.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="pageSize">The amount of record rows to retrieve.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="rowCount">The total amount of rows of the event table.</param>
        /// <param name="appLocationFilter">The ApplicationLocation to filter. Set this value to -1 if you don't want to filter by application locations.</param>
        /// <param name="eventTypeFilter">The EventType to filter. Set this value to -1 if you don't want to filter by event types.</param>
        /// <returns></returns>
        public static LogDatasets.EventLogDataTable GetEventPage(int applicationId, int pageSize, int pageIndex, out int? rowCount, Enum appLocationFilter, Enum eventTypeFilter)
        {
            return GetEventPage(applicationId, pageSize, pageIndex, out rowCount, int.Parse(appLocationFilter.ToString("d")), int.Parse(eventTypeFilter.ToString("d")));
        }
        /// <summary>
        /// Gets a record page of all events.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="pageSize">The amount of record rows to retrieve.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="rowCount">The total amount of rows of the event table.</param>
        /// <param name="appLocationFilter">The ApplicationLocation to filter. Set this value to -1 if you don't want to filter by application locations.</param>
        /// <param name="eventTypeFilter">The EventType to filter. Set this value to -1 if you don't want to filter by event types.</param>
        /// <returns></returns>
        public static LogDatasets.EventLogDataTable GetEventPage(int applicationId, int pageSize, int pageIndex, out int? rowCount, int appLocationFilter, int eventTypeFilter)
        {
            rowCount = 0;
            DataAccessManager dam = new DataAccessManager(ConnectionString);

            dam.AddInputParameter("@ApplicationId", applicationId);
            dam.AddInputParameter("@PageIndex", pageIndex);
            dam.AddInputParameter("@PageSize", pageSize);
            dam.AddInputParameter("@AppLocationFilter", appLocationFilter);
            dam.AddInputParameter("@EventTypeFilter", eventTypeFilter);
            dam.AddOutPutParameter("@RowCount", SqlDbType.Int);

            Dictionary<string, object> outputParameters = new Dictionary<string, object>();
            LogDatasets.EventLogDataTable dt = dam.ExecuteTableQuery<LogDatasets.EventLogDataTable>(GetFormattedStoredProcedureName(SP_EVENTLOG_GETPAGE), out outputParameters);
            rowCount = (int)outputParameters["@RowCount"];

            return dt;
        }
        /// <summary>
        /// Gets the event page.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="rowCount">The row count.</param>
        /// <param name="appLocationFilter">The app location filter.</param>
        /// <param name="eventTypeFilter">The event type filter.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public static LogDatasets.EventLogDataTable GetEventPage(int applicationId, int pageSize, int pageIndex, out int? rowCount, int appLocationFilter, int eventTypeFilter
            , int userId, DateTime startDate, DateTime endDate)
        {
            rowCount = 0;
            DataAccessManager dam = new DataAccessManager(ConnectionString);

            dam.AddInputParameter("@ApplicationId", applicationId);
            dam.AddInputParameter("@PageIndex", pageIndex);
            dam.AddInputParameter("@PageSize", pageSize);
            dam.AddInputParameter("@AppLocationFilter", appLocationFilter);
            dam.AddInputParameter("@EventTypeFilter", eventTypeFilter);

            dam.AddInputParameter("@AuthenticatedUserIdFilter", userId.ToString());
            dam.AddInputParameter("@StartDate", startDate);
            dam.AddInputParameter("@EndDate", endDate);

            dam.AddOutPutParameter("@RowCount", SqlDbType.Int);

            Dictionary<string, object> outputParameters = new Dictionary<string, object>();
            LogDatasets.EventLogDataTable dt = dam.ExecuteTableQuery<LogDatasets.EventLogDataTable>(GetFormattedStoredProcedureName(SP_EVENTLOG_GETPAGE_WITHFILTERS), out outputParameters);
            rowCount = (int)outputParameters["@RowCount"];

            return dt;
        }

        /// <summary>
        /// Gets the exception page.
        /// </summary>
        /// <param name="pageSize">The amount of record rows to retrieve.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="rowCount">The total amount of rows of the exception table.</param>
        /// <returns></returns>
        public static LogDatasets.ExceptionLogDataTable GetExceptionPage(int pageSize, int pageIndex, out int? rowCount)
        {
            return GetExceptionPage(LogController.ApplicationId, pageSize, pageIndex, out rowCount, -1, -1, ExceptionsOrderBy.DateLastOccurredDesc);
        }
        /// <summary>
        /// Gets the exception page.
        /// </summary>
        /// <param name="pageSize">The amount of record rows to retrieve.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="rowCount">The total amount of rows of the exception table.</param>
        /// <param name="appLocationFilter">The ApplicationLocation to filter. Set this value to -1 if you don't want to filter by application locations.</param>
        /// <returns></returns>
        public static LogDatasets.ExceptionLogDataTable GetExceptionPage(int pageSize, int pageIndex, out int? rowCount, Enum appLocationFilter)
        {
            return GetExceptionPage(LogController.ApplicationId, pageSize, pageIndex, out rowCount, int.Parse(appLocationFilter.ToString("d")), -1, ExceptionsOrderBy.DateLastOccurredDesc);
        }
        /// <summary>
        /// Gets the exception page.
        /// </summary>
        /// <param name="pageSize">The amount of record rows to retrieve.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="rowCount">The total amount of rows of the exception table.</param>
        /// <param name="appLocationFilter">The ApplicationLocation to filter. Set this value to -1 if you don't want to filter by application locations.</param>
        /// <param name="handlingStatus">The handling status of this exception</param>
        /// <returns></returns>
        public static LogDatasets.ExceptionLogDataTable GetExceptionPage(int pageSize, int pageIndex, out int? rowCount, Enum appLocationFilter, ExceptionHandlingStatus handlingStatus)
        {
            return GetExceptionPage(LogController.ApplicationId, pageSize, pageIndex, out rowCount, int.Parse(appLocationFilter.ToString("d")), (int)handlingStatus, ExceptionsOrderBy.DateLastOccurredDesc);
        }
        /// <summary>
        /// Gets the exception page.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="rowCount">The row count.</param>
        /// <param name="appLocationFilter">The app location filter.</param>
        /// <param name="handlingStatus">The handling status.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns></returns>
        public static LogDatasets.ExceptionLogDataTable GetExceptionPage(int pageSize, int pageIndex, out int? rowCount, int appLocationFilter, int handlingStatus
            , ExceptionsOrderBy orderBy)
        {
            return GetExceptionPage(LogController.ApplicationId, pageSize, pageIndex, out rowCount, appLocationFilter, handlingStatus, ExceptionsOrderBy.DateLastOccurredDesc);
        }
        /// <summary>
        /// Gets the exception page.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="pageSize">The amount of record rows to retrieve.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="rowCount">The total amount of rows of the exception table.</param>
        /// <returns></returns>
        public static LogDatasets.ExceptionLogDataTable GetExceptionPage(int applicationId, int pageSize, int pageIndex, out int? rowCount)
        {
            return GetExceptionPage(applicationId, pageSize, pageIndex, out rowCount, -1, -1, ExceptionsOrderBy.DateLastOccurredDesc);
        }
        /// <summary>
        /// Gets the exception page.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="pageSize">The amount of record rows to retrieve.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="rowCount">The total amount of rows of the exception table.</param>
        /// <param name="appLocationFilter">The ApplicationLocation to filter. Set this value to -1 if you don't want to filter by application locations.</param>
        /// <returns></returns>
        public static LogDatasets.ExceptionLogDataTable GetExceptionPage(int applicationId, int pageSize, int pageIndex, out int? rowCount, Enum appLocationFilter)
        {
            return GetExceptionPage(applicationId, pageSize, pageIndex, out rowCount, int.Parse(appLocationFilter.ToString("d")), -1, ExceptionsOrderBy.DateLastOccurredDesc);
        }
        /// <summary>
        /// Gets the exception page.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="pageSize">The amount of record rows to retrieve.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="rowCount">The total amount of rows of the exception table.</param>
        /// <param name="appLocationFilter">The ApplicationLocation to filter. Set this value to -1 if you don't want to filter by application locations.</param>
        /// <param name="handlingStatus">The handling status of this exception</param>
        /// <returns></returns>
        public static LogDatasets.ExceptionLogDataTable GetExceptionPage(int applicationId, int pageSize, int pageIndex, out int? rowCount, Enum appLocationFilter, ExceptionHandlingStatus handlingStatus)
        {
            return GetExceptionPage(applicationId, pageSize, pageIndex, out rowCount, int.Parse(appLocationFilter.ToString("d")), (int)handlingStatus, ExceptionsOrderBy.DateLastOccurredDesc);
        }
        /// <summary>
        /// Gets the exception page.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="pageSize">The amount of record rows to retrieve.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="rowCount">The total amount of rows of the exception table.</param>
        /// <param name="appLocationFilter">The ApplicationLocation to filter. Set this value to -1 if you don't want to filter by application locations.</param>
        /// <param name="handlingStatus">The handling status of this exception. Set this value to -1 if you don't want to filter by handling status.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns></returns>
        public static LogDatasets.ExceptionLogDataTable GetExceptionPage(int applicationId, int pageSize, int pageIndex, out int? rowCount, int appLocationFilter, int handlingStatus
            , ExceptionsOrderBy orderBy)
        {
            rowCount = 0;
            DataAccessManager dam = new DataAccessManager(ConnectionString);

            dam.AddInputParameter("@ApplicationId", applicationId);
            dam.AddInputParameter("@PageIndex", pageIndex);
            dam.AddInputParameter("@PageSize", pageSize);
            dam.AddInputParameter("@AppLocationFilter", appLocationFilter);
            dam.AddInputParameter("@HandlingStatus", handlingStatus);
            dam.AddOutPutParameter("@RowCount", SqlDbType.Int);
            dam.AddInputParameter("@OrderBy", (int)orderBy);

            Dictionary<string, object> outputParameters = new Dictionary<string, object>();
            LogDatasets.ExceptionLogDataTable dt = dam.ExecuteTableQuery<LogDatasets.ExceptionLogDataTable>(GetFormattedStoredProcedureName(SP_EXCEPTIONS_GETPAGE), out outputParameters);
            rowCount = (int)outputParameters["@RowCount"];

            return dt;
        }
        #endregion

        #region update
        /// <summary>
        /// Updates the exception handling status.
        /// </summary>
        /// <param name="exceptionId">The id of the exception record to update</param>
        /// <param name="handlingStatus">The new handlig status</param>
        public static void UpdateExceptionHandlingStatus(long exceptionId, ExceptionHandlingStatus handlingStatus)
        {
            DataAccessManager dam = new DataAccessManager(ConnectionString);

            dam.AddInputParameter("@ExceptionId", exceptionId);
            dam.AddInputParameter("@HandlingStatus", (byte)handlingStatus);

            dam.ExecuteNonQuery(GetFormattedStoredProcedureName(SP_EXCEPTIONS_UPDATE));
        }
        #endregion

        #endregion
    }
}
