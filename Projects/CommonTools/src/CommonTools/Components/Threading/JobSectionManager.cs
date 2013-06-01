using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace CommonTools.Components.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public static class JobSectionManager
    {
        /// <summary>
        /// Gets the job section.
        /// </summary>
        /// <value>The job section.</value>
        public static JobSection JobSection
        {
            get
            {
                return (JobSection)ConfigurationManager.GetSection(JobControllerFactory.SECTION_NAME);
            }
        }
    }
}
