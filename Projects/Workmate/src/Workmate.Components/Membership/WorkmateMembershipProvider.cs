using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Collections.Specialized;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Security.Cryptography;
using CommonTools.Components.Security;
using CommonTools.Components.Testing;
using System.Configuration.Provider;
using System.Configuration;
using System.Text.RegularExpressions;
using Workmate.Data.Entities;
using Workmate.Data;
using log4net;
using Workmate.Components.Contracts.Membership;
using Workmate.Components.Entities.Membership;
using Workmate.Configuration;
using CommonTools.Components.BusinessTier;
using CommonTools.Extensions;
using CommonTools.Components.RegularExpressions;

namespace Workmate.Components.Membership
{
  public class WorkmateMembershipProvider : MembershipProvider, IWorkmateMembershipProvider
  {
    private static ILog _Log = LogManager.GetLogger("MembershipProvider");

    protected IMembershipSettings _MembershipSettings;
    private IDataStore _DataStore;
    private IApplicationSettings _ApplicationSettings;

    #region configuration values
    protected IDataStore DataStore { get { return _DataStore; } }

    public override string ApplicationName
    {
      get { return this._MembershipSettings.ApplicationName; }
      set { throw new NotSupportedException(); }
    }
    public override bool EnablePasswordReset { get { return this._MembershipSettings.EnablePasswordReset; } }
    public override bool EnablePasswordRetrieval { get { return this._MembershipSettings.EnablePasswordRetrieval; } }
    public override int MaxInvalidPasswordAttempts { get { return this._MembershipSettings.MaxInvalidPasswordAttempts; } }
    public override int MinRequiredNonAlphanumericCharacters { get { return this._MembershipSettings.MinRequiredNonAlphanumericCharacters; } }
    public override int MinRequiredPasswordLength { get { return this._MembershipSettings.MinRequiredPasswordLength; } }
    public override int PasswordAttemptWindow { get { return this._MembershipSettings.PasswordAttemptWindow; } }
    public override MembershipPasswordFormat PasswordFormat { get { return this._MembershipSettings.PasswordFormat; } }
    public override string PasswordStrengthRegularExpression { get { return this._MembershipSettings.PasswordStrengthRegularExpression; } }
    public override bool RequiresQuestionAndAnswer { get { return this._MembershipSettings.RequiresQuestionAndAnswer; } }
    public override bool RequiresUniqueEmail { get { return this._MembershipSettings.RequiresUniqueEmail; } }
    public string EncryptionAlgorithm { get { return this._MembershipSettings.EncryptionAlgorithm; } }
    public string PasswordPassphrase { get { return this._MembershipSettings.PasswordPassphrase; } }
    public string PasswordInitVector { get { return this._MembershipSettings.PasswordInitVector; } }
    public int AuthenticationTimeoutInSeconds { get { return this._MembershipSettings.AuthenticationTimeoutInSeconds; } }
    public int PersistentAuthenticationTimeoutInSeconds { get { return this._MembershipSettings.PersistentAuthenticationTimeoutInSeconds; } }
    public int LastActivityUpdateWindowInSeconds { get { return this._MembershipSettings.LastActivityUpdateWindowInSeconds; } }
    public int LastRecordCheckWindowInSeconds { get { return this._MembershipSettings.LastRecordCheckWindowInSeconds; } }
    #endregion

