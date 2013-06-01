using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Workmate.Web.Components.Application.Sitemaps
{
  public class RouteTag
  {
    public string Name { get; set; }
    public string Url { get; set; }
    public string RouteHandlerType { get; set; }
    public RouteValueDictionary Constraints { get; set; }
    public RouteValueDictionary Defaults { get; set; }
    public RouteValueDictionary DataTokens { get; set; }
    public IRouteHandler RouteHandler { get; set; }
    
    public string TopMenuName { get; set; }
    public string BreadcrumbContentKey { get; set; }
    public RouteTag BreadcrumbParentRouteTag { get; set; }
    public string BreadcrumbParent { get; set; }

    public RouteTag()
    {
      this.Constraints = new RouteValueDictionary();
      this.Defaults = new RouteValueDictionary();
      this.DataTokens = new RouteValueDictionary();
    }
  }
}
