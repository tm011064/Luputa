using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Components.BusinessTier;

namespace Workmate.Components.Contracts.Emails
{
  public interface IEmailManager
  {
    BusinessObjectActionReport<DataRepositoryActionStatus> Create(IEmail email);
    List<IEmail> GetEmails(int applicationId, EmailStatus status);
    List<IEmail> GetEmails(EmailStatus status);
    List<IEmail> PutInSendQueue(int queuedEmailsThresholdInSeconds, int failedEmailsThresholdInSeconds, int totalEmailsToEnqueue);
    DataRepositoryActionStatus SetToSent(int emailId, EmailStatus status, EmailPriority priority);
  }
}
