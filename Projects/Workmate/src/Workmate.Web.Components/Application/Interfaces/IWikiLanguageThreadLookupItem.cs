using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Web.Components.Application.Sitemaps;

namespace Workmate.Web.Components.Application
{
  public interface IWikiLanguageThreadLookupItem
  {
    string ShortCode { get; }
    string LanguageName { get; }
    int ArticleGroupThreadId { get; }
  }
}
