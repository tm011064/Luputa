using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Web.Components.Application.Sitemaps;
using Workmate.Messaging;
using Workmate.Configuration;
using Workmate.Components.Contracts.CMS.Articles;

namespace Workmate.Web.Components.Application
{
  public interface IApplicationContext : IDisposable
  {
    void Refresh(string applicationRoutePath);

    IThemeFolderLookup GetThemeFolderLookup(string applicationGroup);
    ISitemapLookup GetSitemapLookup(string applicationGroup);
    IStaticContentLookup GetStaticContentLookup(string applicationGroup);

    IApplicationThemeInfo GetApplicationThemeInfoFromDomainName(string domainName);
    IApplicationThemeInfo[] GetAllApplicationThemeInfos();
    string[] GetApplicationGroups();

    IApplicationMessageHandler ApplicationMessageHandler { get; }
    string BaseDirectory { get; }

    IApplicationSettings ApplicationSettings { get; }
  }
}
