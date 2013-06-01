using System;
using System.Configuration;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// This is the class that's capable of enumerating the SiteMapMenu section of your app.config.
    /// </summary>
    public static class SiteMapMenuSectionManager
    {
        /// <summary>
        /// Gets the SiteMapMenu section settings of the app.config or web.config
        /// </summary>
        public static SiteMapMenuSection GetSiteMapMenuSection(IUrlRewriteController urlRewriteController)
        {
            SiteMapMenuSection section = (SiteMapMenuSection)ConfigurationManager.GetSection(SiteMapMenuControllerFactory.SECTION_NAME);
            section.UrlRewriteController = urlRewriteController;
            return section;
        }
    }
}
