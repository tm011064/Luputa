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
  public class TicketManager : ITicketManager
  {
    #region members
    private IApplicationSettings _ApplicationSettings;
    private IWorkmateMembershipProvider _WorkmateMembershipProvider;
    #endregion

    #region public methods

    public bool IsAuthorized(HttpContext context)
    {
      IPrincipal authorizedUser;
      bool isAuthorized = IsAuthorized(context.User, out authorizedUser);
      context.User = authorizedUser;
      return isAuthorized;
    }
    public bool IsAuthorized(HttpContextBase context)
    {
      IPrincipal authorizedUser;
      bool isAuthorized = IsAuthorized(context.User, out authorizedUser);
      context.User = authorizedUser;
      return isAuthorized;
    }

    private bool IsAuthorized(IPrincipal user, out IPrincipal authorizedUser)
    {
      bool isAuthorized = false;

      if (user != null
          && user.Identity.IsAuthenticated
          && user.Identity is System.Web.Security.FormsIdentity)
      {
        // we are authenticated, so let's check whether the cookie has the correct format
        WMUserPrincipal principal;
        try
        {
          principal = new WMUserPrincipal((FormsIdentity)user.Identity);
          authorizedUser = principal;
        }
        catch (Exception)
        {
          // this means we have a dodgy session cookie, so redirect
          authorizedUser = WMUserPrincipal.AnonymousInstance;
          return false;
        }

        IWorkmateMembershipProvider provider = InstanceContainer.WorkmateMembershipProvider;
        if (principal.WMUserIdentity.LastRecordCheckUtc.AddSeconds(provider.LastRecordCheckWindowInSeconds) < DateTime.UtcNow)
        {
          IUserBasic userBasic = provider.GetUserBasic(principal.WMUserIdentity.UserId, true);
          if (userBasic != null)
          {
            this.UpdateAuthenticationCookie(userBasic);
            isAuthorized = true;
          }
        }
        else if (FormsAuthentication.SlidingExpiration)
        {// refresh the cookie if we have sliding expiration
          // check whether we should update the last activity date
          if (principal.WMUserIdentity.LastActivityUpdate.AddSeconds(provider.LastActivityUpdateWindowInSeconds) < DateTime.UtcNow)
          {
            // we have to update the lastactivity date...
            IUserBasic userBasic = provider.GetUserBasic(principal.WMUserIdentity.UserId, true);
            if (userBasic != null)
            {
              this.UpdateAuthenticationCookie(userBasic);
              isAuthorized = true;
            }
          }
          else
          {
            // refresh the cookie
            this.RefreshAuthenticationCookie(principal.WMUserIdentity);
            isAuthorized = true;
          }
        }
      }
      else
      {
        authorizedUser = WMUserPrincipal.AnonymousInstance;
      }

      return isAuthorized;
    }


    /// <summary>
    /// Updates the authentication cookie.
    /// </summary>
    /// <param name="userBasic">The user basic.</param>
    public void UpdateAuthenticationCookie(IUserBasic userBasic)
    {
      WMUserIdentity identity = WMUserIdentity.Create(userBasic, true, DateTime.UtcNow, DateTime.UtcNow);
      
      AuthenticationCookieManager.UpdateAuthenticationCookie<WMUserIdentity>(
          _WorkmateMembershipProvider.AuthenticationTimeoutInSeconds
          , _WorkmateMembershipProvider.PersistentAuthenticationTimeoutInSeconds
          , identity);
    }
    /// <summary>
    /// Refreshes the authentication cookie.
    /// </summary>
    /// <param name="email">The email.</param>
    public void RefreshAuthenticationCookie(WMUserIdentity identity)
    {
      AuthenticationCookieManager.RefreshAuthenticationCookie<WMUserIdentity>(
          _WorkmateMembershipProvider.AuthenticationTimeoutInSeconds
          , _WorkmateMembershipProvider.PersistentAuthenticationTimeoutInSeconds
          , identity);
    }

    /// <summary>
    /// Writes the authentication cookie.
    /// </summary>
    /// <param name="userBasic">The user basic.</param>
    /// <param name="rememberMe">if set to <c>true</c> [remember me].</param>
    public void WriteAuthenticationCookie(IUserBasic userBasic, bool rememberMe)
    {
      WMUserIdentity identity = WMUserIdentity.Create(userBasic, true, DateTime.UtcNow, DateTime.UtcNow);

      AuthenticationCookieManager.WriteAuthenticationCookie<WMUserIdentity>(
          userBasic.UserName
          , _WorkmateMembershipProvider.AuthenticationTimeoutInSeconds
          , _WorkmateMembershipProvider.PersistentAuthenticationTimeoutInSeconds
          , identity
          , rememberMe);
    }


    /// <summary>
    /// Signouts this instance.
    /// </summary>
    public void Signout()
    {
      AuthenticationCookieManager.Signout();
    }
    /// <summary>
    /// Signouts the and redirect to login.
    /// </summary>
    public void SignoutAndRedirectToLogin()
    {
      Signout();
      FormsAuthentication.RedirectToLoginPage();
    }

    /// <summary>
    /// Logs the user in.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <param name="rememberMe">if set to <c>true</c> [remember me].</param>
    /// <param name="userBasic">The user basic.</param>
    /// <returns></returns>
    public ValidateUserStatus LogUserIn(int applicationId, string username, string password, bool rememberMe, out IUserBasic userBasic)
    {
      userBasic = null;
      ValidateUserStatus status = ValidateUserStatus.SqlError;

      status = _WorkmateMembershipProvider.ValidateUser(applicationId, username, password, out userBasic);
      switch (status)
      {
        case ValidateUserStatus.Valid:
          WriteAuthenticationCookie(userBasic, rememberMe);
          break;

        case ValidateUserStatus.AccountStatusBanned:
        case ValidateUserStatus.AccountStatusDeleted:
        case ValidateUserStatus.AccountStatusPending:
        case ValidateUserStatus.PasswordAnswerNotCorrect:
        case ValidateUserStatus.SqlError:
        case ValidateUserStatus.UserIsLockedOut:
        case ValidateUserStatus.UserNotFound:
        default:
          break;
      }
      return status;
    }
    /// <summary>
    /// Logs the user in.
    /// </summary>
    /// <param name="userBasic">The user basic.</param>
    /// <param name="rememberMe">if set to <c>true</c> [remember me].</param>
    /// <returns></returns>
    public bool LogUserIn(UserBasic userBasic, bool rememberMe)
    {
      WriteAuthenticationCookie(userBasic, rememberMe);
      return true;
    }
    #endregion

    #region constructors
    public TicketManager(IApplicationSettings applicationSettings, IWorkmateMembershipProvider workmateMembershipProvider)
    {
      _ApplicationSettings = applicationSettings;
      _WorkmateMembershipProvider = workmateMembershipProvider;
    }
    #endregion
  }
}
