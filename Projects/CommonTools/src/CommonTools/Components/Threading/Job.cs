using System;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Generic;
using CommonTools.Components.Localization;

namespace CommonTools.Components.Threading
{
    /// <summary>
    /// Configurable IJob descritpion
    /// </summary>
    [Serializable]
    [XmlRoot("job")]
    public class Job : IDisposable
    {
        #region members
        private IJob _IJob;
        [NonSerialized]
        private Timer _Timer = null;
        [NonSerialized]
        private PreciseTimer _PreciseTimer = null;
        private bool _Disposed = false;
        [NonSerialized]
        private Dictionary<string, string> _Options = null;
        #endregion

        #region Properities

        /// <summary>
        /// Gets the daily localized execution time zone info.
        /// </summary>
        public TimeZoneInfo DailyLocalizedExecutionTimeZoneInfo { get; private set; }
        /// <summary>
        /// Gets the daily localized execution time.
        /// </summary>
        public DateTime DailyLocalizedExecutionTime { get; private set; }
        /// <summary>
        /// Gets the kind of the daily execution date time.
        /// </summary>
        /// <value>
        /// The kind of the daily execution date time.
        /// </value>
        public DateTimeKind DailyExecutionDateTimeKind { get; private set; }

        private bool _IsRunning;
        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning
        {
            get { return _IsRunning; }
        }

        private DateTime _LastStart;
        /// <summary>
        /// Gets the last started.
        /// </summary>
        /// <value>The last started.</value>
        public DateTime LastStarted
        {
            get { return _LastStart; }
        }

        private DateTime _LastEnd;
        /// <summary>
        /// Gets the last end.
        /// </summary>
        /// <value>The last end.</value>
        public DateTime LastEnd
        {
            get { return _LastEnd; }
        }

        private DateTime _LastSucess;
        /// <summary>
        /// Gets the last success.
        /// </summary>
        /// <value>The last success.</value>
        public DateTime LastSuccess
        {
            get { return _LastSucess; }
        }

        private bool _ExecuteOnOwnThread = true;
        /// <summary>
        /// Gets a value indicating whether this job is ExecuteOnOwnThreaded or not.
        /// </summary>
        /// <value><c>true</c> if ExecuteOnOwnThreaded; otherwise, <c>false</c>.</value>
        public bool ExecuteOnOwnThread
        {
            get { return _ExecuteOnOwnThread; }
        }

        private Type _JobType;
        /// <summary>
        /// Named type of class which implements IJob
        /// </summary>
        public Type JobType
        {
            get { return this._JobType; }
        }

        private bool _EnableShutDown = false;
        /// <summary>
        /// Gets a value indicating whether the job gets shut down when an exception occured during it's execution.
        /// </summary>
        /// <value><c>true</c> if the job gets shut down when an exception occured during it's execution; otherwise, <c>false</c>.</value>
        public bool EnableShutDown
        {
            get { return this._EnableShutDown; }
        }

        private string _Name;
        /// <summary>
        /// Name of Job
        /// </summary>
        public string Name
        {
            get { return this._Name; }
        }

        private bool _Enabled = true;
        /// <summary>
        /// Is this job enabled
        /// </summary>
        public bool Enabled
        {
            get { return this._Enabled; }
        }


        private bool _ExecuteDaily = false;
        /// <summary>
        /// Gets or sets a value indicating whether to execute this job daily at a determined time.
        /// </summary>
        /// <value><c>true</c> if [execute daily]; otherwise, <c>false</c>.</value>
        public bool ExecuteDaily
        {
            get { return this._ExecuteDaily; }
        }
        private DateTime _DailyUTCExecutionTime = DateTime.UtcNow;
        /// <summary>
        /// Gets or sets the time of the day that this job should execute on a daily basis.
        /// </summary>
        /// <value>The execute daily at.</value>
        public DateTime DailyUTCExecutionTime
        {
            get { return this._DailyUTCExecutionTime; }
        }

