using System;
using System.Configuration;

namespace CommonTools.Components.Caching
{
    /// <summary>
    /// This is the class that's capable of enumerating the ClusteredCache section of your app.config.
    /// </summary>
    public static class ClusteredCacheControllerFactory
    {
        /// <summary>
        /// The configuration's configsection key
        /// </summary>
        public const string SECTION_NAME = "ClusteredCache";

        /// <summary>
        /// Gets the ClusteredCache section settings of the app.config
        /// </summary>
        public static IClusteredCacheController CreateClusteredCacheController()
        {
            ClusteredCacheSection section = (ClusteredCacheSection)ConfigurationManager.GetSection(SECTION_NAME);

            if (!string.IsNullOrEmpty(section.ClusteredCacheSectionProviderType))
            {
                IClusteredCacheController customController = Activator.CreateInstance(Type.GetType(section.ClusteredCacheSectionProviderType)) as IClusteredCacheController;
                if (customController != null)
                {
                    return customController.CreateClusteredCacheControllerInstance();
                }
            }

            return section;
        }
    }
}
