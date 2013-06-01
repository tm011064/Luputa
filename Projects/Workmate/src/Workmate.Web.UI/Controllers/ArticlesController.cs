using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Workmate.Web.Components;
using System.Web.Routing;
using Workmate.Web.UI.Models;
using Workmate.Components.Contracts.Membership;
using Workmate.Web.UI.Models.Account;
using Workmate.Web.Components.Security;
using Workmate.Components.Contracts.CMS.Articles;
using Workmate.Web.UI.Models.Articles;
using Workmate.Web.Components.Application;
using System.Configuration;
using Workmate.Web.Components.Application.Sitemaps;

namespace Workmate.Web.Controllers
{
  public class ArticlesController : BaseController
  {
    [AllowAnonymous]
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult Create()
    {
      RequestContextData requestContextData = this.RequestContextData;

      IApplicationDataCache applicationDataCache = InstanceContainer.ApplicationDataCache;

      List<IWikiLanguageThreadLookupItem> wikiLanguageThreadLookupItems = applicationDataCache.GetWikiLanguageThreadIdLookup(requestContextData.ApplicationThemeInfo.ApplicationName);
      
      return View(new CreateArticleModel(
        5
        , new SelectList(wikiLanguageThreadLookupItems, "ArticleGroupThreadId", "LanguageName")
      ));
    }
    [HttpPost]
    public ActionResult Create(CreateArticleModel createArticleModel)
    {
      RequestContextData requestContextData = this.RequestContextData;

      IApplicationDataCache applicationDataCache = InstanceContainer.ApplicationDataCache;

      List<IWikiLanguageThreadLookupItem> wikiLanguageThreadLookupItems = applicationDataCache.GetWikiLanguageThreadIdLookup(requestContextData.ApplicationThemeInfo.ApplicationName);

      return View(new CreateArticleModel(
        5
        , new SelectList(wikiLanguageThreadLookupItems, "ArticleGroupThreadId", "LanguageName")
      ));
    }

    public ActionResult LoadCategories(int level, int? parentContentLevelNodeId, int articleGroupThreadId)
    {
      return new JsonResult() { Data = InstanceContainer.ArticleManager.GetArticleCategories(level, parentContentLevelNodeId, articleGroupThreadId, null) };
    }

    [AllowAnonymous]
    public ActionResult Search()
    {
      return View();
    }

    [AllowAnonymous]
    public ActionResult ViewArticle()
    {
      string name = null, category = null, subcategory = null;

      if (this.RouteData.Values.ContainsKey("name"))
        name = this.RouteData.Values["name"].ToString();      
      if (this.RouteData.Values.ContainsKey("category"))
        category = this.RouteData.Values["category"].ToString();
      if (this.RouteData.Values.ContainsKey("subcategory"))
        subcategory = this.RouteData.Values["subcategory"].ToString();

      if (string.IsNullOrWhiteSpace(name))
      {
        // TODO (Roman): do something
      }

      IArticleModel model = InstanceContainer.ArticleManager.GetArticleModel(name, category, subcategory);
      if (model != null)
      {
        return View(model);
      }

      return View();
    }
  }
}
