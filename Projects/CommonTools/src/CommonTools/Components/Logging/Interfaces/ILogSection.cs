using System;

namespace CommonTools.Components.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogController
    {
        /// <summary>
        /// Gets or sets the name of the connection string.
        /// </summary>
        /// <value>The name of the connection string.</value>
        string ConnectionStringName { get; set; }
        /// <summary>
        /// Gets or sets the stored procedure prefix.
        /// </summary>
        /// <value>The stored procedure prefix.</value>
        string StoredProcedurePrefix { get; set; }
        /// <summary>
        /// Gets or sets the type of the log section provider.
        /// </summary>
        /// <value>The type of the log section provider.</value>
        string LogSectionProviderType { get; set; }
        /// <summary>
        /// Creates a log controller instance.
        /// </summary>
        /// <returns></returns>
        ILogController CreateLogControllerInstance();
        /// <summary>
        /// Gets or sets the size of the asynchronous batch.
        /// </summary>
        /// <value>The size of the asynchronous batch.</value>
        int AsynchronousBatchSize { get; set; }
        /// <summary>
        /// Gets or sets the application id. This value can be set either via the defaultApplicationId attribute or an
        /// ApplicationId appsettings key/value pair.
        /// </summary>
        /// <value>The application id.</value>
        int ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets the log level if you use a hirarchic eventlogtype mechanism. For example, if you define the eventlogtypes to
        /// be Error = 1, Warning = 2, Info = 3, then you can use this property to determine that all messages lower than 2 (Warning)
        /// should be logged to the database while Infos (3) will be ignored.
        /// </summary>
        /// <value>The log level.</value>
        int LogLevel { get; set; }
        /// <summary>
        /// Gets or sets the log backup file path. This path is used if for some reason the log manager class can't connect to the
        /// specified database and therefore logs to a file.
        /// </summary>
        /// <remarks>The file log mechanism is not optimized for performance so it should not be used for normal log records. It is
        /// a mere backup mechanism in case the database crashes or the network connection fails.</remarks>
        /// <value>The log backup file path.</value>
        string LogBackupFilePath { get; set; }

    }
}
