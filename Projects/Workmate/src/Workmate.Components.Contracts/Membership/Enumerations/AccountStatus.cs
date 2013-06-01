namespace Workmate.Components.Contracts.Membership
{
  public enum AccountStatus : byte
  {
    Valid = 1,
    Pending = 2,
    Deleted = 3,
    Banned = 4,
    AwaitingEmailVerification = 5,
    Locked = 6,
    LockedAwaitingEmailVerification = 7
  }
}