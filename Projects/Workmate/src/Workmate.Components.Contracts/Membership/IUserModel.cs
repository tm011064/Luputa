using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Workmate.Components.Contracts.Membership
{
  public interface IUserModel : IBaseUserModel
  {
    DateTime DateCreatedUtc { get; set; }
    string Email { get; set; }
    XElement ExtraInfo { get; set; }
    int FailedPasswordAttemptCount { get; set; }
    string FirstName { get; set; }
    DateTime LastActivityDateUtc { get; set; }
    DateTime LastLockoutDateUtc { get; set; }
    DateTime LastLoginDateUtc { get; set; }
    string LastName { get; set; }
    DateTime LastPasswordChangeDateUtc { get; set; }
    int ProfileImageId { get; set; }
    AccountStatus Status { get; set; }
    string TimeZoneInfoId { get; set; }
    Guid UniqueId { get; set; }
    int UserId { get; set; }
    string UserName { get; set; }
    UserNameDisplayMode UserNameDisplayMode { get; set; }
    Gender Gender { get; set; }
    HashSet<string> UserRoles { get; }
  }
}
