using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Components.Threading;
using log4net;
using Workmate.Components.Contracts.Emails;
using System.Net.Mail;
using CommonTools.Components.RegularExpressions;
using System.Net;
using Workmate.ApplicationServer.Configuration;
using System.Threading;

namespace Workmate.ApplicationServer
{
  public class EmailPublisherDaemon
  {
    #region members
    private ILog _Log = LogManager.GetLogger("EmailPublisherDaemon");
    private IApplicationSettings _ApplicationSettings;

    private Thread _BackgroundThread;
    private bool _IsRunning = false;
    #endregion

    internal bool SendEmail(IEmail mail)
    {
      using (MailMessage message = new MailMessage())
      {
        foreach (string toAddress in mail.Recipients.Replace(';', ',')
                                                    .Split(','))
        {
          if (ValidationExpressions.IsValidEmail(toAddress))
            message.To.Add(toAddress);
        }

        if (message.To.Count == 0)
        {
          _Log.WarnFormat("No valid email address found at reccipient string {0}", mail.Recipients);
          return false;
        }

        if (!ValidationExpressions.IsValidEmail(mail.Sender))
        {
          _Log.WarnFormat("Sender {0} is not a valid email address", mail.Sender);
          return false;
        }

        message.From = new MailAddress(mail.Sender);

        message.Subject = mail.Subject;
        message.Body = mail.Body;

        message.IsBodyHtml = true;

        using (SmtpClient smtp = new SmtpClient())
        {
          smtp.Host = _ApplicationSettings.SmtpHost;
          smtp.Port = _ApplicationSettings.SmtpPort;
          smtp.EnableSsl = _ApplicationSettings.SmtpEnableSsl;
          smtp.DeliveryMethod = _ApplicationSettings.SmtpDeliveryMethod;

          smtp.UseDefaultCredentials = _ApplicationSettings.SmtpUseDefaultCredentials;
          if (!smtp.UseDefaultCredentials)
            smtp.Credentials = new NetworkCredential(_ApplicationSettings.SmtpUserName, _ApplicationSettings.SmtpPassword);

          bool success = false;
          try
          {
            smtp.Send(message);
            success = true;
          }
          catch (SmtpFailedRecipientsException err)
          {
            _Log.Warn(string.Format("The following email addresses failed: {0}", err.FailedRecipient), err);

            // we update the db so we can resend later
            if (mail.TotalSendAttempts > _ApplicationSettings.TotalResendAttempts + 1)
              InstanceContainer.EmailManager.SetToSent(mail.EmailId, EmailStatus.SendFailed, EmailPriority.CanWait);
            else
              InstanceContainer.EmailManager.SetToSent(mail.EmailId, EmailStatus.Undelivered, EmailPriority.CanWait);
          }
          catch (SmtpException err)
          {
            _Log.Error("Unable to send email. See inner exception for further details", err);

            // we update the db so we can resend later
            if (mail.TotalSendAttempts > _ApplicationSettings.TotalResendAttempts + 1)
              InstanceContainer.EmailManager.SetToSent(mail.EmailId, EmailStatus.SendFailed, EmailPriority.CanWait);
            else
              InstanceContainer.EmailManager.SetToSent(mail.EmailId, EmailStatus.Undelivered, EmailPriority.CanWait);
          }
          catch (Exception err)
          {
            _Log.Error("Unable to send email. See inner exception for further details", err);
          }

          if (success)
          {
            // make sure the db gets updated...
            InstanceContainer.EmailManager.SetToSent(mail.EmailId, EmailStatus.Sent, mail.Priority);
            _Log.InfoFormat("Successfully sent {0} email to {1}", mail.EmailType.ToString(), mail.Recipients.ToString());
          }

          return success;
        }
      }

    }

    private void PublishLoop()
    {
      List<IEmail> emails;
      while (_IsRunning)
      {
        emails = InstanceContainer.EmailManager.PutInSendQueue(
          _ApplicationSettings.QueuedEmailsThresholdInSeconds
          , _ApplicationSettings.FailedEmailsThresholdInSeconds
          , _ApplicationSettings.TotalEmailsToEnqueue);

        if (emails.Count == 0)
        {
          _Log.Info("Email publisher daemon has no emails to send, wait for next iteration in 15 seconds");
          Thread.Sleep(15000); // wait for 15 seconds before next attempt
        }
        else
        {
          _Log.InfoFormat("Start publishing {0} emails", emails.Count);
          foreach (IEmail email in emails)
          {
            SendEmail(email);
          }
          _Log.InfoFormat("Finished publishing {0} emails", emails.Count);
        }
      }
    }

    public void Start()
    {
      if (_IsRunning)
        return;
      else
      {
        _IsRunning = true;
      }

      _BackgroundThread = new Thread(new ThreadStart(PublishLoop));
      _BackgroundThread.IsBackground = true;
      _BackgroundThread.Start();
    }

    public EmailPublisherDaemon(IApplicationSettings applicationSettings)
    {
      this._ApplicationSettings = applicationSettings;
    }
  }
}
