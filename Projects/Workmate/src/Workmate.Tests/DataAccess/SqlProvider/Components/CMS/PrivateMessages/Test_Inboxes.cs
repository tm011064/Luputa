using System;
using System.Diagnostics;
using CommonTools.Components.BusinessTier;
using CommonTools.Components.Testing;
using NUnit.Framework;
using Workmate.Components.CMS.PrivateMessages;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS.PrivateMessages;
using Workmate.Configuration;
using Workmate.Data;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS.PrivateMessages
{
  [TestFixture]
  public class Test_Inboxs : TestSetup
  {
    internal static Inbox Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application, Random random)
    {
      InboxManager manager = new InboxManager(dataStore);

      Inbox inbox = new Inbox(
        application.ApplicationId
        , "TestInbox " + random.Next(1000000, 10000000)
        , true);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(inbox);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.Greater(inbox.InboxId, 0);

      Inbox dsInbox = manager.GetInbox(inbox.InboxId);
      Assert.IsNotNull(dsInbox);

      return dsInbox;
    }
    internal static void Delete(IDataStore dataStore, Inbox inbox)
    {
      InboxManager manager = new InboxManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(inbox);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetInbox(inbox.InboxId));

      Trace.WriteLine("Successfully deleted inbox " + inbox.Name);
    }

    internal static void PopulateWithRandomValues(Inbox record, DummyDataManager dtm, Random random)
    {
      record.Name = "TestInbox " + random.Next(1000000, 10000000);
      record.Description = "Description " + random.Next(1000000, 10000000);
      record.IsActive = DebugUtility.FlipCoin(random);
      record.IsModerated = DebugUtility.FlipCoin(random);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteInbox()
    {
      InboxManager manager = new InboxManager(this.DataStore);
      Inbox record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);

      Inbox recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetInbox(record.InboxId);

        string errors = string.Empty;

        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, record);
    }
    #endregion
  }
}
