using System;
using System.Configuration;

namespace CommonTools.Web.Navigation
{
	/// <summary>
	/// This is the class that's capable of enumerating the UrlRewrite section of your app.config.
	/// </summary>
    public static class UrlRewriteSectionManager
    {
		/// <summary>
		/// Gets the UrlRewrite section settings of the app.config or web.config
		/// </summary>
        public static UrlRewriteSection UrlRewriteSection
        {
            get
			{
                return (UrlRewriteSection)ConfigurationManager.GetSection(UrlRewriteControllerFactory.SECTION_NAME);
			}
        }
    }
}
