using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Web.Components.Application.Sitemaps;

namespace Workmate.Web.Components.Application
{
  public interface IStaticContentLookup
  {
    List<Language> GetLanguages(string theme);

    string GetContent(string theme, string key);
    string GetContent(string theme, string key, bool useDefaultThemeContentIfNotExists);
    string GetContent(string theme, string key, Dictionary<string, string> placeHolderReplacements);
    string GetContent(string theme, string key, bool useDefaultThemeContentIfNotExists, Dictionary<string, string> placeHolderReplacements);
    string GetContentFormat(string theme, string key, object arg1);
    string GetContentFormat(string theme, string key, object arg1, object arg2);
    string GetContentFormat(string theme, string key, object arg1, object arg2, object arg3);
    string GetContentFormat(string theme, string key, params object[] args);
    string GetContentFormat(string theme, string key, bool useDefaultThemeContentIfNotExists, object arg1);
    string GetContentFormat(string theme, string key, bool useDefaultThemeContentIfNotExists, object arg1, object arg2);
    string GetContentFormat(string theme, string key, bool useDefaultThemeContentIfNotExists, object arg1, object arg2, object arg3);
    string GetContentFormat(string theme, string key, bool useDefaultThemeContentIfNotExists, params object[] args);
  }
}
