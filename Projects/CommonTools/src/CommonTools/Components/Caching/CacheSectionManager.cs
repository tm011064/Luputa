using System;
using System.Configuration;

namespace CommonTools.Components.Caching
{
	/// <summary>
	/// This is the class that's capable of enumerating the cache section of your app.config.
	/// </summary>
    public static class CacheSectionManager
    {
		/// <summary>
		/// Gets the Cache section settings of the app.config
		/// </summary>
        public static CacheSection CacheSection
        {
            get
			{
                return (CacheSection)ConfigurationManager.GetSection(CacheControllerFactory.SECTION_NAME);
			}
        }
    }
}
