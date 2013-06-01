using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Web.Components.Application.Sitemaps;

namespace Workmate.Web.Components.Application
{
  public interface ISitemapLookup
  {
    RouteTag[] RouteTags { get; }
    RouteTag GetRouteTag(string name);

    SitemapItem GetSitemapItem(SitemapItemType sitemapItemType, string name);
    Menu GetMenu(string name);
  }
}
