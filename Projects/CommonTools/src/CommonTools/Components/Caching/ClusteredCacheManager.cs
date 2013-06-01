using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.Caching;
using CommonTools.Data;
using System.Threading;
using System.Collections;

namespace CommonTools.Components.Caching
{
    /// <summary>
    /// This class handles HttpRuntime Cache synchronization on a cluster using SQL Server 2005 service broker
    /// query notifications. It basically stores all keys from the HttpRuntime.Cache collection at a database
    /// which has to have Service Broker enabled (sql command: ALTER DATABASE MyDatabase SET ENABLE_BROKER, only 
    /// works on SQL Server 2005 Enterprise Edition).
    /// </summary>
    /// <example>
    /// <![CDATA[
    ///     
    ///     // ****************************************
    ///     // Service Broker mode:
    ///     // ----------------------------------------
    ///     
    ///     // this could be called at global.asax
    ///     ClusteredCacheManager.RestartSQLDependencyListener("myConnectionString");
    /// 
    ///     string cacheKey1 = "ck_1";
    ///     int oldCacheValue = 1;
    ///     int newCacheValue = 2;
    ///     
    ///     ClusteredCacheManager.AddCacheItem(cacheKey1, oldCacheValue, CacheItemPriority.Normal);
    ///     
    ///     // get the value from cache -> test == 1
    ///     int test = ClusteredCacheManager.GetCacheItem(cacheKey1, delegate() { return newCacheValue; }, CacheItemPriority.Normal));
    ///     
    ///     // purge from cache
    ///     ClusteredCacheManager.PurgeCacheItem(cacheKey1);
    ///     
    ///     // this call should refresh the cache... -> test == 2
    ///     cachedValue = ClusteredCacheManager.GetCacheItem(cacheKey1, delegate() { return newCacheValue; }, CacheItemPriority.Normal));
    ///
    ///     
    ///     // this could be called at global.asax
    ///     ClusteredCacheManager.StopSQLDependencyListener();
    ///     
    /// 
    ///     // ****************************************
    ///     // CheckAtRequest mode:
    ///     // ----------------------------------------
    ///     
    ///     string cacheKey1 = "ck_1";
    ///     int oldCacheValue = 1;
    ///     int newCacheValue = 2;
    ///     
    ///     ClusteredCacheManager.AddCacheItem(cacheKey1, oldCacheValue, CacheItemPriority.Normal);
    ///     
    ///     // get the value from cache -> test == 1
    ///     int test = ClusteredCacheManager.GetCacheItem(cacheKey1, delegate() { return newCacheValue; }, CacheItemPriority.Normal));
    ///     
    ///     // purge from cache
    ///     ClusteredCacheManager.PurgeCacheItem(cacheKey1);
    ///     
    ///     // this call should refresh the cache... -> test == 2
    ///     cachedValue = ClusteredCacheManager.GetCacheItem(cacheKey1, delegate() { return newCacheValue; }, CacheItemPriority.Normal));
    ///
    ///     // remove dependency and cache...
    ///     ClusteredCacheManager.RemoveDependecy(cacheKey1);
    ///     
    ///     // the cachedValue will be null...
    ///     cachedValue = ClusteredCacheManager.GetCacheItem(cacheKey1, delegate() { return newCacheValue; }, CacheItemPriority.Normal));
    /// ]]>
    /// </example>
    /// <remarks>
    /// THIS METHOD IS NOT PROPERLY TESTED YET (Roman Majeski, 09.05.2008)!
    /// </remarks>
    public static class ClusteredCacheManager
    {
        #region delegates
        /// <summary>
        /// 
        /// </summary>
        public delegate object ReloadObjectAtCache();
        #endregion

        #region constant members
        private const string SP_SELECT = "CacheKeys_GetLastUpdate";
        private const string SP_INSERT_UPDATE = "CacheKeys_InsertOrUpdate";
        private const string SP_DELETE = "CacheKeys_Delete";
        private const string SP_GET_ALL = "CacheKeys_Get";
        private const string SP_DELETE_ALL = "CacheKeys_DeleteAll";
        #endregion

