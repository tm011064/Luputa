using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Web.Components.Application
{  
  public class ApplicationDataCache : IApplicationDataCache
  {
    #region members
    private Dictionary<string, List<IWikiLanguageThreadLookupItem>> _WikiLanguageThreadIdLookup = new Dictionary<string, List<IWikiLanguageThreadLookupItem>>();
    #endregion

    #region static content
    public List<IWikiLanguageThreadLookupItem> GetWikiLanguageThreadIdLookup(string applicationName)
    {
      if (_WikiLanguageThreadIdLookup.ContainsKey(applicationName.ToLowerInvariant()))
        return _WikiLanguageThreadIdLookup[applicationName.ToLowerInvariant()];

      return new List<IWikiLanguageThreadLookupItem>();
    }
    #endregion

    #region dynamic content

    #endregion

    #region public methods
    /// <summary>
    /// Refreshes the specified wiki language thread id lookup.
    /// </summary>
    /// <param name="wikiLanguageThreadIdLookup">The wiki language thread id lookup. This is static throughout the application life cycle and
    /// needs config input to build so we have to pass it in via the refresh method</param>
    public void Refresh(Dictionary<string, List<IWikiLanguageThreadLookupItem>> wikiLanguageThreadIdLookup)
    {
      _WikiLanguageThreadIdLookup = wikiLanguageThreadIdLookup;
    }
    #endregion

    #region constructors
    public ApplicationDataCache()
    {

    }
    #endregion
  }
}