        private bool _FirstRunAtInitialization = true;
        /// <summary>
        /// Determines whether this job should execute immediately at the global Jobs instanciation or
        /// wait for the first interval.
        /// </summary>
        /// <example>
        /// 
        ///     Job A: Minutes = 10, FirstRunAtInitialization = true
        ///     Job B: Minutes = 10, FirstRunAtInitialization = false
        /// 
        /// CommonTools.Components.Threading.Jobs.Instance().Start(); // executed at 14:00:00.000
        /// 
        /// Job A executes at 14:00:00.000, 14:10:00.000, 14:20:00.000, ...
        /// Job B executes at 14:10:00.000, 14:20:00.000, 14:30:00.000, ...
        /// 
        /// </example>
        public bool FirstRunAtInitialization
        {
            get { return this._FirstRunAtInitialization; }
        }

        /// <summary>
        /// Specified the interval to run the job. ie. run the job every five minutes.
        /// </summary>
        public TimeSpan Interval { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [use precise timer].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use precise timer]; otherwise, <c>false</c>.
        /// </value>
        public bool UsePreciseTimer { get; private set; }
        /// <summary>
        /// Gets the precise timer callback mode.
        /// </summary>
        public PreciseTimerCallbackMode PreciseTimerCallbackMode { get; private set; }
        #endregion

        #region events
        /// <summary>
        /// Event fired when the job is about to start.
        /// </summary>
        public static event EventHandler PreJob;

        /// <summary>
        /// Event fired when the job has returned from its execute method.
        /// </summary>
        public static event EventHandler PostJob;

