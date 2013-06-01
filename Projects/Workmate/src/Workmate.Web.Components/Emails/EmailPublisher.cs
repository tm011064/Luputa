using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Web.Components.Application;
using Workmate.Components.Contracts.Membership;
using Workmate.Components.Entities.Emails;
using Workmate.Components.Contracts.Emails;
using System.Threading;
using CommonTools.Components.Threading;
using log4net;
using Workmate.Components.Contracts;

namespace Workmate.Web.Components.Emails
{
  public interface IEmailPublisher
  {
    void EnqueueUserCreatedEmail(int applicationId, string theme, string senderEmail, IStaticContentLookup staticContentLookup
      , string firstName, string lastName, string userEmail, int userId, string password);    
  }

  public class EmailPublisher : IEmailPublisher
  {
    #region members
    private ILog _Log = LogManager.GetLogger("EmailPublisher");
    #endregion

    #region public methods
    public void EnqueueUserCreatedEmail(int applicationId, string theme, string senderEmail, IStaticContentLookup staticContentLookup
      , string firstName, string lastName, string userEmail, int userId, string password)
    {
      ThreadPool.QueueUserWorkItem(delegate(object state)
      {
        Dictionary<string, string> placeholders = new Dictionary<string, string>()
        {
          { "@firstname", firstName },
          { "@lastname", lastName },
          { "@email", userEmail },
          { "@password", password }
        };

        string subject = staticContentLookup.GetContent(theme, "mail_AccountCreated_Subject", true, placeholders);
        string body = staticContentLookup.GetContent(theme, "mail_AccountCreated_Body", true, placeholders);

        Email email = new Email(
          applicationId
          , subject
          , body
          , userEmail
          , senderEmail
          , userId
          , EmailStatus.Unsent
          , EmailPriority.SendImmediately
          , EmailType.UserCreated);

        if (InstanceContainer.EmailManager.Create(email).Status == DataRepositoryActionStatus.Success)
        {
          _Log.Info("Successfully created user created notification email for user " + userId);
        }
      });
    }
    #endregion

  }
}
