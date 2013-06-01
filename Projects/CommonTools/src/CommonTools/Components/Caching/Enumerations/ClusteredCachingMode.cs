using System;

namespace CommonTools.Components.Caching
{
    /// <summary>
    /// Defines which caching mode to use for keeping the HttpRuntime.Cache objects synchronized.
    /// </summary>
    public enum ClusteredCachingMode
    {
        /// <summary>
        /// This mode uses SQL Server 2005 service broker query notifications to keep all machines
        /// synchronized on the cluster. It's the most performant way of doing the synchronization,
        /// but you need to run SQL Server 2005 Enterprise edition to make this work.
        /// </summary>
        ServiceBroker = 0,
        /// <summary>
        /// This mode stores the 'LastUpdate' state of each cache item as an unremovable hashtable
        /// at each machine's HttpRuntime.Cache. Each time a cache item is requested, this value is
        /// checked against the database.
        /// This approach is less performant than the ServiceBroker mode, but it works on all SQL
        /// versions (2000 and 2005).
        /// </summary>
        CheckAtRequest = 1
    }
}
