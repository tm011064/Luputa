using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web.Caching;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// Provides the handler for the UrlRewrite section of app.config.
    /// </summary>
    public class UrlRewriteSection : ConfigurationSection, IUrlRewriteController
    {
        private List<IUrlRewriteItem> _UrlRewriteItems;

        /// <summary>
        /// holds the collection of UrlRewrite objects in the app.config.
        /// </summary>
        [ConfigurationProperty("objects", IsRequired = false)]
        public UrlRewriteElements UrlRewriteElementCollection
        {
            get { return (UrlRewriteElements)base["objects"]; }
        }

        /// <summary>
        /// The number of minutes to hold the UrlRewrited object.
        /// </summary>
        [ConfigurationProperty("urlRewriteControllerProviderType", IsRequired = false)]
        public string UrlRewriteControllerProviderType
        {
            get
            {
                if (base["urlRewriteControllerProviderType"] != null)
                    return (string)base["urlRewriteControllerProviderType"];

                return null;
            }
        }

        /// <summary>
        /// Gets the _ sitemap urls cache duration in seconds.
        /// </summary>
        /// <value>The _ sitemap urls cache duration in seconds.</value>
        [ConfigurationProperty("sitemapUrlsCacheDurationInSeconds", IsKey = false, IsRequired = false, DefaultValue = "10")]
        public int SitemapUrlsCacheDurationInSeconds
        {
            get { return (int)base["sitemapUrlsCacheDurationInSeconds"]; }
        }
        /// <summary>
        /// Gets the _ sitemap urls cache key.
        /// </summary>
        /// <value>The _ sitemap urls cache key.</value>
        [ConfigurationProperty("sitemapUrlsCacheKey", IsKey = false, IsRequired = false, DefaultValue = "___smu_125287")]
        public string SitemapUrlsCacheKey
        {
            get { return (string)base["sitemapUrlsCacheKey"]; }
        }
        /// <summary>
        /// Gets the _ sitemap urls cache item priority.
        /// </summary>
        /// <value>The _ sitemap urls cache item priority.</value>
        [ConfigurationProperty("sitemapUrlsCacheItemPriority", IsKey = false, IsRequired = false, DefaultValue = "High")]
        public CacheItemPriority SitemapUrlsCacheItemPriority
        {
            get { return (CacheItemPriority)base["sitemapUrlsCacheItemPriority"]; }
        }

        #region IUrlRewriteController Members

        /// <summary>
        /// Creates an instance of this object's default controller.
        /// </summary>
        /// <returns></returns>
        public IUrlRewriteController CreateUrlRewriteControllerInstance()
        {
            return UrlRewriteSectionManager.UrlRewriteSection;
        }

        /// <summary>
        /// Gets the IUrlRewriteItem collection associated with this IUrlRewriteController.
        /// </summary>
        /// <value>The UrlRewrite items.</value>
        public List<IUrlRewriteItem> UrlRewriteItems
        {
            get
            {
                if (_UrlRewriteItems == null)
                {
                    _UrlRewriteItems = new List<IUrlRewriteItem>();
                    foreach (UrlRewriteElement item in UrlRewriteElementCollection)
                        _UrlRewriteItems.Add((IUrlRewriteItem)item);
                }
                return _UrlRewriteItems;
            }
        }

        /// <summary>
        /// Gets an IUrlRewriteItem from the IUrlRewriteItem collection by name
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns></returns>
        public IUrlRewriteItem GetUrlRewriteItem(string name)
        {
            IUrlRewriteItem item = null;
            IEnumerator enumerator = this.UrlRewriteElementCollection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (((IUrlRewriteItem)enumerator.Current).Name.Equals(name))
                {
                    item = ((IUrlRewriteItem)enumerator.Current);
                    break;
                }
            }

            return item;
        }

        #endregion
    }

    /// <summary>
    /// Provides the handler for each individual UrlRewrite node in the UrlRewrite section of app.config.
    /// </summary>
    public class UrlRewriteElements : ConfigurationElementCollection
    {
        /// <summary>
        /// Overridden. Creates a new UrlRewriteElement.
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new UrlRewriteElement();
        }

        /// <summary>
        /// Overridden. Retrieves the specified UrlRewrite element name for the given weak typed node.
        /// </summary>
        /// <param name="element">The node in app.config</param>
        /// <returns>The UrlRewrite element name located at the configuration element.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UrlRewriteElement)element).Name;
        }
    }

    /// <summary>
    /// Provides the UrlRewrite element object as specified through app.config.
    /// </summary>
    public class UrlRewriteElement : ConfigurationElement, IUrlRewriteItem
    {
        #region IUrlRewriteItem Members
        /// <summary>
        /// A suffix to uniquely identify the UrlRewrite type.
        /// </summary>
        [ConfigurationProperty("pattern")]
        public string Pattern
        {
            get { return (string)base["pattern"]; }
            set { }
        }
        /// <summary>
        /// A suffix to uniquely identify the UrlRewrite type.
        /// </summary>
        [ConfigurationProperty("vanity")]
        public string Vanity
        {
            get { return (string)base["vanity"]; }
            set { }
        }
        /// <summary>
        /// A suffix to uniquely identify the UrlRewrite type.
        /// </summary>
        [ConfigurationProperty("path")]
        public string Path
        {
            get { return (string)base["path"]; }
            set { }
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [ConfigurationProperty("url", IsRequired = true, IsKey = false)]
        public string Url
        {
            get { return (string)base["url"]; }
            set { }
        }

        /// <summary>
        /// The name of the UrlRewrite object.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { }
        }

        /// <summary>
        /// The name of the UrlRewrite object.
        /// </summary>
        [ConfigurationProperty("parentName", IsRequired = false, IsKey = false, DefaultValue = "")]
        public string ParentName
        {
            get { return (string)base["parentName"]; }
            set { }
        }

        /// <summary>
        /// The name of the UrlRewrite object.
        /// </summary>
        [ConfigurationProperty("breadcrumbTitle", IsRequired = false, IsKey = false, DefaultValue = "")]
        public string BreadcrumbTitle
        {
            get { return (string)base["breadcrumbTitle"]; }
            set { }
        }

        /// <summary>
        /// The name of the UrlRewrite object.
        /// </summary>
        [ConfigurationProperty("title", IsRequired = false, IsKey = false, DefaultValue = "")]
        public string Title
        {
            get { return (string)base["title"]; }
            set { }
        }

        /// <summary>
        /// The name of the UrlRewrite object.
        /// </summary>
        [ConfigurationProperty("isHttps", IsRequired = false, IsKey = false, DefaultValue = "")]
        public bool IsHttps
        {
            get { return (bool)base["isHttps"]; }
            set { }
        }

        /// <summary>
        /// Gets the name of the <see cref="IUrlRewriteItem"/>.
        /// </summary>
        /// <value>The name.</value>
        public string FullVirtualPath
        {
            get { return (this.Url + this.Path).Replace("//", "/"); ; }
            set { }
        }

        #endregion
    }
}