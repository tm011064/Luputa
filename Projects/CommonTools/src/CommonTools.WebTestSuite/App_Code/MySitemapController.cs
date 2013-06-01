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
using System.Xml.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using CommonTools.Components.Caching;
using System.Xml;
using System.Web.Caching;
using CommonTools.TestApp.Components;
using CommonTools.Web.Navigation;

namespace CommonTools.TestApp.Components
{
    /// <summary>
    /// Summary description for MyCacheController
    /// </summary>
    [Serializable]
    public class MySitemapController : IUrlRewriteController, IXmlSerializable
    {
        #region members
        private List<IUrlRewriteItem> _MySitemapPageItems = new List<IUrlRewriteItem>();
        #endregion

        #region IUrlRewriteController Members

        public IUrlRewriteController CreateUrlRewriteControllerInstance()
        {
            string errormessage = string.Empty;
            MySitemapController c = CommonTools.Xml.XmlSerializationHelper<MySitemapController>.ConvertFromFile(
                HttpContext.Current.Server.MapPath("~/SitemapController.xml"), out errormessage);

            if (!string.IsNullOrEmpty(errormessage))
                throw new Exception(errormessage);

            return c;
        }

        public List<IUrlRewriteItem> UrlRewriteItems
        {
            get { return this._MySitemapPageItems; }
            set { this._MySitemapPageItems = value; }
        }

        public IUrlRewriteItem GetUrlRewriteItem(string name)
        {
            foreach (IUrlRewriteItem item in UrlRewriteItems)
                if (item.Name == name)
                    return item;

            return null;
        }


        public int SitemapUrlsCacheDurationInSeconds { get { throw new NotImplementedException(); } }

        public string SitemapUrlsCacheKey { get { throw new NotImplementedException(); } }

        public CacheItemPriority SitemapUrlsCacheItemPriority { get { throw new NotImplementedException(); } }

        #endregion

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XElement el = XElement.Load(reader);

            _MySitemapPageItems = new List<IUrlRewriteItem>();
            MySitemapPageItem item;

            foreach (XElement element in el.DescendantNodes())
            {
                if (element.Name == "page")
                {
                    item = new MySitemapPageItem();

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
                    if (element.Attribute("parentName") != null)
                        item.ParentName = element.Attribute("parentName").Value;
                    if (element.Attribute("breadcrumbTitle") != null)
                        item.BreadcrumbTitle = element.Attribute("breadcrumbTitle").Value;

                    _MySitemapPageItems.Add(item);
                }
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XElement xCacheSection =
                new XElement("MySitemapController",
                            from c in this.UrlRewriteItems
                            select new XElement("page",
                                new XAttribute("name", c.Name),
                                new XAttribute("parentName", c.ParentName),
                                new XAttribute("breadcrumbTitle", c.BreadcrumbTitle),
                                new XAttribute("directory", c.Url),
                                new XAttribute("filepath", c.Path),
                                new XAttribute("pattern", c.Pattern),
                                new XAttribute("vanity", c.Vanity)));

            xCacheSection.Save(writer);
        }

        #endregion

        #region constructor
        public MySitemapController()
        {

        }
        #endregion


    }
}