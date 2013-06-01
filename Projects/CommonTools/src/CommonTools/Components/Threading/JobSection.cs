using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web.Configuration;
using System.Web;
using System.Collections;

namespace CommonTools.Components.Threading
{
    /// <summary>
    /// Provides the handler for the jobpool section of app.config.
    /// </summary>
    public class JobSection : ConfigurationSection, IJobController
    {
        private List<IJobItem> _JobItems;

        /// <summary>
        /// Gets the job pool.
        /// </summary>
        [ConfigurationProperty("jobPool", IsRequired = true)]
        public JobElements JobItemCollection
        {
            get { return (JobElements)base["jobPool"]; }
        }

        /// <summary>
        /// The number of minutes to hold the cached object.
        /// </summary>
        [ConfigurationProperty("jobControllerProviderType", IsRequired = false)]
        public string JobControllerProviderType
        {
            get
            {
                if (base["jobControllerProviderType"] != null)
                    return (string)base["jobControllerProviderType"];

                return null;
            }
        }

        #region IJobController Members

        /// <summary>
        /// Gets the execution interval in minutes for all IJobItems at this object's IJobItem collection. This value can be overwritten
        /// by the IJobItem itself.
        /// </summary>
        /// <value>The minutes.</value>
        public int Minutes
        {
            get { return JobItemCollection.Minutes; }
        }

        /// <summary>
        /// Creates an instance of this object's default controller.
        /// </summary>
        /// <returns></returns>
        public IJobController CreateJobControllerInstance()
        {
            return JobSectionManager.JobSection;
        }

        /// <summary>
        /// Gets the IJobItem collection associated with this ICacheController.
        /// </summary>
        /// <value>The job items.</value>
        public List<IJobItem> JobItems
        {
            get
            {
                if (_JobItems == null)
                {
                    _JobItems = new List<IJobItem>();
                    foreach (JobElement item in JobItemCollection)
                        _JobItems.Add((IJobItem)item);
                }
                return _JobItems;
            }
        }
        #endregion
    }

    /// <summary>
    /// Provides the handler for the job elements section of app.config.
    /// </summary>
    public class JobElements : ConfigurationElementCollection
    {
        #region attributes
        /// <summary>
        /// The number of minutes to run the job for.
        /// </summary>
        [ConfigurationProperty("minutes", DefaultValue = "15")]
        public int Minutes
        {
            get { return ((int)base["minutes"]); }
            set { base["minutes"] = value; }
        }
        #endregion

        /// <summary>
        /// Overridden. Creates a new job element in the child node of this section.
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new JobElement();
        }

