using System;
using System.Configuration;
using System.Collections.Generic;

namespace CommonTools.Components.Threading
{
	/// <summary>
	/// Provides the base interface for the job configuration section.
	/// </summary>
	public interface IJobController
	{
        /// <summary>
        /// Gets the execution interval in minutes for all IJobItems at this object's IJobItem collection. This value can be overwritten
        /// by the IJobItem itself.
        /// </summary>
        /// <value>The minutes.</value>
        int Minutes { get; }
        /// <summary>
        /// Creates an instance of this object's default controller.
        /// </summary>
        /// <returns></returns>
        IJobController CreateJobControllerInstance();
        /// <summary>
        /// Gets the IJobItem collection associated with this ICacheController.
        /// </summary>
        /// <value>The job items.</value>
        List<IJobItem> JobItems { get; } 
	}
}
