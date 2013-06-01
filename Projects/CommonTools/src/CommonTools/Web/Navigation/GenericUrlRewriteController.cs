using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.Web.Caching;
using System.Reflection;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TIUrlRewriteItem">The type of the I URL rewrite item.</typeparam>
    /// <typeparam name="TISiteMapMenuController">The type of the I site map menu controller.</typeparam>
    public abstract class GenericUrlRewriteController<TIUrlRewriteItem, TISiteMapMenuController> : IUrlRewriteController, IXmlSerializable
        where TIUrlRewriteItem : IUrlRewriteItem, new()
        where TISiteMapMenuController : ISiteMapMenuController, new()
    {
        #region members
        /// <summary>
        /// 
        /// </summary>
        protected List<IUrlRewriteItem> _IUrlRewriteItems = new List<IUrlRewriteItem>();
        #endregion

        #region properties
        /// <summary>
        /// 
        /// </summary>
        protected ISiteMapMenuController _SiteMapMenuController = null;
        /// <summary>
        /// Gets the site map menu controller.
        /// </summary>
        /// <value>The site map menu controller.</value>
        protected virtual ISiteMapMenuController SiteMapMenuController
        {
            get
            {
                if (_SiteMapMenuController == null)
                    _SiteMapMenuController = ((ISiteMapMenuController)Activator.CreateInstance(typeof(TISiteMapMenuController))).CreateSiteMapMenuControllerInstance(this);

                return _SiteMapMenuController;
            }
        }

        /// <summary>
        /// Gets the site map menus.
        /// </summary>
        /// <value>The site map menus.</value>
        public virtual Dictionary<string, ISiteMapMenu> SiteMapMenus
        {
            get { return this.SiteMapMenuController.SiteMapMenus; }
        }

        private ISiteMapUrls _SiteMapUrls;
        /// <summary>
        /// Gets the site map urls.
        /// </summary>
        /// <value>The site map urls.</value>
        private ISiteMapUrls SiteMapUrls
        {
            get
            {
                if (this._SiteMapUrls == null)
                {
                    if (this.UseCachedSitemapUrls)
                        this._SiteMapUrls = CachedSiteMapUrls.Instance();
                    else
                        this._SiteMapUrls = Navigation.SiteMapUrls.Instance(this);
                }
                return _SiteMapUrls;
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Gets the matching rewrite item.
        /// </summary>
        /// <param name="pathAndQuery">The path and query.</param>
        /// <returns></returns>
        public IUrlRewriteItem GetMatchingRewriteItem(string pathAndQuery)
        {
            return this.SiteMapUrls.GetMatchingRewriteItem(pathAndQuery);
        }

        /// <summary>
        /// Gets the matching site map menu item.
        /// </summary>
        /// <param name="menuName">Name of the menu.</param>
        /// <param name="pathAndQuery">The path and query.</param>
        /// <returns></returns>
        public ISiteMapMenuItem GetMatchingSiteMapMenuItem(string menuName, string pathAndQuery)
        {
            return GetMatchingSiteMapMenuItem(menuName, pathAndQuery, false);
        }

        private object _ISiteMapMenuItemLock = new object();
        /// <summary>
        /// Gets the matching site map menu item.
        /// </summary>
        /// <param name="menuName">Name of the menu.</param>
        /// <param name="pathAndQuery">The path and query.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <returns></returns>
        public ISiteMapMenuItem GetMatchingSiteMapMenuItem(string menuName, string pathAndQuery, bool useCache)
        {
            if (!this.SiteMapMenus.ContainsKey(menuName))
                throw new NavigationException("Menu " + menuName + " not found at the SiteMapMenu section.");

            ISiteMapMenu menu = this.SiteMapMenus[menuName];
            IUrlRewriteItem rewriteItem;
            if (useCache && menu.EnableCaching)
            {// we want to get the menu item from cache

                // define cachekey
                string cacheKey = "__" + menuName + "_" + pathAndQuery;
                lock (_ISiteMapMenuItemLock)
                {
                    ISiteMapMenuItem item = HttpRuntime.Cache[cacheKey] as ISiteMapMenuItem;
                    if (item != null)
                    {// item found at cache collection, so return it
                        return item;
                    }
                    else
                    {// item not found at cache collection, so find/insert it
                        rewriteItem = this.SiteMapUrls.GetMatchingRewriteItem(pathAndQuery);
                        if (rewriteItem != null)
                            item = menu.GetMatchingSiteMapMenuItem(rewriteItem.Name);

                        if (item != null)
                        {
                            HttpRuntime.Cache.Remove(cacheKey);
                            HttpRuntime.Cache.Insert(cacheKey
                                                     , item
                                                     , null
                                                     , DateTime.Now.AddSeconds(menu.CacheDurationInSeconds)
                                                     , Cache.NoSlidingExpiration
                                                     , menu.CacheItemPriority
                                                     , null);
                            return item;
                        }
                    }
                }
            }

            rewriteItem = this.SiteMapUrls.GetMatchingRewriteItem(pathAndQuery);
            if (rewriteItem != null)
                return menu.GetMatchingSiteMapMenuItem(rewriteItem.Name);

            return null;
        }
        #endregion

        #region IUrlRewriteController Members
        /// <summary>
        /// Creates an instance of this object's default controller.
        /// </summary>
        /// <returns></returns>
        public abstract IUrlRewriteController CreateUrlRewriteControllerInstance();

        /// <summary>
        /// Gets the IUrlRewriteItem collection associated with this IUrlRewriteController.
        /// </summary>
        /// <value>The UrlRewrite items.</value>
        public virtual List<IUrlRewriteItem> UrlRewriteItems
        {
            get { return this._IUrlRewriteItems; }
            set { this._IUrlRewriteItems = value; }
        }

        /// <summary>
        /// Gets an IUrlRewriteItem from the IUrlRewriteItem collection by name
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns></returns>
        public virtual IUrlRewriteItem GetUrlRewriteItem(string name)
        {
            foreach (IUrlRewriteItem item in UrlRewriteItems)
                if (item.Name == name)
                    return item;

            return null;
        }

        /// <summary>
        /// Gets the sitemap urls cache duration in seconds.
        /// </summary>
        /// <value>The sitemap urls cache duration in seconds.</value>
        public abstract int SitemapUrlsCacheDurationInSeconds { get; protected set; }
        /// <summary>
        /// Gets the sitemap urls cache key.
        /// </summary>
        /// <value>The sitemap urls cache key.</value>
        public abstract string SitemapUrlsCacheKey { get; protected set; }
        /// <summary>
        /// Gets the sitemap urls cache item priority.
        /// </summary>
        /// <value>The sitemap urls cache item priority.</value>
        public abstract System.Web.Caching.CacheItemPriority SitemapUrlsCacheItemPriority { get; protected set; }
        /// <summary>
        /// Gets or sets a value indicating whether to cache the sitemapurls or not.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [use cached sitemap urls]; otherwise, <c>false</c>.
        /// </value>
        protected abstract bool UseCachedSitemapUrls { get; set; }

        #endregion

        #region protected methods
        /// <summary>
        /// Gets the generic rewrite items from XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<IUrlRewriteItem> GetGenericRewriteItemsFromXml(XmlReader reader)
        {
            XElement el = XElement.Load(reader);

            List<IUrlRewriteItem> rewriteItems = new List<IUrlRewriteItem>();
            IUrlRewriteItem item;
            foreach (XElement element in el.Elements("page"))
            {
                item = Activator.CreateInstance(typeof(TIUrlRewriteItem)) as IUrlRewriteItem;

                if (element.Attribute("name") != null)
                    item.Name = element.Attribute("name").Value;
                if (element.Attribute("directory") != null)
                    item.Url = element.Attribute("directory").Value;
                if (element.Attribute("filepath") != null)
                    item.Path = element.Attribute("filepath").Value;
                if (element.Attribute("pattern") != null)
                    item.Pattern = element.Attribute("pattern").Value;
                if (element.Attribute("vanity") != null)
                    item.Vanity = element.Attribute("vanity").Value;
                if (element.Attribute("parentName") != null)
                    item.ParentName = element.Attribute("parentName").Value;
                if (element.Attribute("breadcrumbTitle") != null)
                    item.BreadcrumbTitle = element.Attribute("breadcrumbTitle").Value;
                if (element.Attribute("title") != null)
                    item.Title = element.Attribute("title").Value;
                if (element.Attribute("isHttps") != null)
                    item.IsHttps = bool.Parse(element.Attribute("isHttps").Value);

                #region get properties
                object[] attributes;
                object value;
                SiteMapItemPropertyAttribute propertyAttribute;
                PropertyInfo[] propertyInfos = item.GetType().GetProperties();
                if (propertyInfos != null)
                {
                    foreach (PropertyInfo info in propertyInfos)
                    {
                        attributes = (object[])info.GetCustomAttributes(typeof(SiteMapItemPropertyAttribute), false);
                        if (attributes.Length > 0)
                        {// attribute exists
                            propertyAttribute = ((SiteMapItemPropertyAttribute)attributes[0]);

                            if (element.Attribute(propertyAttribute.PropertyName) != null)
                            {
                                if (info.PropertyType != Type.GetType("System.String")
                                    && string.IsNullOrEmpty(element.Attribute(propertyAttribute.PropertyName).Value))
                                    continue;

                                // first try to convert the value of the menuNode to the correct type
                                try { value = Convert.ChangeType(element.Attribute(propertyAttribute.PropertyName).Value, info.PropertyType); }
                                catch (Exception err)
                                {
                                    throw new NavigationException("Error at converting SiteMapMenu property value " + element.Attribute(propertyAttribute.PropertyName).Value + " to type "
                                        + info.PropertyType.ToString() + ". See inner exception for further details.", err);
                                }

                                // now set the value of the object
                                try { info.SetValue(item, value, null); }
                                catch (Exception err)
                                {
                                    throw new NavigationException("Error at setting property value " + element.Attribute(propertyAttribute.PropertyName).Value + " for property "
                                        + info.Name + ". See inner exception for further details.", err);
                                }
                            }
                        }
                    }
                }
                #endregion

                rewriteItems.Add(item);
            }

            return rewriteItems;
        }
        #endregion

        #region IXmlSerializable Members

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public virtual System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            _IUrlRewriteItems = GetGenericRewriteItemsFromXml(reader);
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public virtual void WriteXml(System.Xml.XmlWriter writer)
        {

        }

        #endregion
    }
}
