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

namespace FantasyLeague.CommonTools.TestSuite.Components
{
    /// <summary>
    /// Summary description for MockupCacheController
    /// </summary>
    [Serializable]
    public class MockupSiteMapMenuController : ISiteMapMenuController, IXmlSerializable
    {
        #region members
        private Dictionary<string, ISiteMapMenu> _MockupSiteMapMenus = new Dictionary<string, ISiteMapMenu>();
        #endregion

        #region ISiteMapMenuController Members

        public ISiteMapMenuController CreateSiteMapMenuControllerInstance(IUrlRewriteController urlRewriteController)
        {
            string errormessage = string.Empty;
            MockupSiteMapMenuController c = FantasyLeague.CommonTools.Xml.XmlSerializationHelper<MockupSiteMapMenuController>.ConvertFromFile(
                Configuration.SiteMapMenuControllerXmlPath, out errormessage);

            if (!string.IsNullOrEmpty(errormessage))
                throw new Exception(errormessage);

            this.UrlRewriteController = urlRewriteController;
            foreach (ISiteMapMenu menu in c.SiteMapMenus.Values)
                foreach (ISiteMapMenuItem item in menu.MenuNodes)
                    LoadRewriteItems(item);

            return c;
        }

        public Dictionary<string, ISiteMapMenu> SiteMapMenus
        {
            get { return this._MockupSiteMapMenus; }
            set { this._MockupSiteMapMenus = value; }
        }

        public ISiteMapMenu GetSiteMapMenu(string name)
        {
            if (this.SiteMapMenus.ContainsKey(name))
                return this.SiteMapMenus[name];

            return null;
        }

        /// <summary>
        /// Gets the URL rewrite controller.
        /// </summary>
        /// <value>The URL rewrite controller.</value>
        public IUrlRewriteController UrlRewriteController { get; set; }

        #endregion

        #region private methods
        private void LoadRewriteItems(ISiteMapMenuItem item)
        {
            item.UrlRewriteItem = UrlRewriteController.GetUrlRewriteItem(item.UrlRewriteItemName);
            foreach (ISiteMapMenuItem child in item.ChildNodes)
                LoadRewriteItems(child);
        }

        private ISiteMapMenuItem GetMenuItemsRecursive(ISiteMapMenuItem parent, List<ISiteMapMenuItem> items, XElement menuNode)
        {
            MockupSiteMapMenuItem item = new MockupSiteMapMenuItem(parent);
            if (menuNode.Attribute("name") != null)
                item.Name = menuNode.Attribute("name").Value;
            if (menuNode.Attribute("urlRewriteItemName") != null)
                item.UrlRewriteItemName = menuNode.Attribute("urlRewriteItemName").Value;
            if (menuNode.Attribute("breadcrumbTitle") != null)
                item.BreadcrumbTitle = menuNode.Attribute("breadcrumbTitle").Value;
            if (menuNode.Attribute("title") != null)
                item.Title = menuNode.Attribute("title").Value;

            foreach (XElement element in menuNode.Elements("menunode"))
                item.ChildNodes.Add(GetMenuItemsRecursive(item, items, element));

            return item;
        }
        #endregion

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XElement el = XElement.Load(reader);

            _MockupSiteMapMenus = new Dictionary<string, ISiteMapMenu>();

            MockupSiteMapMenu menu;
            foreach (XElement element in el.Elements("menu"))
            {
                menu = new MockupSiteMapMenu();
                foreach (XElement menuNodeElement in element.Elements("menunode"))
                    menu.MenuNodes.Add(GetMenuItemsRecursive(null, new List<ISiteMapMenuItem>(), menuNodeElement));

                if (element.Attribute("name") != null)
                    menu.Name = element.Attribute("name").Value;

                _MockupSiteMapMenus.Add(menu.Name, menu);
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {

        }

        #endregion

        #region constructor
        public MockupSiteMapMenuController()
        {

        }
        #endregion
    }
}