        /// <summary>
        /// Called when before a job executes
        /// </summary>
        private void OnPreJob()
        {
            if (PreJob != null)
                PreJob(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called after a job has executes
        /// </summary>
        private void OnPostJob()
        {
            if (PostJob != null)
                PostJob(this, EventArgs.Empty);
        }
        #endregion

        #region private methods
        /// <summary>
        /// Precises the timer callback.
        /// </summary>
        /// <param name="state">The state.</param>
        private void PreciseTimerCallback(object state)
        {
            if (!Enabled)
                return;

            try { ExecuteJob(); }
            catch { }
        }
        /// <summary>
        /// Internal call back which is responsible for firing IJob.Execute()
        /// </summary>
        /// <param name="state"></param>
        private void TimerThread(object state)
        {
            if (!Enabled)
                return;

            _Timer.Change(Timeout.Infinite, Timeout.Infinite);

            try
            {
                ExecuteJob();
            }
            catch { }

            if (Enabled)
            {
                if (ExecuteDaily)
                    _Timer.Change(GetDailyDueTime(), TimeSpan.FromMilliseconds(-1));
                else
                    _Timer.Change(Interval, Interval);
            }
        }

        private TimeSpan GetDailyDueTime()
        {
            DateTime now, firstExecution;
            switch (this.DailyExecutionDateTimeKind)
            {
                case DateTimeKind.Local:
                    #region DateTimeKind.Local
                    now = TimeZoneUtility.ConvertUTCDateToLocalizedDate(DateTime.UtcNow, this.DailyLocalizedExecutionTimeZoneInfo);

                    firstExecution = new DateTime(now.Year, now.Month, now.Day
                            , this.DailyLocalizedExecutionTime.Hour
                            , this.DailyLocalizedExecutionTime.Minute
                            , this.DailyLocalizedExecutionTime.Second
                            , this.DailyLocalizedExecutionTime.Millisecond);

                    if (firstExecution < now)
                        firstExecution = firstExecution.AddDays(1);                    

                    return (TimeSpan)(TimeZoneUtility.ConvertUTCDateToLocalizedDate(firstExecution, this.DailyLocalizedExecutionTimeZoneInfo)
                                      - now);
                    #endregion

                case DateTimeKind.Utc:
                    #region DateTimeKind.Utc
                    now = DateTime.UtcNow;

                    firstExecution = new DateTime(now.Year, now.Month, now.Day
                        , this.DailyUTCExecutionTime.Hour
                        , this.DailyUTCExecutionTime.Minute
                        , this.DailyUTCExecutionTime.Second
                        , this.DailyUTCExecutionTime.Millisecond);

                    if (firstExecution < now)
                        firstExecution = firstExecution.AddDays(1);

                    return (TimeSpan)(firstExecution - now);
                    #endregion

                default: throw new NotSupportedException();
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Gets a cloned version of this job's options dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetOptionsClone()
        {
            Dictionary<string, string> options = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> item in this._Options)
                options.Add(item.Key, item.Value);
            return options;
        }

        /// <summary>
        /// Attempts to create an instance of the IJob. If the type
        /// can not be created, this Job will be disabled.
        /// </summary>
        /// <returns></returns>
        public IJob CreateJobInstance()
        {
            if (Enabled)
            {
                if (_IJob == null)
                {

                    if (_JobType != null)
                    {
                        _IJob = Activator.CreateInstance(_JobType) as IJob;
                    }
                    _Enabled = (_IJob != null);
                }
            }
            return _IJob;
        }

        /// <summary>
        /// Creates the timer and sets the callback if it is enabled
        /// </summary>
        public void InitializeTimer()
        {
            if (_Timer == null && Enabled)
            {
                if (ExecuteDaily)
                    _Timer = new Timer(new TimerCallback(TimerThread), null, GetDailyDueTime(), TimeSpan.FromMilliseconds(-1));
                else
                {
                    if (UsePreciseTimer)
                    {
                        _PreciseTimer = PreciseTimer.StartNew(new TimerCallback(PreciseTimerCallback), Interval, this.PreciseTimerCallbackMode);
                    }
                    else
                    {
                        _Timer = new Timer(new TimerCallback(TimerThread), null, Interval, Interval);
                    }
                }
            }
        }

        /// <summary>
        /// Performs the task of executing the job.
        /// </summary>
        public Exception ExecuteJob()
        {
            Exception returnException = null;

            try { OnPreJob(); }
            catch { }

            _IsRunning = true;
            IJob ijob = this.CreateJobInstance();
            if (ijob != null)
            {
                _LastStart = DateTime.Now;
                try
                {
                    ijob.Execute(this._Options);
                    _LastEnd = _LastSucess = DateTime.Now;
                }
                catch (Exception ex)
                {
                    this._Enabled = !this.EnableShutDown;
                    _LastEnd = DateTime.Now;
                    returnException = ex;
                }
            }
            _IsRunning = false;

            try { OnPostJob(); }
            catch { }

            return returnException;
        }
        #endregion

        #region constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ijob"></param>
        /// <param name="element"></param>
        /// <param name="options"></param>
        public Job(Type ijob, IJobItem element, Dictionary<string, string> options)
        {
            _JobType = ijob;

            this._Enabled = element.Enabled;
            this._EnableShutDown = element.EnableShutDown;
            this._Name = element.Name;
            this.Interval = element.Period;
            this._ExecuteOnOwnThread = element.ExecuteOnOwnThread;
            this._FirstRunAtInitialization = element.FirstRunAtInitialization;
            this._ExecuteDaily = element.ExecuteDaily;
            this.UsePreciseTimer = element.UsePreciseTimer;
            this.PreciseTimerCallbackMode = element.PreciseTimerCallbackMode;

            if (this._ExecuteDaily)
            {
                if (TimeZoneUtility.HasTimeZone(element.DailyLocalizedExecutionTimeZoneName))
                {
                    this.DailyLocalizedExecutionTimeZoneInfo = TimeZoneUtility.GetTimeZoneInfo(element.DailyLocalizedExecutionTimeZoneName);
                    this.DailyLocalizedExecutionTime = element.DailyLocalizedExecutionTime;
                    this.DailyExecutionDateTimeKind = DateTimeKind.Local;
                }
                else
                {
                    this._DailyUTCExecutionTime = element.DailyUTCExecutionTime;
                    this.DailyExecutionDateTimeKind = DateTimeKind.Utc;
                }
            }

            this._Options = options;
        }
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!this._Disposed)
            {
                lock (this)
                {
                    this._Disposed = true;
                    if ((this._Timer != null))
                    {
                        this._Timer.Dispose();
                        this._Timer = null;
                    }
                    if (this._PreciseTimer != null)
                    {
                        this._PreciseTimer.Stop();
                        this._PreciseTimer = null;
                    }
                }
            }
        }
        #endregion
    }
}

