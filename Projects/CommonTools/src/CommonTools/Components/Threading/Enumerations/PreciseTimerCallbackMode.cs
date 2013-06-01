using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.Components.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public enum PreciseTimerCallbackMode
    {
        /// <summary>
        /// Use Synchronized if you want to make sure that a timer callback is executed before the next interval starts
        /// </summary>
        Synchronized,
        /// <summary>
        /// Use Async if you want to fire the timer callback on a separate thread and immediately start the next interval iteration. This may result
        /// in concurrent callback calls so you have to make sure that the callbacks are thread-safe.
        /// </summary>
        Async
    }
}
