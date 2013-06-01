using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using System.Net.Mail;

namespace Workmate.ApplicationServer.Configuration
{
  public interface IApplicationSettings
  {
    string Log4NetConfigPath { get; }
    string DataStoreContextType { get; }

    string DefaultConnectionString { get; }
    string DefaultConnectionStringName { get; }

    string SmtpHost { get; }
    int SmtpPort { get; }
    bool SmtpEnableSsl { get; }
    SmtpDeliveryMethod SmtpDeliveryMethod { get; }
    bool SmtpUseDefaultCredentials { get; }
    string SmtpUserName { get; }
    string SmtpPassword { get; }

    int QueuedEmailsThresholdInSeconds { get; }
    int FailedEmailsThresholdInSeconds { get; }
    int TotalEmailsToEnqueue { get; }
    int TotalResendAttempts { get; }      
  }

  public class ApplicationSettings : IApplicationSettings
  {
    public string Log4NetConfigPath { get; private set; }
    public string DataStoreContextType { get; private set; }

    public string DefaultConnectionString { get; private set; }
    public string DefaultConnectionStringName { get; private set; }

    public string SmtpHost { get; private set; }
    public int SmtpPort { get; private set; }
    public bool SmtpEnableSsl { get; private set; }
    public SmtpDeliveryMethod SmtpDeliveryMethod { get; private set; }
    public bool SmtpUseDefaultCredentials { get; private set; }
    public string SmtpUserName { get; private set; }
    public string SmtpPassword { get; private set; }

    public int QueuedEmailsThresholdInSeconds { get; private set; }
    public int FailedEmailsThresholdInSeconds { get; private set; }
    public int TotalEmailsToEnqueue { get; private set; }
    public int TotalResendAttempts { get; private set; }    
    
    #region constructors
    public ApplicationSettings(NameValueCollection appSettings, ConnectionStringSettingsCollection connectionStrings)
    {
      this.Log4NetConfigPath = appSettings["log4netConfigPath"] ?? "log4net.config";
      this.DataStoreContextType = appSettings["dataStoreContextType"] ?? "NOT SET";

      this.DefaultConnectionStringName = appSettings["defaultConnectionStringName"] ?? "NOT SET";
      this.DefaultConnectionString = connectionStrings[this.DefaultConnectionStringName].ConnectionString;

      this.SmtpHost = appSettings["smtpHost"];
      this.SmtpPort = int.Parse(appSettings["smtpPort"]);
      this.SmtpEnableSsl = bool.Parse(appSettings["smtpEnableSsl"]);
      this.SmtpDeliveryMethod = (SmtpDeliveryMethod)Enum.Parse(typeof(SmtpDeliveryMethod), appSettings["smtpDeliveryMethod"] ?? SmtpDeliveryMethod.Network.ToString());
      this.SmtpUseDefaultCredentials = bool.Parse(appSettings["smtpUseDefaultCredentials"]);
      this.SmtpUserName = appSettings["smtpUserName"];
      this.SmtpPassword = appSettings["smtpPassword"];

      this.QueuedEmailsThresholdInSeconds = int.Parse(appSettings["queuedEmailsThresholdInSeconds"]);
      this.FailedEmailsThresholdInSeconds = int.Parse(appSettings["failedEmailsThresholdInSeconds"]);
      this.TotalEmailsToEnqueue = int.Parse(appSettings["totalEmailsToEnqueue"]);
      this.TotalResendAttempts = int.Parse(appSettings["totalResendAttempts"]);      
    }
    #endregion
  }
}
