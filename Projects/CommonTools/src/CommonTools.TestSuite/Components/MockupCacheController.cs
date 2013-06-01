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
using CommonTools.Components.Caching;
using System.Xml;
using System.Web.Caching;
using CommonTools.TestSuite.Components;
using System.Xml.Linq;

namespace CommonTools.TestSuite.Components
{
    /// <summary>
    /// Summary description for MockupCacheController
    /// </summary>
    [Serializable]
    public class MockupCacheController : ICacheController, IXmlSerializable
    {
        #region members
        private bool _Enabled = false;
        private int _Minutes = 15;
        private List<ICacheItem> _CacheItems = new List<ICacheItem>();
        #endregion

        #region ICacheController Members
        public ICacheController CreateCacheControllerInstance()
        {
            string errormessage = string.Empty;
            MockupCacheController c = CommonTools.Xml.XmlSerializationHelper<MockupCacheController>.ConvertFromFile(
                Configuration.CacheConfigXmlPath, out errormessage);

            if (!string.IsNullOrEmpty(errormessage))
                throw new Exception(errormessage);

            return c;
        }

        public bool Enabled
        {
            get { return this._Enabled; }
            set { this._Enabled = value; }
        }

        public ICacheItem GetCacheItem(string key)
        {
            foreach (ICacheItem item in CacheItems)
                if (item.Name == key)
                    return item;

            return null;
        }

        public int Minutes
        {
            get { return this._Minutes; }
            set { this._Minutes = value; }
        }

        public System.Collections.Generic.List<ICacheItem> CacheItems
        {
            get { return this._CacheItems; }
            set { this._CacheItems = value; }
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

            XElement section = el.Element("MyCacheSection");
            if (section != null)
            {
                if (section.Attribute("minutes") != null)
                    this._Minutes = int.Parse(section.Attribute("minutes").Value);
                if (section.Attribute("enabled") != null)
                    this._Enabled = bool.Parse(section.Attribute("enabled").Value);


                _CacheItems = new List<ICacheItem>();
                MockupCacheItem item;

                foreach (XElement element in section.Descendants("MockupCacheItem"))
                {
                    item = new MockupCacheItem();

                    if (element.Attribute("suffix") != null)
                        item.Suffix = element.Attribute("suffix").Value;
                    if (element.Attribute("cacheItemPriority") != null)
                        item.CacheItemPriority = (CacheItemPriority)Enum.Parse(typeof(CacheItemPriority), element.Attribute("cacheItemPriority").Value);
                    if (element.Attribute("enabled") != null)
                        item.Enabled = bool.Parse(element.Attribute("enabled").Value);
                    if (element.Attribute("isIterating") != null)
                        item.IsIterating = bool.Parse(element.Attribute("isIterating").Value);
                    if (element.Attribute("minutes") != null)
                        item.Minutes = int.Parse(element.Attribute("minutes").Value);
                    if (element.Attribute("name") != null)
                        item.Name = element.Attribute("name").Value;
                    if (element.Attribute("cacheKey") != null)
                        item.CacheKey = element.Attribute("cacheKey").Value;

                    _CacheItems.Add(item);
                }
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XElement xCacheSection =
                new XElement("MyCacheSection",
                        new XAttribute("enabled", _Enabled.ToString()),
                        new XAttribute("minutes", _Minutes.ToString()),
                            from c in this.CacheItems
                            select new XElement("MockupCacheItem",
                                new XAttribute("cacheItemPriority", c.CacheItemPriority.ToString()),
                                new XAttribute("cacheKey", c.CacheKey),
                                new XAttribute("enabled", c.Enabled.ToString()),
                                new XAttribute("isIterating", c.IsIterating.ToString()),
                                new XAttribute("minutes", c.Minutes.ToString()),
                                new XAttribute("name", c.Name),
                                new XAttribute("suffix", c.Suffix)));

            xCacheSection.Save(writer);
        }

        #endregion

        #region constructor
        public MockupCacheController()
        {

        }
        #endregion

        #region ICacheController Members

        public string ContinuousAccessStaleKeySuffixForMemcached
        {
            get { throw new NotImplementedException(); }
        }

        Dictionary<string, ICacheItem> ICacheController.CacheItems
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}