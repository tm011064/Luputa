using System;
using System.Configuration;

namespace CommonTools.Components.Caching
{
    /// <summary>
    /// Provides the handler for the ClusteredCache section of app.config.
    /// </summary>
    public class ClusteredCacheSection : ConfigurationSection, IClusteredCacheController
    {
        #region IClusteredCacheController Members

        /// <summary>
        /// Gets or sets the name of the connection string.
        /// </summary>
        /// <value>The name of the connection string.</value>
        [ConfigurationProperty("connectionStringName", IsRequired = true)]
        public string ConnectionStringName
        {
            get { return (string)base["connectionStringName"]; }
            set { base["connectionStringName"] = value; }
        }

        /// <summary>
        /// Gets or sets the stored procedure prefix.
        /// </summary>
        /// <value>The stored procedure prefix.</value>
        [ConfigurationProperty("storedProcedurePrefix", IsRequired = false)]
        public string StoredProcedurePrefix
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
        /// Gets or sets the type of the log section provider.
        /// </summary>
        /// <value>The type of the log section provider.</value>
        [ConfigurationProperty("logSectionProviderType", IsRequired = false)]
        public string ClusteredCacheSectionProviderType
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
        /// Gets or sets the clustered caching mode to use to synchronize the cache. See the documentation
        /// for further info.
        /// </summary>
        /// <value>The clustered caching mode.</value>
        [ConfigurationProperty("clusteredCachingMode", IsRequired = true)]
        public ClusteredCachingMode ClusteredCachingMode
        {
            get
            {
                if (base["clusteredCachingMode"] != null)
                    return (ClusteredCachingMode)base["clusteredCachingMode"];

                return ClusteredCachingMode.CheckAtRequest;
            }
            set { base["clusteredCachingMode"] = value; }
        }

        /// <summary>
        /// Gets or sets the CheckAtRequest cache key for storing the cache dictionary.
        /// </summary>
        /// <value>The check at request cache key.</value>
        [ConfigurationProperty("checkAtRequestCacheKey", IsRequired = false)]
        public string CheckAtRequestCacheKey
        {
            get
            {
                if (base["checkAtRequestCacheKey"] != null)
                    return (string)base["checkAtRequestCacheKey"];

                return "__carck";
            }
            set { base["checkAtRequestCacheKey"] = value; }
        }

        /// <summary>
        /// Gets or sets the milliseconds to sleep after a purge cache call. This value can be
        /// used to force the purge cache calling thread to sleep for a certain amount of time
        /// in order to wait for the sql dependency to fire. Normally, this event should fire quite
        /// instantanously, but if you expirience any delays adjust this value. Set this value to
        /// -1 if you want to disable the thread sleep.
        /// </summary>
        /// <value>The milliseconds to sleep after cache purge.</value>
        [ConfigurationProperty("millisecondsToSleepAfterCachePurge", IsRequired = false)]
        public int MillisecondsToSleepAfterCachePurge
        {
            get
            {
                if (base["millisecondsToSleepAfterCachePurge"] != null)
                    return (int)base["millisecondsToSleepAfterCachePurge"];

                return -1;
            }
            set { base["millisecondsToSleepAfterCachePurge"] = value; }
        }

        private int? _CheckAtRequestIsUpToDateDelayInMilliseconds = null;
        /// <summary>
        /// This value defines how often the clustered cache mechanism checks against the database whether
        /// the current synchronyzed value of the cached item is up to date. If this value is not set or set
        /// to a value &lt;= 0, at each object request a call is made to the database to check whether
        /// the requested cached object is up to date. If this value is greater than 0, every xxx milliseconds
        /// after the last call the clustered cache manager makes a call to the database to check whether we are
        /// up to date.
        /// This can improve performance significally if you have a very busy site (E.g.: if there are 10000
        /// object requests a second, if this value is set to 200, the database is only called 5 times whereas it
        /// is called 10000 times if this property is not used or &lt;= 0).
        /// </summary>
        /// <value>The check at request is up to date delay.</value>
        [ConfigurationProperty("checkAtRequestIsUpToDateDelayInMilliseconds", IsRequired = false)]
        public int CheckAtRequestIsUpToDateDelayInMilliseconds
        {
            get
            {
                if (!_CheckAtRequestIsUpToDateDelayInMilliseconds.HasValue)
                {
                    if (base["checkAtRequestIsUpToDateDelayInMilliseconds"] != null)
                        _CheckAtRequestIsUpToDateDelayInMilliseconds = (int)base["checkAtRequestIsUpToDateDelayInMilliseconds"];
                    else
                        _CheckAtRequestIsUpToDateDelayInMilliseconds = -1;
                }
                return _CheckAtRequestIsUpToDateDelayInMilliseconds.Value;
            }
            set { _CheckAtRequestIsUpToDateDelayInMilliseconds = value; }
        }

        /// <summary>
        /// Gets or sets the application id of the application which uses the clustered cache component. Use
        /// this property if you have to synchronize multiple autonomous applications within one database
        /// </summary>
        /// <value>The application id.</value>
        [ConfigurationProperty("applicationId", IsRequired = false)]
        public int ApplicationId
        {
            get
            {
                if (base["applicationId"] != null)
                    return (int)base["applicationId"];

                return 0;
            }
            set { base["applicationId"] = value; }
        }

        /// <summary>
        /// Creates the clustered cache controller instance.
        /// </summary>
        /// <returns></returns>
        public IClusteredCacheController CreateClusteredCacheControllerInstance()
        {
            return ClusteredCacheSectionManager.ClusteredCacheSection;
        }

        #endregion
    }
}