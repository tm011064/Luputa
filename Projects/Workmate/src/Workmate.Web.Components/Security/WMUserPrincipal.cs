using System.Security.Principal;
using System.Web.Security;
using Workmate.Components.Entities.Membership;
using System;

namespace Workmate.Web.Components.Security
{
  public class WMUserPrincipal : IPrincipal
  {
    #region properties
    private WMUserIdentity _WMUserIdentity;
    /// <summary>
    /// Gets the WM user identity.
    /// </summary>
    /// <value>The WM user identity.</value>
    public WMUserIdentity WMUserIdentity
    {
      get { return _WMUserIdentity; }
    }

    #region IPrincipal Members

    public IIdentity Identity
    {
      get { return _WMUserIdentity; }
    }

    public bool IsInRole(string role)
    {
      foreach (string userRole in _WMUserIdentity.UserRoles)
        if (userRole == role)
          return true;

      return false;
    }

    #endregion

    #endregion

    #region constructors

    private static WMUserPrincipal _AnonymousInstance;
    public static WMUserPrincipal AnonymousInstance { get { return _AnonymousInstance; } }
    static WMUserPrincipal()
    {
      _AnonymousInstance = new WMUserPrincipal(
        WMUserIdentity.Create(UserBasic.GetAnonymousUserInstance()
        , false
        , DateTime.MinValue
        , DateTime.MinValue));
    }

    public WMUserPrincipal(FormsIdentity identity)
    {
      _WMUserIdentity = new WMUserIdentity(identity);
    }
    private WMUserPrincipal(WMUserIdentity userIdentity)
    {
      this._WMUserIdentity = userIdentity;
    }
    #endregion
  }
}
