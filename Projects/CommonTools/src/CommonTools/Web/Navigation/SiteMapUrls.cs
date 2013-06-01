using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// This class handles sitemap nodes.
    /// </summary>
    public class SiteMapUrls : ISiteMapUrls
    {
        #region properties
        private IUrlRewriteController _Controller;
        /// <summary>
        /// Gets the controller.
        /// </summary>
        /// <value>The controller.</value>
        private IUrlRewriteController Controller
        {
            get
            {
                if (_Controller == null)
                    _Controller = UrlRewriteControllerFactory.CreateUrlRewriteController();

                return _Controller;
            }
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

        private Dictionary<string, FormattedRedirectRule> _Rules;
        private Dictionary<string, FormattedRedirectRule> Rules
        {
            get
            {
                if (_Rules == null)
                {
                    _Rules = new Dictionary<string, FormattedRedirectRule>();
                    foreach (IUrlRewriteItem item in Controller.UrlRewriteItems)
                        _Rules.Add(item.Name, new FormattedRedirectRule(item));
                }

                return _Rules;
            }
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
            _Rules = null;
            _Controller = null;
        }
        #endregion

        #region constructors
        private static SiteMapUrls _Instance;
        /// <summary>
        /// Instances this instance.
        /// </summary>
        /// <returns></returns>
        public static SiteMapUrls Instance()
        {
            if (_Instance == null)
                _Instance = new SiteMapUrls();

            return _Instance;
        }

        /// <summary>
        /// Instances the specified controller.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns></returns>
        public static SiteMapUrls Instance(IUrlRewriteController controller)
        {
            if (_Instance == null)
                _Instance = new SiteMapUrls();

            _Instance._Controller = controller;

            return _Instance;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapUrls"/> class.
        /// </summary>
        private SiteMapUrls()
        {

        }
        #endregion
    }
}