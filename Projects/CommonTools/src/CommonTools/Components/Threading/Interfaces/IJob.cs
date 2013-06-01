using System;
using System.Configuration;
using System.Collections.Generic;

namespace CommonTools.Components.Threading
{
	/// <summary>
	/// Provides the base interface for the job configuration section.
	/// </summary>
	public interface IJob
	{
		/// <summary>
		/// Runs all jobs in the Configuration collection.
		/// </summary>
		/// <param name="node">The system.web.jobs config
		/// section that contains the list of jobs to run.</param>
        void Execute(Dictionary<string, string> node);
	}
}
