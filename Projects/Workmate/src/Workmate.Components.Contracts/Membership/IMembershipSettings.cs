using System.Web.Security;

namespace Workmate.Components.Contracts.Membership
{
  public interface IMembershipSettings
  {
    string ApplicationName { get; set; }
    bool EnablePasswordReset { get; set; }
    bool EnablePasswordRetrieval { get; set; }
    int MaxInvalidPasswordAttempts { get; set; }
    int MinRequiredNonAlphanumericCharacters { get; set; }
    int MinRequiredPasswordLength { get; set; }
    int PasswordAttemptWindow { get; set; }
    MembershipPasswordFormat PasswordFormat { get; set; }
    string PasswordStrengthRegularExpression { get; set; }
    bool RequiresQuestionAndAnswer { get; set; }
    bool RequiresUniqueEmail { get; set; }
    string EncryptionAlgorithm { get; set; }
    string PasswordPassphrase { get; set; }
    string PasswordInitVector { get; set; }
    int AuthenticationTimeoutInSeconds { get; set; }
    int PersistentAuthenticationTimeoutInSeconds { get; set; }
    int LastActivityUpdateWindowInSeconds { get; set; }
    int LastRecordCheckWindowInSeconds { get; set; }
  }
}
