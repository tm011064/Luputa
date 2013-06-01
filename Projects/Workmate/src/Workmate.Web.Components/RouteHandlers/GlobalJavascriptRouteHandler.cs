using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using Workmate.Web.Components.Application;

namespace Workmate.Web.Components.RouteHandlers
{
  public class GlobalJavascriptRouteHandler : IRouteHandler
  {
    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      // TODO (Roman): errorhandling, cleanup, upper/lower case stuff
      RequestContextData requestContextData = InstanceContainer.RequestHelper.GetRequestContextData(requestContext.HttpContext.Request);
      IApplicationContext applicationContext = InstanceContainer.ApplicationContext;
      
      IThemeFolderLookup themeFolderLookup = applicationContext.GetThemeFolderLookup(requestContextData.ApplicationThemeInfo.ApplicationGroup);
      IStaticContentLookup staticContentLookup = applicationContext.GetStaticContentLookup(requestContextData.ApplicationThemeInfo.ApplicationGroup);

      string text = ContentFormatUtility.Format(
        requestContextData.Theme
        , staticContentLookup.GetContent(requestContextData.Theme, "js_globals")
        , staticContentLookup
        , themeFolderLookup
        , ContentFormatUtility.FormatMode.EscapeSingleQuotes);

      requestContext.HttpContext.Response.Clear();
      requestContext.HttpContext.Response.Write(text);
      requestContext.HttpContext.Response.End();

      return null;
    }
  }
}
