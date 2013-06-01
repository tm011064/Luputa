using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Web.Caching;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// Provides the handler for the SiteMapMenu section of app.config.
    /// </summary>
    public class SiteMapMenuSection : ConfigurationSection, ISiteMapMenuController
    {
        private Dictionary<string, ISiteMapMenu> _SiteMapMenus;

        /// <summary>
        /// holds the collection of SiteMapMenu objects in the app.config.
        /// </summary>
        [ConfigurationProperty("objects", IsRequired = false)]
        public SiteMapMenuElements SiteMapMenuElementCollection
        {
            get { return (SiteMapMenuElements)base["objects"]; }
        }

        /// <summary>
        /// The number of minutes to hold the SiteMapMenud object.
        /// </summary>
        [ConfigurationProperty("siteMapMenuControllerProviderType", IsRequired = false)]
        public string SiteMapMenuControllerProviderType
        {
            get
            {
                if (base["siteMapMenuControllerProviderType"] != null)
                    return (string)base["siteMapMenuControllerProviderType"];

                return null;
            }
        }

        #region ISiteMapMenuController Members

        /// <summary>
        /// Creates an instance of this object's default controller.
        /// </summary>
        /// <returns></returns>
        public ISiteMapMenuController CreateSiteMapMenuControllerInstance(IUrlRewriteController urlRewriteController)
        {
            return SiteMapMenuSectionManager.GetSiteMapMenuSection(urlRewriteController);
        }

        /// <summary>
        /// Gets the ISiteMapMenuItem collection associated with this ISiteMapMenuController.
        /// </summary>
        /// <value>The SiteMapMenu items.</value>
        public Dictionary<string, ISiteMapMenu> SiteMapMenus
        {
            get
            {
                if (_SiteMapMenus == null)
                {
                    _SiteMapMenus = new Dictionary<string, ISiteMapMenu>();
                    foreach (SiteMapMenuElement item in SiteMapMenuElementCollection)
                        _SiteMapMenus.Add(item.Name, (ISiteMapMenu)item);
                }
                return _SiteMapMenus;
            }
        }

        /// <summary>
        /// Gets an ISiteMapMenuItem from the ISiteMapMenuItem collection by name
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns></returns>
        public ISiteMapMenu GetSiteMapMenu(string name)
        {
            ISiteMapMenu item = null;
            IEnumerator enumerator = this.SiteMapMenuElementCollection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (((ISiteMapMenu)enumerator.Current).Name.Equals(name))
                {
                    item = ((ISiteMapMenu)enumerator.Current);
                    break;
                }
            }

            return item;
        }

        /// <summary>
        /// Gets the URL rewrite controller.
        /// </summary>
        /// <value>The URL rewrite controller.</value>
        public IUrlRewriteController UrlRewriteController { get; set; }

        #endregion
    }

    #region collections
    /// <summary>
    /// Provides the handler for each individual SiteMapMenu node in the SiteMapMenu section of app.config.
    /// </summary>
    public class SiteMapMenuElements : ConfigurationElementCollection
    {
        /// <summary>
        /// Overridden. Creates a new SiteMapMenuElement.
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new SiteMapMenuElement();
        }

        /// <summary>
        /// Overridden. Retrieves the specified SiteMapMenu element name for the given weak typed node.
        /// </summary>
        /// <param name="element">The node in app.config</param>
        /// <returns>The SiteMapMenu element name located at the configuration element.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SiteMapMenuElement)element).Name;
        }
    }

    /// <summary>
    /// Provides the handler for each individual SiteMapMenuItem node in the SiteMapMenu section of app.config.
    /// </summary>
    public class SiteMapMenuItemElements : ConfigurationElementCollection
    {
        /// <summary>
        /// Overridden. Creates a new SiteMapMenuElement.
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new SiteMapMenuItemElement();
        }

        /// <summary>
        /// Overridden. Retrieves the specified SiteMapMenu element name for the given weak typed node.
        /// </summary>
        /// <param name="element">The node in app.config</param>
        /// <returns>The SiteMapMenu element name located at the configuration element.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SiteMapMenuItemElement)element).Name;
        }
    }
    #endregion

    #region elements
    /// <summary>
    /// Provides the SiteMapMenu element object as specified through app.config.
    /// </summary>
    public class SiteMapMenuElement : ConfigurationElement, ISiteMapMenu
    {
        /// <summary>
        /// holds the collection of SiteMapMenu objects in the app.config.
        /// </summary>
        [ConfigurationProperty("objects", IsRequired = true)]
        public SiteMapMenuItemElements SiteMapMenuItemElementCollection
        {
            get { return (SiteMapMenuItemElements)base["objects"]; }
        }

        #region ISiteMapMenu Members

        /// <summary>
        /// Gets the name of the <see cref="ISiteMapMenuItem"/>.
        /// </summary>
        /// <value>The name.</value>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { }
        }

        /// <summary>
        /// Gets the child nodes.
        /// </summary>
        /// <value>The child nodes.</value>
        public List<ISiteMapMenuItem> MenuNodes
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the matching site map menu item.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public ISiteMapMenuItem GetMatchingSiteMapMenuItem(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A suffix to uniquely identify the UrlRewrite type.
        /// </summary>
        [ConfigurationProperty("enableCaching")]
        public bool EnableCaching
        {
            get { return (bool)base["enableCaching"]; }
            set { }
        }

        /// <summary>
        /// A suffix to uniquely identify the UrlRewrite type.
        /// </summary>
        [ConfigurationProperty("cacheDurationInSeconds")]
        public int CacheDurationInSeconds
        {
            get { return (int)base["cacheDurationInSeconds"]; }
            set { }
        }

        /// <summary>
        /// A suffix to uniquely identify the UrlRewrite type.
        /// </summary>
        [ConfigurationProperty("cacheItemPriority")]
        public CacheItemPriority CacheItemPriority
        {
            get { return (CacheItemPriority)base["cacheItemPriority"]; }
            set { }
        }

        #endregion
    }

    /// <summary>
    /// Provides the SiteMapMenuItem element object as specified through app.config.
    /// </summary>
    public class SiteMapMenuItemElement : ConfigurationElement, ISiteMapMenuItem
    {
        #region members
        private List<ISiteMapMenuItem> _SiteMapMenuItems;
        #endregion

        #region properties
        /// <summary>
        /// holds the collection of SiteMapMenu objects in the app.config.
        /// </summary>
        [ConfigurationProperty("objects", IsRequired = true)]
        public SiteMapMenuItemElements SiteMapMenuItemElementCollection
        {
            get { return (SiteMapMenuItemElements)base["objects"]; }
            set { }
        }
        #endregion

        #region ISiteMapMenuItem Members

        /// <summary>
        /// Gets the name of the <see cref="IUrlRewriteItem"/>.
        /// </summary>
        /// <value>The name.</value>
        [ConfigurationProperty("urlRewriteItemName", IsRequired = true, IsKey = false)]
        public string UrlRewriteItemName
        {
            get { return (string)base["urlRewriteItemName"]; }
            set { }
        }

        /// <summary>
        /// Gets the name of the <see cref="IUrlRewriteItem"/>.
        /// </summary>
        /// <value>The name.</value>
        [ConfigurationProperty("name", IsRequired = false, IsKey = false)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { }
        }

        /// <summary>
        /// Gets the breadcrumb title.
        /// </summary>
        /// <value>The breadcrumb title.</value>
        [ConfigurationProperty("breadcrumbTitle", IsRequired = false, IsKey = false, DefaultValue = "")]
        public string BreadcrumbTitle
        {
            get { return (string)base["breadcrumbTitle"]; }
            set { }
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        [ConfigurationProperty("title", IsRequired = false, IsKey = false, DefaultValue = "")]
        public string Title
        {
            get { return (string)base["title"]; }
            set { }
        }

        /// <summary>
        /// Gets the child nodes.
        /// </summary>
        /// <value>The child nodes.</value>
        public List<ISiteMapMenuItem> ChildNodes
        {
            get
            {
                if (_SiteMapMenuItems == null)
                {
                    _SiteMapMenuItems = new List<ISiteMapMenuItem>();
                    foreach (SiteMapMenuItemElement item in SiteMapMenuItemElementCollection)
                    {
                        item.ParentNode = this;
                        _SiteMapMenuItems.Add((ISiteMapMenuItem)item);
                    }
                }
                return _SiteMapMenuItems;
            }
            set { }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is the root node.
        /// </summary>
        /// <value><c>true</c> if this instance is root; otherwise, <c>false</c>.</value>
        public bool IsRoot
        {
            get { return this.ParentNode == null; }
            set { }
        }

        private ISiteMapMenuItem _ParentNode;
        /// <summary>
        /// Gets the parent node.
        /// </summary>
        /// <value>The parent node.</value>
        public ISiteMapMenuItem ParentNode
        {
            get { return _ParentNode; }
            set { this._ParentNode = value; }
        }
        /// <summary>
        /// Gets the parent node.
        /// </summary>
        /// <value>The parent node.</value>
        public IUrlRewriteItem UrlRewriteItem
        {
            get { return null; }
            set { }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapMenuItemElement"/> class.
        /// </summary>
        public SiteMapMenuItemElement() : this(null) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapMenuItemElement"/> class.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        public SiteMapMenuItemElement(ISiteMapMenuItem parentNode)
        {
            this.ParentNode = parentNode;
        }
        #endregion
    }
    #endregion
}