using System;
using System.Configuration;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// This is the class that's capable of enumerating the SiteMapMenu section of your app.config.
    /// </summary>
    public static class SiteMapMenuControllerFactory
    {
        /// <summary>
        /// The configuration's configsection key
        /// </summary>
        public const string SECTION_NAME = "SiteMapMenu";

        /// <summary>
        /// Gets the SiteMapMenu section settings of the app.config
        /// </summary>
        public static ISiteMapMenuController CreateSiteMapMenuController(IUrlRewriteController urlRewriteController)
        {
            SiteMapMenuSection section = (SiteMapMenuSection)ConfigurationManager.GetSection(SECTION_NAME);

            if (!string.IsNullOrEmpty(section.SiteMapMenuControllerProviderType))
            {
                ISiteMapMenuController customController = Activator.CreateInstance(Type.GetType(section.SiteMapMenuControllerProviderType)) as ISiteMapMenuController;
                if (customController != null)
                {
                    return customController.CreateSiteMapMenuControllerInstance(urlRewriteController);
                }
            }

            return section;
        }
    }
}
