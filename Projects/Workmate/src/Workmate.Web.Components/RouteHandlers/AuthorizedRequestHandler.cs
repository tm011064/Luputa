using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using Workmate.Web.Components.Security;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Web.Components.RouteHandlers
{
  public abstract class AuthorizedRequestHandler : IRouteHandler, IHttpHandler
  {
    #region members
    private RequestContext _RequestContext;
    #endregion

    #region properties

    protected IUserBasic User { get; private set; }

    private RequestContextData _RequestContextData;
    protected RequestContextData RequestContextData
    {
      get
      {
        if (_RequestContextData == null)
          _RequestContextData = InstanceContainer.RequestHelper.GetRequestContextData(_RequestContext.HttpContext.Request);

        return _RequestContextData;
      }
    }

    #endregion

    #region IRouteHandler Members

    public virtual IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      _RequestContext = requestContext;
      return this;
    }

    #endregion

    #region IHttpHandler Members

    public virtual bool IsReusable { get { return false; } }

    public void ProcessRequest(HttpContext context)
    {
      ITicketManager ticketManager = InstanceContainer.TicketManager;
      if (!ticketManager.IsAuthorized(context))
      {
        ticketManager.SignoutAndRedirectToLogin();
        return;
      }

      this.User = context.User.Identity as IUserBasic;

      ProcessAuthorizedRequest(context);
    }

    #endregion

    public abstract void ProcessAuthorizedRequest(HttpContext context);
  }
}
