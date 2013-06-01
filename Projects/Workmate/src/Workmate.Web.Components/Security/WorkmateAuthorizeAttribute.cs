using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.IO;
using System.Web.Security;
using System.Text.RegularExpressions;
using Workmate.Components.Contracts.Membership;
using Workmate.Components.Entities.Membership;
using System.Diagnostics;
using Workmate.Web.Components.Application;

namespace Workmate.Web.Components.Security
{
  public sealed class WorkmateAuthorizeAttribute : AuthorizeAttribute
  {
    public override void OnAuthorization(AuthorizationContext filterContext)
    {
      ITicketManager ticketManager = InstanceContainer.TicketManager;      

      bool isAuthorized = ticketManager.IsAuthorized(filterContext.HttpContext);

      // this information can be cached so we don't reflect on every call
      bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
        || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

      if (!skipAuthorization
          && !isAuthorized)
      {
        ticketManager.Signout();
        // auth failed, redirect to login page
        filterContext.Result = new HttpUnauthorizedResult();
        return;
      }
    }
  }
}
