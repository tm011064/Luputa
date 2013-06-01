using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Workmate.Web.Components.Application;
using System.Configuration;
using Workmate.Web.Components.Application.Sitemaps;

namespace Workmate.Web.Components
{
  public interface IRequestHelper
  {
    RequestContextData GetRequestContextData(HttpRequestBase request);
  }

  public class RequestHelper : IRequestHelper
  {
    public RequestContextData GetRequestContextData(HttpRequestBase request)
    {
      RequestContextData requestContextData;
      HttpContextBase httpContext = request.RequestContext.HttpContext;

      if (!httpContext.Items.Contains(MagicStrings.REQUEST_CONTEXT_DATA_KEY))
      {
        // request context data not present. This means this is the primary cshtml file, so do all the plumbing only once
        requestContextData = new RequestContextData();

        IApplicationContext applicationContext = InstanceContainer.ApplicationContext;
        requestContextData.ApplicationThemeInfo = applicationContext.GetApplicationThemeInfoFromDomainName(request.Url.DnsSafeHost);

        requestContextData.ThemeFolderLookup = applicationContext.GetThemeFolderLookup(requestContextData.ApplicationThemeInfo.ApplicationGroup);
        requestContextData.SitemapLookup = applicationContext.GetSitemapLookup(requestContextData.ApplicationThemeInfo.ApplicationGroup);
        requestContextData.StaticContentLookup = applicationContext.GetStaticContentLookup(requestContextData.ApplicationThemeInfo.ApplicationGroup);
        
        try { requestContextData.MenuInfo = request.RequestContext.RouteData.DataTokens[MagicStrings.DATATOKENS_MENUINFO] as MenuInfo; }
        catch (Exception ex) { throw new ConfigurationErrorsException("Menu info not set", ex); }

        requestContextData.BreadCrumb = request.RequestContext.RouteData.DataTokens[MagicStrings.DATATOKENS_BREADCRUMB] as Breadcrumb;

        httpContext.Items[MagicStrings.REQUEST_CONTEXT_DATA_KEY] = requestContextData;
      }
      else
      {
        requestContextData = httpContext.Items[MagicStrings.REQUEST_CONTEXT_DATA_KEY] as RequestContextData;
      }

      return requestContextData;
    }
  }
}
