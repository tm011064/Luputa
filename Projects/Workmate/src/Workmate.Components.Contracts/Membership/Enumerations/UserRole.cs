namespace Workmate.Components.Contracts.Membership
{
  // TODO (Roman): make sure that install scripts include all user roles
  public enum UserRole
  {
    Registered,  
    SystemAdministrator,

    WikiUser,
    WikiEditor,
    WikiAdministrator,

    AccountAdministrator,
    AnnualLeaveAdministrator,
  }
}