    #region private methods
    private string GenerateSalt()
    {
      byte[] array = new byte[16];
      new RNGCryptoServiceProvider().GetBytes(array);
      return Convert.ToBase64String(array);
    }
    private string DecodePassword(string dbpassword, string salt, MembershipPasswordFormat? format)
    {
      switch (format ?? this.PasswordFormat)
      {
        case MembershipPasswordFormat.Clear:
        case MembershipPasswordFormat.Hashed:
          return dbpassword;
        case MembershipPasswordFormat.Encrypted:
          {
            string result = string.Empty;
            if (this.EncryptionAlgorithm.ToUpper() == "DPAPI")
            {
              result = EncryptionManager.DecryptDPAPI(dbpassword);
            }
            else
            {
              if (!Enum.IsDefined(typeof(RijndaelSimpleHashAlgorithm), this.EncryptionAlgorithm))
              {
                throw new ProviderException("Encryption algorithm not valid. Possible values are " + DebugUtility.GetDebugString(typeof(RijndaelSimpleHashAlgorithm)) + " and DPAPI");
              }
              result = EncryptionManager.AES_Simple_Decrypt(dbpassword, this.PasswordPassphrase, salt, this.PasswordInitVector, (RijndaelSimpleHashAlgorithm)Enum.Parse(typeof(RijndaelSimpleHashAlgorithm), this.EncryptionAlgorithm), RijndaelSimpleKeySize.Medium, 3);
            }
            return result;
          }
        default:
          throw new NotImplementedException(("MembershipPasswordFormat " + format) ?? (this.PasswordFormat + " not implemented."));
      }
    }
    private bool CheckPassword(string password, string dbpassword, string salt, MembershipPasswordFormat format)
    {
      switch (format)
      {
        case MembershipPasswordFormat.Clear:
          return dbpassword.Equals(password);
        case MembershipPasswordFormat.Hashed:
          if (!Enum.IsDefined(typeof(SimpleHashAlgorithm), this.EncryptionAlgorithm))
          {
            throw new ProviderException("Membership.HashAlgorithmType not found. Possible values are " + DebugUtility.GetDebugString(typeof(SimpleHashAlgorithm)) + " when using hashed encryption.");
          }
          return EncryptionManager.VerifyHash(password, (SimpleHashAlgorithm)Enum.Parse(typeof(SimpleHashAlgorithm), this.EncryptionAlgorithm), dbpassword);
        case MembershipPasswordFormat.Encrypted:
          return this.DecodePassword(dbpassword, salt, format).Equals(password);
        default:
          throw new NotImplementedException(("MembershipPasswordFormat " + format) ?? (this.PasswordFormat + " not implemented."));
      }
    }
    private string EncodePassword(string pass, string salt, MembershipPasswordFormat format)
    {
      if (format == MembershipPasswordFormat.Clear)
      {
        return pass;
      }
      if (format == MembershipPasswordFormat.Hashed)
      {
        if (!Enum.IsDefined(typeof(SimpleHashAlgorithm), this.EncryptionAlgorithm))
        {
          throw new ProviderException("Encryption algorithm not valid. Possible values are " + DebugUtility.GetDebugString(typeof(SimpleHashAlgorithm)) + " when using hashed encryption.");
        }
        return EncryptionManager.ComputeHash(pass, (SimpleHashAlgorithm)Enum.Parse(typeof(SimpleHashAlgorithm), this.EncryptionAlgorithm), Encoding.Unicode.GetBytes(salt));
      }
      else
      {
        if (this.EncryptionAlgorithm.ToUpper() == "DPAPI")
        {
          return EncryptionManager.EncryptDPAPI(pass);
        }
        if (!Enum.IsDefined(typeof(RijndaelSimpleHashAlgorithm), this.EncryptionAlgorithm))
        {
          throw new ProviderException("Membership.HashAlgorithmType not found. Possible values are " + DebugUtility.GetDebugString(typeof(RijndaelSimpleHashAlgorithm)) + " and DPAPI");
        }
        return EncryptionManager.AES_Simple_Encrypt(pass, this.PasswordPassphrase, salt, this.PasswordInitVector, (RijndaelSimpleHashAlgorithm)Enum.Parse(typeof(RijndaelSimpleHashAlgorithm), this.EncryptionAlgorithm), RijndaelSimpleKeySize.Medium, 3);
      }
    }
    private bool IsPasswordValid(string password, string salt, out ValidatePasswordStatus validatePasswordStatus)
    {
      validatePasswordStatus = ValidatePasswordStatus.Valid;
      int num = 0;
      for (int i = 0; i < password.Length; i++)
      {
        if (!char.IsLetterOrDigit(password, i))
        {
          num++;
        }
      }
      if (num < this.MinRequiredNonAlphanumericCharacters)
      {
        validatePasswordStatus = ValidatePasswordStatus.MinRequiredNonAlphanumericCharactersError;
        return false;
      }
      if (this.PasswordStrengthRegularExpression.Length > 0 && !Regex.IsMatch(password, this.PasswordStrengthRegularExpression))
      {
        validatePasswordStatus = ValidatePasswordStatus.PasswordStrengthRegularExpression;
        return false;
      }
      string text = this.EncodePassword(password, salt, this.PasswordFormat);
      if (text.Length > 128)
      {
        validatePasswordStatus = ValidatePasswordStatus.TooLong;
        return false;
      }
      if (password.Length < this.MinRequiredPasswordLength)
      {
        validatePasswordStatus = ValidatePasswordStatus.TooShort;
        return false;
      }
      return true;
    }
    protected virtual void GetEmailOrUsername(string usernameOrEmail, out string username, out string email)
    {
      string text;
      username = (text = null);
      email = text;
      if (Regex.IsMatch(usernameOrEmail, "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", RegexOptions.IgnoreCase))
      {
        email = usernameOrEmail;
        return;
      }
      username = usernameOrEmail;
    }

