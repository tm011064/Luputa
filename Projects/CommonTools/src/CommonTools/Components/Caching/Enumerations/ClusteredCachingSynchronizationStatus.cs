using System;

namespace CommonTools.Components.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public enum ClusteredCachingSynchronizationStatus
    {
        /// <summary>
        /// This item does not exist at the sql cache synchronization database any more
        /// </summary>
        RemovedFromCollection = -1,
        /// <summary>
        /// The HttpRuntime.Cache item is out of date
        /// </summary>
        OutOfDate = 0,
        /// <summary>
        /// The HttpRuntime.Cache item is up to date
        /// </summary>
        UpToDate = 1,
        /// <summary>
        /// The item was not found at the HttpRuntime.Cache collection
        /// </summary>
        NotFound = 2,
    }
}
