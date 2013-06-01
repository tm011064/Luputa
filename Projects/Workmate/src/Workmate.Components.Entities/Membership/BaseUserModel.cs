using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Entities.Membership
{
  public class BaseUserModel : IBaseUserModel
  {
    #region IBaseUserModel Members

    public int UserId { get; set; }

    public string Email { get; set; }

    public int ProfileImageId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    #endregion

    public BaseUserModel(int userId, string email, int profileImageId, string firstName, string lastName)
    {
      this.UserId = userId;
      this.Email = email;
      this.ProfileImageId = profileImageId;
      this.FirstName = firstName;
      this.LastName = lastName;
    }
  }
}
