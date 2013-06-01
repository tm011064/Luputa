using System;
using System.Data.SqlClient;

namespace CommonTools.Components.Caching
{
    /// <summary>
    /// Struct used for the ClusteredCacheManager
    /// </summary>
    internal struct CacheKeySqlDependency
    {
        private string _CacheKey;
        public string CacheKey
        {
            get { return _CacheKey; }
            set { _CacheKey = value; }
        }
        private SqlDependency _SqlDependency;
        public SqlDependency SqlDependency
        {
            get { return _SqlDependency; }
            set { _SqlDependency = value; }
        }
    }
}
