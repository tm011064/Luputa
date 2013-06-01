using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace CommonTools.Components.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public static class JobControllerFactory
    {
        /// <summary>
        /// The configuration's configsection key
        /// </summary>
        public const string SECTION_NAME = "Jobs";

        /// <summary>
        /// Gets the Job section settings of the app.config
        /// </summary>
        public static IJobController CreateJobController()
        {
            JobSection section = (JobSection)ConfigurationManager.GetSection(SECTION_NAME);

            if (!string.IsNullOrEmpty(section.JobControllerProviderType))
            {
                IJobController customController = Activator.CreateInstance(Type.GetType(section.JobControllerProviderType)) as IJobController;
                if (customController != null)
                {
                    return customController.CreateJobControllerInstance();
                }
            }

            return section;
        }
    }
}
