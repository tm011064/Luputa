using System;
using System.Configuration;

namespace CommonTools.Components.Logging
{
	/// <summary>
	/// This is the class that's capable of enumerating the Log section of your app.config.
	/// </summary>
    public static class LogSectionManager
    {
		/// <summary>
		/// Gets the Log section settings of the app.config or web.config
		/// </summary>
        public static LogSection LogSection
        {
            get
			{
                return (LogSection)ConfigurationManager.GetSection(LogControllerFactory.SECTION_NAME);
			}
        }
    }
}
