using System;
using System.Configuration;

namespace CommonTools.Components.Caching
{
	/// <summary>
	/// This is the class that's capable of enumerating the ClusteredCache section of your app.config.
	/// </summary>
    public static class ClusteredCacheSectionManager
    {
		/// <summary>
		/// Gets the ClusteredCache section settings of the app.config or web.config
		/// </summary>
        public static ClusteredCacheSection ClusteredCacheSection
        {
            get
			{
                return (ClusteredCacheSection)ConfigurationManager.GetSection(ClusteredCacheControllerFactory.SECTION_NAME);
			}
        }
    }
}
