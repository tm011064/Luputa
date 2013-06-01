using System;
using System.Configuration;

namespace CommonTools.Components.Logging
{
    /// <summary>
    /// Provides the handler for the Log section of app.config.
    /// </summary>
    public class LogSection : ConfigurationSection, ILogController
    {
        #region ILogController Members

        /// <summary>
        /// Gets or sets the name of the connectionstrin gto use defined at app.config or web.config
        /// </summary>
        [ConfigurationProperty("connectionStringName", IsRequired = true)]
        public virtual string ConnectionStringName
        {
            get { return (string)base["connectionStringName"]; }
            set { base["connectionStringName"] = value; }
        }

        /// <summary>
        /// Gets or sets the prefix to use for all logging stored procedures
        /// </summary>
        [ConfigurationProperty("storedProcedurePrefix", IsRequired = false)]
        public virtual string StoredProcedurePrefix
        {
            get
            {
                if (base["storedProcedurePrefix"] != null)
                    return (string)base["storedProcedurePrefix"];

                return null;
            }
            set { base["storedProcedurePrefix"] = value; }
        }

        /// <summary>
        /// Gets or sets the application id. This value can be set either via the defaultApplicationId attribute or an
        /// ApplicationId appsettings key/value pair.
        /// </summary>
        /// <value>The application id.</value>
        [ConfigurationProperty("defaultApplicationId", IsRequired = false)]
        public virtual int ApplicationId
        {
            get
            {
                if (base["defaultApplicationId"] != null)
                    return (int)base["defaultApplicationId"];
                else if (ConfigurationManager.AppSettings["ApplicationId"] != null)
                    return int.Parse(ConfigurationManager.AppSettings["ApplicationId"]);

                return -1;
            }
            set { base["defaultApplicationId"] = value; }
        }

        /// <summary>
        /// Gets or sets the log section provider type.
        /// </summary>
        /// <value>The type of the log section provider.</value>
        [ConfigurationProperty("logSectionProviderType", IsRequired = false)]
        public virtual string LogSectionProviderType
        {
            get
            {
                if (base["logSectionProviderType"] != null)
                    return (string)base["logSectionProviderType"];

                return null;
            }
            set { base["logSectionProviderType"] = value; }
        }

        /// <summary>
        /// Gets or sets the log section provider type.
        /// </summary>
        /// <value>The type of the log section provider.</value>
        [ConfigurationProperty("asynchronousBatchSize", IsRequired = false)]
        public virtual int AsynchronousBatchSize
        {
            get
            {
                if (base["asynchronousBatchSize"] != null)
                    return (int)base["asynchronousBatchSize"];

                return 100;
            }
            set { base["asynchronousBatchSize"] = value; }
        }
        
        /// <summary>
        /// Gets or sets the log level if you use a hirarchic eventlogtype mechanism. For example, if you define the eventlogtypes to
        /// be Error = 1, Warning = 2, Info = 3, then you can use this property to determine that all messages lower than 2 (Warning)
        /// should be logged to the database while Infos (3) will be ignored.
        /// </summary>
        /// <value>The log level.</value>
        [ConfigurationProperty("logLevel", IsRequired = false)]
        public int LogLevel
        {
            get
            {
                if (base["logLevel"] != null)
                    return (int)base["logLevel"];

                return int.MaxValue;
            }
            set { base["logLevel"] = value; }
        }

        /// <summary>
        /// Gets or sets the log backup file path. This path is used if for some reason the log manager class can't connect to the
        /// specified database and therefore logs to a file.
        /// </summary>
        /// <value>The log backup file path.</value>
        /// <remarks>The file log mechanism is not optimized for performance so it should not be used for normal log records. It is
        /// a mere backup mechanism in case the database crashes or the network connection fails.</remarks>
        [ConfigurationProperty("logBackupFilePath", IsRequired = false)]
        public string LogBackupFilePath
        {
            get
            {
                if (base["logBackupFilePath"] != null)
                    return (string)base["logBackupFilePath"];

                return string.Empty;
            }
            set { base["logBackupFilePath"] = value; }
        }

        /// <summary>
        /// Creates an instance of a log controller.
        /// </summary>
        /// <returns></returns>
        public virtual ILogController CreateLogControllerInstance()
        {
            return LogSectionManager.LogSection;
        }

        #endregion
    }
}