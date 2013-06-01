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
  public interface IApplicationDataCache
  {
    List<IWikiLanguageThreadLookupItem> GetWikiLanguageThreadIdLookup(string applicationName);
    void Refresh(Dictionary<string, List<IWikiLanguageThreadLookupItem>> wikiLanguageThreadIdLookup);
  }
}