    #endregion

    #region public methods
    public virtual IUserModel GetUserModel(int userId)
    {
      IUserModel record;
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
          record = dataStoreContext.wm_Users_GetUserModel(userId);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Users_GetUserModel", ex);
        throw new DataStoreException(ex, true);
      }
      return record;
    }

    public ChangePasswordStatus ChangePassword(int applicationId, IUserBasic userBasic, string oldPassword, string newPassword)
    {
      wm_User_GetPassword_QueryResult getPasswordResult = null;
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
        {
          getPasswordResult = dataStoreContext.wm_Users_GetPassword(applicationId, userBasic.UserName, null);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at Users_GetPassword", ex);
        throw new DataStoreException(ex, true);
      }

      if (!this.CheckPassword(oldPassword, getPasswordResult.Password, getPasswordResult.PasswordSalt, ((MembershipPasswordFormat)getPasswordResult.PasswordFormat)))
        return ChangePasswordStatus.OldPasswordValidationFailed;

      string text = this.GenerateSalt();
      ValidatePasswordStatus result = ValidatePasswordStatus.Valid;
      if (!this.IsPasswordValid(newPassword, text, out result))
        return (ChangePasswordStatus)result;

      ChangePasswordStatus changePasswordStatus;
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
        {
          changePasswordStatus = dataStoreContext.wm_Users_SetPassword(userBasic.UserId, this.EncodePassword(newPassword, text, this.PasswordFormat), text, (byte)this.PasswordFormat) == 0
            ? ChangePasswordStatus.NoRecordRowAffected
            : ChangePasswordStatus.Success;
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at ChangePassword.Users_SetPassword", ex);
        throw new DataStoreException(ex, true);
      }
      return changePasswordStatus;
    }
    public ChangePasswordStatus ResetPassword(int applicationId, IUserBasic userBasic, string newPassword)
    {
      string text = this.GenerateSalt();
      ValidatePasswordStatus result = ValidatePasswordStatus.Valid;
      if (!this.IsPasswordValid(newPassword, text, out result))
        return (ChangePasswordStatus)result;

      ChangePasswordStatus changePasswordStatus;
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
        {
          changePasswordStatus = dataStoreContext.wm_Users_SetPassword(userBasic.UserId, this.EncodePassword(newPassword, text, this.PasswordFormat), text, (byte)this.PasswordFormat) == 0
            ? ChangePasswordStatus.NoRecordRowAffected
            : ChangePasswordStatus.Success;
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at ResetPassword.Users_SetPassword", ex);
        throw new DataStoreException(ex, true);
      }

      if (changePasswordStatus == ChangePasswordStatus.Success)
      {
        _Log.InfoFormat("User {0} changed password", userBasic.UserId);
      }

      return changePasswordStatus;
    }

