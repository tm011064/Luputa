using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Reflection;
using System.Web.Caching;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// This class can act as an abstract base class for a SiteMapMenu Controller.
    /// </summary>
    /// <typeparam name="TISiteMapMenu">The type of the IISiteMapMenu to create</typeparam>
    /// <typeparam name="TISiteMapMenuItem">The type of the ISiteMapMenuItem to create</typeparam>
    public abstract class GenericSiteMapMenuController<TISiteMapMenu, TISiteMapMenuItem> : ISiteMapMenuController, IXmlSerializable
        where TISiteMapMenu : ISiteMapMenu, new()
        where TISiteMapMenuItem : ISiteMapMenuItem, new()
    {
        #region members
        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<string, ISiteMapMenu> _SiteMapMenus = new Dictionary<string, ISiteMapMenu>();
        #endregion

        #region ISiteMapMenuController Members

        /// <summary>
        /// Creates an instance of this object's default controller.
        /// </summary>
        /// <param name="urlRewriteController">The URL rewrite controller.</param>
        /// <returns></returns>
        public abstract ISiteMapMenuController CreateSiteMapMenuControllerInstance(IUrlRewriteController urlRewriteController);
        
        /// <summary>
        /// Gets the site map menus. Format: Key -&gt; name of the menu, Value -&gt; the menu
        /// </summary>
        /// <value>The site map menus.</value>
        public virtual Dictionary<string, ISiteMapMenu> SiteMapMenus
        {
            get { return this._SiteMapMenus; }
            set { this._SiteMapMenus = value; }
        }

        /// <summary>
        /// Gets an ISiteMapMenuItem from the ISiteMapMenuItem collection by name
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns></returns>
        public virtual ISiteMapMenu GetSiteMapMenu(string name)
        {
            if (this.SiteMapMenus.ContainsKey(name))
                return this.SiteMapMenus[name];

            return null;
        }

        /// <summary>
        /// Gets the URL rewrite controller.
        /// </summary>
        /// <value>The URL rewrite controller.</value>
        public virtual IUrlRewriteController UrlRewriteController { get; set; }

        #endregion

        #region public methods
        /// <summary>
        /// Gets the matching site map menu item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public ISiteMapMenuItem GetMatchingSiteMapMenuItem(ISiteMapMenuItem item, string name)
        {
            if (item.Name == name)
                return item;
            else
            {
                foreach (ISiteMapMenuItem child in item.ChildNodes)
                    return GetMatchingSiteMapMenuItem(child, name);
            }
            return null;
        }
        #endregion

        #region protected methods
        /// <summary>
        /// Loads the rewrite items.
        /// </summary>
        /// <param name="item">The item.</param>
        protected void LoadRewriteItems(ISiteMapMenuItem item)
        {
            item.UrlRewriteItem = UrlRewriteController.GetUrlRewriteItem(item.UrlRewriteItemName);
            foreach (ISiteMapMenuItem child in item.ChildNodes)
                LoadRewriteItems(child);
        }

        /// <summary>
        /// Gets generic site map menus from a given XmlReader.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="items">The items.</param>
        /// <param name="menuNode">The menu node.</param>
        /// <returns></returns>
        protected ISiteMapMenuItem GetMenuItemsRecursive(ISiteMapMenuItem parent, List<ISiteMapMenuItem> items, XElement menuNode)
        {
            TISiteMapMenuItem item = new TISiteMapMenuItem();

            #region get properties
            object[] attributes;
            object value;
            SiteMapMenuPropertyAttribute propertyAttribute;
            PropertyInfo[] propertyInfos = item.GetType().GetProperties();
            if (propertyInfos != null)
            {
                foreach (PropertyInfo info in propertyInfos)
                {
                    attributes = (object[])info.GetCustomAttributes(typeof(SiteMapMenuPropertyAttribute), false);
                    if (attributes.Length > 0)
                    {// attribute exists
                        propertyAttribute = ((SiteMapMenuPropertyAttribute)attributes[0]);

                        if (menuNode.Attribute(propertyAttribute.PropertyName) != null)
                        {
                            if (info.PropertyType != Type.GetType("System.String")
                                && string.IsNullOrEmpty(menuNode.Attribute(propertyAttribute.PropertyName).Value))
                                continue;

                            // first try to convert the value of the menuNode to the correct type
                            try { value = Convert.ChangeType(menuNode.Attribute(propertyAttribute.PropertyName).Value, info.PropertyType); }
                            catch (Exception err)
                            {
                                throw new NavigationException("Error at converting SiteMapMenu property value " + menuNode.Attribute(propertyAttribute.PropertyName).Value + " to type "
                                    + info.PropertyType.ToString() + ". See inner exception for further details.", err);
                            }

                            // now set the value of the object
                            try { info.SetValue(item, value, null); }
                            catch (Exception err)
                            {
                                throw new NavigationException("Error at setting property value " + menuNode.Attribute(propertyAttribute.PropertyName).Value + " for property "
                                    + info.Name + ". See inner exception for further details.", err);
                            }
                        }
                    }
                }
            }
            #endregion

            if (menuNode.Attribute("name") != null)
                item.Name = menuNode.Attribute("name").Value;
            if (menuNode.Attribute("urlRewriteItemName") != null)
                item.UrlRewriteItemName = menuNode.Attribute("urlRewriteItemName").Value;
            if (menuNode.Attribute("breadcrumbTitle") != null)
                item.BreadcrumbTitle = menuNode.Attribute("breadcrumbTitle").Value;
            if (menuNode.Attribute("title") != null)
                item.Title = menuNode.Attribute("title").Value;

            item.ParentNode = parent;

            foreach (XElement element in menuNode.Elements("menunode"))
                item.ChildNodes.Add(GetMenuItemsRecursive(item, items, element));

            return item;
        }

        /// <summary>
        /// Gets generic site map menus from a given XmlReader. The specified types must have an empty default constructor in order
        /// to make this method work.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected Dictionary<string, ISiteMapMenu> GetGenericSiteMapMenusFromXml(XmlReader reader)
        {
            XElement el = XElement.Load(reader);

            Dictionary<string, ISiteMapMenu> siteMapMenus = new Dictionary<string, ISiteMapMenu>();

            ISiteMapMenu menu;
            foreach (XElement element in el.Elements("menu"))
            {
                menu = Activator.CreateInstance(typeof(TISiteMapMenu)) as ISiteMapMenu;
                foreach (XElement menuNodeElement in element.Elements("menunode"))
                    menu.MenuNodes.Add(GetMenuItemsRecursive(null, new List<ISiteMapMenuItem>(), menuNodeElement));

                if (element.Attribute("name") != null)
                    menu.Name = element.Attribute("name").Value;
                if (element.Attribute("enableCaching") != null)
                    menu.EnableCaching = bool.Parse(element.Attribute("enableCaching").Value);
                if (element.Attribute("cacheDurationInSeconds") != null)
                    menu.CacheDurationInSeconds = int.Parse(element.Attribute("cacheDurationInSeconds").Value);
                if (element.Attribute("cacheItemPriority") != null)
                    menu.CacheItemPriority = (CacheItemPriority)Enum.Parse(typeof(CacheItemPriority), element.Attribute("cacheItemPriority").Value);

                siteMapMenus.Add(menu.Name, menu);
            }
            return siteMapMenus;
        }
        #endregion

        #region IXmlSerializable Members

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public virtual XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public virtual void ReadXml(XmlReader reader)
        {
            this._SiteMapMenus = GetGenericSiteMapMenusFromXml(reader);
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
