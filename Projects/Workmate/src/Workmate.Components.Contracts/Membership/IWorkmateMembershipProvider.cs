using System;
using System.Collections.Generic;
using CommonTools.Components.BusinessTier;

namespace Workmate.Components.Contracts.Membership
{
  public interface IWorkmateMembershipProvider
  {
    ValidateUserStatus ValidateUser(int applicationId, string usernameOrEmail, string password, out IUserBasic userBasic);
    int AuthenticationTimeoutInSeconds { get; }
    int PersistentAuthenticationTimeoutInSeconds { get; }
    int LastRecordCheckWindowInSeconds { get; }
    int LastActivityUpdateWindowInSeconds { get; }

    IUserBasic GetUserBasic(int userId, bool updateLastActivity);

    List<IBaseUserModel> GetBaseUserModels(int applicationId, string searchTerm, ref int pageIndex, int pageSize, out int rowCount);
    IUserModel GetUserModel(int userId);

    BusinessObjectActionReport<UserCreateStatus> CreateUser(ref IUserBasic user, string password, List<UserRole> roles, UserNameDisplayMode userNameDisplayMode
      , string firstName, string lastName, Gender gender, out Guid uniqueId, int applicationId);
  }
}
