using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web;
using Workmate.Components.Membership;
using Workmate.Components.Contracts.Membership;
using Workmate.Components.Entities.Membership;
using Workmate.Configuration;
using System.Security.Principal;

namespace Workmate.Web.Components.Security
{
  public interface ITicketManager
  {
    void UpdateAuthenticationCookie(IUserBasic userBasic);
    void RefreshAuthenticationCookie(WMUserIdentity identity);
    void WriteAuthenticationCookie(IUserBasic userBasic, bool rememberMe);
    void Signout();
    void SignoutAndRedirectToLogin();
    ValidateUserStatus LogUserIn(int applicationId, string username, string password, bool rememberMe, out IUserBasic userBasic);
    bool LogUserIn(UserBasic userBasic, bool rememberMe);
    bool IsAuthorized(HttpContext context);
    bool IsAuthorized(HttpContextBase context);
  }
}
