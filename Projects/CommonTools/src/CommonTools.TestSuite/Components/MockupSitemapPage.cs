using System;
using FantasyLeague.CommonTools.Web.Navigation;

namespace FantasyLeague.CommonTools.TestSuite.Components
{
    /// <summary>
    /// Summary description for MockupCacheItem
    /// </summary>
    [Serializable]
    public class MockupSitemapPageItem : IUrlRewriteItem
    {
        private string _Name = null;
        private string _Directory = null;
        private string _FilePath = null;
        private string _Pattern = null;
        private string _Vanity = null;
        private string _ParentName = null;
        private string _BreadcrumbTitle = null;
        private string _Title = null;
        private bool _IsHttps = false;
        
        #region IUrlRewriteItem Members

        public string Pattern
        {
            get { return this._Pattern; }
            set { this._Pattern = value; }
        }

        public string Vanity
        {
            get { return this._Vanity; }
            set { this._Vanity = value; }
        }

        public string Path
        {
            get { return this._FilePath; }
            set { this._FilePath = value; }
        }

        public string Url
        {
            get { return this._Directory; }
            set { this._Directory = value; }
        }
        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }
        public string ParentName
        {
            get { return this._ParentName; }
            set { this._ParentName = value; }
        }

        public string FullVirtualPath
        {
            get { return (this.Url + this.Path).Replace("//", "/"); }
            set { }
        }

        public string BreadcrumbTitle
        {
            get { return _BreadcrumbTitle; }
            set { _BreadcrumbTitle = value; }
        }

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        public bool IsHttps
        {
            get { return this._IsHttps; }
            set { this._IsHttps = value; }
        }
        #endregion

        #region constructor
        public MockupSitemapPageItem()
        {

        }
        #endregion
    }
}
