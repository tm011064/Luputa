using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Web.Components.Application.Sitemaps;
using System.Web;
using System.IO;
using System.Configuration;
using Workmate.Messaging;
using Workmate.Configuration;
using Workmate.Data;

namespace Workmate.Web.Components.Application
{
  public class ApplicationContext : IApplicationContext
  {
    #region members
    private Dictionary<string, ThemeFolderLookup> _ThemeFolderLookups = new Dictionary<string, ThemeFolderLookup>();
    private Dictionary<string, SitemapLookup> _SitemapLookups = new Dictionary<string, SitemapLookup>();
    private Dictionary<string, StaticContentLookup> _StaticContentLookups = new Dictionary<string, StaticContentLookup>();
    private List<string> _ApplicationGroups = new List<string>();
    private Dictionary<string, IApplicationThemeInfo> _ApplicationThemeInfoLookup = new Dictionary<string, IApplicationThemeInfo>();
    #endregion

    #region IApplicationContext Members

    public void Refresh(string applicationRootPath)
    {
      Dictionary<string, ThemeFolderLookup> themeFolderLookups = new Dictionary<string, ThemeFolderLookup>();
      Dictionary<string, SitemapLookup> sitemapLookups = new Dictionary<string, SitemapLookup>();
      Dictionary<string, StaticContentLookup> staticContentLookups = new Dictionary<string, StaticContentLookup>();
      Dictionary<string, IApplicationThemeInfo> applicationThemeInfoLookup = new Dictionary<string, IApplicationThemeInfo>();

      List<string> applicationGroups = new List<string>();
      DirectoryInfo directoryInfo = new DirectoryInfo(applicationRootPath + @"\Themes");
      applicationGroups = (from c in directoryInfo.GetDirectories()
                           select c.Name.ToLowerInvariant()).ToList();

      ThemeFolderLookup themeFolderLookup;
      SitemapLookup sitemapLookup;
      StaticContentLookup staticContentLookup;

      string sitemapConfigFilePath;

      Dictionary<string, SitemapItem> dnsSitemapItems;
      Dictionary<string, Tuple<SitemapItem, string>> defaultDnsSitemapItems;
      SitemapItem defaultThemeSitemapItem;
      string defaultDomainName;

      foreach (string applicationGroup in applicationGroups)
      {
        sitemapConfigFilePath = applicationRootPath + @"\Themes\" + applicationGroup + @"\sitemap.config";

        if (!File.Exists(sitemapConfigFilePath))
          throw new ConfigurationErrorsException("File " + sitemapConfigFilePath + " not found. Please provide a sitemap file for application " + applicationGroup);

        sitemapLookup = new SitemapLookup();
        sitemapLookup.Initialize(sitemapConfigFilePath);

        if (string.IsNullOrWhiteSpace(sitemapLookup.DefaultTheme))
          throw new ConfigurationErrorsException("Please provide a default theme within the sitemap.config file for application " + applicationGroup);

        themeFolderLookup = new ThemeFolderLookup();
        themeFolderLookup.Initialize(applicationRootPath, applicationGroup, sitemapLookup.DefaultTheme, sitemapLookup.GetDnsLookup());

        staticContentLookup = new StaticContentLookup();
        staticContentLookup.Initialize(themeFolderLookup, sitemapLookup.DefaultTheme, applicationRootPath);

        dnsSitemapItems = sitemapLookup.GetDnsSitemapItems();
        defaultDnsSitemapItems = new Dictionary<string, Tuple<SitemapItem, string>>();

        foreach (string domainName in dnsSitemapItems.Keys)
        {
          if (dnsSitemapItems[domainName].IsDefault)
          {
            defaultDnsSitemapItems[dnsSitemapItems[domainName].ApplicationName.ToLowerInvariant()] = new Tuple<SitemapItem, string>(dnsSitemapItems[domainName], domainName.ToLowerInvariant().TrimEnd('/') + "/"); ;
          }
        }
        foreach (string domainName in dnsSitemapItems.Keys)
        {
          defaultThemeSitemapItem = defaultDnsSitemapItems[dnsSitemapItems[domainName].ApplicationName.ToLowerInvariant()].Item1;
          defaultDomainName = defaultDnsSitemapItems[dnsSitemapItems[domainName].ApplicationName.ToLowerInvariant()].Item2;

          ApplicationThemeInfoImages applicationThemeInfoImages = new ApplicationThemeInfoImages();
          applicationThemeInfoImages.ImageFolderServerPath = dnsSitemapItems[domainName].HasImageFolderServerPath
              ? dnsSitemapItems[domainName].ImageFolderServerPath.TrimEnd('\\') + @"\"
              : applicationRootPath.TrimEnd('\\') + @"\Themes\" + applicationGroup + @"\" + dnsSitemapItems[domainName].Name + @"\images\";
          applicationThemeInfoImages.ImageFolderRootUrl = dnsSitemapItems[domainName].HasImageFolderRootUrl
            ? dnsSitemapItems[domainName].ImageFolderRootUrl
            : (defaultDomainName.StartsWith("http://") ? defaultDomainName : "http://" + defaultDomainName)
              + "themes/" + applicationGroup + "/" + dnsSitemapItems[domainName].Name + "/images/";               

          applicationThemeInfoImages.DefaultThemeImageFolderServerPath = defaultThemeSitemapItem.HasImageFolderServerPath
            ? defaultThemeSitemapItem.ImageFolderServerPath.TrimEnd('\\') + @"\"
            : applicationRootPath.TrimEnd('\\') + @"\Themes\" + applicationGroup + @"\" + defaultThemeSitemapItem.Name + @"\images\";

          applicationThemeInfoImages.DefaultThemeImageFolderRootUrl = defaultThemeSitemapItem.HasImageFolderRootUrl
            ? defaultThemeSitemapItem.ImageFolderRootUrl
            : (defaultDomainName.StartsWith("http://") ? defaultDomainName : "http://" + defaultDomainName)
              + "themes/" + applicationGroup + "/" + dnsSitemapItems[domainName].Name + "/images/";   
          
          applicationThemeInfoLookup[domainName] = new ApplicationThemeInfo()
          {
            ApplicationGroup = dnsSitemapItems[domainName].ApplicationGroup,
            ApplicationId = int.MinValue,
            ApplicationName = dnsSitemapItems[domainName].ApplicationName,
            DomainName = domainName,
            ThemeName = dnsSitemapItems[domainName].Name,
            IsDefault = dnsSitemapItems[domainName].IsDefault,
            DefaultThemeName = defaultThemeSitemapItem.Name,
            Images = applicationThemeInfoImages,
          };
        }

        themeFolderLookups.Add(applicationGroup, themeFolderLookup);
        sitemapLookups.Add(applicationGroup, sitemapLookup);
        staticContentLookups.Add(applicationGroup, staticContentLookup);
      }

      _ApplicationThemeInfoLookup = applicationThemeInfoLookup;
      _ThemeFolderLookups = themeFolderLookups;
      _SitemapLookups = sitemapLookups;
      _ApplicationGroups = applicationGroups;
      _StaticContentLookups = staticContentLookups;
    }

