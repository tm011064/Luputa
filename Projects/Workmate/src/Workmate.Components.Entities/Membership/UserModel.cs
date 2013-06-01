using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Contracts.Membership;
using System.Xml.Linq;

namespace Workmate.Components.Entities.Membership
{
  public class UserModel : BaseUserModel, IUserModel
  {
    #region properties
    
    public string UserName { get; set; }

    public AccountStatus Status { get; set; }

    public DateTime DateCreatedUtc { get; set; }

    public DateTime LastLoginDateUtc { get; set; }

    public DateTime LastPasswordChangeDateUtc { get; set; }

    public DateTime LastLockoutDateUtc { get; set; }

    public DateTime LastActivityDateUtc { get; set; }

    public int FailedPasswordAttemptCount { get; set; }

    public XElement ExtraInfo { get; set; }

    public string TimeZoneInfoId { get; set; }

    public Guid UniqueId { get; set; }

    public UserNameDisplayMode UserNameDisplayMode { get; set; }
    
    public Gender Gender { get; set; }
    
    public HashSet<string> UserRoles { get; set; }

    #endregion

    #region constructors
    public UserModel(int userId, string email, string userName, AccountStatus status
      , DateTime dateCreatedUtc, DateTime lastLoginDateUtc, DateTime lastPasswordChangeDateUtc
      , DateTime lastLockoutDateUtc, DateTime lastActivityDateUtc, int failedPasswordAttemptCount
      , XElement extraInfo, string timeZoneInfoId, int profileImageId, Guid uniqueId, UserNameDisplayMode userNameDisplayMode
      , string firstName, string lastName, Gender gender)
      : base(userId, email, profileImageId, firstName, lastName)
    {
      this.UserName = userName;
      this.Status = status;
      this.DateCreatedUtc = dateCreatedUtc;
      this.LastLoginDateUtc = lastLoginDateUtc;
      this.LastPasswordChangeDateUtc = lastPasswordChangeDateUtc;
      this.LastLockoutDateUtc = lastLockoutDateUtc;
      this.LastActivityDateUtc = lastActivityDateUtc;
      this.FailedPasswordAttemptCount = failedPasswordAttemptCount;
      this.ExtraInfo = extraInfo;
      this.TimeZoneInfoId = timeZoneInfoId;
      this.UniqueId = uniqueId;
      this.UserNameDisplayMode = userNameDisplayMode;
      this.Gender = gender;

      this.UserRoles = new HashSet<string>();
    }
    #endregion
  }
}
