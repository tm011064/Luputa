using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Components.BusinessTier;
using CommonTools.Components.Localization;
using CommonTools.Core;
using log4net;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Entities.Membership
{
  public class UserBasic : IUserBasic
  {
    private static ILog _Log = LogManager.GetLogger("UserBasic");
    private static Type _UserRoleType = typeof(UserRole);

    private HashSet<string> _UserRoles;
    public List<UserRole> _Roles;

    [BusinessObjectProperty(IsMandatoryForInstance = false)]
    public int UserId { get; set; }

    [BusinessObjectProperty(IsMandatoryForInstance = true)]
    public AccountStatus AccountStatus { get; set; }

    [BusinessObjectProperty(IsMandatoryForInstance = true)
    , BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)
    , BusinessObjectValidation(IsRequired = true, IsRequiredMessageResourceKey = "Error_UserBasic_UserName_Required", MinLength = 3, MaxLength = 32, OutOfRangeErrorMessageResourceKey = "Error_UserBasic_UserName_Length")]
    public string UserName { get; set; }

    [BusinessObjectProperty(IsMandatoryForInstance = true)
    , BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)
    , BusinessObjectValidation(IsRequired = true, IsRequiredMessageResourceKey = "Error_UserBasic_Email_Required", MinLength = 3, MaxLength = 256
       , OutOfRangeErrorMessageResourceKey = "Error_UserBasic_Email_Length", Regex = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", RegexMessageResourceKey = "Error_UserBasic_Email_Regex")]
    public string Email { get; set; }

    [BusinessObjectProperty(IsMandatoryForInstance = false)]
    public DateTime DateCreatedUtc { get; set; }

    [BusinessObjectProperty(IsMandatoryForInstance = false)]
    public DateTime LastActivityDateUtc { get; set; }

    [BusinessObjectProperty(IsMandatoryForInstance = false)]
    public DateTime LastLoginDateUtc { get; internal set; }

    [BusinessObjectProperty(IsMandatoryForInstance = true)]
    public int ProfileImageId { get; set; }

    [BusinessObjectProperty(IsMandatoryForInstance = true)
    , BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)
    , BusinessObjectValidation(IsRequired = true, IsRequiredMessageResourceKey = "Error_UserBasic_TimeZoneInfoId_Required")]
    public string TimeZoneInfoId { get; protected set; }

    public virtual TimeZoneInfo TimeZoneInfo
    {
      get
      {
        if (!string.IsNullOrEmpty(this.TimeZoneInfoId) && TimeZoneUtility.HasTimeZone(this.TimeZoneInfoId))
        {
          return TimeZoneUtility.GetTimeZoneInfo(this.TimeZoneInfoId);
        }
        TimeZoneInfo gMTStandardTimeZone;
        try
        {
          gMTStandardTimeZone = TimeZoneUtility.GetGMTStandardTimeZone();
        }
        catch (TimeZoneNotFoundException ex)
        {
          _Log.Error("Error at TimeZoneUtility.GetGMTStandardTimeZone()", ex);
          throw new CatastrophicException(ex, true);
        }
        return gMTStandardTimeZone;
      }
      set
      {
        if (value == null)
        {
          try
          {
            this.TimeZoneInfoId = TimeZoneUtility.GetGMTStandardTimeZone().Id;
            return;
          }
          catch (TimeZoneNotFoundException ex)
          {
            _Log.Error("Error at TimeZoneUtility.GetGMTStandardTimeZone()", ex);
            throw new CatastrophicException(ex, true);
          }
        }
        this.TimeZoneInfoId = value.Id;
      }
    }

    public HashSet<string> UserRoles
    {
      get { return this._UserRoles; }
      set
      {
        this._UserRoles = value;
        this._Roles = null;
      }
    }
    public List<UserRole> Roles
    {
      get
      {
        if (this._Roles == null)
        {
          this._Roles = new List<UserRole>();
          foreach (string role in new List<string>(this.UserRoles))
          {
            if (Enum.IsDefined(_UserRoleType, role))
              this._Roles.Add((UserRole)Enum.Parse(_UserRoleType, role, true));
          }
        }
        return this._Roles;
      }
    }

    #region role definitions
    public bool IsRegistered { get { return this.IsInRole(UserRole.Registered); } }
    public bool IsSystemAdministrator { get { return this.IsInRole(UserRole.SystemAdministrator); } }
    
    public bool IsAnonymous { get; private set; }
    #endregion

    internal bool IsInRole(string role)
    {
      return _UserRoles.Contains(role);
    }
    internal bool IsInRoles(IEnumerable<string> roles)
    {
      foreach (string role in roles)
        if (_UserRoles.Contains(role))
          return true;

      return false;
    }
    public bool IsInRole(UserRole role)
    {
      return this.IsInRole(role.ToString());
    }
    public bool IsInRoles(List<UserRole> roles)
    {
      return this.IsInRoles((from c in roles
                             select c.ToString()));
    }
    public string GetFormattedRoles()
    {
      StringBuilder stringBuilder = new StringBuilder();
      HashSet<string> userRoles = this.UserRoles;
      foreach (string role in new List<string>(this.UserRoles))
      {
        stringBuilder.Append(role);
        stringBuilder.Append(", ");
      }
      return stringBuilder.ToString().TrimEnd(", ".ToCharArray());
    }

    #region constructors

    #region static
    private static UserBasic _AnonymousUser;
    static UserBasic()
    {
      _AnonymousUser = new UserBasic();
    }
    public static UserBasic GetAnonymousUserInstance() { return _AnonymousUser; }
    #endregion

    public UserBasic(string userName, string email, int profileImageId)
    {
      this.Email = email;
      this.UserName = userName;
      this.AccountStatus = AccountStatus.Pending;
      this.ProfileImageId = profileImageId;
      this.TimeZoneInfoId = TimeZoneUtility.GetGMTStandardTimeZone().Id;
    }

    public UserBasic(IUserBasic userBasic)
      : this(userBasic.UserId, userBasic.UserName, userBasic.Email, userBasic.LastActivityDateUtc, userBasic.AccountStatus, userBasic.DateCreatedUtc, userBasic.LastLoginDateUtc
      , userBasic.ProfileImageId, userBasic.TimeZoneInfoId, userBasic.UserRoles)
    { }
    public UserBasic(int userId, string userName, string email, DateTime lastActivityDateUtc, AccountStatus accountStatus, DateTime dateCreatedUtc, DateTime lastLoginDateUtc
      , int profileImageId, string timeZoneInfoId, IEnumerable<string> userRoles)
    {
      this.IsAnonymous = false;
      this.UserId = userId;
      this.UserName = userName;
      this.LastActivityDateUtc = lastActivityDateUtc;
      this.AccountStatus = accountStatus;
      this.DateCreatedUtc = dateCreatedUtc;
      this.Email = email;
      this.LastLoginDateUtc = lastLoginDateUtc;
      this.ProfileImageId = profileImageId;
      this.TimeZoneInfoId = (timeZoneInfoId ?? TimeZoneUtility.GetGMTStandardTimeZone().Id);
      this.UserRoles = new HashSet<string>(userRoles ?? new HashSet<string>());
    }
    private UserBasic()
    {
      this.IsAnonymous = true;
      this.AccountStatus = AccountStatus.Pending;
      this.TimeZoneInfoId = TimeZoneUtility.GetGMTStandardTimeZone().Id;
      this.UserRoles = new HashSet<string>();
    }
    #endregion
  }
}
