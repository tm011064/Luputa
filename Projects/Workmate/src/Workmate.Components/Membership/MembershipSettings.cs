using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.Security;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Membership
{
  public class MembershipSettings : IMembershipSettings
  {
    #region properties
    public virtual string ApplicationName { get; set; }
    public virtual bool EnablePasswordReset { get; set; }
    public virtual bool EnablePasswordRetrieval { get; set; }
    public virtual int MaxInvalidPasswordAttempts { get; set; }
    public virtual int MinRequiredNonAlphanumericCharacters { get; set; }
    public virtual int MinRequiredPasswordLength { get; set; }
    public virtual int PasswordAttemptWindow { get; set; }
    public virtual MembershipPasswordFormat PasswordFormat { get; set; }
    public virtual string PasswordStrengthRegularExpression { get; set; }
    public virtual bool RequiresQuestionAndAnswer { get; set; }
    public virtual bool RequiresUniqueEmail { get; set; }
    public virtual string EncryptionAlgorithm { get; set; }
    public virtual string PasswordPassphrase { get; set; }
    public virtual string PasswordInitVector { get; set; }
    public virtual int AuthenticationTimeoutInSeconds { get; set; }
    public virtual int PersistentAuthenticationTimeoutInSeconds { get; set; }
    public virtual int LastActivityUpdateWindowInSeconds { get; set; }
    public virtual int LastRecordCheckWindowInSeconds { get; set; }
    #endregion
    
    #region constructors
    public MembershipSettings(NameValueCollection config)
    {
      if (config != null)
      {
        this.ApplicationName = config["applicationName"] ?? Assembly.GetExecutingAssembly().FullName;
        this.EnablePasswordReset = bool.Parse(config["enablePasswordReset"] ?? "false");
        this.EnablePasswordRetrieval = bool.Parse(config["enablePasswordRetrieval"] ?? "false");
        this.MaxInvalidPasswordAttempts = int.Parse(config["maxInvalidPasswordAttempts"] ?? "5");
        this.MinRequiredNonAlphanumericCharacters = int.Parse(config["minRequiredNonAlphanumericCharacters"] ?? "0");
        this.MinRequiredPasswordLength = int.Parse(config["minRequiredPasswordLength"] ?? "1");
        this.PasswordAttemptWindow = int.Parse(config["passwordAttemptWindow"] ?? "10");
        this.PasswordFormat = (MembershipPasswordFormat)Enum.Parse(typeof(MembershipPasswordFormat), config["passwordFormat"] ?? "Hashed", true);
        this.PasswordStrengthRegularExpression = config["passwordStrengthRegularExpression"] ?? string.Empty;
        this.RequiresQuestionAndAnswer = bool.Parse(config["requiresQuestionAndAnswer"] ?? "false");
        this.RequiresUniqueEmail = bool.Parse(config["requiresUniqueEmail"] ?? "true");
        this.EncryptionAlgorithm = config["encryptionAlgorithm"] ?? "SHA1";
        this.PasswordPassphrase = config["passwordPassphrase"] ?? "Sy5l+6GaZe7";
        this.PasswordInitVector = config["passwordInitVector"] ?? "Lr8?-Ww6g{2Z_4Ro";
        this.AuthenticationTimeoutInSeconds = int.Parse(config["authenticationTimeoutInSeconds"] ?? "600");
        this.PersistentAuthenticationTimeoutInSeconds = int.Parse(config["persistentAuthenticationTimeoutInSeconds"] ?? "14400");
        this.LastActivityUpdateWindowInSeconds = int.Parse(config["lastActivityUpdateWindowInSeconds"] ?? "300");
        this.LastRecordCheckWindowInSeconds = int.Parse(config["lastRecordCheckWindowInSeconds"] ?? "300");
      }
    }
    #endregion
  }
}
