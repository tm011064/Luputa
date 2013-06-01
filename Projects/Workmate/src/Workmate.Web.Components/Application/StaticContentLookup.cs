using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Web.Components.Application.Sitemaps;
using System.IO;
using System.Xml.Linq;
using System.Web;

namespace Workmate.Web.Components.Application
{
  public class StaticContentLookup : IStaticContentLookup
  {
    #region members
    private Dictionary<string, Dictionary<string, string>> _Content = new Dictionary<string, Dictionary<string, string>>();
    private Dictionary<string, List<Language>> _Languages = new Dictionary<string, List<Language>>();
    private Dictionary<string, List<Country>> _Countries = new Dictionary<string, List<Country>>();
    private string _DefaultTheme;
    #endregion

    #region public methods
    public List<Language> GetLanguages(string theme)
    {
      try
      {
        return new List<Language>(_Languages[theme]);
      }
      catch (KeyNotFoundException)
      {
        throw new ApplicationException("Theme " + theme + " does not exist");
      }
    }
    public List<Country> GetCountries(string theme)
    {
      try
      {
        return new List<Country>(_Countries[theme]);
      }
      catch (KeyNotFoundException)
      {
        throw new ApplicationException("Theme " + theme + " does not exist");
      }
    }

    public string GetContent(string theme, string key)
    {
      if (key == null)
        return string.Empty;

      try
      {
        if (_Content[theme].ContainsKey(key))
          return _Content[theme][key];

        return string.Empty;
      }
      catch (KeyNotFoundException)
      {
        throw new ApplicationException("Theme " + theme + " does not exist");
      }
    }
    public string GetContent(string theme, string key, bool useDefaultThemeContentIfNotExists)
    {
      if (key == null)
        return string.Empty;

      try
      {
        if (_Content[theme].ContainsKey(key))
          return _Content[theme][key];
        else if (_Content[_DefaultTheme].ContainsKey(key))
          return _Content[_DefaultTheme][key];

        return string.Empty;
      }
      catch (KeyNotFoundException)
      {
        throw new ApplicationException("Theme " + theme + " does not exist");
      }
    }
    public string GetContent(string theme, string key, Dictionary<string, string> placeHolderReplacements)
    {
      string content = GetContent(theme, key);
      foreach (KeyValuePair<string, string> kvp in placeHolderReplacements)
        content = content.Replace(kvp.Key, kvp.Value);
      return content;
    }
    public string GetContent(string theme, string key, bool useDefaultThemeContentIfNotExists, Dictionary<string, string> placeHolderReplacements)
    {
      string content = GetContent(theme, key, useDefaultThemeContentIfNotExists);
      foreach (KeyValuePair<string, string> kvp in placeHolderReplacements)
        content = content.Replace(kvp.Key, kvp.Value);
      return content;
    }
    public string GetContentFormat(string theme, string key, object arg1)
    {
      return string.Format(GetContent(theme, key), arg1);
    }
    public string GetContentFormat(string theme, string key, object arg1, object arg2)
    {
      return string.Format(GetContent(theme, key), arg1, arg2);
    }
    public string GetContentFormat(string theme, string key, object arg1, object arg2, object arg3)
    {
      return string.Format(GetContent(theme, key), arg1, arg2, arg3);
    }
    public string GetContentFormat(string theme, string key, params object[] args)
    {
      return string.Format(GetContent(theme, key), args);
    }
    public string GetContentFormat(string theme, string key, bool useDefaultThemeContentIfNotExists, object arg1)
    {
      return string.Format(GetContent(theme, key, useDefaultThemeContentIfNotExists), arg1);
    }
    public string GetContentFormat(string theme, string key, bool useDefaultThemeContentIfNotExists, object arg1, object arg2)
    {
      return string.Format(GetContent(theme, key, useDefaultThemeContentIfNotExists), arg1, arg2);
    }
    public string GetContentFormat(string theme, string key, bool useDefaultThemeContentIfNotExists, object arg1, object arg2, object arg3)
    {
      return string.Format(GetContent(theme, key, useDefaultThemeContentIfNotExists), arg1, arg2, arg3);
    }
    public string GetContentFormat(string theme, string key, bool useDefaultThemeContentIfNotExists, params object[] args)
    {
      return string.Format(GetContent(theme, key, useDefaultThemeContentIfNotExists), args);
    }

    private void ReadFromXml(ThemeFolderLookup themeFolderLookup, string theme, string fileName
      , string physicalApplicationPath, Dictionary<string, Dictionary<string, string>> lookup)
    {
      XAttribute key;
      string xmlFilePath;
      if (themeFolderLookup.TryGetVirtualThemePath(theme, fileName, out xmlFilePath))
      {
        XDocument document = XDocument.Load(Path.Combine(
          physicalApplicationPath
          , xmlFilePath.Remove(0, 2).Replace("/", @"\")) // make relative...
        );

        foreach (XElement xElement in document.Descendants("content"))
        {
          key = xElement.Attribute("key");
          if (key != null)
            lookup[theme][key.Value] = xElement.Value;
        }
      }
    }

    public void Initialize(ThemeFolderLookup themeFolderLookup, string defaultTheme, string physicalApplicationPath)
    {
      Dictionary<string, Dictionary<string, string>> lookup = new Dictionary<string, Dictionary<string, string>>();
      Dictionary<string, Dictionary<string, string>> languages = new Dictionary<string, Dictionary<string, string>>();
      Dictionary<string, Dictionary<string, string>> countries = new Dictionary<string, Dictionary<string, string>>();

      foreach (string theme in themeFolderLookup.ThemeNames)
      {
        lookup.Add(theme, new Dictionary<string, string>());
        languages.Add(theme, new Dictionary<string, string>());
        countries.Add(theme, new Dictionary<string, string>());

        ReadFromXml(themeFolderLookup, theme, "contents.xml", physicalApplicationPath, lookup);
        ReadFromXml(themeFolderLookup, theme, "contents.scripts.xml", physicalApplicationPath, lookup);
        ReadFromXml(themeFolderLookup, theme, "contents.emails.xml", physicalApplicationPath, lookup);
        ReadFromXml(themeFolderLookup, theme, "contents.countries.xml", physicalApplicationPath, countries);
        ReadFromXml(themeFolderLookup, theme, "contents.languages.xml", physicalApplicationPath, languages);
      }

      _DefaultTheme = defaultTheme;
      _Content = lookup;

      // do language conversion
      Dictionary<string, List<Language>> languageLookup = new Dictionary<string, List<Language>>();
      foreach (var item in languages)
      {
        languageLookup[item.Key] = new List<Language>();
        foreach (var kvp in item.Value)
          languageLookup[item.Key].Add(new Language(kvp.Key, kvp.Value));
      }
      _Languages = languageLookup;
      
      // do country conversion
      Dictionary<string, List<Country>> countryLookup = new Dictionary<string, List<Country>>();
      foreach (var item in countries)
      {
        countryLookup[item.Key] = new List<Country>();
        foreach (var kvp in item.Value)
          countryLookup[item.Key].Add(new Country(kvp.Key, kvp.Value));
      }
      _Countries = countryLookup;
    }
    #endregion

    #region constructors
    public StaticContentLookup() { }
    #endregion
  }
}
