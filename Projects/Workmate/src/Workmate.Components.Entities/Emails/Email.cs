using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Contracts.Emails;

namespace Workmate.Components.Entities.Emails
{
  public class Email :  IEmail
  {
    #region properties

    public int ApplicationId { get; set; }

    public int EmailId { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    public string Recipients { get; set; }

    public string Sender { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime DateCreatedUtc { get; set; }

    public DateTime? SentUtc { get; set; }

    public EmailStatus Status { get; set; }

    public EmailPriority Priority { get; set; }

    public EmailType EmailType { get; set; }

    public int TotalSendAttempts { get; set; }

    #endregion

    #region constructors
    public Email(int applicationId, string subject, string body, string recipients, string sender, int? createdByUserId
      , EmailStatus status, EmailPriority priority, EmailType emailType)
    {
      this.ApplicationId = applicationId;
      this.Subject = subject;
      this.Body = body;
      this.Recipients = recipients;
      this.Sender = sender;
      this.CreatedByUserId = createdByUserId;
      this.Status = status;
      this.Priority = priority;
      this.EmailType = emailType;

      this.TotalSendAttempts = 0;
    }
    public Email(int applicationId, int emailId, string subject, string body, string recipients, string sender
      , int? createdByUserId, DateTime dateCreatedUtc, DateTime? sentUtc, EmailStatus status, EmailPriority priority
      , EmailType emailType, int totalSendAttempts)
    {
      this.ApplicationId = applicationId;
      this.EmailId = emailId;
      this.Subject = subject;
      this.Body = body;
      this.Recipients = recipients;
      this.Sender = sender;
      this.CreatedByUserId = createdByUserId;
      this.DateCreatedUtc = dateCreatedUtc;
      this.SentUtc = sentUtc;
      this.Status = status;
      this.Priority = priority;
      this.EmailType = emailType;
      this.TotalSendAttempts = totalSendAttempts;
    }
    #endregion
  }
}
