using System;
using System.Collections.Generic;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// The ISiteMapMenuController provides a collection of ISiteMapMenuItems.
    /// </summary>
    public interface ISiteMapMenuController
    {
        /// <summary>
        /// Creates an instance of this object's default controller.
        /// </summary>
        /// <param name="urlRewriteController">The URL rewrite controller.</param>
        /// <returns></returns>
        ISiteMapMenuController CreateSiteMapMenuControllerInstance(IUrlRewriteController urlRewriteController);
        /// <summary>
        /// Gets the site map menus. Format: Key -&gt; name of the menu, Value -&gt; the menu
        /// </summary>
        /// <value>The site map menus.</value>
        Dictionary<string, ISiteMapMenu> SiteMapMenus { get; }
        /// <summary>
        /// Gets an ISiteMapMenuItem from the ISiteMapMenuItem collection by name
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns></returns>
        ISiteMapMenu GetSiteMapMenu(string name);
        /// <summary>
        /// Gets the URL rewrite controller.
        /// </summary>
        /// <value>The URL rewrite controller.</value>
        IUrlRewriteController UrlRewriteController { get; }
    }
}
