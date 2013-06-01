namespace Workmate.Components.Contracts.Membership
{
  public enum ValidateUserStatus
  {
    Valid,
    UserNotFound,
    PasswordAnswerNotCorrect = 3,
    WrongPassword,
    UserIsLockedOut = 99,
    SqlError = -1,
    AccountStatusDeleted = 10,
    AccountStatusBanned,
    AccountStatusPending,
    AccountStatusAwaitingEmail
  }
}