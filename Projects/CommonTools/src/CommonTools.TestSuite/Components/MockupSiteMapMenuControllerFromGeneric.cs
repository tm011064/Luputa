using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Xml.Serialization;
using FantasyLeague.CommonTools.Components.Caching;
using System.Xml;
using System.Web.Caching;
using FantasyLeague.CommonTools.TestSuite.Components;
using FantasyLeague.CommonTools.Web.Navigation;
using System.Xml.Linq;
using System.Xml.Schema;

namespace FantasyLeague.CommonTools.TestSuite.Components
{
    /// <summary>
    /// Summary description for MockupCacheController
    /// </summary>
    [Serializable]
    public class MockupSiteMapMenuControllerFromGeneric : GenericSiteMapMenuController<MockupSiteMapMenu, MockupSiteMapMenuItem>
    {
        #region abstract members

        /// <summary>
        /// Creates an instance of this object's default controller.
        /// </summary>
        /// <param name="urlRewriteController">The URL rewrite controller.</param>
        /// <returns></returns>
        public override ISiteMapMenuController CreateSiteMapMenuControllerInstance(IUrlRewriteController urlRewriteController)
        {
            string errormessage = string.Empty;
            MockupSiteMapMenuControllerFromGeneric c = FantasyLeague.CommonTools.Xml.XmlSerializationHelper<MockupSiteMapMenuControllerFromGeneric>.ConvertFromFile(
                Configuration.GenericSiteMapMenuControllerXmlPath, out errormessage);

            if (!string.IsNullOrEmpty(errormessage))
                throw new Exception(errormessage);

            this.UrlRewriteController = urlRewriteController;
            foreach (ISiteMapMenu menu in c.SiteMapMenus.Values)
                foreach (ISiteMapMenuItem item in menu.MenuNodes)
                    LoadRewriteItems(item);

            return c;
        }

        #endregion

        /// <remarks>
        /// The ReadXml could be overwritten in case we want to deserialize our object from here
        /// </remarks>
        //public override void ReadXml(XmlReader reader)
        //{
        //    this._SiteMapMenus = GetGenericSiteMapMenusFromXml(reader);
        //}

        #region constructor
        public MockupSiteMapMenuControllerFromGeneric()
        {

        }
        #endregion
    }
}