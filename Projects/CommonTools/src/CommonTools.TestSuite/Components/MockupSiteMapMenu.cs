using System;
using FantasyLeague.CommonTools.Web.Navigation;
using System.Collections.Generic;

namespace FantasyLeague.CommonTools.TestSuite.Components
{
    /// <summary>
    /// Summary description for MockupCacheItem
    /// </summary>
    [Serializable]
    public class MockupSiteMapMenu : ISiteMapMenu
    {
        #region ISiteMapMenu Members

        private string _Name = null;
        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }

        private List<ISiteMapMenuItem> _MenuNodes = new List<ISiteMapMenuItem>();
        public List<ISiteMapMenuItem> MenuNodes
        {
            get { return this._MenuNodes; }
            set { this._MenuNodes = value; }
        }

        public ISiteMapMenuItem GetMatchingSiteMapMenuItem(string name)
        {
            ISiteMapMenuItem matchingItem;
            foreach (ISiteMapMenuItem item in this.MenuNodes)
            {
                matchingItem = GetMatchingSiteMapMenuItem(item, name);
                if (matchingItem != null)
                    return matchingItem;
            }
            return null;
        }

        private bool _EnableCaching = false;
        public bool EnableCaching
        {
            get { return this._EnableCaching; }
            set { this._EnableCaching = value; }
        }

        private int _CacheDurationInSeconds = 60;
        public int CacheDurationInSeconds
        {
            get { return this._CacheDurationInSeconds; }
            set { this._CacheDurationInSeconds = value; }
        }

        private System.Web.Caching.CacheItemPriority _CacheItemPriority = System.Web.Caching.CacheItemPriority.Low;
        public System.Web.Caching.CacheItemPriority CacheItemPriority
        {
            get { return this._CacheItemPriority; }
            set { this._CacheItemPriority = value; }
        }

        #endregion

        #region private methods

        private ISiteMapMenuItem GetMatchingSiteMapMenuItem(ISiteMapMenuItem item, string name)
        {
            if (item.UrlRewriteItemName == name)
                return item;
            else
            {
                ISiteMapMenuItem match;
                foreach (ISiteMapMenuItem child in item.ChildNodes)
                {
                    match = GetMatchingSiteMapMenuItem(child, name);
                    if (match != null)
                        return match;
                }
            }
            return null;
        }
        
        #endregion

        #region constructor
        public MockupSiteMapMenu()
        {

        }
        #endregion
    }
}
