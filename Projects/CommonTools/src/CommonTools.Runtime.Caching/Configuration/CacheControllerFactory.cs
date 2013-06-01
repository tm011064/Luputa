using System;
using System.Configuration;

namespace CommonTools.Runtime.Caching.Configuration
{
    /// <summary>
    /// This is the class that's capable of enumerating the cache section of your app.config.
    /// </summary>
    public static class CacheControllerFactory
    {
        /// <summary>
        /// The configuration's configsection key
        /// </summary>
        public const string SECTION_NAME = "Cache";

        /// <summary>
        /// Gets the Cache section settings of the app.config
        /// </summary>
        public static ICacheController CreateCacheController()
        {
            CacheSection section = (CacheSection)ConfigurationManager.GetSection(SECTION_NAME);

            if (!string.IsNullOrEmpty(section.CacheControllerType))
            {
                ICacheController customController = Activator.CreateInstance(Type.GetType(section.CacheControllerType)) as ICacheController;
                if (customController != null)
                {
                    return customController.CreateCacheControllerInstance();
                }
            }

            return section;
        }
    }
}
