namespace Workmate.Components.Contracts.Membership
{
  public enum ChangePasswordStatus
  {
    Success,
    SqlException = -1,
    ValidationFailed = 101,
    NoRecordRowAffected,
    UnknownError = 99,
    OldPasswordValidationFailed = 1,
    MinRequiredNonAlphanumericCharactersError = 200,
    PasswordStrengthRegularExpression,
    TooLong,
    TooShort
  }
}