    public IThemeFolderLookup GetThemeFolderLookup(string applicationGroup)
    {
      try { return _ThemeFolderLookups[applicationGroup]; }
      catch (KeyNotFoundException)
      {
        throw new ConfigurationErrorsException("Theme folder lookup not defined for application group " + applicationGroup);
      }
    }

    public ISitemapLookup GetSitemapLookup(string applicationGroup)
    {
      try { return _SitemapLookups[applicationGroup]; }
      catch (KeyNotFoundException)
      {
        throw new ConfigurationErrorsException("Sitemap lookup not defined for application " + applicationGroup);
      }
    }

    public IStaticContentLookup GetStaticContentLookup(string applicationGroup)
    {
      try { return _StaticContentLookups[applicationGroup]; }
      catch (KeyNotFoundException)
      {
        throw new ConfigurationErrorsException("Static content lookup not defined for application " + applicationGroup);
      }
    }

    public IApplicationThemeInfo GetApplicationThemeInfoFromDomainName(string domainName)
    {
      try { return _ApplicationThemeInfoLookup[domainName]; }
      catch (KeyNotFoundException)
      {
        throw new ConfigurationErrorsException("Domain name " + domainName + " not registered");
      }
    }

    public IApplicationThemeInfo[] GetAllApplicationThemeInfos()
    {
      return _ApplicationThemeInfoLookup.Values.ToArray();
    }

    public string[] GetApplicationGroups()
    {
      return _ApplicationGroups.ToArray();
    }

    public string BaseDirectory { get; private set; }

    public IApplicationMessageHandler ApplicationMessageHandler { get; private set; }

    public IApplicationSettings ApplicationSettings { get; private set; }

    public IDataStore DataStore { get; private set; }

    #endregion

    #region constructors
    public ApplicationContext(string baseDirectory, IApplicationMessageHandler applicationMessageHandler
      , IApplicationSettings applicationSettings, IDataStore dataStore)
    {
      this.BaseDirectory = baseDirectory;
      this.ApplicationMessageHandler = applicationMessageHandler;
      this.ApplicationSettings = applicationSettings;
      this.DataStore = dataStore;
    }
    #endregion

    #region IDisposable Members

    private bool _IsDisposing = false;
    public void Dispose()
    {
      if (!_IsDisposing)
      {
        this._IsDisposing = true;

        try { this.ApplicationMessageHandler.Dispose(); }
        catch { }
      }
    }

    #endregion
  }
}
