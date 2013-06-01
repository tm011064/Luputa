using System;
using System.Configuration;

namespace CommonTools.Components.Logging
{
    /// <summary>
    /// This is the class that's capable of enumerating the Log section of your app.config.
    /// </summary>
    public static class LogControllerFactory
    {
        /// <summary>
        /// The configuration's configsection key
        /// </summary>
        public const string SECTION_NAME = "Logging";

        /// <summary>
        /// Gets the Log section settings of the app.config
        /// </summary>
        public static ILogController CreateLogController()
        {
            LogSection section = (LogSection)ConfigurationManager.GetSection(SECTION_NAME);

            if (!string.IsNullOrEmpty(section.LogSectionProviderType))
            {
                ILogController customController = Activator.CreateInstance(Type.GetType(section.LogSectionProviderType)) as ILogController;
                if (customController != null)
                {
                    return customController.CreateLogControllerInstance();
                }
            }

            return section;
        }
    }
}
