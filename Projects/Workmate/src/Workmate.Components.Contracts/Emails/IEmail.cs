using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Components.Contracts.Emails
{
  public interface IEmail
  {
    int ApplicationId { get; set; }
    string Body { get; set; }
    int? CreatedByUserId { get; set; }
    DateTime DateCreatedUtc { get; set; }
    int EmailId { get; set; }
    string Subject { get; set; }
    EmailPriority Priority { get; set; }
    string Recipients { get; set; }
    string Sender { get; set; }
    DateTime? SentUtc { get; set; }
    EmailStatus Status { get; set; }
    EmailType EmailType { get; set; }
    int TotalSendAttempts { get; set; }
  }
}
