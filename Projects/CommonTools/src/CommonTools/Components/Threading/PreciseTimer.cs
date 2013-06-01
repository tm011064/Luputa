using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace CommonTools.Components.Threading
{
    /// <summary>
    /// This class is a timer which ensures that callbacks are made with millisecond precision
    /// </summary>
    public class PreciseTimer : IDisposable
    {
        #region members
        private PreciseTimerCallbackMode _PreciseTimerCallbackMode;
        private Thread _TimerThread;
        private TimeSpan _Period;
        private bool _IsRunning;
        private static readonly object _Lock = new object();
        private TimerCallback _TimerCallback;
        #endregion

        #region public methods
        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            Start(TimeSpan.Zero, this._Period, false);
        }
        /// <summary>
        /// Forces to start this timer. This means that any previous timer callback threads which were asked to stop but might still be running
        /// in the background are terminated straight away and a new timer thread gets started.
        /// </summary>
        public void ForceStart()
        {
            Start(TimeSpan.Zero, this._Period, true);
        }
        /// <summary>
        /// Starts the timer
        /// </summary>
        /// <param name="dueTime">The amount of time to delay before callback is invoked. Specify TimeSpan.Zero to start the timer immediately.</param>
        /// <param name="period">The time interval between invocations of callback.</param>
        /// <param name="forceStart">if set to <c>true</c> and the timer was stopped but is still executing a callback delegate from a previous
        /// timer start, this flag ensures that the previous thread is aborted immediately and a new thread can be started.</param>
        public void Start(TimeSpan dueTime, TimeSpan period, bool forceStart)
        {
            if (forceStart)
            {
                ForceStop();
            }
            else if (_TimerThread != null
                     && _TimerThread.ThreadState == System.Threading.ThreadState.Running)
            {
                // we stop the time in case we are running in Synchronized mode and a current callback might be running at the moment. In
                // that case we have to wait until the current callback is finished.
                Stopwatch sw = Stopwatch.StartNew();
                lock (_Lock)
                {
                    _IsRunning = false;
                    Monitor.Pulse(_Lock);
                }
                sw.Stop();
                dueTime.Add(TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds * -1));
            }
            if (dueTime.TotalMilliseconds > 0)
            {
                Thread.Sleep(dueTime);
            }

            _TimerThread = new Thread(new ThreadStart(() => { TimerThreadCallback(_TimerCallback); }));
            _IsRunning = true;
            _TimerThread.Start();
        }
        /// <summary>
        /// Forces to stop this timer, regardless whether any callback delegate is still running.
        /// </summary>
        public void ForceStop()
        {
            if (_TimerThread != null
                && _TimerThread.ThreadState == System.Threading.ThreadState.Running)
            {
                _IsRunning = false;
                _TimerThread.Abort();
            }
        }
        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            lock (_Lock)
            {
                _IsRunning = false;
                Monitor.Pulse(_Lock);
            }
        }
        #endregion

        #region private methods
        private void TimerThreadCallback(TimerCallback timerCallback)
        {
            ThreadStart threadStart = new ThreadStart(() =>
            {
                Thread.Sleep(_Period);

                lock (_Lock)
                {
                    if (_IsRunning)
                    {// we have to check whether we are still running, otherwise we might end up sending this signal for a new thread start
                        Monitor.Pulse(_Lock);
                    }
                }
            });

            lock (_Lock)
            {
                while (_IsRunning)
                {
                    if (_PreciseTimerCallbackMode == PreciseTimerCallbackMode.Synchronized)
                        timerCallback.Invoke(null);
                    else
                        timerCallback.BeginInvoke(null, null, null);

                    threadStart.BeginInvoke(null, null);
                    Monitor.Wait(_Lock);
                }
            }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Creates a new timer object and starts it straight away
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="period">The time interval between invocations of callback.</param>
        /// <param name="preciseTimerCallbackMode">The precise timer callback mode.</param>
        /// <returns></returns>
        public static PreciseTimer StartNew(TimerCallback callback, TimeSpan period, PreciseTimerCallbackMode preciseTimerCallbackMode)
        {
            return StartNew(callback, TimeSpan.Zero, period, preciseTimerCallbackMode);
        }
        /// <summary>
        /// Creates a new timer object and starts it straight away
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="dueTime">The amount of time to delay before callback is invoked. Specify TimeSpan.Zero to start the timer immediately.</param>
        /// <param name="period">The time interval between invocations of callback.</param>
        /// <param name="preciseTimerCallbackMode">The precise timer callback mode.</param>
        /// <returns></returns>
        public static PreciseTimer StartNew(TimerCallback callback, TimeSpan dueTime, TimeSpan period, PreciseTimerCallbackMode preciseTimerCallbackMode)
        {
            PreciseTimer preciseTimer = new PreciseTimer(callback, period, preciseTimerCallbackMode);
            preciseTimer.Start(dueTime, period, false);
            return preciseTimer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreciseTimer"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="period">The period.</param>
        /// <param name="preciseTimerCallbackMode">The precise timer callback mode.</param>
        public PreciseTimer(TimerCallback callback, TimeSpan period, PreciseTimerCallbackMode preciseTimerCallbackMode)
        {
            this._PreciseTimerCallbackMode = preciseTimerCallbackMode;
            this._Period = period;
            this._TimerCallback = callback;
        }
        #endregion

        #region IDisposable Members
        private bool _IsDisposed = false;
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!_IsDisposed)
            {
                _IsDisposed = true;
                ForceStop();
            }
        }
        #endregion
    }
}
