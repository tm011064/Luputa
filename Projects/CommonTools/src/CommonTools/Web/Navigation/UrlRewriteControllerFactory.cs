using System;
using System.Configuration;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// This is the class that's capable of enumerating the UrlRewrite section of your app.config.
    /// </summary>
    public static class UrlRewriteControllerFactory
    {
        /// <summary>
        /// The configuration's configsection key
        /// </summary>
        public const string SECTION_NAME = "UrlRewriting";

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <returns></returns>
        public static UrlRewriteSection GetSection()
        {
            return (UrlRewriteSection)ConfigurationManager.GetSection(SECTION_NAME);
        }

        /// <summary>
        /// Gets the UrlRewrite section settings of the app.config
        /// </summary>
        public static IUrlRewriteController CreateUrlRewriteController()
        {
            UrlRewriteSection section = (UrlRewriteSection)ConfigurationManager.GetSection(SECTION_NAME);

            if (!string.IsNullOrEmpty(section.UrlRewriteControllerProviderType))
            {
                switch (section.UrlRewriteControllerProviderType.ToLower())
                {
                    case "web.sitemap":
                        return new WebSitemapWrapper();

                    default:

                        IUrlRewriteController customController = Activator.CreateInstance(Type.GetType(section.UrlRewriteControllerProviderType)) as IUrlRewriteController;
                        if (customController != null)
                        {
                            return customController.CreateUrlRewriteControllerInstance();
                        } 

                        break;
                }
            }

            return section;
        }
    }
}
