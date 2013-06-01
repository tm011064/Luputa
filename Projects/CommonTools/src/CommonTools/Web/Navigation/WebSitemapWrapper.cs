using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CommonTools.Web.Navigation
{
    internal class WebSitemapWrapper : IUrlRewriteController
    {
        #region globals
        private Dictionary<string, int> _SiteMapLookUp;
        #endregion

        #region properties
        private Dictionary<string, RedirectRule> _Rules;
        private Dictionary<string, RedirectRule> Rules
        {
            get
            {
                if (_Rules == null)
                {
                    _Rules = new Dictionary<string, RedirectRule>();

                    foreach (SiteMapNode node in this.Nodes)
                        _Rules.Add(node.ResourceKey, new RedirectRule(
                            node.ResourceKey
                            , node.Url
                            , node["vanity"]
                            , node["pattern"]
                            , node["path"]));
                }

                return _Rules;
            }
        }

        private SiteMapNodeCollection _SiteMapNodes;
        /// <summary>
        /// Gets all sitemap nodes
        /// </summary>
        /// <value>The nodes.</value>
        private SiteMapNodeCollection Nodes
        {
            get { return _SiteMapNodes; }
        }
        #endregion

        #region public methods
        public void Reload()
        {
            _SiteMapLookUp = new Dictionary<string, int>();
            _SiteMapNodes = SiteMap.RootNode.GetAllNodes();

            for (int i = 0; i < _SiteMapNodes.Count; i++)
                _SiteMapLookUp.Add(_SiteMapNodes[i].ResourceKey, i);
        }
        #endregion

        #region IUrlRewriteController Members

        public IUrlRewriteController CreateUrlRewriteControllerInstance()
        {
            return new WebSitemapWrapper();
        }

        private List<IUrlRewriteItem> _UrlRewriteItems;
        public List<IUrlRewriteItem> UrlRewriteItems
        {
            get
            {
                if (_UrlRewriteItems == null)
                {
                    _UrlRewriteItems = new List<IUrlRewriteItem>();
                    foreach (string key in Rules.Keys)
                        _UrlRewriteItems.Add(Rules[key]);
                }
                return _UrlRewriteItems;
            }
        }

        public IUrlRewriteItem GetUrlRewriteItem(string name)
        {
            if (Rules.ContainsKey(name))
                return Rules[name];

            return null;
        }
        
        public int SitemapUrlsCacheDurationInSeconds
        {
            get { throw new NotImplementedException(); }
        }

        public string SitemapUrlsCacheKey
        {
            get { throw new NotImplementedException(); }
        }

        public System.Web.Caching.CacheItemPriority SitemapUrlsCacheItemPriority
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WebSitemapWrapper"/> class.
        /// </summary>
        public WebSitemapWrapper()
        {
            Reload();
        }
        #endregion
    }
}