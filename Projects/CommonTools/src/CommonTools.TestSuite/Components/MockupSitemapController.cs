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
    public class MockupSitemapController : IUrlRewriteController, IXmlSerializable
    {
        #region members
        private List<IUrlRewriteItem> _MockupSitemapPageItems = new List<IUrlRewriteItem>();
        #endregion

        private MockupSiteMapMenuController _SiteMapMenuController;
        private MockupSiteMapMenuController SiteMapMenuController
        {
            get
            {
                if (_SiteMapMenuController == null)
                    _SiteMapMenuController = ((MockupSiteMapMenuController)new MockupSiteMapMenuController().CreateSiteMapMenuControllerInstance(this));
                return _SiteMapMenuController;
            }
        }

        #region IUrlRewriteController Members

        public IUrlRewriteController CreateUrlRewriteControllerInstance()
        {
            string errormessage = string.Empty;
            MockupSitemapController c = FantasyLeague.CommonTools.Xml.XmlSerializationHelper<MockupSitemapController>.ConvertFromFile(
                Configuration.SitemapControllerXmlPath, out errormessage);

            if (!string.IsNullOrEmpty(errormessage))
                throw new Exception(errormessage);

            return c;
        }

        public List<IUrlRewriteItem> UrlRewriteItems
        {
            get { return this._MockupSitemapPageItems; }
            set { this._MockupSitemapPageItems = value; }
        }

        public IUrlRewriteItem GetUrlRewriteItem(string name)
        {
            foreach (IUrlRewriteItem item in UrlRewriteItems)
                if (item.Name == name)
                    return item;

            return null;
        }

        public Dictionary<string, ISiteMapMenu> SiteMapMenus
        {
            get { return this.SiteMapMenuController.SiteMapMenus; }
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

            _MockupSitemapPageItems = new List<IUrlRewriteItem>();
            MockupSitemapPageItem item;

            foreach (XElement element in el.Elements("page"))
            {
                item = new MockupSitemapPageItem();

                if (element.Attribute("name") != null)
                    item.Name = element.Attribute("name").Value;
                if (element.Attribute("directory") != null)
                    item.Url = element.Attribute("directory").Value;
                if (element.Attribute("filepath") != null)
                    item.Path = element.Attribute("filepath").Value;
                if (element.Attribute("pattern") != null)
                    item.Pattern = element.Attribute("pattern").Value;
                if (element.Attribute("vanity") != null)
                    item.Vanity = element.Attribute("vanity").Value;

                _MockupSitemapPageItems.Add(item);
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XElement xCacheSection =
                new XElement("MockupSitemapController",
                            from c in this.UrlRewriteItems
                            select new XElement("page",
                                new XAttribute("name", c.Name),
                                new XAttribute("directory", c.Url),
                                new XAttribute("filepath", c.Path),
                                new XAttribute("pattern", c.Pattern),
                                new XAttribute("vanity", c.Vanity)));

            xCacheSection.Save(writer);
        }

        #endregion

        #region constructor
        public MockupSitemapController()
        {

        }
        #endregion
    }
}