    public BusinessObjectActionReport<UserCreateStatus> CreateUser(ref IUserBasic user, string password, List<UserRole> roles
      , UserNameDisplayMode userNameDisplayMode, string firstName, string lastName, Workmate.Components.Contracts.Membership.Gender gender, out Guid uniqueId, int applicationId)
    {
      uniqueId = Guid.NewGuid();
      UserCreateStatus userCreateStatus = UserCreateStatus.UnknownError;

      BusinessObjectActionReport<UserCreateStatus> businessObjectActionReport = new BusinessObjectActionReport<UserCreateStatus>(UserCreateStatus.UnknownError);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(user);
      if (!businessObjectActionReport.ValidationResult.IsValid)
      {
        return businessObjectActionReport;
      }
      string text = this.GenerateSalt();
      ValidatePasswordStatus status = ValidatePasswordStatus.Valid;
      if (!this.IsPasswordValid(password, text, out status))
      {
        businessObjectActionReport.Status = (UserCreateStatus)status;
        return businessObjectActionReport;
      }

      int userId;
      int returnValue;
      DateTime dateCreatedUtc;

      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
        {
          returnValue = dataStoreContext.wm_Users_Insert(applicationId, user.UserName, user.Email, this.EncodePassword(password, text, this.PasswordFormat)
            , text, (int)this.PasswordFormat, user.AccountStatus, roles.Select(c => c.ToString()).ToList(), user.ProfileImageId, uniqueId, userNameDisplayMode
            , user.TimeZoneInfoId, firstName, lastName, gender, out userId, out dateCreatedUtc);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at Users_Insert", ex);

        businessObjectActionReport.Status = UserCreateStatus.SqlError;
        return businessObjectActionReport;
      }

      if (returnValue == 0)
      {
        user.UserId = userId;
        user.LastActivityDateUtc = dateCreatedUtc;
        user.DateCreatedUtc = dateCreatedUtc;
        userCreateStatus = UserCreateStatus.Success;
      }
      else
      {
        switch (returnValue)
        {
          case -1: userCreateStatus = UserCreateStatus.UsernameAlreadyExists; break;
          case -2: userCreateStatus = UserCreateStatus.EmailAlreadyExists; break;

          default:
            _Log.Error("Error at Users_Insert, ErrorCode: " + returnValue);
            userCreateStatus = UserCreateStatus.SqlError; break;
        }
      }

      businessObjectActionReport.Status = userCreateStatus;

      if (businessObjectActionReport.Status == UserCreateStatus.Success)
      {
        _Log.InfoFormat("Successfully created user {0}.", user.UserId);
      }

      return businessObjectActionReport;
    }

