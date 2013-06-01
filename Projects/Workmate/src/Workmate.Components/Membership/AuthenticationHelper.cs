using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Membership
{
  static class AuthenticationHelper
  {
    public static IUserBasic GetUserBasicFromAuthenticationCookie(HttpContext currentContext)
    {
      if (currentContext == null)
      {
        throw new ArgumentNullException("Current HttpContext must not be null.");
      }
      if (!currentContext.User.Identity.IsAuthenticated)
      {
        return UserBasic.GetAnonymousUserInstance();
      }
      IUserBasic userBasic = currentContext.User.Identity as IUserBasic;
      if (userBasic != null)
      {
        return new UserBasic(userBasic);
      }
      throw new Exception("Current user is not of type FLUserPrincipal.");
    }
  }
}
