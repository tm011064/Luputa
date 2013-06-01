using System;
using System.Collections;
using System.Threading;
using System.Xml;
using System.Collections.Generic;
using System.Web.Configuration;

namespace CommonTools.Components.Threading
{
    /// <summary>
    /// Class that handles jobs defined in web.config 
    /// </summary>
    public class Jobs
    {
        #region globals
        //Holds single instance of Jobs
        private static readonly Jobs _Jobs = null;

        //Internal job container
        List<Job> _JobList = new List<Job>();

        private int _Interval = 15 * 60000;
        private Timer _SingleTimer = null;
        private DateTime _Created;
        private DateTime _Started;
        private DateTime _Completed;
        private bool _IsRunning;
        #endregion

        #region properties
        /// <summary>
        /// Gets the current list of jobs available on the server.
        /// </summary>
        public List<Job> CurrentJobs
        {
            get { return _JobList; }
        }

        /// <summary>
        /// Gets the time the job was created.
        /// </summary>
        public DateTime Created
        {
            get { return _Created; }
        }

        /// <summary>
        /// Returns the last time the job was started.
        /// </summary>
        public DateTime LastStart
        {
            get { return _Started; }
        }

        /// <summary>
        /// Returns the time the job was last stopped.
        /// </summary>
        public DateTime LastStop
        {
            get { return _Completed; }
        }

        /// <summary>
        /// Whether the job is running
        /// </summary>
        public bool IsRunning
        {
            get { return _IsRunning; }
        }

        /// <summary>
        /// The time the job has run.
        /// </summary>
        public int Minutes
        {
            get { return _Interval / 60000; }
        }

        #endregion

        #region public methods
        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            Start(JobControllerFactory.CreateJobController());
        }
        /// <summary>
        /// Finds and Starts all jobs. Any existing jobs are shutdown first
        /// </summary>
        /// <param name="controller">The controller.</param>
        public void Start(IJobController controller)
        {
            if (_JobList.Count != 0)
                return;

            try { _Interval = controller.Minutes * 60000; }
            catch { _Interval = 15 * 60000; }

            foreach (IJobItem el in controller.JobItems)
            {
                _JobList.Add(new Job(
                    Type.GetType(el.Type)
                    , el
                    , el.Options));
            }

            // run jobs for the first time
            ExecuteFirstRuns();

            bool useGeneralTimerThread = false;
            // Now initialize timer callbacks
            foreach (Job job in _JobList)
            {
                if (job.Enabled 
                    && (job.ExecuteOnOwnThread 
                        || job.ExecuteDaily))
                    job.InitializeTimer();
                else
                    useGeneralTimerThread = true;
            }
            if (useGeneralTimerThread)
            {
                //Create a new timer to iterate over each job
                _SingleTimer = new Timer(new TimerCallback(SingleThreadTimerCallback), null, _Interval, _Interval);
            }
        }

        /// <summary>
        /// Calls dispose on all current jobs and clears the job list
        /// </summary>
        public void Stop()
        {
            if (_JobList != null)
            {
                foreach (Job job in this._JobList)
                    job.Dispose();

                _JobList.Clear();

                if (_SingleTimer != null)
                {
                    _SingleTimer.Dispose();
                    _SingleTimer = null;
                }
            }
        }

        /// <summary>
        /// Returns a reference to the current instance of Jobs
        /// </summary>
        /// <returns></returns>
        public static Jobs Instance()
        {
            return _Jobs;
        }

        /// <summary>
        /// Returns the current stats for the job.
        /// </summary>
        public System.Collections.Specialized.ListDictionary CurrentStats
        {
            get
            {
                System.Collections.Specialized.ListDictionary stats = new System.Collections.Specialized.ListDictionary();
                stats.Add("Created", _Created);
                stats.Add("LastStart", _Started);
                stats.Add("LastStop", _Completed);
                stats.Add("IsRunning", _IsRunning);
                stats.Add("Minutes", _Interval / 60000);
                return stats;
            }
        }

        /// <summary>
        /// Overridden. Shows the job status in the debugger visualiser.
        /// </summary>
        /// <returns>A job status.</returns>
        public override string ToString()
        {
            return string.Format("Created: {0}, LastStart: {1}, LastStop: {2}, IsRunning: {3}, Minutes: {4}", _Created, _Started, _Completed, _IsRunning, _Interval / 60000);
        }

        /// <summary>
        /// Checks to see whether the specified job is currently enabled or disabled.
        /// </summary>
        /// <param name="jobName">The name of the job</param>
        /// <returns>bool</returns>
        public bool IsJobEnabled(string jobName)
        {
            foreach (Job job in _JobList)
                if (job.Name.Equals(jobName))
                    return job.Enabled;

            return false;
        }

        /// <summary>
        /// Determines whether the specified job is currently running.
        /// </summary>
        /// <param name="jobName">Name of the job.</param>
        /// <returns>
        /// 	<c>true</c> if the specified job is currently running; otherwise, <c>false</c>.
        /// </returns>
        public bool IsJobRunning(string jobName)
        {
            foreach (Job job in _JobList)
                if (job.Name.Equals(jobName))
                    return job.IsRunning;

            return false;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Callback used to iterate over jobs when they are not excuted on their own Timer
        /// </summary>
        /// <param name="state"></param>
        private void SingleThreadTimerCallback(object state)
        {
            _IsRunning = true;
            _Started = DateTime.Now;
            _SingleTimer.Change(Timeout.Infinite, Timeout.Infinite);

            foreach (Job job in _JobList)
                if (job.Enabled && job.ExecuteOnOwnThread)
                    job.ExecuteJob();

            _SingleTimer.Change(_Interval, _Interval);
            _IsRunning = false;
            _Completed = DateTime.Now;
        }

        private void ExecuteFirstRuns_WaitCallback(object state)
        {
            Job job = state as Job;
            if (job != null)
                job.ExecuteJob();
        }

        /// <summary>
        /// Executes all jobs that should run at this object's instanciation. After this first execute call,
        /// all jobs will execute on their intervals.
        /// </summary>
        private void ExecuteFirstRuns()
        {
            _IsRunning = true;
            _Started = DateTime.Now;

            foreach (Job job in _JobList)
            {
                if (job.Enabled && job.FirstRunAtInitialization && !job.ExecuteDaily)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ExecuteFirstRuns_WaitCallback), job);
                }
            }

            _IsRunning = false;
            _Completed = DateTime.Now;
        }
        #endregion

        #region constructors
        //Create single instance of Jobs 
        static Jobs()
        {
            _Jobs = new Jobs();
        }
        /// <summary>
        /// Do not allow direct creation
        /// </summary>
        private Jobs()
        {
            _Created = DateTime.Now;
        }
        #endregion
    }
}
