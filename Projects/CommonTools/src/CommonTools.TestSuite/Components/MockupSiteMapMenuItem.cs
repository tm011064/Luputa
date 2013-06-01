using System;
using FantasyLeague.CommonTools.Web.Navigation;
using System.Collections.Generic;

namespace FantasyLeague.CommonTools.TestSuite.Components
{
    /// <summary>
    /// Summary description for MockupCacheItem
    /// </summary>
    [Serializable]
    public class MockupSiteMapMenuItem : ISiteMapMenuItem
    {
        #region ISiteMapMenuItem Members

        private string _Name = null;
        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }

        private string _BreadcrumbTitle = null;
        public string BreadcrumbTitle
        {
            get { return this._BreadcrumbTitle; }
            set { this._BreadcrumbTitle = value; }
        }

        private string _Title = null;
        public string Title
        {
            get { return this._Title; }
            set { this._Title = value; }
        }

        private List<ISiteMapMenuItem> _ChildNodes = new List<ISiteMapMenuItem>();
        public List<ISiteMapMenuItem> ChildNodes
        {
            get { return this._ChildNodes; }
            set { this._ChildNodes = value; }
        }

        public bool IsRoot
        {
            get { return this.ChildNodes.Count == 0; }
            set { }
        }

        private ISiteMapMenuItem _ParentNode = null;
        public ISiteMapMenuItem ParentNode
        {
            get { return this._ParentNode; }
            set { this._ParentNode = value; }
        }

        private string _UrlRewriteItemName = null;
        public string UrlRewriteItemName
        {
            get { return this._UrlRewriteItemName; }
            set { this._UrlRewriteItemName = value; }
        }

        private IUrlRewriteItem _UrlRewriteItem = null;
        public IUrlRewriteItem UrlRewriteItem
        {
            get { return this._UrlRewriteItem; }
            set { this._UrlRewriteItem = value; }
        }

        #endregion

        private string _TestProperty = null;
        [SiteMapMenuPropertyAttribute("testproperty")]
        public string TestProperty
        {
            get { return this._TestProperty; }
            set { this._TestProperty = value; }
        }
        private int _TestInteger = -1;
        [SiteMapMenuProperty("testinteger")]
        public int TestInteger
        {
            get { return this._TestInteger; }
            set { this._TestInteger = value; }
        }

        #region constructor
        public MockupSiteMapMenuItem() : this(null) { }
        public MockupSiteMapMenuItem(ISiteMapMenuItem parentNode)
        {
            this.ParentNode = parentNode;
        }
        #endregion
    }
}
