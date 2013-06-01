namespace Workmate.Components.Contracts.Membership
{
  public enum ChangeCredentialsStatus
  {
    Success,
    SqlException,
    UsernameAlreadyExists = -1,
    EmailAlreadyExists = -2,
    RecordNotFound = -3,
    UnknownError,
    UserNameNotValid,
    EmailNotValid
  }
}
