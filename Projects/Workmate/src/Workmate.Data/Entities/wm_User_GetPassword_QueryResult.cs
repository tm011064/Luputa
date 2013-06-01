using System;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Data.Entities
{
  public class wm_User_GetPassword_QueryResult
  {
    public string Password { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; }
    public DateTime DateCreatedUtc { get; set; }
    public DateTime LastLoginDateUtc { get; set; }
    public int PasswordFormat { get; set; }
    public string PasswordSalt { get; set; }
    public int ProfileImageId { get; set; }
    public string UserName { get; set; }
    public string TimeZoneInfoId { get; set; }
  }
}