        #region internal test methods
        /// <summary>
        /// Sets the CheckAtRequestIsUpToDateDelayInMilliseconds attribute of the current ClusteredCacheController. This method is 
        /// used for testing purposes only.
        /// </summary>
        /// <param name="milliseconds">The value.</param>
        internal static void SetCheckAtRequestIsUpToDateDelayInMilliseconds(int milliseconds)
        {
            ClusteredCacheController.CheckAtRequestIsUpToDateDelayInMilliseconds = milliseconds;
        }
        #endregion

        #region static members
        private static Dictionary<string, CacheKeySqlDependency> _ServiceBrokerDependencies = new Dictionary<string, CacheKeySqlDependency>();
        private static object _CacheLock = new object();

        private static IClusteredCacheController _ClusteredCacheController;
        private static IClusteredCacheController ClusteredCacheController
        {
            get
            {
                if (_ClusteredCacheController == null)
                    _ClusteredCacheController = ClusteredCacheControllerFactory.CreateClusteredCacheController();

                return _ClusteredCacheController;
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
                {
                    _ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[ClusteredCacheController.ConnectionStringName].ConnectionString;
                }

                return _ConnectionString;
            }
            private set
            {
                _ConnectionString = value;
            }
        }

        private static string _StoredProcedurePrefix = string.Empty;
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        private static string StoredProcedurePrefix
        {
            get
            {
                if (string.IsNullOrEmpty(_StoredProcedurePrefix))
                {
                    _StoredProcedurePrefix = ClusteredCacheController.StoredProcedurePrefix;
                }
                return _StoredProcedurePrefix;
            }
            set
            {
                _StoredProcedurePrefix = value;
            }
        }

        private static Dictionary<string, CheckAtRequestInfo> _CheckAtRequestDependencies = new Dictionary<string, CheckAtRequestInfo>();
        /// <summary>
        /// Gets the check at request dependencies.
        /// </summary>
        /// <value>The check at request dependencies.</value>
        private static Dictionary<string, CheckAtRequestInfo> CheckAtRequestDependencies
        {
            get
            {
                if (HttpRuntime.Cache[ClusteredCacheController.CheckAtRequestCacheKey] == null)
                {
                    Dictionary<string, CheckAtRequestInfo> keys = new Dictionary<string, CheckAtRequestInfo>();

                    DataAccessManager dam = new DataAccessManager(ConnectionString);
                    dam.AddInputParameter("@ApplicationId", ClusteredCacheController.ApplicationId);
                    ClusteredCacheDatasets.CacheKeysDataTable dt = dam.ExecuteTableQuery<ClusteredCacheDatasets.CacheKeysDataTable>(
                        GetFormattedStoredProcedureName(SP_GET_ALL));

                    if (ClusteredCacheController.CheckAtRequestIsUpToDateDelayInMilliseconds > 0)
                    {
                        foreach (ClusteredCacheDatasets.CacheKeysRow dr in dt)
                            keys.Add(dr.CacheKey, new CheckAtRequestInfo(dr.LastUpdate, DateTime.UtcNow));
                    }
                    else
                    {
                        foreach (ClusteredCacheDatasets.CacheKeysRow dr in dt)
                            keys.Add(dr.CacheKey, new CheckAtRequestInfo(dr.LastUpdate));
                    }

                    HttpRuntime.Cache.Insert(
                        ClusteredCacheController.CheckAtRequestCacheKey
                        , keys
                        , null
                        , Cache.NoAbsoluteExpiration
                        , Cache.NoSlidingExpiration
                        , CacheItemPriority.NotRemovable
                        , null);
                }

                return (Dictionary<string, CheckAtRequestInfo>)HttpRuntime.Cache[ClusteredCacheController.CheckAtRequestCacheKey];
            }
        }

