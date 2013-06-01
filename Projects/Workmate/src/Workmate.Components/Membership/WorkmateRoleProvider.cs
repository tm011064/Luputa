using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Workmate.Data;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Configuration;
using log4net;
using System.Web;
using Workmate.Components.Contracts.Membership;
using Workmate.Components.Entities.Membership;
using Workmate.Configuration;

namespace Workmate.Components.Membership
{
  public class WorkmateRoleProvider : RoleProvider, IWorkmateRoleProvider
  {
    #region members
    private static ILog _Log = LogManager.GetLogger("RoleProvider");
    protected IMembershipSettings _MembershipSettings;
    private IApplicationSettings _ApplicationSettings;
    #endregion

    #region properties
    private IDataStore _DataStore;
    protected IDataStore DataStore { get { return _DataStore; } }

    public override string ApplicationName
    {
      get { throw new NotSupportedException(); }
      set { throw new NotSupportedException(); }
    }
    #endregion

    public void CreateRolesIfNotExist(int applicationId, string[] userRoles)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
        {
          foreach (string role in userRoles)
          {
            try
            {
              dataStoreContext.wm_Roles_InsertIfNotExists(applicationId, role, null);
            }
            catch (Exception ex)
            {
              _Log.Error("Error at wm_Roles_InsertIfNotExists", ex);
              throw new DataStoreException(ex, true);
            }
          }
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Roles_InsertIfNotExists", ex);
        throw new DataStoreException(ex, true);
      }
    }

    public override void CreateRole(string roleName)
    {
      throw new NotImplementedException();
    }
    public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
    {
      throw new NotImplementedException();
    }
    public override string[] GetAllRoles()
    {
      throw new NotImplementedException();
    }
    public override bool RoleExists(string roleName)
    {
      throw new NotImplementedException();
    }
    public override bool IsUserInRole(string username, string roleName)
    {
      throw new NotSupportedException();
    }
    public bool IsUserInRole(int userId, UserRole role)
    {
      throw new NotImplementedException();
    }
    public override string[] GetRolesForUser(string username)
    {
      return AuthenticationHelper.GetUserBasicFromAuthenticationCookie(HttpContext.Current).UserRoles.ToArray();
    }
    public string[] GetRoleNamesForUser(int userId)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
        {
          return dataStoreContext.wm_Roles_GetByUserId(userId).ToArray();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Roles_GetByUserId", ex);
        throw new DataStoreException(ex, true);
      }
    }
    public List<UserRole> GetRolesForUser(int userId)
    {
      List<UserRole> list = new List<UserRole>();
      string[] roleNamesForUser = this.GetRoleNamesForUser(userId);
      string[] array = roleNamesForUser;
      for (int i = 0; i < array.Length; i++)
      {
        if (Enum.IsDefined(typeof(UserRole), array[i]))
          list.Add((UserRole)Enum.Parse(typeof(UserRole), array[i]));
      }
      return list;
    }
    public override string[] GetUsersInRole(string roleName)
    {
      throw new NotSupportedException();
    }
    public int[] GetUserIdsInRole(int applicationId, UserRole role)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
        {
          return dataStoreContext.wm_UserRole_GetByRoleName(applicationId, role.ToString()).ToArray();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at UserRole_GetByRoleName", ex);
        throw new DataStoreException(ex, true);
      }
    }
    public override string[] FindUsersInRole(string roleName, string usernameToMatch)
    {
      throw new NotSupportedException();
    }
    public int[] FindUserIdsInRole(UserRole role, string usernameToMatch)
    {
      throw new NotImplementedException();
    }
    public override void AddUsersToRoles(string[] usernames, string[] roleNames)
    {
      throw new NotSupportedException();
    }
    public void AddUserToRole(IUserBasic userBasic, UserRole role)
    {
      this.AddUserToRoles(userBasic, new List<UserRole> { role });
    }
    public void AddUserToRoles(IUserBasic userBasic, List<UserRole> roles)
    {
      this.AddUsersToRoles(new List<int> { userBasic.UserId }, roles);
    }
    public void AddUsersToRoles(List<int> userIds, List<UserRole> roles)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
        {
          dataStoreContext.wm_UserRole_Insert(userIds, roles.Select(c => c.ToString()).ToList());
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at UserRole_Insert", ex);
        throw new DataStoreException(ex, true);
      }
    }
    public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
    {
      throw new NotSupportedException();
    }
    public void RemoveUserFromRole(int userId, UserRole role)
    {
      this.RemoveUsersFromRoles(new List<int> { userId }, new List<UserRole> { role });
    }
    public void RemoveUserFromRoles(int userId, List<UserRole> roles)
    {
      this.RemoveUsersFromRoles(new List<int> { userId }, roles);
    }
    public void RemoveUsersFromRoles(List<int> userIds, List<UserRole> roles)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
        {
          dataStoreContext.wm_UserRole_Delete(userIds, roles.Select(c => c.ToString()).ToList());
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at UserRole_Delete", ex);
        throw new DataStoreException(ex, true);
      }
    }

    #region constructors
    public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
    {
      if (name == null)
      {
        throw new ArgumentNullException("The name of the provider is null.");
      }
      if (name.Length == 0)
      {
        throw new ArgumentException("The name of the provider has a length of zero.");
      }
      base.Initialize(name, config);
    }

    public WorkmateRoleProvider()
      : this(InstanceContainer.MembershipSettings, InstanceContainer.ApplicationSettings, InstanceContainer.DataStore)
    {
      this._MembershipSettings = InstanceContainer.MembershipSettings;
      this._ApplicationSettings = InstanceContainer.ApplicationSettings;
      this._DataStore = InstanceContainer.DataStore;
    }

    internal WorkmateRoleProvider(IMembershipSettings membershipSettings, IApplicationSettings applicationSettings, IDataStore dataStore)
    {
      this._MembershipSettings = membershipSettings;
      this._ApplicationSettings = applicationSettings;
      this._DataStore = dataStore;
    }
    #endregion
  }
}