    public virtual IUserBasic GetUserBasicByUniqueId(Guid uniqueId, bool updateLastActivity)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
          return dataStoreContext.wm_Users_GetUserBasicByUniqueId(uniqueId, updateLastActivity);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at Users_GetUserBasicByUniqueId", ex);
        throw new DataStoreException(ex, true);
      }
    }
    public virtual IUserBasic GetUserBasicByEmail(int applicationId, string email, bool updateLastActivity)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
          return dataStoreContext.wm_Users_GetUserBasicByEmail(email, applicationId, updateLastActivity);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at Users_GetUserBasicByEmail", ex);
        throw new DataStoreException(ex, true);
      }
    }
    public virtual IUserBasic GetUserBasicByUserName(int applicationId, string username, bool updateLastActivity)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
          return dataStoreContext.wm_Users_GetUserBasicByUserName(username, applicationId, updateLastActivity);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at Users_GetUserBasicByUserName", ex);
        throw new DataStoreException(ex, true);
      }
    }
    public virtual IUserBasic GetUserBasic(int userId, bool updateLastActivity)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
          return dataStoreContext.wm_Users_GetUserBasicByUserId(userId, updateLastActivity);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at Users_GetUserBasicByUserId", ex);
        throw new DataStoreException(ex, true);
      }
    }
    public virtual Dictionary<int, IUserBasic> GetAllUserBasic(int applicationId)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
          return dataStoreContext.wm_Users_GetAllUserBasics(applicationId);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at Users_GetAllUserBasics", ex);
        throw new DataStoreException(ex, true);
      }
    }

    public int GetNumberOfUsersOnline(int applicationId)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
          return dataStoreContext.wm_Users_GetNumberOfUsersOnline(applicationId, System.Web.Security.Membership.UserIsOnlineTimeWindow);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at Users_GetAllUserBasics", ex);
        throw new DataStoreException(ex, true);
      }
    }

    public ChangeCredentialsStatus ChangeUserName(IUserBasic userBasic, string newUserName)
    {
      string userName = newUserName.RemoveMaliciousTags()
                                   .RemoveScriptTags()
                                   .RemoveMaliciousSQLCharacters()
                                   .DefuseScriptTags();

      if (userName != newUserName)
      {
        return ChangeCredentialsStatus.UserNameNotValid;
      }

      ChangeCredentialsStatus changeCredentialsStatus;
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
          changeCredentialsStatus = dataStoreContext.wm_Users_UpdateUserName(userBasic.UserId, newUserName);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at Users_UpdateUserName", ex);
        throw new DataStoreException(ex, true);
      }

      if (changeCredentialsStatus == ChangeCredentialsStatus.Success)
      {
        _Log.InfoFormat("User {0} changed username from {1} to {2}.", userBasic.UserId, userBasic.UserName, newUserName);
        userBasic.UserName = newUserName;
      }
      return changeCredentialsStatus;
    }
    public ChangeCredentialsStatus ChangeEmail(IUserBasic userBasic, string newEmail)
    {
      if (!ValidationExpressions.IsValidEmail(newEmail))
        return ChangeCredentialsStatus.EmailNotValid;

      ChangeCredentialsStatus changeCredentialsStatus;
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
          changeCredentialsStatus = dataStoreContext.wm_Users_UpdateEmail(userBasic.UserId, newEmail);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at Users_UpdateUserName", ex);
        throw new DataStoreException(ex, true);
      }

      if (changeCredentialsStatus == ChangeCredentialsStatus.Success)
      {
        _Log.InfoFormat("User {0} changed email from {1} to {2}.", userBasic.UserId, userBasic.Email, newEmail);
        userBasic.Email = newEmail;
      }
      return changeCredentialsStatus;
    }

    public ValidateUserStatus ValidateUser(int applicationId, string usernameOrEmail, string password, out IUserBasic userBasic)
    {
      userBasic = UserBasic.GetAnonymousUserInstance();

      wm_User_GetPassword_QueryResult getPasswordResult = null;
      string userName = null;
      string email = null;
      this.GetEmailOrUsername(usernameOrEmail, out userName, out email);

      using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
      {
        // first, get the password information
        try { getPasswordResult = dataStoreContext.wm_Users_GetPassword(applicationId, userName, email); }
        catch (Exception ex)
        {
          _Log.Error("Error at Users_GetPassword", ex);
          return ValidateUserStatus.SqlError;
        }
        if (getPasswordResult == null)
          return ValidateUserStatus.UserNotFound;

        if (getPasswordResult.AccountStatus == AccountStatus.Locked)
          return ValidateUserStatus.UserIsLockedOut;

        if (getPasswordResult.AccountStatus == AccountStatus.Valid || getPasswordResult.AccountStatus == AccountStatus.AwaitingEmailVerification)
        {
          int returnValue;

          DateTime? lastActivityDateUtc = null;
          DateTime? lastLoginDateUtc = null;
          AccountStatus? status = null;
          int? failedPasswordAttemptCount = null;
          DateTime? lastLockoutDateUtc = null;

          if (!this.CheckPassword(password, getPasswordResult.Password, getPasswordResult.PasswordSalt, ((MembershipPasswordFormat)getPasswordResult.PasswordFormat)))
          {// wrong password
            try
            {
              returnValue = dataStoreContext.wm_Users_UpdateUserInfo(getPasswordResult.UserId, false, false, this.MaxInvalidPasswordAttempts
                , out lastActivityDateUtc, out lastLoginDateUtc, out status, out failedPasswordAttemptCount, out lastLockoutDateUtc);
            }
            catch (Exception ex)
            {
              _Log.Error("Error at Users_GetPassword", ex);
              return ValidateUserStatus.SqlError;
            }
            if (returnValue < 0)
              _Log.Error("Error at Users_UpdateUserInfo, ErrorCode: " + returnValue);

            _Log.InfoFormat("User {0} entered an invalid password.", getPasswordResult.UserId);
            return ValidateUserStatus.WrongPassword; // we return wrong password for now, if the user was locked out due to too many invalid password attempts, we will get this information at the next login attempt
          }
          else
          {
            try
            {
              returnValue = dataStoreContext.wm_Users_UpdateUserInfo(getPasswordResult.UserId, true, true, this.MaxInvalidPasswordAttempts
                , out lastActivityDateUtc, out lastLoginDateUtc, out status, out failedPasswordAttemptCount, out lastLockoutDateUtc);
            }
            catch (Exception ex)
            {
              _Log.Error("Error at Users_GetPassword", ex);
              return ValidateUserStatus.SqlError;
            }
            if (returnValue < 0)
            {
              _Log.Error("Error at Users_UpdateUserInfo, ErrorCode: " + returnValue);
              return ValidateUserStatus.SqlError;
            }
            else
            {
              try
              {
                userBasic = new UserBasic(
                  getPasswordResult.UserId
                  , getPasswordResult.UserName
                  , getPasswordResult.Email
                  , lastActivityDateUtc.Value
                  , status.Value
                  , getPasswordResult.DateCreatedUtc
                  , lastLoginDateUtc.Value
                  , getPasswordResult.ProfileImageId
                  , getPasswordResult.TimeZoneInfoId
                  , dataStoreContext.wm_Roles_GetByUserId(getPasswordResult.UserId));
              }
              catch (Exception ex)
              {
                _Log.Error("Error at Roles_GetByUserId", ex);
                return ValidateUserStatus.SqlError;
              }

              switch (status.Value)
              {
                case AccountStatus.AwaitingEmailVerification: return ValidateUserStatus.AccountStatusAwaitingEmail;

                case AccountStatus.LockedAwaitingEmailVerification:
                case AccountStatus.Locked: return ValidateUserStatus.UserIsLockedOut;

                case AccountStatus.Valid:
                  _Log.DebugFormat("User {0} successfully validated", userBasic.UserId);
                  return ValidateUserStatus.Valid;

                default: return ValidateUserStatus.SqlError;
              }
            }
          }
        }
        else
        {
          switch (getPasswordResult.AccountStatus)
          {
            case AccountStatus.Pending: return ValidateUserStatus.AccountStatusPending;
            case AccountStatus.Deleted: return ValidateUserStatus.AccountStatusDeleted;
            case AccountStatus.Banned: return ValidateUserStatus.AccountStatusBanned;

            case AccountStatus.LockedAwaitingEmailVerification:
            case AccountStatus.Locked: return ValidateUserStatus.UserIsLockedOut;

            default: return ValidateUserStatus.SqlError;
          }
        }
      }
    }

    public bool UnlockUser(int userId)
    {
      bool success;
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
          success = dataStoreContext.wm_Users_UnlockUser(userId) == 1;
      }
      catch (Exception ex)
      {
        _Log.Error("Error at Users_GetPassword", ex);
        return false;
      }
      if (success)
      {
        _Log.InfoFormat("User {0} was successfully unlocked", userId);
      }
      return success;
    }

    public List<IBaseUserModel> GetBaseUserModels(int applicationId, string searchTerm, ref int pageIndex, int pageSize, out int rowCount)
    {
      List<IBaseUserModel> records;
      try
      {
        using (IDataStoreContext dataStoreContext = this.DataStore.CreateContext())
          records = dataStoreContext.wm_Users_GetBaseUserModels(applicationId, searchTerm, ref pageIndex, pageSize, out rowCount );
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Users_GetBaseUserModels", ex);

        rowCount = 0;
        return new List<IBaseUserModel>();
      }
      return records;
    }

    #region unused

    public override bool ValidateUser(string username, string password) { throw new NotImplementedException(); }
    public override int GetNumberOfUsersOnline() { throw new NotImplementedException(); }
    public override string ResetPassword(string username, string answer) { throw new NotSupportedException(); }
    public override bool ChangePassword(string username, string oldPassword, string newPassword) { throw new NotSupportedException(); }
    public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer) { throw new NotSupportedException(); }
    public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status) { throw new NotSupportedException(); }
    public override bool DeleteUser(string username, bool deleteAllRelatedData) { throw new NotSupportedException(); }
    public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords) { throw new NotSupportedException(); }
    public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords) { throw new NotSupportedException(); }
    public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords) { throw new NotSupportedException(); }
    public override string GetPassword(string username, string answer) { throw new NotSupportedException(); }
    public override MembershipUser GetUser(string username, bool userIsOnline) { throw new NotSupportedException(); }
    public override MembershipUser GetUser(object providerUserKey, bool userIsOnline) { throw new NotSupportedException(); }
    public override string GetUserNameByEmail(string email) { throw new NotSupportedException(); }
    public override bool UnlockUser(string userName) { throw new NotSupportedException(); }
    public override void UpdateUser(MembershipUser user) { throw new NotSupportedException(); }
    #endregion

    #endregion

    #region constructors
    /// <summary>
    /// Initializes the provider.
    /// </summary>
    /// <param name="name">The friendly name of the provider.</param>
    /// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// The name of the provider is null.
    ///   </exception>
    ///   
    /// <exception cref="T:System.ArgumentException">
    /// The name of the provider has a length of zero.
    ///   </exception>
    ///   
    /// <exception cref="T:System.InvalidOperationException">
    /// An attempt is made to call <see cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)"/> on a provider after the provider has already been initialized.
    ///   </exception>
    public override void Initialize(string name, NameValueCollection config)
    {
      if (name == null)
        throw new ArgumentNullException("The name of the provider is null.");

      if (name.Length == 0)
        throw new ArgumentException("The name of the provider has a length of zero.");

      base.Initialize(name, config);

      System.Configuration.Configuration configuration = WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
      if (this.PasswordFormat != MembershipPasswordFormat.Clear && (MachineKeySection)configuration.GetSection("system.web/machineKey") == null)
      {// we need the same machine key across web farms for encryption
        throw new ProviderException("system.web/machineKey not found.");
      }
    }

    public WorkmateMembershipProvider()
      : this(InstanceContainer.MembershipSettings, InstanceContainer.ApplicationSettings, InstanceContainer.DataStore)
    {
      this._MembershipSettings = InstanceContainer.MembershipSettings;
      this._ApplicationSettings = InstanceContainer.ApplicationSettings;
      this._DataStore = InstanceContainer.DataStore;
    }

    internal WorkmateMembershipProvider(IMembershipSettings membershipSettings, IApplicationSettings applicationSettings, IDataStore dataStore)
    {
      this._MembershipSettings = membershipSettings;
      this._ApplicationSettings = applicationSettings;
      this._DataStore = dataStore;
    }

    #endregion
  }
}
