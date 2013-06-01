using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Components.Contracts.Membership
{
  public enum UserCreateStatus
  {
    Success = 0,
    UsernameAlreadyExists = -1,
    EmailAlreadyExists = -2,
    UnknownError = -100,
    SqlError = -1000,
    ValidationFailed = -1001,
    PasswordMinRequiredNonAlphanumericCharactersError = -2000,
    PasswordStrengthRegularExpression = -2001,
    PasswordTooLong = -2002,
    PasswordTooShort = -2003
  }
}
