using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Workmate.Components.Membership;
using System.Web.Security;
using Workmate.Configuration;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Contracts.Membership;
using CommonTools.Components.Localization;
using CommonTools.Components.Testing;
using Workmate.Data;
using Workmate.Components;
using Workmate.Components.Contracts;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.Membership
{
  [TestFixture]
  public class Test_WorkmateMembershipProvider : TestSetup
  {
    #region private methods
    internal static IUserBasic CreateUser(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application, DummyDataManager dummyDataManager)
    {
      IMembershipSettings membershipSettings = InstanceContainer.MembershipSettings;

      WorkmateRoleProvider roleProvider = new WorkmateRoleProvider();
      WorkmateMembershipProvider membershipProvider = new WorkmateMembershipProvider();

      return CreateUser(applicationSettings, application, dummyDataManager, roleProvider, membershipProvider, "password", AccountStatus.Valid);
    }
    internal static IUserBasic CreateUser(IApplicationSettings applicationSettings, IApplication application
      , DummyDataManager dummyDataManager, WorkmateRoleProvider roleProvider
      , WorkmateMembershipProvider membershipProvider, string password, AccountStatus accountStatus)
    {
      DummyUser user = dummyDataManager.GetDummy();

      string firstName = user.Firstname;
      string lastName = user.Surname;
      TimeZoneInfo timeZoneInfo = TimeZoneUtility.GetGMTStandardTimeZone();

      IUserBasic userBasic = new UserBasic(user.Email, user.Email, 1)
      {
        AccountStatus = accountStatus,
        TimeZoneInfo = timeZoneInfo
      };

      Guid uniqueId;
      List<UserRole> userRoles = new List<UserRole>() { UserRole.SystemAdministrator, UserRole.Registered };
      UserCreateStatus userCreateStatus = membershipProvider.CreateUser(ref userBasic, password, userRoles, UserNameDisplayMode.FullName
         , firstName, lastName
         , Workmate.Components.Contracts.Membership.Gender.Male
         , out uniqueId, application.ApplicationId).Status;

      Assert.AreEqual(UserCreateStatus.Success, userCreateStatus);
      Assert.Greater(userBasic.UserId, 0);

      return membershipProvider.GetUserBasic(userBasic.UserId, false);
    }
    #endregion

    #region crete user
    [Test]
    public void Test_CreateUser2()
    {
      IApplicationSettings applicationSettings = Workmate.Components.InstanceContainer.ApplicationSettings;

      WorkmateRoleProvider roleProvider = new WorkmateRoleProvider();
      WorkmateMembershipProvider membershipProvider = new WorkmateMembershipProvider();

      //DummyUser user = this.DummyDataManager.GetDummy();

      string firstName = "admin";
      string lastName = "admin";
      string password = "123";
      AccountStatus accountStatus = AccountStatus.Valid;
      TimeZoneInfo timeZoneInfo = TimeZoneUtility.GetGMTStandardTimeZone();

      string email = "admin" + this.Random.Next(100000, 1000000) + "@workmate.com";
      IUserBasic userBasic = new UserBasic(email, email, 1)
      {
        AccountStatus = accountStatus,
        TimeZoneInfo = timeZoneInfo
      };

      Guid uniqueId;
      List<UserRole> userRoles = new List<UserRole>() { UserRole.SystemAdministrator, UserRole.Registered };
      UserCreateStatus userCreateStatus = membershipProvider.CreateUser(ref userBasic, password
        , userRoles, UserNameDisplayMode.FullName
        , firstName, lastName, Workmate.Components.Contracts.Membership.Gender.Male, out uniqueId, this.Application.ApplicationId).Status;

      Assert.AreEqual(UserCreateStatus.Success, userCreateStatus);
      Assert.Greater(userBasic.UserId, 0);

      userBasic = membershipProvider.GetUserBasic(userBasic.UserId, false);
    }

    [Test]
    public void Test_CreateUser()
    {
      IApplicationSettings applicationSettings = Workmate.Components.InstanceContainer.ApplicationSettings;

      WorkmateRoleProvider roleProvider = new WorkmateRoleProvider();
      WorkmateMembershipProvider membershipProvider = new WorkmateMembershipProvider();

      DummyUser user = this.DummyDataManager.GetDummy();

      string firstName = user.Firstname;
      string lastName = user.Surname;
      string password = "123";
      AccountStatus accountStatus = AccountStatus.Valid;
      TimeZoneInfo timeZoneInfo = TimeZoneUtility.GetGMTStandardTimeZone();

      IUserBasic userBasic = new UserBasic(user.Email, user.Email, 1)
      {
        AccountStatus = accountStatus,
        TimeZoneInfo = timeZoneInfo
      };

      Guid uniqueId;
      List<UserRole> userRoles = new List<UserRole>() { UserRole.SystemAdministrator, UserRole.Registered };
      UserCreateStatus userCreateStatus = membershipProvider.CreateUser(ref userBasic, password, userRoles, UserNameDisplayMode.FullName
         , firstName, lastName
         , DebugUtility.GetRandomEnum<Workmate.Components.Contracts.Membership.Gender>(this.Random)
         , out uniqueId, this.Application.ApplicationId).Status;

      Assert.AreEqual(UserCreateStatus.Success, userCreateStatus);
      Assert.Greater(userBasic.UserId, 0);

      userBasic = membershipProvider.GetUserBasic(userBasic.UserId, false);

      Assert.AreEqual(user.Email, userBasic.UserName);
      Assert.AreEqual(user.Email, userBasic.Email);
      Assert.AreEqual(accountStatus, userBasic.AccountStatus);
      Assert.AreEqual(timeZoneInfo.Id, userBasic.TimeZoneInfoId);
    }
    #endregion

    #region change passwords
    [Test]
    public void Test_ChangePassword_Clear()
    {
      Test_ChangePassword(MembershipPasswordFormat.Clear);
    }
    [Test]
    public void Test_ChangePassword_Hashed()
    {
      Test_ChangePassword(MembershipPasswordFormat.Hashed);
    }
    [Test]
    public void Test_ChangePassword_Encrypted()
    {
      Test_ChangePassword(MembershipPasswordFormat.Encrypted);
    }
    private void Test_ChangePassword(MembershipPasswordFormat membershipPasswordFormat)
    {
      IMembershipSettings membershipSettings = Workmate.Components.InstanceContainer.MembershipSettings;
      membershipSettings.PasswordFormat = membershipPasswordFormat;

      IApplicationSettings applicationSettings = Workmate.Components.InstanceContainer.ApplicationSettings;

      WorkmateRoleProvider roleProvider = new WorkmateRoleProvider();
      WorkmateMembershipProvider membershipProvider = new WorkmateMembershipProvider();

      string password = "123";
      string newPassword = "456";

      IUserBasic userBasic = CreateUser(applicationSettings, this.Application, this.DummyDataManager, roleProvider, membershipProvider, password, AccountStatus.Valid);

      IUserBasic validatedUserBasic;
      ValidateUserStatus validateUserStatus = membershipProvider.ValidateUser(this.Application.ApplicationId, userBasic.Email, password, out validatedUserBasic);
      Assert.AreEqual(ValidateUserStatus.Valid, validateUserStatus);

      ChangePasswordStatus changePasswordStatus = membershipProvider.ChangePassword(this.Application.ApplicationId, userBasic, password, newPassword);
      Assert.AreEqual(ChangePasswordStatus.Success, changePasswordStatus);

      validateUserStatus = membershipProvider.ValidateUser(this.Application.ApplicationId, userBasic.Email, newPassword, out validatedUserBasic);
      Assert.AreEqual(ValidateUserStatus.Valid, validateUserStatus);
    }
    #endregion

    #region change passwords
    [Test]
    public void Test_ResetPassword()
    {
      IMembershipSettings membershipSettings = Workmate.Components.InstanceContainer.MembershipSettings;
      IApplicationSettings applicationSettings = Workmate.Components.InstanceContainer.ApplicationSettings;

      WorkmateRoleProvider roleProvider = new WorkmateRoleProvider();
      WorkmateMembershipProvider membershipProvider = new WorkmateMembershipProvider();

      string password = "123";
      string newPassword = "456";

      IUserBasic userBasic = CreateUser(applicationSettings, this.Application, this.DummyDataManager, roleProvider, membershipProvider, password, AccountStatus.Valid);

      IUserBasic validatedUserBasic;
      ValidateUserStatus validateUserStatus = membershipProvider.ValidateUser(this.Application.ApplicationId, userBasic.Email, password, out validatedUserBasic);
      Assert.AreEqual(ValidateUserStatus.Valid, validateUserStatus);

      ChangePasswordStatus changePasswordStatus = membershipProvider.ResetPassword(this.Application.ApplicationId, userBasic, newPassword);
      Assert.AreEqual(ChangePasswordStatus.Success, changePasswordStatus);

      validateUserStatus = membershipProvider.ValidateUser(this.Application.ApplicationId, userBasic.Email, newPassword, out validatedUserBasic);
      Assert.AreEqual(ValidateUserStatus.Valid, validateUserStatus);
    }
    #endregion

    #region change credentials

    [Test]
    public void Test_ChangeUserName()
    {
      IMembershipSettings membershipSettings = Workmate.Components.InstanceContainer.MembershipSettings;
      IApplicationSettings applicationSettings = Workmate.Components.InstanceContainer.ApplicationSettings;

      WorkmateRoleProvider roleProvider = new WorkmateRoleProvider();
      WorkmateMembershipProvider membershipProvider = new WorkmateMembershipProvider();

      string password = "123";

      IUserBasic userBasic = CreateUser(applicationSettings, this.Application, this.DummyDataManager, roleProvider, membershipProvider, password, AccountStatus.Valid);

      userBasic = membershipProvider.GetUserBasicByUserName(this.Application.ApplicationId, userBasic.UserName, false);
      Assert.IsNotNull(userBasic);

      DummyUser dummy = this.DummyDataManager.GetDummy();
      Assert.AreNotEqual(userBasic.UserName, dummy.Username);
      membershipProvider.ChangeUserName(userBasic, dummy.Username);
      Assert.AreEqual(userBasic.UserName, dummy.Username);
      userBasic = membershipProvider.GetUserBasicByUserName(this.Application.ApplicationId, userBasic.UserName, false);
      Assert.AreEqual(userBasic.UserName, dummy.Username);
    }
    [Test]
    public void Test_ChangeEmail()
    {
      IMembershipSettings membershipSettings = Workmate.Components.InstanceContainer.MembershipSettings;
      IApplicationSettings applicationSettings = Workmate.Components.InstanceContainer.ApplicationSettings;

      WorkmateRoleProvider roleProvider = new WorkmateRoleProvider();
      WorkmateMembershipProvider membershipProvider = new WorkmateMembershipProvider();

      string password = "123";

      IUserBasic userBasic = CreateUser(applicationSettings, this.Application, this.DummyDataManager, roleProvider, membershipProvider, password, AccountStatus.Valid);

      userBasic = membershipProvider.GetUserBasicByUserName(this.Application.ApplicationId, userBasic.UserName, false);
      Assert.IsNotNull(userBasic);

      DummyUser dummy = this.DummyDataManager.GetDummy();
      Assert.AreNotEqual(userBasic.Email, dummy.Email);
      membershipProvider.ChangeEmail(userBasic, dummy.Email);
      Assert.AreEqual(userBasic.Email, dummy.Email);
      userBasic = membershipProvider.GetUserBasicByEmail(this.Application.ApplicationId, userBasic.Email, false);
      Assert.AreEqual(userBasic.Email, dummy.Email);
    }


    #endregion

    #region locking
    [Test]
    public void Test_UserLocking_Valid()
    {
      Test_UserLocking_Valid(AccountStatus.Valid);
    }
    [Test]
    public void Test_UserLocking_AwatingEmailVerificataion()
    {
      Test_UserLocking_Valid(AccountStatus.AwaitingEmailVerification);
    }
    private void Test_UserLocking_Valid(AccountStatus accountStatus)
    {
      IMembershipSettings membershipSettings = Workmate.Components.InstanceContainer.MembershipSettings;
      membershipSettings.MaxInvalidPasswordAttempts = 3;

      IApplicationSettings applicationSettings = Workmate.Components.InstanceContainer.ApplicationSettings;

      WorkmateRoleProvider roleProvider = new WorkmateRoleProvider();
      WorkmateMembershipProvider membershipProvider = new WorkmateMembershipProvider();

      string password = "123";

      IUserBasic userBasic = CreateUser(applicationSettings, this.Application, this.DummyDataManager, roleProvider, membershipProvider, password, accountStatus);

      IUserBasic validatedUser;
      ValidateUserStatus validateUserStatus;
      for (int i = 0; i < membershipSettings.MaxInvalidPasswordAttempts; i++)
      {
        validateUserStatus = membershipProvider.ValidateUser(this.Application.ApplicationId, userBasic.UserName, "465", out validatedUser);
        Assert.AreEqual(ValidateUserStatus.WrongPassword, validateUserStatus);
      }
      // by the next attempt we expect the user to be locked
      validateUserStatus = membershipProvider.ValidateUser(this.Application.ApplicationId, userBasic.UserName, "123", out validatedUser);
      Assert.AreEqual(ValidateUserStatus.UserIsLockedOut, validateUserStatus);

      // test unlock
      Assert.IsTrue(membershipProvider.UnlockUser(userBasic.UserId));
      userBasic = membershipProvider.GetUserBasic(userBasic.UserId, false);
      Assert.AreEqual(accountStatus, userBasic.AccountStatus);
    }
    #endregion

    #region gets

    [Test]
    public void Test_GetUserModel()
    {
      throw new NotImplementedException("TODO (Roman): implement baseusermodel");
    }
    [Test]
    public void Test_GetBaseUserModel()
    {
      throw new NotImplementedException("TODO (Roman): implement baseusermodel");
    }
    #endregion
  }
}
