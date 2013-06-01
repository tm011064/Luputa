using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Workmate.Components.CMS.Articles;
using Workmate.Components.CMS.Membership;
using Workmate.Components.Contracts;
using Workmate.Components.Contracts.CMS.Articles;
using Workmate.Components.Entities.CMS.Articles;
using Workmate.Components.Entities.CMS.Membership;
using Workmate.Components.Membership;
using Workmate.Configuration;
using Workmate.Data;
using Workmate.Messaging;
using Workmate.Messaging.Configuration;
using Workmate.Web.Components.Application;
using Workmate.Web.Components.Application.Sitemaps;
using Workmate.Web.Components.Security;
using Workmate.Web.Components.Validation;
using CommonTools.Components.Graphics;
using System.Drawing;
using log4net;
using System.Threading;
using System.Drawing.Imaging;
using Workmate.Components.Emails;
using Workmate.Web.Components.Emails;
using Workmate.Components.Contracts.Membership;
using Workmate.Components.Company;

namespace Workmate.Web.Components
{
  public class MvcApplication : System.Web.HttpApplication
  {
    private ILog _Log = LogManager.GetLogger("MvcApplication");

    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new WorkmateAuthorizeAttribute());
      filters.Add(new HandleErrorAttribute());
    }

    #region private methods

    #region database content methods
    private bool TryCreateSystemProfileImage(IThemeFolderLookup themeFolderLookup, IApplicationThemeInfo applicationThemeInfo
      , string imageFilePath, string fileName)
    {
      SystemProfileImage systemProfileImage = new SystemProfileImage(applicationThemeInfo.ApplicationId);

      Image image = Image.FromFile(imageFilePath);

      systemProfileImage.Content = File.ReadAllBytes(imageFilePath);
      systemProfileImage.ContentSize = systemProfileImage.Content.Length;
      systemProfileImage.ContentType = ImageHelper.GetMimeType(fileName);
      systemProfileImage.FriendlyFileName = fileName;
      systemProfileImage.Height = image.Height;
      systemProfileImage.Width = image.Width;
      systemProfileImage.IsTemporary = false;

      var report = InstanceContainer.SystemProfileImageManager.Create(systemProfileImage);
      switch (report.Status)
      {
        case DataRepositoryActionStatus.Success:
        case DataRepositoryActionStatus.UniqueKeyConstraint:
          return true;

        default:
          _Log.ErrorFormat("Unble to create system profile image for application {0} (ID: {1}) from file {2}"
            , applicationThemeInfo.ApplicationName
            , applicationThemeInfo.ApplicationId
            , imageFilePath);
          return false;
      }
    }
    private bool TrySaveProfileImage(SystemProfileImage systemProfileImage, IApplicationThemeInfo applicationThemeInfo
      , string folder, Size size)
    {
      if (!Directory.Exists(applicationThemeInfo.Images.DefaultThemeImageFolderServerPath + folder))
      {
        try
        {
          Directory.CreateDirectory(applicationThemeInfo.Images.DefaultThemeImageFolderServerPath + folder);
        }
        catch (Exception err)
        {
          _Log.Error("Unable to create directory " + applicationThemeInfo.Images.DefaultThemeImageFolderServerPath + folder + ". See inner exception for further details", err);
          return false;
        }
      }
      using (MemoryStream ms = new MemoryStream(systemProfileImage.Content))
      {
        ms.Position = 0;
        Image image = Image.FromStream(ms);

        if (image.Width != size.Width
            || image.Height != size.Height)
        {
          image = ImageHelper.Resize(image, size);
        }

        try
        {
          image.Save(applicationThemeInfo.Images.DefaultThemeImageFolderServerPath
            + folder + MagicStrings.FILE_PROFILEIMAGE_PREFIX + systemProfileImage.ImageId
            + ".png"
            , ImageFormat.Png);
        }
        catch (Exception err)
        {
          _Log.Error("Unable to save write profile image to disk. See inner exception for further details", err);
          return false;
        }
      }

      return true;
    }
    #endregion

    #region breadcrumbs
    private IEnumerable<Breadcrumb> GetBreadCrumbParents(RouteTag routeTag, Dictionary<string, Breadcrumb> breadCrumbs)
    {
      if (routeTag == null)
        yield break;

      if (routeTag.BreadcrumbParentRouteTag != null)
      {
        foreach (Breadcrumb breadCrumb in GetBreadCrumbParents(routeTag.BreadcrumbParentRouteTag, breadCrumbs))
          yield return breadCrumb;
      }
      yield return breadCrumbs[routeTag.Name];
    }
    private void CreateBreadcrumbs(Dictionary<string, Breadcrumb> breadCrumbs, ISitemapLookup sitemapLookup)
    {
      foreach (RouteTag routeTag in sitemapLookup.RouteTags)
      {        
        breadCrumbs[routeTag.Name] = new Breadcrumb(
          routeTag.Name
          , routeTag.Defaults.ContainsKey("action") ? routeTag.Defaults["action"].ToString() : null
          , routeTag.Defaults.ContainsKey("controller") ? routeTag.Defaults["controller"].ToString() : null
          , routeTag.BreadcrumbContentKey);
      }
      foreach (RouteTag routeTag in sitemapLookup.RouteTags)      
        breadCrumbs[routeTag.Name].Parents.AddRange(GetBreadCrumbParents(routeTag.BreadcrumbParentRouteTag, breadCrumbs));      
    }
    #endregion

    private void RefreshApplicationData()
    {
      // TODO (Roman): try/catch errorhandling?
      IApplicationContext context = InstanceContainer.ApplicationContext;

      #region context data
      context.Refresh(context.BaseDirectory);
      #endregion

      #region database content
      // TODO (Roman): this must be done at the windows service, not at a web client app

      Dictionary<string, List<IWikiLanguageThreadLookupItem>> wikiLanguageThreadIdLookup = new Dictionary<string, List<IWikiLanguageThreadLookupItem>>();

      Dictionary<string, IWikiLanguageThreadLookupItem> wikiLanguageThreadIds = new Dictionary<string, IWikiLanguageThreadLookupItem>();
      IApplicationManager applicationManger = InstanceContainer.ApplicationManager;
      IArticleGroupManager articleGroupManager = InstanceContainer.ArticleGroupManager;
      IArticleGroupThreadManager articleGroupThreadManager = InstanceContainer.ArticleGroupThreadManager;
      ISystemProfileImageManager systemProfileImageManager = InstanceContainer.SystemProfileImageManager;
      IApplication application;
      IStaticContentLookup staticContentLookup;
      ArticleGroupThread articleGroupThread;
      ArticleGroup articleGroup;

      IApplicationThemeInfo[] applicationThemeInfos = context.GetAllApplicationThemeInfos();
      Dictionary<int, IApplication> applicationLookup = new Dictionary<int, IApplication>();

      #region applications
      foreach (IApplicationThemeInfo applicationThemeInfo in applicationThemeInfos)
      {
        application = applicationManger.GetApplication(applicationThemeInfo.ApplicationName);
        if (application == null)
        {
          application = new Workmate.Components.Entities.Application(applicationThemeInfo.ApplicationName, "Generated on Application Data Refresh at " + DateTime.UtcNow.ToString() + " (UTC)");

          #region do default settings - we really want this to be prepopulated by sql scripts
          application.DefaultAdminSenderEmailAddress = "admin@" + applicationThemeInfo.ApplicationName + ".com";
          #endregion

          var report = applicationManger.Create(application);
          if (report.Status != DataRepositoryActionStatus.Success)
          {
            // TODO (Roman): error handling?
            continue;
          }
        }
        // this was not set yet as the context refresh above did not have all data available, so we have to do it here
        applicationThemeInfo.ApplicationId = application.ApplicationId;
        applicationThemeInfo.Application = application;

        applicationLookup[application.ApplicationId] = application;
      }
      #endregion

      #region Userroles
      string[] userRoles = Enum.GetNames(typeof(UserRole));
      foreach (int applicationId in applicationLookup.Keys)
      {
        InstanceContainer.WorkmateRoleProvider.CreateRolesIfNotExist(applicationId, userRoles);
      }
      #endregion

      IThemeFolderLookup themeFolderLookup;
      Dictionary<string, SystemProfileImage> systemProfileImageLookup;
      HashSet<int> usedApplicationIds = new HashSet<int>();
      foreach (IApplicationThemeInfo applicationThemeInfo in applicationThemeInfos.Where(c => c.ApplicationGroup == MagicStrings.APPLICATIONGROUP_NAME_WORKMATE))
      {
        themeFolderLookup = context.GetThemeFolderLookup(applicationThemeInfo.ApplicationGroup);

        #region profile images
        if (!usedApplicationIds.Contains(applicationThemeInfo.ApplicationId))
        {
          systemProfileImageLookup = systemProfileImageManager.GetSystemProfileImages(applicationThemeInfo.ApplicationId);

          if (!systemProfileImageLookup.ContainsKey(MagicStrings.PROFILE_IMAGE_MALE_FILENAME))
          {
            if (File.Exists(applicationThemeInfo.Images.DefaultThemeImageFolderServerPath + MagicStrings.PROFILE_IMAGE_MALE_FILENAME))
            {
              TryCreateSystemProfileImage(themeFolderLookup, applicationThemeInfo
                , applicationThemeInfo.Images.DefaultThemeImageFolderServerPath + MagicStrings.PROFILE_IMAGE_MALE_FILENAME
                , MagicStrings.PROFILE_IMAGE_MALE_FILENAME);
            }
          }
          if (!systemProfileImageLookup.ContainsKey(MagicStrings.PROFILE_IMAGE_FEMALE_FILENAME))
          {
            if (File.Exists(applicationThemeInfo.Images.DefaultThemeImageFolderServerPath + MagicStrings.PROFILE_IMAGE_FEMALE_FILENAME))
            {
              TryCreateSystemProfileImage(themeFolderLookup, applicationThemeInfo
                , applicationThemeInfo.Images.DefaultThemeImageFolderServerPath + MagicStrings.PROFILE_IMAGE_FEMALE_FILENAME
                , MagicStrings.PROFILE_IMAGE_FEMALE_FILENAME);
            }
          }

          // now reload in case we've created profile images for the first time
          systemProfileImageLookup = systemProfileImageManager.GetSystemProfileImages(applicationThemeInfo.ApplicationId);
          foreach (SystemProfileImage systemProfileImage in systemProfileImageLookup.Values)
          {
            TrySaveProfileImage(systemProfileImage, applicationThemeInfo, MagicStrings.FOLDER_IMAGES_PROFILE_NORMAL, MagicNumbers.PROFILEIMAGE_SIZE_NORMAL);
            TrySaveProfileImage(systemProfileImage, applicationThemeInfo, MagicStrings.FOLDER_IMAGES_PROFILE_TINY, MagicNumbers.PROFILEIMAGE_SIZE_TINY);
          }

          usedApplicationIds.Add(applicationThemeInfo.ApplicationId);
        }

        systemProfileImageLookup = systemProfileImageManager.GetSystemProfileImages(applicationThemeInfo.ApplicationId);
        foreach (SystemProfileImage systemProfileImage in systemProfileImageLookup.Values)
        {
          switch (systemProfileImage.FriendlyFileName)
          {
            case MagicStrings.PROFILE_IMAGE_MALE_FILENAME: applicationThemeInfo.Images.MaleSystemProfileImageId = systemProfileImage.ImageId; break;
            case MagicStrings.PROFILE_IMAGE_FEMALE_FILENAME: applicationThemeInfo.Images.FemaleSystemProfileImageId = systemProfileImage.ImageId; break;
          }
        }
        #endregion

        #region create wiki language threads
        articleGroup = articleGroupManager.GetArticleGroup(applicationThemeInfo.ApplicationId, MagicStrings.WIKI_LANGUAGE_SECTION);
        if (articleGroup == null)
        {
          articleGroup = new ArticleGroup(applicationThemeInfo.ApplicationId, MagicStrings.WIKI_LANGUAGE_SECTION, true);
          articleGroup.Description = "Generated on Application Data Refresh at " + DateTime.UtcNow.ToString() + " (UTC)";

          var report = articleGroupManager.Create(articleGroup);
          if (report.Status != DataRepositoryActionStatus.Success)
          {
            // TODO (Roman): error handling?
            continue;
          }
        }

        wikiLanguageThreadIds = new Dictionary<string, IWikiLanguageThreadLookupItem>();
        staticContentLookup = context.GetStaticContentLookup(applicationThemeInfo.ApplicationGroup);
        foreach (string theme in themeFolderLookup.ThemeNames)
        {
          foreach (Language language in staticContentLookup.GetLanguages(theme))
          {
            if (wikiLanguageThreadIds.ContainsKey(language.ShortCode))
              continue;

            articleGroupThread = articleGroupThreadManager.GetArticleGroupThread(language.ShortCode);
            if (articleGroupThread == null)
            {
              articleGroupThread = new ArticleGroupThread(articleGroup, ArticleGroupThreadStatus.Enabled, language.ShortCode);
              articleGroupThread.IsApproved = true;
              articleGroupThread.IsLocked = false;

              var report = articleGroupThreadManager.Create(articleGroupThread);
              if (report.Status != DataRepositoryActionStatus.Success)
              {
                // TODO (Roman): error handling?
                continue;
              }
            }

            wikiLanguageThreadIds[language.ShortCode] = new WikiLanguageThreadLookupItem(language.ShortCode, language.Name, articleGroupThread.ArticleGroupThreadId);
          }
        }

        wikiLanguageThreadIdLookup[applicationThemeInfo.ApplicationName.ToLowerInvariant()] = wikiLanguageThreadIds.Values
                                                                                                                   .ToList();

        #endregion
      }

      IApplicationDataCache applicationDataCache = InstanceContainer.ApplicationDataCache;
      applicationDataCache.Refresh(wikiLanguageThreadIdLookup);

      #endregion

      #region routes
      RouteCollection routes = RouteTable.Routes;

      // TODO (Roman): get read lock as well?
      using (IDisposable writeLock = routes.GetWriteLock())
      {
        routes.Clear();

        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        ISitemapLookup sitemapLookup;
        Menu topMenu;
        MenuInfo menuInfo;
        Dictionary<string, Breadcrumb> breadCrumbs = new Dictionary<string, Breadcrumb>();
        foreach (string applicationGroup in context.GetApplicationGroups())
        {
          sitemapLookup = context.GetSitemapLookup(applicationGroup);

          breadCrumbs.Clear();
          CreateBreadcrumbs(breadCrumbs, sitemapLookup);

          foreach (RouteTag routeTag in sitemapLookup.RouteTags)
          {
            topMenu = sitemapLookup.GetMenu(routeTag.TopMenuName);

            #region data tokens extensions
            routeTag.DataTokens[MagicStrings.DATATOKENS_BREADCRUMB] = breadCrumbs[routeTag.Name];
            routeTag.DataTokens[MagicStrings.DATATOKENS_ROUTENAME] = routeTag.Name;

            menuInfo = new MenuInfo();
            menuInfo.TopMenuName = routeTag.TopMenuName;
            if (topMenu != null)
            {
              foreach (MenuItem item in topMenu.MenuItems)
              {
                if (item.RouteName == routeTag.Name)
                  menuInfo.TopMenuItemName = item.Name;

                foreach (MenuItem subItem in item.Children)
                {
                  if (subItem.RouteName == routeTag.Name)
                  {
                    menuInfo.TopMenuItemName = subItem.Parent.Name;
                    menuInfo.TopMenuSubItemName = subItem.Name;
                    break;
                  }
                }
                foreach (MenuItem subItem in item.DropdownMenuItems)
                {
                  foreach (MenuItem dropdownSubItem in subItem.Children)
                  {
                    if (dropdownSubItem.RouteName == routeTag.Name)
                    {
                      menuInfo.TopMenuItemName = dropdownSubItem.Parent.Name;
                      menuInfo.TopMenuSubItemName = dropdownSubItem.Name;
                      break;
                    }
                  }
                }
              }
            }
            routeTag.DataTokens[MagicStrings.DATATOKENS_MENUINFO] = menuInfo;
            #endregion

            routes.Add(
              MagicStrings.FormatRouteName(applicationGroup, routeTag.Name)
              , new Route(routeTag.Url
                          , routeTag.Defaults
                          , routeTag.Constraints
                          , routeTag.DataTokens
                          , routeTag.RouteHandler));
          }
        }
      }
      #endregion
    }
    #endregion

    #region events
    void applicationMessageHandler_RefreshApplicationDataRequest(object sender, EventArgs e)
    {
      RefreshApplicationData();
    }
    void applicationMessageHandler_CommunicationStateChanged(object sender, CommunicationStateChangedEventArgs e)
    {

      // we have successfully connected to the application server
    }
    #endregion

    #region application methods

    protected void Application_Start()
    {
      DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EmailAttribute), typeof(RegularExpressionAttributeAdapter));

      #region initialize
      string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

      IApplicationSettings applicationSettings = new ApplicationSettings(ConfigurationManager.AppSettings, ConfigurationManager.ConnectionStrings);

      log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(applicationSettings.Log4NetConfigPath));

      IMessageClientConfigurationSection section = (IMessageClientConfigurationSection)ConfigurationManager.GetSection("clientSettings");
      ClientMessageHandlerProxy clientMessageHandlerProxy = new ClientMessageHandlerProxy(section);

      ApplicationMessageHandler applicationMessageHandler = new ApplicationMessageHandler(clientMessageHandlerProxy);

      applicationMessageHandler.RefreshApplicationDataRequest += new EventHandler<EventArgs>(applicationMessageHandler_RefreshApplicationDataRequest);
      applicationMessageHandler.CommunicationStateChanged += new EventHandler<CommunicationStateChangedEventArgs>(applicationMessageHandler_CommunicationStateChanged);

      applicationMessageHandler.Connect();

      IDataStore dataStore = null;

      try
      {
        string dataStoreContextTypeAssemblyName = applicationSettings.DataStoreContextType.Substring(applicationSettings.DataStoreContextType.LastIndexOf(',') + 1).Trim();
        string dataStoreContextTypeName = applicationSettings.DataStoreContextType.Substring(0, applicationSettings.DataStoreContextType.IndexOf(','));

        Assembly assembly = Assembly.Load(dataStoreContextTypeAssemblyName); // load into default load context
        Type type = assembly.GetType(dataStoreContextTypeName);
        dataStore = Activator.CreateInstance(type) as IDataStore;
      }
      catch (Exception ex)
      {
        throw new ConfigurationErrorsException("Error loading data store object, see inner exception for further details.", ex);
      }
      if (dataStore == null)
        throw new ConfigurationErrorsException("Datastore is not provided.");

      dataStore.Initialize(applicationSettings.DefaultConnectionString);

      System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
      MembershipSection configSection = (MembershipSection)config.GetSection("system.web/membership");

      MembershipSettings membershipSettings = new MembershipSettings(
        (NameValueCollection)configSection.Providers[configSection.DefaultProvider].Parameters);

      Workmate.Components.InstanceContainer.Initialize(
        dataStore
        , applicationSettings
        , membershipSettings);

      // we need to load this after singletons initialize
      WorkmateMembershipProvider workmateMembershipProvider = new WorkmateMembershipProvider();
      WorkmateRoleProvider workmateRoleProvider = new WorkmateRoleProvider();

      ArticleManager articleManager = new ArticleManager(dataStore);

      // the singletons class must be initialized here so it can be used later on
      InstanceContainer.Initialize(
        new ApplicationContext(baseDirectory, applicationMessageHandler, applicationSettings, dataStore)
        , new TicketManager(applicationSettings, workmateMembershipProvider)
        , workmateMembershipProvider
        , workmateRoleProvider
        , articleManager
        , new RequestHelper()
        , new ArticleAttachmentManager(dataStore)
        , new Workmate.Components.ApplicationManager(dataStore)
        , new ArticleGroupManager(dataStore)
        , new ArticleGroupThreadManager(dataStore)
        , new ApplicationDataCache()
        , new ProfileImageManager(dataStore)
        , new SystemProfileImageManager(dataStore)
        , new EmailManager(dataStore)
        , new EmailPublisher()
        , new OfficeManager(dataStore)
        , new DepartmentManager(dataStore)
        );
      #endregion

      #region startup
      AreaRegistration.RegisterAllAreas();
      RegisterGlobalFilters(GlobalFilters.Filters);

      RefreshApplicationData();
      #endregion

      #region messaging

      #endregion
    }

    #endregion

    #region IDisposable Members
    private bool _IsDisposing = false;
    public override void Dispose()
    {
      if (!_IsDisposing)
      {
        try { InstanceContainer.ApplicationContext.Dispose(); }
        catch { }
      }

      base.Dispose();
    }
    #endregion
  }
}