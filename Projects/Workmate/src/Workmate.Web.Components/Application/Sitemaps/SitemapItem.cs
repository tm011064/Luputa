using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Web.Components.Application.Sitemaps
{
  public class SitemapItem
  {
    public SitemapItemType SitemapItemType { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string Url { get; set; }

    public HashSet<string> DomainNames { get; set; }

    public string Value { get; set; }
    public string FormattedPath { get; set; }
    public bool Minified { get; set; }
    public bool IsDefault { get; set; }
    public int Index { get; set; }
    public string ApplicationName { get; set; }
    public string ApplicationGroup { get; set; }

    public string ImageFolderServerPath { get; set; }
    public string ImageFolderRootUrl { get; set; }
    public bool HasImageFolderServerPath { get { return !string.IsNullOrWhiteSpace(this.ImageFolderServerPath); } }
    public bool HasImageFolderRootUrl { get { return !string.IsNullOrWhiteSpace(this.ImageFolderRootUrl); } }

    public SitemapItem()
    {
      this.SitemapItemType = SitemapItemType.Undefined;
      this.DomainNames = new HashSet<string>();
    }    
  }
}