        /// <summary>
        /// Overridden. Gets the id of the JobElement for the specified JobElement.
        /// </summary>
        /// <param name="element">The JobElement to get.</param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JobElement)element).Name;
        }
    }

    /// <summary>
    /// Provides the handler for the jobelement config section.
    /// </summary>
    public class JobElement : ConfigurationElement, IJobItem
    {
        /// <summary>
        /// Any other options that may appear in the config file.
        /// </summary>
        [ConfigurationProperty("Options", IsRequired = false)]
        public NameValueConfigurationCollection AdditionalOptions
        {
            get { return (NameValueConfigurationCollection)base["Options"]; }
        }

        private Dictionary<string, string> _Options;

        #region IJobItem Members
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("enabled", DefaultValue = "true")]
        public bool Enabled
        {
            get
            {
                if (base["enabled"] == null)
                    return false;

                return ((bool)base["enabled"]);
            }
            set { base["enabled"] = value; }
        }

        /// <summary>
        /// Shuts down the thread on close down.
        /// </summary>
        [ConfigurationProperty("enableShutDown", DefaultValue = "false")]
        public bool EnableShutDown
        {
            get { return ((bool)base["enableShutDown"]); }
            set { base["enableShutDown"] = value; }
        }

        /// <summary>
        /// (Deprecated, use Period instead) Defines the interval for the thread.
        /// </summary>
        [ConfigurationProperty("minutes", DefaultValue = "-1")]
        public int Minutes
        {
            get { return ((int)base["minutes"]); }
            set { base["minutes"] = value; }
        }

        /// <summary>
        /// (Deprecated, use Period instead) Defines the interval for the thread.
        /// </summary>
        [ConfigurationProperty("seconds", DefaultValue = "-1")]
        public int Seconds
        {
            get { return ((int)base["seconds"]); }
            set { base["seconds"] = value; }
        }

        private TimeSpan _Period;
        private bool _IsPeriodInstanciated = false;
        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        /// <value>
        /// The period.
        /// </value>
        [ConfigurationProperty("period", DefaultValue = "00:00:00.000")]
        public TimeSpan Period
        {
            get
            {
                if (!_IsPeriodInstanciated)
                {
                    _Period = ((TimeSpan)base["period"]);
                    if (_Period.TotalMilliseconds <= 0)
                    {
                        int helper = this.Minutes;
                        if (helper > 0)
                            _Period.Add(TimeSpan.FromMinutes(helper));
                        helper = this.Seconds;
                        if (helper > 0)
                            _Period.Add(TimeSpan.FromSeconds(helper));
                    }
                }
                return _Period;
            }
            set { base["period"] = value; }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this job should use a precise timer. Set this value to true if you need millisecond precision
        /// job execution.
        /// </summary>
        [ConfigurationProperty("usePreciseTimer", DefaultValue = "false")]
        public bool UsePreciseTimer
        {
            get { return ((bool)base["usePreciseTimer"]); }
            set { base["usePreciseTimer"] = value; }
        }
        /// <summary>
        /// If the UsePreciseTimer flag is set to true, this value indicates whether the actual callbacks should be fired off as asynchonous delegates or
        /// iteratively. If you use Async, make sure that the callbacks are thread-safe.
        /// </summary>
        [ConfigurationProperty("preciseTimerCallbackMode", DefaultValue = "Synchronized")]
        public PreciseTimerCallbackMode PreciseTimerCallbackMode
        {
            get { return ((PreciseTimerCallbackMode)base["preciseTimerCallbackMode"]); }
            set { base["preciseTimerCallbackMode"] = value; }
        }

        /// <summary>
        /// whether to run the job single threaded.
        /// </summary>
        [ConfigurationProperty("executeOnOwnThread", DefaultValue = "false")]
        public bool ExecuteOnOwnThread
        {
            get { return ((bool)base["executeOnOwnThread"]); }
            set { base["executeOnOwnThread"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to execute this job daily at a determined time.
        /// </summary>
        /// <value><c>true</c> if [execute daily]; otherwise, <c>false</c>.</value>
        [ConfigurationProperty("executeDaily", DefaultValue = "false")]
        public bool ExecuteDaily
        {
            get { return ((bool)base["executeDaily"]); }
            set { base["executeDaily"] = value; }
        }

        /// <summary>
        /// Gets or sets the time of the day that this job should execute on a daily basis. The format must be the
        /// following -> 16:04:12.123  { = hh:mm:ss.ms }
        /// </summary>
        /// <value>The execute daily at.</value>
        [ConfigurationProperty("dailyUTCExecutionTime", DefaultValue = "03:00:00.000")]
        public DateTime DailyUTCExecutionTime
        {
            get
            {
                return ((DateTime)base["dailyUTCExecutionTime"]);
            }
            set { base["dailyUTCExecutionTime"] = value; }
        }
        /// <summary>
        /// The name of the job
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return ((string)base["name"]); }
            set { base["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the daily localized execution time.
        /// </summary>
        /// <value>
        /// The daily localized execution time.
        /// </value>
        [ConfigurationProperty("dailyLocalizedExecutionTime", DefaultValue = "03:00:00.000")]
        public DateTime DailyLocalizedExecutionTime
        {
            get { return ((DateTime)base["dailyLocalizedExecutionTime"]); }
            set { base["dailyLocalizedExecutionTime"] = value; }
        }
        /// <summary>
        /// Gets or sets the name of the daily localized execution time zone.
        /// </summary>
        /// <value>
        /// The name of the daily localized execution time zone.
        /// </value>
        [ConfigurationProperty("dailyLocalizedExecutionTimeZoneName", IsRequired = false, IsKey = true)]
        public string DailyLocalizedExecutionTimeZoneName
        {
            get { return ((string)base["dailyLocalizedExecutionTimeZoneName"]); }
            set { base["dailyLocalizedExecutionTimeZoneName"] = value; }
        }

        /// <summary>
        /// The type of the job
        /// </summary>
        [ConfigurationProperty("type")]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

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
        [ConfigurationProperty("firstRunAtInitialization", IsRequired = false, DefaultValue = "true")]
        public bool FirstRunAtInitialization
        {
            get { return (bool)base["firstRunAtInitialization"]; }
            set { base["firstRunAtInitialization"] = value; }
        }

        /// <summary>
        /// Any other options that may appear in the config file.
        /// </summary>
        /// <value></value>
        public Dictionary<string, string> Options
        {
            get
            {
                if (_Options == null)
                {
                    _Options = new Dictionary<string, string>();
                    foreach (NameValueConfigurationElement element in AdditionalOptions)
                    {
                        _Options.Add(element.Name, element.Value);
                    }
                }
                return _Options;
            }
            internal set { this._Options = value; }
        }

        #endregion
    }
}
