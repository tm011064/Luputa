using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CommonTools.Components.Testing
{
    /// <summary>
    /// 
    /// </summary>
    public static class GlobalStopwatchSingleThread
    {
        #region nested classes
        /// <summary>
        /// 
        /// </summary>
        class StopwatchInfo
        {
            #region members
            private string _Name;
            private int _CurrentStarts = 0;
            private int _TotalStartCalls = 0;
            private Stopwatch _Stopwatch;
            #endregion

            #region public methods
            /// <summary>
            /// Gets the elapsed time formatted.
            /// </summary>
            /// <returns></returns>
            public string GetElapsedTimeFormatted()
            {
                return "Stopwatch: " + _Name.PadRight(30) + " - " + _Stopwatch.ElapsedMilliseconds + " ms"
                    + " for " + _TotalStartCalls + " calls (avg: " + ((decimal)_Stopwatch.ElapsedMilliseconds / (decimal)((_TotalStartCalls == 0) ? 1 : _TotalStartCalls)) + " ms)";
            }
            /// <summary>
            /// Gets the elapsed.
            /// </summary>
            /// <returns></returns>
            public TimeSpan GetElapsed()
            {
                return _Stopwatch.Elapsed;
            }

            /// <summary>
            /// Starts this instance.
            /// </summary>
            public void Start()
            {
                _TotalStartCalls++;

                if (!_Stopwatch.IsRunning)
                    _Stopwatch.Start();

                _CurrentStarts++;
            }
            /// <summary>
            /// Stops this instance.
            /// </summary>
            public void Stop()
            {
                _CurrentStarts--;
                if (_CurrentStarts <= 0)
                {
                    _Stopwatch.Stop();
                    _CurrentStarts = 0;
                }
            }
            /// <summary>
            /// Resets this instance.
            /// </summary>
            public void Reset()
            {
                _Stopwatch.Reset();
                _CurrentStarts = 0;
            }
            #endregion

            #region constructors
            /// <summary>
            /// Initializes a new instance of the <see cref="StopwatchInfo"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            public StopwatchInfo(string name)
            {
                _Name = name ?? Guid.NewGuid().ToString();
                _CurrentStarts = 0;
                _Stopwatch = new Stopwatch();
            }
            #endregion
        }
        #endregion

        #region members
        private static Dictionary<string, StopwatchInfo> _Stopwatches = new Dictionary<string, StopwatchInfo>();
        private static readonly object _Lock = new object();
        #endregion

        #region private methods
        private static StopwatchInfo GetStopwatch(string stopwatchId)
        {
            if (!_Stopwatches.ContainsKey(stopwatchId))
            {
                lock (_Lock)
                {
                    if (!_Stopwatches.ContainsKey(stopwatchId))
                    {
                        _Stopwatches.Add(stopwatchId, new StopwatchInfo(stopwatchId));
                    }
                }
            }

            return _Stopwatches[stopwatchId];
        }
        #endregion

        #region public methods
        /// <summary>
        /// Resets the specified stopwatch id.
        /// </summary>
        /// <param name="stopwatchId">The stopwatch id.</param>
        public static void Reset(string stopwatchId)
        {
            GetStopwatch(stopwatchId).Reset();
        }
        /// <summary>
        /// Starts the specified stopwatch id.
        /// </summary>
        /// <param name="stopwatchId">The stopwatch id.</param>
        public static void Start(string stopwatchId)
        {
            GetStopwatch(stopwatchId).Start();
        }
        /// <summary>
        /// Stops the specified stopwatch id.
        /// </summary>
        /// <param name="stopwatchId">The stopwatch id.</param>
        public static void Stop(string stopwatchId)
        {
            GetStopwatch(stopwatchId).Stop();
        }

        /// <summary>
        /// Gets the elapsed time.
        /// </summary>
        /// <param name="stopwatchId">The stopwatch id.</param>
        /// <returns></returns>
        public static TimeSpan GetElapsedTime(string stopwatchId)
        {
            return GetStopwatch(stopwatchId).GetElapsed();
        }
        /// <summary>
        /// Gets the elapsed time formatted.
        /// </summary>
        /// <param name="stopwatchId">The stopwatch id.</param>
        /// <returns></returns>
        public static string GetElapsedTimeFormatted(string stopwatchId)
        {
            return GetStopwatch(stopwatchId).GetElapsedTimeFormatted();
        }

        /// <summary>
        /// Gets all elapsed times formatted.
        /// </summary>
        /// <returns></returns>
        public static string GetAllElapsedTimesFormatted()
        {
            StringBuilder sb = new StringBuilder();
            foreach (StopwatchInfo stopwatchInfo in _Stopwatches.Values)
            {
                sb.AppendLine(stopwatchInfo.GetElapsedTimeFormatted());
            }
            return sb.ToString();
        }
        #endregion
    }
}
