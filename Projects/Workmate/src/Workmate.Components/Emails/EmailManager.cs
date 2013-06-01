using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Workmate.Components.Contracts.Membership;
using Workmate.Components.Contracts.Emails;
using Workmate.Components.Entities.Emails;
using Workmate.Data;
using log4net;
using CommonTools.Components.BusinessTier;
using Workmate.Components.Contracts;
using CommonTools.Components.Testing;
using CommonTools;

namespace Workmate.Components.Emails
{
  public class EmailManager : IEmailManager
  {
    #region members
    protected IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("EmailManager");
    #endregion

    #region CRUD
    public IEmail GetEmail(int emailId)
    {
      IEmail record = null;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          record = dataStoreContext.wm_Emails_Get(emailId);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Emails_Get", ex);
        throw new DataStoreException(ex, true);
      }
      return record;
    }
    public List<IEmail> GetEmails(EmailStatus status)
    {
      List<IEmail> records = new List<IEmail>();
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          records = dataStoreContext.wm_Emails_Get(status).ToList();
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Emails_Get", ex);
        throw new DataStoreException(ex, true);
      }
      return records;
    }
    public List<IEmail> GetEmails(int applicationId, EmailStatus status)
    {
      List<IEmail> records = new List<IEmail>();
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          records = dataStoreContext.wm_Emails_Get(applicationId, status).ToList();
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Emails_Get", ex);
        throw new DataStoreException(ex, true);
      }
      return records;
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Create(IEmail email)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(email);
      int num;
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
            num = dataStoreContext.wm_Emails_Insert(
              email.ApplicationId
              , email.Subject
              , email.Body
              , email.Recipients
              , email.Sender
              , email.CreatedByUserId
              , email.Status
              , email.Priority
              , email.EmailType);
        }
        catch (Exception ex)
        {
          _Log.Error("Error at wm_Emails_Insert", ex);
          throw new DataStoreException(ex, true);
        }

        if (num > 0)
        {
          email.EmailId = num;
        }
        else
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.ErrorFormat("Email was not inserted at the database (ErrorCode: {0}).", num);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("Email {0} was not inserted at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(email)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }

    public DataRepositoryActionStatus SetToSent(int emailId, EmailStatus status, EmailPriority priority)
    {
      int num = 0;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          num = dataStoreContext.wm_Emails_SetToSent(emailId, status, priority);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Emails_SetToSent", ex);
        throw new DataStoreException(ex, true);
      }

      if (num == 0)
      {
        _Log.WarnFormat("Email {0} was not set to sent (ErrorCode: {1})."
          , emailId
          , num);

        return DataRepositoryActionStatus.NoRecordRowAffected;
      }

      return DataRepositoryActionStatus.Success;

    }
    
    public List<IEmail> PutInSendQueue(int queuedEmailsThresholdInSeconds, int failedEmailsThresholdInSeconds, int totalEmailsToEnqueue)
    {
      List<IEmail> records = new List<IEmail>();
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          records = dataStoreContext.wm_Emails_PutInSendQueue(queuedEmailsThresholdInSeconds, failedEmailsThresholdInSeconds, totalEmailsToEnqueue);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Emails_PutInSendQueue", ex);
        throw new DataStoreException(ex, true);
      }
      return records;
    }

    public DataRepositoryActionStatus Delete(int emailId)
    {
      int num = 0;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          num = dataStoreContext.wm_Emails_Delete(emailId);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Emails_Delete", ex);
        throw new DataStoreException(ex, true);
      }
      if (num == 0)
      {
        _Log.WarnFormat("Email {0} was not deleted from the database (ErrorCode: {1})."
          , emailId
          , num);

        return DataRepositoryActionStatus.NoRecordRowAffected;
      }

      return DataRepositoryActionStatus.Success;
    }
    #endregion

    #region constructors
    public EmailManager(IDataStore dataStore)
    {
      _DataStore = dataStore;
    }
    #endregion
  }
}
