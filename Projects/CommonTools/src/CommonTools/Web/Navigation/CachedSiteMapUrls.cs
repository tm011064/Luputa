using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Caching;
using CommonTools.Components.Caching;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// This class handles sitemap nodes.
    /// </summary>
    public class CachedSiteMapUrls : ISiteMapUrls
    {
        #region members
        private IUrlRewriteController _UrlRewriteController;
        private int _CacheDurationInSeconds;
        private string _CacheKey;
        private CacheItemPriority _CacheItemPriority;

        private object _RulesCacheLock = new object();

        #endregion

        #region properties
        private string RulesCacheKey { get { return _CacheKey + "__ru"; } }

        /// <summary>
        /// Gets the controller.
        /// </summary>
        /// <value>The controller.</value>
        private IUrlRewriteController Controller
        {
            get { return _UrlRewriteController; }
        }
        /// <summary>
        /// Gets the <see cref="CommonTools.Web.Navigation.IUrlRewriteItem"/> with the specified resource key.
        /// </summary>
        /// <value></value>
        public IUrlRewriteItem this[string name]
        {
            get
            {
                return Controller.GetUrlRewriteItem(name);
            }
        }

        private Dictionary<string, FormattedRedirectRule> Rules
        {
            get
            {
                if (HttpRuntime.Cache[RulesCacheKey] == null
                    || !(HttpRuntime.Cache[RulesCacheKey] is Dictionary<string, FormattedRedirectRule>))
                {
                    lock (_RulesCacheLock)
                    {
                        if (HttpRuntime.Cache[RulesCacheKey] == null
                            || !(HttpRuntime.Cache[RulesCacheKey] is Dictionary<string, FormattedRedirectRule>))
                        {
                            Dictionary<string, FormattedRedirectRule> rules = new Dictionary<string, FormattedRedirectRule>();
                            foreach (IUrlRewriteItem item in Controller.UrlRewriteItems)
                                rules.Add(item.Name, new FormattedRedirectRule(item));

                            HttpRuntime.Cache.Insert(
                                RulesCacheKey
                                , rules
                                , null
                                , DateTime.UtcNow.AddSeconds(_CacheDurationInSeconds)
                                , Cache.NoSlidingExpiration
                                , _CacheItemPriority
                                , null);
                            return rules;
                        }
                    }
                }

                return (Dictionary<string, FormattedRedirectRule>)HttpRuntime.Cache[RulesCacheKey];
            }
        }
        #endregion

        #region private methods
        private void RemovedCallback(string str, object obj, CacheItemRemovedReason reason)
        {
            HttpRuntime.Cache.Remove(RulesCacheKey);
        }
        #endregion

        #region public methods
        /// <summary>
        /// Finds the node by resource key.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IUrlRewriteItem FindNodeByResourceKey(string name)
        {
            return Controller.GetUrlRewriteItem(name);
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <param name="key">The resourceKey of the sitemapnode</param>
        /// <returns></returns>
        public string GetPath(string key)
        {
            if (this.Rules.ContainsKey(key))
                return this.Rules[key].Path;

            return this[key].Url;
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <param name="key">The resourceKey of the sitemapnode</param>
        /// <param name="args">The arguments to replace the path of the SitemapNode with.</param>
        /// <returns></returns>
        public string GetPath(string key, params string[] args)
        {
            if (this.Rules.ContainsKey(key))
                return String.Format(this.Rules[key].Path, args);

            return this[key].Url;
        }

        /// <summary>
        /// Gets the matching rewrite.
        /// </summary>
        /// <param name="pattern">The pattern to match</param>
        /// <returns></returns>
        public string GetMatchingRewrite(string pattern)
        {
            string returnString = "";
            Regex regex;

            foreach (FormattedRedirectRule rule in Rules.Values)
            {
                if (!string.IsNullOrEmpty(rule.Pattern))
                {
                    regex = new Regex(rule.Pattern, RegexOptions.IgnoreCase);
                    if (regex.IsMatch(pattern))
                    {
                        return regex.Replace(pattern, rule.Vanity);
                    }
                }
            }

            return returnString;
        }
        /// <summary>
        /// Gets the matching rewrite item. This method is intended for testing, don't use it in production because it is
        /// inefficient.
        /// </summary>
        /// <param name="pathAndQuery">The path and query url.</param>
        /// <returns></returns>
        public IUrlRewriteItem GetMatchingRewriteItem(string pathAndQuery)
        {
            // try to get the item via vanity
            foreach (FormattedRedirectRule rule in Rules.Values)
            {
                if (!string.IsNullOrEmpty(rule.Vanity))
                {
                    if (pathAndQuery.ToLower().EndsWith(rule.Vanity.ToLower()))
                        return this.Controller.GetUrlRewriteItem(rule.Name);
                }
            }

            // not found, try to get via path
            foreach (FormattedRedirectRule rule in Rules.Values)
            {
                if (pathAndQuery.ToLower().EndsWith(rule.Path.ToLower()))
                    return this.Controller.GetUrlRewriteItem(rule.Name);
            }

            return null;
        }

        /// <summary>
        /// Reloads the site map.
        /// </summary>
        public void Reload()
        {
            HttpRuntime.Cache.Remove(RulesCacheKey);
        }
        #endregion

        #region constructors
        private static object _StaticInstanceLock = new object();
        private static CachedSiteMapUrls _CachedSiteMapUrls;
        /// <summary>
        /// Instances this instance.
        /// </summary>
        /// <returns></returns>
        public static CachedSiteMapUrls Instance()
        {
            if (_CachedSiteMapUrls == null)
            {
                lock (_StaticInstanceLock)
                {
                    if (_CachedSiteMapUrls == null)
                    {
                        _CachedSiteMapUrls = new CachedSiteMapUrls();
                    }
                }
            }
            return _CachedSiteMapUrls;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapUrls"/> class.
        /// </summary>
        private CachedSiteMapUrls()
        {
            try { _UrlRewriteController = UrlRewriteControllerFactory.CreateUrlRewriteController(); }
            catch (Exception err) { throw new NavigationException("Error at creating IUrlRewriteController instance. See inner exception for further details.", err); }

            this._CacheKey = _UrlRewriteController.SitemapUrlsCacheKey;
            this._CacheDurationInSeconds = _UrlRewriteController.SitemapUrlsCacheDurationInSeconds;
            this._CacheItemPriority = _UrlRewriteController.SitemapUrlsCacheItemPriority;

            Reload();
        }
        #endregion
    }
}