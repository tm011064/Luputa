using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Web.Components.Application;
using Workmate.Web.Components.Application.Sitemaps;

namespace Workmate.Web.Components
{
  public class RequestContextData
  {
    public SortedDictionary<int, SitemapItem> CssIncludes { get; set; }
    public SortedDictionary<int, SitemapItem> JavascriptIncludes { get; set; }
    public IThemeFolderLookup ThemeFolderLookup { get; set; }
    public ISitemapLookup SitemapLookup { get; set; }
    public IStaticContentLookup StaticContentLookup { get; set; }
    public MenuInfo MenuInfo { get; set; }
    public Breadcrumb BreadCrumb { get; set; }

    public IApplicationThemeInfo ApplicationThemeInfo { get; set; }
    public string Theme { get { return this.ApplicationThemeInfo.ThemeName; } }

    private StringBuilder _JavascriptMarkup = new StringBuilder();
    public StringBuilder JavascriptMarkup { get { return _JavascriptMarkup; } }

    public RequestContextData()
    {
      this.CssIncludes = new SortedDictionary<int, SitemapItem>();
      this.JavascriptIncludes = new SortedDictionary<int, SitemapItem>();
    }
  }
}
