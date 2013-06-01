using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CommonTools.Components.Testing
{
    /// <summary>
    /// This class contains all disposable stopwatch related data
    /// </summary>
    public class DisposableStopwatch : IDisposable
    {
        #region nested classes
        /// <summary>
        /// 
        /// </summary>
        public enum TraceMode
        {
            /// <summary>
            /// 
            /// </summary>
            Milliseconds,
            /// <summary>
            /// 
            /// </summary>
            FullTime
        }
        #endregion

        #region members
        private Stopwatch _Stopwatch = new Stopwatch();
        private string _Name;
        private TraceMode _TraceMode;
        #endregion

        #region public methods
        /// <summary>
        /// Resets the and start.
        /// </summary>
        public void ResetAndStart()
        {
            this._Stopwatch.Reset();
            this._Stopwatch.Start();
        }
        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            this._Stopwatch.Start();
        }
        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            this._Stopwatch.Stop();
        }
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this._Stopwatch.Stop();

            switch (this._TraceMode)
            {
                case TraceMode.Milliseconds:
                    Trace.WriteLine("Stopwatch" + (string.IsNullOrEmpty(_Name) ? string.Empty : " " + _Name) + ": " + _Stopwatch.ElapsedMilliseconds + " ms");
                    break;
                case TraceMode.FullTime:
                    Trace.WriteLine("Stopwatch" + (string.IsNullOrEmpty(_Name) ? string.Empty : " " + _Name) + ": " + _Stopwatch.Elapsed.ToString());
                    break;
            }
        }

        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableStopwatch"/> class.
        /// </summary>
        public DisposableStopwatch() : this(true, string.Empty, TraceMode.Milliseconds) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableStopwatch"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="traceMode">The trace mode.</param>
        public DisposableStopwatch(string name, TraceMode traceMode) : this(true, name, TraceMode.Milliseconds) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableStopwatch"/> class.
        /// </summary>
        /// <param name="startImmediately">if set to <c>true</c> [start immediately].</param>
        /// <param name="name">The name.</param>
        /// <param name="traceMode">The trace mode.</param>
        public DisposableStopwatch(bool startImmediately, string name, TraceMode traceMode)
        {
            _Stopwatch = new Stopwatch();
            _Name = name ?? "undefined";
            _TraceMode = traceMode;

            if (startImmediately)
                Start();
        }
        #endregion
    }
}
