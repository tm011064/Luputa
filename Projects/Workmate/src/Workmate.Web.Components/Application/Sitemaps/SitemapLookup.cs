using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Web.Routing;
using System.Web.Mvc;
using System.Configuration;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Web.Components.Application.Sitemaps
{
  public class SitemapLookup : ISitemapLookup
  {
    #region members
    Dictionary<SitemapItemType, Dictionary<string, SitemapItem>> _Lookup;
    Dictionary<string, RouteTag> _RouteTags;
    Dictionary<string, Menu> _Menus;
    #endregion

    #region properties
    public string DefaultTheme { get; private set; }
    #endregion

    #region private methods
    private MenuItem BuildDropdownMenuItem(XElement xElement, MenuItem parent, Dictionary<string, RouteTag> routeTags)
    {
      MenuItem menuItem = new MenuItem();
      menuItem.ShowInNavPills = true;
      foreach (XAttribute attribute in xElement.Attributes())
      {
        switch (attribute.Name.LocalName)
        {
          case "name": menuItem.Name = attribute.Value; break;

          case "routeName":
            menuItem.RouteName = attribute.Value;
            if (routeTags.ContainsKey(menuItem.RouteName))
            {
              menuItem.Action = routeTags[menuItem.RouteName].Defaults["action"].ToString();
              menuItem.Controller = routeTags[menuItem.RouteName].Defaults["controller"].ToString();
            }
            break;

          case "contentKey": menuItem.ContentKey = attribute.Value; break;
          case "licenseKey": menuItem.LicenseKey = attribute.Value; break;
          case "showInNavPills": menuItem.ShowInNavPills = bool.Parse(attribute.Value); break;

          case "allowedRoles":
            foreach (string role in attribute.Value.Split(','))
            {
              menuItem.AllowedRoles.Add(role.Trim());
            }
            menuItem.HasUserRoleRestrictions = menuItem.AllowedRoles.Count > 0;
            break;
        }
      }
      menuItem.Level = parent == null ? 0 : (parent.Level + 1);
      menuItem.Parent = parent;
      menuItem.HasDropdownMenuItems = true;

      foreach (XElement element in xElement.Elements("dropdownMenuItem"))
        menuItem.DropdownMenuItems.Add(BuildDropdownMenuItem(element, menuItem, routeTags));

      foreach (XElement element in xElement.Elements("menuItem"))
        menuItem.Children.Add(BuildMenuItem(element, menuItem, routeTags));

      return menuItem;
    }

    private MenuItem BuildMenuItem(XElement xElement, MenuItem parent, Dictionary<string, RouteTag> routeTags)
    {
      MenuItem menuItem = new MenuItem();
      menuItem.ShowInNavPills = true;
      foreach (XAttribute attribute in xElement.Attributes())
      {
        switch (attribute.Name.LocalName)
        {
          case "name": menuItem.Name = attribute.Value; break;

          case "routeName":
            menuItem.RouteName = attribute.Value;
            if (routeTags.ContainsKey(menuItem.RouteName))
            {
              menuItem.Action = routeTags[menuItem.RouteName].Defaults["action"].ToString();
              menuItem.Controller = routeTags[menuItem.RouteName].Defaults["controller"].ToString();
            }
            break;

          case "contentKey": menuItem.ContentKey = attribute.Value; break;
          case "licenseKey": menuItem.LicenseKey = attribute.Value; break;
          case "showInNavPills": menuItem.ShowInNavPills = bool.Parse(attribute.Value); break;

          case "allowedRoles":
            foreach (string role in attribute.Value.Split(','))
            {
              menuItem.AllowedRoles.Add(role.Trim());
            }
            menuItem.HasUserRoleRestrictions = menuItem.AllowedRoles.Count > 0;
            break;
        }
      }
      menuItem.Level = parent == null ? 0 : (parent.Level + 1);
      menuItem.Parent = parent;
      menuItem.HasDropdownMenuItems = false;

      foreach (XElement element in xElement.Elements("menuItem"))
        menuItem.Children.Add(BuildMenuItem(element, menuItem, routeTags));

      return menuItem;
    }
    #endregion

    #region public methods
    public RouteTag[] RouteTags
    {
      get { return _RouteTags.Values.ToArray(); }
    }
    public RouteTag GetRouteTag(string name)
    {
      if (_RouteTags.ContainsKey(name))
        return _RouteTags[name];

      return null;
    }

    public List<SitemapItem> GetSitemapItems(SitemapItemType sitemapItemType)
    {
      return new List<SitemapItem>(_Lookup[sitemapItemType].Values.OrderBy(c => c.Index));
    }

    public Dictionary<string, string> GetDnsLookup()
    {
      Dictionary<string, string> dict = new Dictionary<string, string>();

      foreach (SitemapItem sitemapItem in _Lookup[SitemapItemType.DNS].Values)
        foreach (string domainName in sitemapItem.DomainNames)
          dict[domainName] = sitemapItem.Name;

      return dict;
    }

    public Dictionary<string, SitemapItem> GetDnsSitemapItems()
    {
      Dictionary<string, SitemapItem> dict = new Dictionary<string, SitemapItem>();

      foreach (SitemapItem sitemapItem in _Lookup[SitemapItemType.DNS].Values)
        foreach (string domainName in sitemapItem.DomainNames)
          dict[domainName.ToLowerInvariant()] = sitemapItem;

      return dict;
    }

    public Menu GetMenu(string name)
    {
      if (_Menus.ContainsKey(name))
        return _Menus[name];

      return null;
    }

    public SitemapItem GetSitemapItem(SitemapItemType sitemapItemType, string name)
    {
      if (_Lookup[sitemapItemType].ContainsKey(name))
        return _Lookup[sitemapItemType][name];

      return null;
    }

    public void Initialize(string xmlFilePath)
    {
      if (!File.Exists(xmlFilePath))
        throw new ApplicationException("Sitemap config path " + xmlFilePath + "doesn't exist");

      Dictionary<SitemapItemType, Dictionary<string, SitemapItem>> lookup = new Dictionary<SitemapItemType, Dictionary<string, SitemapItem>>();
      foreach (string name in Enum.GetNames(typeof(SitemapItemType)))
        lookup[(SitemapItemType)Enum.Parse(typeof(SitemapItemType), name)] = new Dictionary<string, SitemapItem>();

      XDocument document = XDocument.Load(xmlFilePath);

      SitemapItem sitemapItem;
      int count = 0;
      string defaultTheme = null;

      #region items
      foreach (XElement xElement in document.Descendants("item"))
      {
        sitemapItem = new SitemapItem();
        sitemapItem.Index = count++;

        foreach (XAttribute attribute in xElement.Attributes())
        {
          switch (attribute.Name.LocalName)
          {
            case "type":
              switch (attribute.Value)
              {
                case "dns": sitemapItem.SitemapItemType = SitemapItemType.DNS; break;
                case "css": sitemapItem.SitemapItemType = SitemapItemType.CSS; break;
                case "js": sitemapItem.SitemapItemType = SitemapItemType.Javascript; break;
              }
              break;

            case "name": sitemapItem.Name = attribute.Value; break;
            case "path": sitemapItem.Path = attribute.Value; break;
            case "url": sitemapItem.Url = attribute.Value; break;
            case "value": sitemapItem.Value = attribute.Value; break;
            case "applicationName": sitemapItem.ApplicationName = attribute.Value; break;
            case "applicationGroup": sitemapItem.ApplicationGroup = attribute.Value; break;
            case "imageFolderServerPath": sitemapItem.ImageFolderServerPath = attribute.Value; break;
            case "imageFolderRootUrl": sitemapItem.ImageFolderRootUrl = attribute.Value; break;

            case "domains": sitemapItem.DomainNames = new HashSet<string>(from c in attribute.Value.Split(';')
                                                                          select c.Trim().ToLowerInvariant()); break;

            case "isDefault":
              bool isDefault = false;
              bool.TryParse(attribute.Value, out isDefault);
              sitemapItem.IsDefault = isDefault;
              break;

            case "minified":
              bool minified = false;
              bool.TryParse(attribute.Value, out minified);
              sitemapItem.Minified = minified;
              break;
          }
        }
        if (sitemapItem.SitemapItemType != SitemapItemType.Undefined
            && !string.IsNullOrWhiteSpace(sitemapItem.Name))
        {
          sitemapItem.FormattedPath = sitemapItem.Path;

          if (sitemapItem.Minified == true
              && !string.IsNullOrWhiteSpace(sitemapItem.Path))
          {
            int index = sitemapItem.Path.LastIndexOf('.');
            if (index >= 0)
              sitemapItem.FormattedPath = sitemapItem.Path.Insert(index, ".min");
          }

          if (sitemapItem.SitemapItemType == SitemapItemType.DNS
              && sitemapItem.IsDefault == true)
          {
            defaultTheme = sitemapItem.Name;
          }

          lookup[sitemapItem.SitemapItemType][sitemapItem.Name] = sitemapItem;
        }
      }

      if (string.IsNullOrWhiteSpace(defaultTheme))
      {
        if (lookup[SitemapItemType.DNS].Count == 1)
          defaultTheme = lookup[SitemapItemType.DNS].Values.First().Name;
        else
          throw new ConfigurationErrorsException("A default theme must be set");
      }

      DefaultTheme = defaultTheme;
      _Lookup = lookup;
      #endregion

      #region routes
      Dictionary<string, RouteTag> routeTags = new Dictionary<string, RouteTag>();
      RouteTag routeTag;
      foreach (XElement xElement in document.Descendants("route"))
      {
        routeTag = new RouteTag();

        foreach (XAttribute attribute in xElement.Attributes())
        {
          switch (attribute.Name.LocalName)
          {
            case "name": routeTag.Name = attribute.Value; break;
            case "url": routeTag.Url = attribute.Value; break;
            case "routeHandlerType": routeTag.RouteHandlerType = attribute.Value; break;
            case "topMenuName": routeTag.TopMenuName = attribute.Value; break;
            case "breadcrumbContentKey": routeTag.BreadcrumbContentKey = attribute.Value; break;
            case "breadcrumbParent": routeTag.BreadcrumbParent = attribute.Value; break;
          }
        }
        if (!string.IsNullOrWhiteSpace(routeTag.RouteHandlerType))
          routeTag.RouteHandler = Activator.CreateInstance(Type.GetType(routeTag.RouteHandlerType)) as IRouteHandler;
        else
          routeTag.RouteHandler = new MvcRouteHandler();

        if (string.IsNullOrWhiteSpace(routeTag.TopMenuName))
          routeTag.TopMenuName = "Top"; // default

        foreach (XAttribute attribute in (from c in xElement.Elements("defaults")
                                          from d in c.Attributes()
                                          select d))
        {
          routeTag.Defaults[attribute.Name.LocalName] = attribute.Value;
        }
        foreach (XAttribute attribute in (from c in xElement.Elements("dataTokens")
                                          from d in c.Attributes()
                                          select d))
        {
          routeTag.DataTokens[attribute.Name.LocalName] = attribute.Value;
        }

        foreach (XElement constraint in xElement.Elements("constraint"))
        {
          XAttribute typeAttribute = constraint.Attribute("type");
          XAttribute nameAttribute = constraint.Attribute("name");

          if (typeAttribute != null && nameAttribute != null)
          {
            List<string> parameters = new List<string>();
            foreach (XAttribute attribute in (from c in constraint.Elements("parameter")
                                              from d in c.Attributes()
                                              select d))
            {
              switch (attribute.Name.LocalName)
              {
                case "value": parameters.Add(attribute.Value); break;
              }
            }

            if (parameters.Count == 0)
            {
              routeTag.Constraints.Add(nameAttribute.Value, (IRouteConstraint)Activator.CreateInstance(
                Type.GetType(typeAttribute.Value)
                , parameters.ToArray()));
            }
            else
            {
              routeTag.Constraints.Add(nameAttribute.Value, (IRouteConstraint)Activator.CreateInstance(
                Type.GetType(typeAttribute.Value)));
            }
          }
        }
        routeTags.Add(routeTag.Name, routeTag);
      }

      foreach (RouteTag record in routeTags.Values)
      {
        if (!string.IsNullOrWhiteSpace(record.BreadcrumbParent)
            && routeTags.ContainsKey(record.BreadcrumbParent))
        {
          record.BreadcrumbParentRouteTag = routeTags[record.BreadcrumbParent];
        }
      }

      _RouteTags = routeTags;
      #endregion

      #region menus
      Dictionary<string, Menu> menus = new Dictionary<string, Menu>();
      Menu menu;
      foreach (XElement xElement in document.Descendants("menu"))
      {
        menu = new Menu();
        foreach (XAttribute attribute in xElement.Attributes())
        {
          switch (attribute.Name.LocalName)
          {
            case "name": menu.Name = attribute.Value; break;
          }
        }

        foreach (XElement element in xElement.Elements())
        {
          if (element.Name == "menuItem")
            menu.AddMenuItem(BuildMenuItem(element, null, routeTags));
          else if (element.Name == "dropdownMenuItem")
            menu.AddMenuItem(BuildDropdownMenuItem(element, null, routeTags));
        }

        menus[menu.Name] = menu;
      }

      _Menus = menus;
      #endregion
    }
    #endregion

    #region constructors
    public SitemapLookup()
    {
      _Lookup = new Dictionary<SitemapItemType, Dictionary<string, SitemapItem>>();
      _RouteTags = new Dictionary<string, RouteTag>();
    }
    #endregion
  }
}
