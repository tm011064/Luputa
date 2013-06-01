using System;
using System.Collections.Generic;

namespace Workmate.Components.Contracts.Membership
{
  public interface IUserBasic
  {
    int UserId { get; set; }
    string UserName { get; set; }
    string Email { get; set; }
    AccountStatus AccountStatus { get; }
    DateTime DateCreatedUtc { get; set; }
    bool IsAnonymous { get; }
    DateTime LastActivityDateUtc { get; set; }
    DateTime LastLoginDateUtc { get; }
    int ProfileImageId { get; }
    string TimeZoneInfoId { get; }
    HashSet<string> UserRoles { get; }
  }
}
