﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.Components.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public enum CacheMode
    {
        /// <summary>
        /// 
        /// </summary>
        InProcess,
        /// <summary>
        /// 
        /// </summary>
        Memcached,
        /// <summary>
        /// 
        /// </summary>
        MemcachedProtocolBufferSerialization
    }
}
