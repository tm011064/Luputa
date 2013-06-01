using System;

namespace CommonTools.Components.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public interface IClusteredCacheController
    {
        /// <summary>
        /// Gets or sets the CheckAtRequest cache key for storing the cache dictionary.
        /// </summary>
        /// <value>The check at request cache key.</value>
        string CheckAtRequestCacheKey { get; set; }
        /// <summary>
        /// Gets or sets the clustered caching mode to use to synchronize the cache. See the documentation
        /// for further info.
        /// </summary>
        /// <value>The clustered caching mode.</value>
        ClusteredCachingMode ClusteredCachingMode { get; set; }
        /// <summary>
        /// Gets or sets the milliseconds to sleep after a purge cache call. This value can be
        /// used to force the purge cache calling thread to sleep for a certain amount of time
        /// in order to wait for the sql dependency to fire. Normally, this event should fire quite
        /// instantanously, but if you expirience any delays adjust this value. Set this value to 
        /// -1 if you want to disable the thread sleep.
        /// </summary>
        /// <value>The milliseconds to sleep after cache purge.</value>
        int MillisecondsToSleepAfterCachePurge { get; set; }
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
        string ClusteredCacheSectionProviderType { get; set; }
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
        int CheckAtRequestIsUpToDateDelayInMilliseconds { get; set; }
        /// <summary>
        /// Creates the clustered cache controller instance.
        /// </summary>
        /// <returns></returns>
        IClusteredCacheController CreateClusteredCacheControllerInstance();
        /// <summary>
        /// Gets or sets the application id of the application which uses the clustered cache component. Use
        /// this property if you have to synchronize multiple autonomous applications within one database
        /// </summary>
        /// <value>The application id.</value>
        int ApplicationId { get; set; }
    }
}
