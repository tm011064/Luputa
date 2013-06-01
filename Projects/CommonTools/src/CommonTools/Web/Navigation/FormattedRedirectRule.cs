using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// 
    /// </summary>
    public class FormattedRedirectRule : RedirectRule
    {
        #region constructors
        private void Init(string name, string url, string vanity, string pattern, string path)
        {
            this.Name = name;

            this.Url = url.StartsWith("~") ? url.Remove(0, 1) : url;
            this.Url = this.Url.EndsWith("/") ? this.Url : this.Url + "/";

            this.Vanity = string.Empty;
            this.Pattern = string.Empty;
            this.Path = string.Empty;

            if (!string.IsNullOrEmpty(vanity))
                this.Vanity = this.Url + (vanity.StartsWith("/") ? vanity.Remove(0, 1) : vanity);
            if (!string.IsNullOrEmpty(pattern))
                this.Pattern = this.Url + (pattern.StartsWith("/") ? pattern.Remove(0, 1) : pattern);
            if (!string.IsNullOrEmpty(path))
                this.Path = this.Url + (path.StartsWith("/") ? path.Remove(0, 1) : path);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormattedRedirectRule"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public FormattedRedirectRule(IUrlRewriteItem item)
        {
            Init(item.Name, item.Url, item.Vanity, item.Pattern, item.Path);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormattedRedirectRule"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        public FormattedRedirectRule(SiteMapNode node)
        {
            Init(node.ResourceKey, node.Url, node["vanity"], node["pattern"], node["path"]);
        }
        #endregion
    }

    /// <summary>
    /// This struct exposes properties that represent a redirect rule
    /// </summary>
    public class RedirectRule : IUrlRewriteItem
    {
        #region properties
        private string _Pattern;
        /// <summary>
        /// Gets the pattern.
        /// </summary>
        /// <value>The pattern.</value>
        public string Pattern
        {
            get { return _Pattern; }
            set { _Pattern = value; }
        }

        private string _Url;
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
        }

        private string _Vanity;
        /// <summary>
        /// Gets the vanity.
        /// </summary>
        /// <value>The vanity.</value>
        public string Vanity
        {
            get { return _Vanity; }
            set { _Vanity = value; }
        }

        private string _Path;
        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path
        {
            get { return _Path; }
            set { _Path = value; }
        }

        private string _Name;
        /// <summary>
        /// Gets the name of the <see cref="IUrlRewriteItem"/>.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _ParentName;
        /// <summary>
        /// Gets the name of the <see cref="IUrlRewriteItem"/>.
        /// </summary>
        /// <value>The name.</value>
        public string ParentName
        {
            get { return _ParentName; }
            set { _ParentName = value; }
        }

        /// <summary>
        /// Gets the name of the <see cref="IUrlRewriteItem"/>.
        /// </summary>
        /// <value>The name.</value>
        public string FullVirtualPath
        {
            get { return (this.Url + this.Path).Replace("//", "/"); }
            set { }
        }

        private string _BreadcrumbTitle;
        /// <summary>
        /// Gets the breadcrumb title.
        /// </summary>
        /// <value>The breadcrumb title.</value>
        public string BreadcrumbTitle
        {
            get { return _BreadcrumbTitle; }
            set { _BreadcrumbTitle = value; }
        }

        private string _Title;
        /// <summary>
        /// Gets or sets the title of the page.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private bool _IsHttps;
        /// <summary>
        /// Gets a value indicating whether this instance is HTTPS.
        /// </summary>
        /// <value><c>true</c> if this instance is HTTPS; otherwise, <c>false</c>.</value>
        public bool IsHttps
        {
            get { return _IsHttps; }
            set { _IsHttps = value; }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectRule"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="url">The URL.</param>
        /// <param name="vanity">The vanity.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="path">The path.</param>
        public RedirectRule(string name, string url, string vanity, string pattern, string path)
        {
            this.Name = name;
            this.Url = url;
            this.Vanity = vanity;
            this.Pattern = pattern;
            this.Path = path;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectRule"/> class.
        /// </summary>
        public RedirectRule() { }
        #endregion
    }
}
