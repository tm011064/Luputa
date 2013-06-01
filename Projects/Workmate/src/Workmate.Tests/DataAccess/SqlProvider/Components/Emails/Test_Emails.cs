using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Workmate.Components.Emails;
using Workmate.Components.Entities.Emails;
using Workmate.Components.Contracts.Emails;
using Workmate.Components.Contracts;
using CommonTools.Components.BusinessTier;
using Workmate.Data;
using CommonTools.Components.Testing;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Workmate.Components.Contracts.Membership;
using Workmate.Tests.DataAccess.SqlProvider.Components.Membership;
using System.Threading;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.Emails
{
  [TestFixture]
  public class Test_Emails : TestSetup
  {
    internal IEmail Create(IDataStore dataStore, Random random, IApplication application, IUserBasic user)
    {
      return Create(dataStore, random, application, user, DebugUtility.GetRandomEnum<EmailPriority>(random)
        , DebugUtility.GetRandomEnum<EmailStatus>(random));
    }
    internal IEmail Create(IDataStore dataStore, Random random, IApplication application, IUserBasic user
      , EmailPriority emailPriority, EmailStatus emailStatus)
    {
      EmailManager manager = new EmailManager(dataStore);

      Email email = new Email(application.ApplicationId
        , "Subject " + random.Next(1000000, 10000000)
        , "Body " + +random.Next(1000000, 10000000)
        , "dummyemail@wm.com"
        , "dummysender@wm.com"
        , user.UserId
        , emailStatus
        , emailPriority
        , EmailType.UserCreated);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(email);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);

      IEmail dsEmail = manager.GetEmail(email.EmailId);
      Assert.IsNotNull(dsEmail);

      return dsEmail;
    }
    internal static void Delete(IDataStore dataStore, IEmail email)
    {
      EmailManager manager = new EmailManager(dataStore);

      DataRepositoryActionStatus report = manager.Delete(email.EmailId);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report);
      Assert.IsNull(manager.GetEmail(email.EmailId));

      Trace.WriteLine("Successfully deleted email " + email.EmailId);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteEmail()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      EmailManager manager = new EmailManager(this.DataStore);
      IEmail record = Create(this.DataStore, this.Random, this.Application, userBasic);

      Delete(this.DataStore, record);
    }
    [Test]
    public void Test_SendEmails()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      EmailManager manager = new EmailManager(this.DataStore);

      List<IEmail> highPriorityEmails = new List<IEmail>();
      for (int i = 0; i < 3; i++)
        highPriorityEmails.Add(Create(this.DataStore, this.Random, this.Application, userBasic, EmailPriority.SendImmediately, EmailStatus.Unsent));
      List<IEmail> lowPriorityEmails = new List<IEmail>();
      for (int i = 0; i < 3; i++)
        lowPriorityEmails.Add(Create(this.DataStore, this.Random, this.Application, userBasic, EmailPriority.CanWait, EmailStatus.Unsent));

      List<IEmail> emails;

      emails = manager.PutInSendQueue(int.MaxValue, int.MaxValue, 4);

      Assert.AreEqual(4, emails.Count);
      Assert.AreEqual(emails[0].EmailId, highPriorityEmails[0].EmailId);
      Assert.AreEqual(emails[1].EmailId, highPriorityEmails[1].EmailId);
      Assert.AreEqual(emails[2].EmailId, highPriorityEmails[2].EmailId);
      Assert.AreEqual(emails[3].EmailId, lowPriorityEmails[0].EmailId);

      for (int i = 0; i < 4; i++)
      {
        Assert.AreEqual(EmailStatus.Queued, emails[i].Status);
        Assert.AreEqual(EmailStatus.Queued, manager.GetEmail(emails[i].EmailId).Status);
      }

      manager.SetToSent(emails[0].EmailId, EmailStatus.Sent, emails[0].Priority);
      manager.SetToSent(emails[1].EmailId, EmailStatus.Sent, emails[1].Priority);
      manager.SetToSent(emails[2].EmailId, EmailStatus.Sent, emails[2].Priority);
      manager.SetToSent(emails[3].EmailId, EmailStatus.Sent, emails[3].Priority);

      emails = manager.PutInSendQueue(int.MaxValue, int.MaxValue, 4);
      Assert.AreEqual(2, emails.Count);
      Assert.AreEqual(emails[0].EmailId, lowPriorityEmails[1].EmailId);
      Assert.AreEqual(emails[1].EmailId, lowPriorityEmails[2].EmailId);

      foreach (IEmail email in highPriorityEmails)
        Delete(this.DataStore, email);
      foreach (IEmail email in lowPriorityEmails)
        Delete(this.DataStore, email);
    }
    [Test]
    public void Test_SendEmailAttempts()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      EmailManager manager = new EmailManager(this.DataStore);
      IEmail record = Create(this.DataStore, this.Random, this.Application, userBasic, EmailPriority.SendImmediately, EmailStatus.Unsent);

      List<IEmail> emails;

      emails = manager.PutInSendQueue(int.MaxValue, int.MaxValue, 1);

      Assert.AreEqual(1, emails.Count);
      Assert.AreEqual(emails[0].EmailId, record.EmailId, "Please ensure that there are no unsent emails in the database");

      Assert.AreEqual(EmailStatus.Queued, emails[0].Status);
      Assert.AreEqual(EmailStatus.Queued, manager.GetEmail(emails[0].EmailId).Status);

      manager.SetToSent(record.EmailId, EmailStatus.SendFailed, EmailPriority.CanWait);
      record = manager.GetEmail(emails[0].EmailId);

      Assert.AreEqual(1, record.TotalSendAttempts);
      Assert.AreEqual(EmailStatus.SendFailed, record.Status);

      Thread.Sleep(100);

      emails = manager.PutInSendQueue(int.MaxValue, 0, 1);

      Assert.AreEqual(1, emails.Count);
      Assert.AreEqual(EmailStatus.Queued, emails[0].Status);
      Assert.AreEqual(EmailStatus.Queued, manager.GetEmail(emails[0].EmailId).Status);
      
      manager.SetToSent(record.EmailId, EmailStatus.Undelivered, EmailPriority.CanWait);
      record = manager.GetEmail(emails[0].EmailId);

      Assert.AreEqual(2, record.TotalSendAttempts);
      Assert.AreEqual(EmailStatus.Undelivered, record.Status);

      Thread.Sleep(100);

      emails = manager.PutInSendQueue(int.MaxValue, 0, 1);
      Assert.AreEqual(0, emails.Count);
      
      Delete(this.DataStore, record);
    }
    [Test]
    public void Test_SendQueuedEmailPickup()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      EmailManager manager = new EmailManager(this.DataStore);
      IEmail record = Create(this.DataStore, this.Random, this.Application, userBasic, EmailPriority.SendImmediately, EmailStatus.Unsent);

      List<IEmail> emails;

      emails = manager.PutInSendQueue(int.MaxValue, int.MaxValue, 1);

      Assert.AreEqual(1, emails.Count);
      Assert.AreEqual(emails[0].EmailId, record.EmailId, "Please ensure that there are no unsent emails in the database");

      Assert.AreEqual(EmailStatus.Queued, emails[0].Status);
      Assert.AreEqual(EmailStatus.Queued, manager.GetEmail(emails[0].EmailId).Status);

      record = manager.GetEmail(emails[0].EmailId);

      Thread.Sleep(100);

      emails = manager.PutInSendQueue(10000, int.MaxValue, 1);
      Assert.AreEqual(0, emails.Count);

      Thread.Sleep(100);
      emails = manager.PutInSendQueue(0, int.MaxValue, 1);
      Assert.AreEqual(1, emails.Count);

      Delete(this.DataStore, record);
    }


    #endregion
  }
}
