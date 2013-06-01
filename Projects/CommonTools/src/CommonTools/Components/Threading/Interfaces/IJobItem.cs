using System;
using System.Collections.Generic;

namespace CommonTools.Components.Threading
{
    /// <summary>
    /// Provides the base interface for the job configuration section.
    /// </summary>
    public interface IJobItem
    {
        /// <summary>
        /// Defines whether this job is enabled or not
        /// </summary>
        bool Enabled { get; set; }
        /// <summary>
        /// Shuts down the thread on close down.
        /// </summary>
        bool EnableShutDown { get; set; }
        /// <summary>
        /// (Deprecated, use Period instead) Defines the interval for the thread.
        /// </summary>
        int Minutes { get; set; }
        /// <summary>
        /// (Deprecated, use Period instead) Defines the interval for the thread.
        /// </summary>
        int Seconds { get; set; }
        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        /// <value>
        /// The period.
        /// </value>
        TimeSpan Period { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this job should use a precise timer. Set this value to true if you need millisecond precision
        /// job execution.
        /// </summary>
        bool UsePreciseTimer { get; set; }
        /// <summary>
        /// If the UsePreciseTimer flag is set to true, this value indicates whether the actual callbacks should be fired off as asynchonous delegates or
        /// iteratively. If you use Async, make sure that the callbacks are thread-safe.
        /// </summary>
        PreciseTimerCallbackMode PreciseTimerCallbackMode { get; set; }
        /// <summary>
        /// Determines whether to run the job on it's own thread or on the general thread.
        /// </summary>
        bool ExecuteOnOwnThread { get; set; }
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
        /// <value></value>
        bool FirstRunAtInitialization { get; set; }
        /// <summary>
        /// The name of the job
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string Type { get; set; }
        /// <summary>
        /// Any other options that may appear in the config file.
        /// </summary>
        Dictionary<string, string> Options { get; }
        /// <summary>
        /// Gets or sets a value indicating whether to execute this job daily at a determined time.
        /// </summary>
        /// <value><c>true</c> if [execute daily]; otherwise, <c>false</c>.</value>
        bool ExecuteDaily { get; set; }
        /// <summary>
        /// Gets or sets the time of the day that this job should execute on a daily basis.
        /// </summary>
        /// <value>The execute daily at.</value>
        DateTime DailyUTCExecutionTime { get; set; }
        /// <summary>
        /// Gets or sets the daily localized execution time.
        /// </summary>
        /// <value>
        /// The daily localized execution time.
        /// </value>
        DateTime DailyLocalizedExecutionTime { get; set; }
        /// <summary>
        /// Gets or sets the name of the daily localized execution time zone.
        /// </summary>
        /// <value>
        /// The name of the daily localized execution time zone.
        /// </value>
        string DailyLocalizedExecutionTimeZoneName { get; set; }
    }
}
