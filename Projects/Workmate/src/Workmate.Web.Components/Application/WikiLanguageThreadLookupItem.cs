using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Web.Components.Application
{
  public class WikiLanguageThreadLookupItem : IWikiLanguageThreadLookupItem
  {
    public string ShortCode { get; set; }
    public string LanguageName { get; set; }
    public int ArticleGroupThreadId { get; set; }

    public WikiLanguageThreadLookupItem(string shortCode, string languageName, int articleGroupThreadId)
    {
      this.ShortCode = shortCode;
      this.LanguageName = languageName;
      this.ArticleGroupThreadId = articleGroupThreadId;
    }
  }
}
