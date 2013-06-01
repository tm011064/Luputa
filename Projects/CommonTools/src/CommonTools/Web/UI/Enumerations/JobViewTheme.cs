using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Web.UI
{
    /// <summary>
    /// Provides a configuration flag that enables the developer to colour in their job visualisers.
    /// </summary>
    public enum JobViewTheme
    {
        /// <summary>
        /// Porvides a plain job visualiser without styles.
        /// </summary>
        None = 0,
        /// <summary>
        /// Provides default formatting for the job visualiser.
        /// </summary>
        Default = 1
    }
}