        private static ClusteredCachingMode _CachingMode = ClusteredCachingMode.CheckAtRequest;
        private static bool _IsCachingModeSet = false;
        /// <summary>
        /// Gets the caching mode.
        /// </summary>
        /// <value>The caching mode.</value>
        public static ClusteredCachingMode CachingMode
        {
            get
            {
                if (_IsCachingModeSet)
                    return _CachingMode;

                return ClusteredCacheController.ClusteredCachingMode;
            }
            internal set
            {// used for testing
                _IsCachingModeSet = true;
                _CachingMode = value;
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Gets the name of the formatted stored procedure ( = prefix + stored procedure name).
        /// </summary>
        /// <param name="storedProcedure">The procedure</param>
        /// <returns></returns>
        private static string GetFormattedStoredProcedureName(string storedProcedure)
        {
            return StoredProcedurePrefix + storedProcedure;
        }
        /// <summary>
        /// Called when an sql dependency fires.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Data.SqlClient.SqlNotificationEventArgs"/> instance containing the event data.</param>
        private static void OnChange(object sender, SqlNotificationEventArgs e)
        {
            lock (_CacheLock)
            {
                SqlDependency dependency = sender as SqlDependency;
                dependency.OnChange -= OnChange;

                string cacheKey = string.Empty;
                if (_ServiceBrokerDependencies.ContainsKey(dependency.Id))
                {
                    HttpRuntime.Cache.Remove(_ServiceBrokerDependencies[dependency.Id].CacheKey);
                    _ServiceBrokerDependencies.Remove(dependency.Id);
                }
            }
        }

        /// <summary>
        /// Gets the dependency ID from cache key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static string GetDependencyIDFromCacheKey(string key)
        {
            foreach (CacheKeySqlDependency item in _ServiceBrokerDependencies.Values)
            {
                if (item.CacheKey == key)
                {
                    return item.SqlDependency.Id;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the check at request info.
        /// </summary>
        /// <param name="ticks">The ticks.</param>
        /// <returns></returns>
        private static CheckAtRequestInfo GetCheckAtRequestInfo(long ticks)
        {
            return ClusteredCacheController.CheckAtRequestIsUpToDateDelayInMilliseconds > 0
                                                            ? new CheckAtRequestInfo(ticks, DateTime.UtcNow)
                                                            : new CheckAtRequestInfo(ticks);
        }

        #endregion

        #region public methods
        /// <summary>
        /// Removes an sql dependecy set on a HttpRuntime.Cache key. This method only removes
        /// the dependency, it does not purge the actual item from cache.
        /// </summary>
        public static void RemoveAllDependecies()
        {
            SqlConnection con = new SqlConnection(_ConnectionString);
            DataAccessManager dam = new DataAccessManager(ConnectionString);
            dam.AddInputParameter("@ApplicationId", ClusteredCacheController.ApplicationId);

            try { dam.ExecuteNonQuery(GetFormattedStoredProcedureName(SP_DELETE_ALL)); }
            catch { throw; }
            finally
            {
                switch (ClusteredCacheController.ClusteredCachingMode)
                {
                    case ClusteredCachingMode.ServiceBroker:
                        #region service broker
                        _ServiceBrokerDependencies.Clear();
                        #endregion
                        break;
                    case ClusteredCachingMode.CheckAtRequest:
                        CheckAtRequestDependencies.Clear();
                        break;
                }
            }
        }

        /// <summary>
        /// Removes an sql dependecy set on a HttpRuntime.Cache key. This method only removes
        /// the dependency, it does not purge the actual item from cache.
        /// </summary>
        /// <param name="key">The cache key.</param>
        public static void RemoveDependecy(string key)
        {
            SqlConnection con = new SqlConnection(_ConnectionString);
            DataAccessManager dam = new DataAccessManager(ConnectionString);
            dam.AddInputParameter("@CacheKey", key);
            dam.AddInputParameter("@ApplicationId", ClusteredCacheController.ApplicationId);

            try { dam.ExecuteNonQuery(GetFormattedStoredProcedureName(SP_DELETE)); }
            catch { throw; }
            finally
            {
                switch (ClusteredCacheController.ClusteredCachingMode)
                {
                    case ClusteredCachingMode.ServiceBroker:
                        #region service broker
                        string dependencyId = GetDependencyIDFromCacheKey(key);
                        if (!string.IsNullOrEmpty(dependencyId))
                        {
                            _ServiceBrokerDependencies[dependencyId].SqlDependency.OnChange -= OnChange;
                            _ServiceBrokerDependencies.Remove(dependencyId);
                        }
                        #endregion
                        break;
                    case ClusteredCachingMode.CheckAtRequest:
                        CheckAtRequestDependencies.Remove(key);
                        break;
                }
            }
        }

        /// <summary>
        /// Determines whether a certain cache element has a cache dependency on the local machine.
        /// </summary>
        /// <param name="key">The key.</param>
        public static bool HasDependency(string key)
        {
            long ticks = -1;
            return IsUpToDate(key, out ticks) != ClusteredCachingSynchronizationStatus.RemovedFromCollection;
        }

        /// <summary>
        /// Purges a clustered cache item from cache. Calling this method purges the cache record
        /// associated with the specified key from all machines on the cluster.
        /// </summary>
        /// <param name="key">The cache key.</param>
        public static void PurgeCacheItem(string key)
        {
            DataAccessManager dam = new DataAccessManager(ConnectionString);
            dam.AddInputParameter("@CacheKey", key);
            dam.AddInputParameter("@LastUpdate", DateTime.UtcNow.Ticks);
            dam.AddInputParameter("@ApplicationId", ClusteredCacheController.ApplicationId);
            dam.ExecuteNonQuery(GetFormattedStoredProcedureName(SP_INSERT_UPDATE));

            switch (ClusteredCacheController.ClusteredCachingMode)
            {
                case ClusteredCachingMode.ServiceBroker:
                    #region service broker
                    if (ClusteredCacheController.MillisecondsToSleepAfterCachePurge > 0)
                        Thread.Sleep(ClusteredCacheController.MillisecondsToSleepAfterCachePurge);
                    #endregion
                    break;
                case ClusteredCachingMode.CheckAtRequest:

                    break;
            }
        }

        /// <summary>
        /// Determines whether [is up to date] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="synchTicks">The synch ticks.</param>
        /// <returns></returns>
        public static ClusteredCachingSynchronizationStatus IsUpToDate(string key, out long synchTicks)
        {
            #region check whether we can delay the synchronization check
            if (ClusteredCacheController.CheckAtRequestIsUpToDateDelayInMilliseconds > 0
                && CheckAtRequestDependencies.ContainsKey(key)
                && CheckAtRequestDependencies[key].LastChecked.HasValue
                && CheckAtRequestDependencies[key].LastChecked.Value.AddMilliseconds(ClusteredCacheController.CheckAtRequestIsUpToDateDelayInMilliseconds) > DateTime.UtcNow)
            {
                synchTicks = CheckAtRequestDependencies[key].Ticks;
                return ClusteredCachingSynchronizationStatus.UpToDate;
            }
            #endregion

            DataAccessManager dam = new DataAccessManager(ConnectionString);
            dam.AddInputParameter("@CacheKey", key);
            dam.AddInputParameter("@ApplicationId", ClusteredCacheController.ApplicationId);
            synchTicks = dam.ExecuteScalar<long>(GetFormattedStoredProcedureName(SP_SELECT));

            if (synchTicks <= 0)
                return ClusteredCachingSynchronizationStatus.RemovedFromCollection;
            else
            {
                if (CheckAtRequestDependencies.ContainsKey(key))
                {
                    if (CheckAtRequestDependencies[key].Ticks.Equals(synchTicks))
                    {
                        if (ClusteredCacheController.CheckAtRequestIsUpToDateDelayInMilliseconds > 0)
                            CheckAtRequestDependencies[key] = GetCheckAtRequestInfo(synchTicks);

                        return ClusteredCachingSynchronizationStatus.UpToDate;
                    }
                    else
                        return ClusteredCachingSynchronizationStatus.OutOfDate;                    
                }
                else
                    return ClusteredCachingSynchronizationStatus.NotFound;
            }
        }

        /// <summary>
        /// Gets an item with a specified key from the HttpRuntime.Cache.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="loadObj">The delegate returning the item to insert into cache when it doesn't exist.</param>
        /// <param name="priority">The priority.</param>
        /// <returns>
        /// The cached item when present at HttpRuntime.Cache, the loadObj delegate (which re-adds the item
        /// at cache) when the item can't be found at cache, null if the cache item dependency doesn't exist
        /// or the loadObj delegate returned null.
        /// </returns>
        public static object GetCacheItem(string key, ReloadObjectAtCache loadObj, CacheItemPriority priority)
        {
            return GetCacheItem(key, loadObj, priority, false);
        }
        /// <summary>
        /// Gets an item with a specified key from the HttpRuntime.Cache.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="loadObj">The delegate returning the item to insert into cache when it doesn't exist.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="autoInsertDependency">If set to true, a dependency will be automatically created when no cache
        /// dependency is currently set. If this value is false and you try to retrieve an item from cache that was not added
        /// first, this method will return null.</param>
        /// <returns>
        /// The cached item when present at HttpRuntime.Cache, the loadObj delegate (which re-adds the item
        /// at cache) when the item can't be found at cache, null if the cache item dependency doesn't exist and autoInsertDependency
        /// is set to false or the loadObj delegate returned null.
        /// </returns>
        public static object GetCacheItem(string key, ReloadObjectAtCache loadObj, CacheItemPriority priority, bool autoInsertDependency)
        {
            object fetchedItem = null;
            switch (ClusteredCacheController.ClusteredCachingMode)
            {
                case ClusteredCachingMode.ServiceBroker:
                    #region service broker
                    lock (_CacheLock)
                    {
                        if (HttpRuntime.Cache[key] == null)
                        {
                            // check whether we have a dependency...
                            string dependencyId = GetDependencyIDFromCacheKey(key);
                            if (!string.IsNullOrEmpty(dependencyId) || autoInsertDependency)
                            {// we actually have a dependency, so refresh the cache...
                                try { fetchedItem = loadObj(); }
                                catch (Exception err) { throw new CachingException("Error at loading object into cache. See the inner exception for further details.", err); }
                                if (fetchedItem == null)
                                {// we can't insert null as an object, so simply return null.
                                    return null;
                                }
                                HttpRuntime.Cache.Insert(key, fetchedItem, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, priority, null);
                                return fetchedItem;
                            }
                            else
                            {// this key doesn't exist and there is no associated dependency, so return null...
                                return null;
                            }
                        }

                        return HttpRuntime.Cache[key];
                    }
                    #endregion

                case ClusteredCachingMode.CheckAtRequest:
                    #region Check at request
                    long synchTicks = -1;
                    lock (_CacheLock)
                    {
                        ClusteredCachingSynchronizationStatus status = IsUpToDate(key, out synchTicks);
                        switch (status)
                        {
                            case ClusteredCachingSynchronizationStatus.UpToDate:
                                if (HttpRuntime.Cache[key] == null)
                                {
                                    try { fetchedItem = loadObj(); }
                                    catch (Exception err) { throw new CachingException("Error at loading object into cache. See the inner exception for further details.", err); }
                                    if (fetchedItem == null)
                                    {// we can't insert null as an object, so simply return null.
                                        return null;
                                    }
                                    HttpRuntime.Cache.Insert(key, fetchedItem, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, priority, null);
                                    return fetchedItem;
                                }
                                return HttpRuntime.Cache[key];

                            case ClusteredCachingSynchronizationStatus.OutOfDate:
                                CheckAtRequestDependencies[key] = GetCheckAtRequestInfo(synchTicks);

                                HttpRuntime.Cache.Remove(key);

                                try { fetchedItem = loadObj(); }
                                catch (Exception err) { throw new CachingException("Error at loading object into cache. See the inner exception for further details.", err); }
                                if (fetchedItem == null)
                                {// we can't insert null as an object, so simply return null.
                                    return null;
                                }

                                HttpRuntime.Cache.Insert(key, fetchedItem, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, priority, null);
                                return fetchedItem;


                            case ClusteredCachingSynchronizationStatus.NotFound:

                                CheckAtRequestDependencies.Add(key, GetCheckAtRequestInfo(synchTicks));
                                HttpRuntime.Cache.Remove(key);

                                try { fetchedItem = loadObj(); }
                                catch (Exception err) { throw new CachingException("Error at loading object into cache. See the inner exception for further details.", err); }

                                if (fetchedItem == null)
                                {// we can't insert null as an object, so simply return null.
                                    return null;
                                }

                                HttpRuntime.Cache.Insert(key, fetchedItem, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, priority, null);
                                return fetchedItem;


                            case ClusteredCachingSynchronizationStatus.RemovedFromCollection:

                                if (autoInsertDependency)
                                {
                                    try { fetchedItem = loadObj(); }
                                    catch (Exception err) { throw new CachingException("Error at loading object into cache. See the inner exception for further details.", err); }

                                    AddCacheItem(key, fetchedItem, priority);
                                    return fetchedItem;
                                }
                                else
                                {
                                    HttpRuntime.Cache.Remove(key);
                                    CheckAtRequestDependencies.Remove(key);
                                    return null;
                                }
                        }
                        break;
                    }
                    #endregion
            }

            return null;
        }

        /// <summary>
        /// Adds an object to the HttpRuntime.Cache collection and sets an SQL Dependency on it's
        /// key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="item">The item to add to the cache.</param>
        /// <param name="priority">The priority.</param>
        public static void AddCacheItem(string key, object item, CacheItemPriority priority)
        {
            if (ClusteredCacheController.ClusteredCachingMode == ClusteredCachingMode.ServiceBroker)
            {
                if (_ServiceBrokerDependencies.ContainsKey(key))
                    _ServiceBrokerDependencies.Remove(key);
            }

            #region insert/update at database
            long utcNow = DateTime.UtcNow.Ticks;

            DataAccessManager dam = new DataAccessManager(ConnectionString);
            dam.AddInputParameter("@CacheKey", key);
            dam.AddInputParameter("@LastUpdate", utcNow);
            dam.AddInputParameter("@ApplicationId", ClusteredCacheController.ApplicationId);
            dam.ExecuteNonQuery(GetFormattedStoredProcedureName(SP_INSERT_UPDATE));
            #endregion

            #region add to cache collection
            HttpRuntime.Cache.Remove(key);
            HttpRuntime.Cache.Insert(key, item, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, priority, null);
            #endregion

            #region add dependency

            switch (ClusteredCacheController.ClusteredCachingMode)
            {
                case ClusteredCachingMode.ServiceBroker:
                    #region service broker
                    SqlConnection con = new SqlConnection(_ConnectionString);
                    SqlCommand cmd = new SqlCommand(GetFormattedStoredProcedureName(SP_SELECT), con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CacheKey", SqlDbType.NVarChar, 256);
                    cmd.Parameters[0].Value = key;

                    // Clear any existing notifications
                    cmd.Notification = null;

                    // Create the dependency for this command
                    SqlDependency dependency = new SqlDependency(cmd);

                    // Add the event handler
                    dependency.OnChange += new OnChangeEventHandler(OnChange);

                    #region execute reader
                    try
                    {
                        con.Open();
                        // Execute the command.
                        cmd.ExecuteReader();
                        // Process the DataReader
                    }
                    catch (SqlException err)
                    {
                        DataAccessManager.ThrowDataAccessManagerException(err, GetFormattedStoredProcedureName(SP_SELECT));
                    }
                    finally
                    {
                        con.Close();
                    }
                    #endregion

                    _ServiceBrokerDependencies.Add(dependency.Id, new CacheKeySqlDependency() { CacheKey = key, SqlDependency = dependency });
                    #endregion
                    break;
                case ClusteredCachingMode.CheckAtRequest:
                    if (CheckAtRequestDependencies.ContainsKey(key))                    
                        CheckAtRequestDependencies[key] = GetCheckAtRequestInfo(utcNow);
                    else
                        CheckAtRequestDependencies.Add(key, GetCheckAtRequestInfo(utcNow));
                    break;
            }
            #endregion
        }



        /// <summary>
        /// Restarts the SQL dependency listener. This is only necessary if you are running the
        /// clustered cache component in service broker mode. 
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public static void RestartSQLDependencyListener(string connectionString)
        {
            _ConnectionString = connectionString;

            // Stop an existing services on this connection string
            // just be sure
            SqlDependency.Stop(_ConnectionString);

            // Start the dependency
            // User must have SUBSCRIBE QUERY NOTIFICATIONS permission
            // Database must also have SSB enabled
            // ALTER DATABASE Chatter SET ENABLE_BROKER
            SqlDependency.Start(_ConnectionString);
        }
        /// <summary>
        /// Stops the SQL dependency listener. This is only necessary if you are running the
        /// clustered cache component in service broker mode.
        /// </summary>
        public static void StopSQLDependencyListener()
        {
            if (!string.IsNullOrEmpty(_ConnectionString))
                SqlDependency.Stop(_ConnectionString);
        }
        #endregion
    }
}
