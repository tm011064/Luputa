using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Components.Contracts.Membership
{
  public interface IBaseUserModel
  {
    int UserId { get;  }
    string Email { get; set; }
    int ProfileImageId { get; }
    string FirstName { get; set; }
    string LastName { get; set; }
  }
}
