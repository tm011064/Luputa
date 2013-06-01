namespace Workmate.Components.Contracts.Membership
{
  public enum ValidatePasswordStatus
  {
    Valid,
    MinRequiredNonAlphanumericCharactersError = 200,
    PasswordStrengthRegularExpression,
    TooLong,
    TooShort
  }
}