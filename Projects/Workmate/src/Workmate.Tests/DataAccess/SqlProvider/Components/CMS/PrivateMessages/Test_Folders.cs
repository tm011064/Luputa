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
  public class Test_Folders : TestSetup
  {
    internal static Folder Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application, Inbox inbox, Random random)
    {
      FolderManager manager = new FolderManager(dataStore);

      Folder folder = new Folder(
        inbox
        , "TestThread " + random.Next(1000000, 10000000));

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(folder);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.Greater(folder.FolderId, 0);

      Folder dsThread = manager.GetFolder(folder.FolderId);
      Assert.IsNotNull(dsThread);

      return dsThread;
    }
    internal static void Delete(IDataStore dataStore, Folder folder)
    {
      FolderManager manager = new FolderManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(folder);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetFolder(folder.FolderId));

      Trace.WriteLine("Successfully deleted folder " + folder.FolderName);
    }

    internal static void PopulateWithRandomValues(Folder record, DummyDataManager dtm, Random random)
    {
      record.FolderName = "TestThread " + random.Next(1000000, 10000000);
      record.IsApproved = DebugUtility.FlipCoin(random);
      record.IsLocked = DebugUtility.FlipCoin(random);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteThread()
    {
      Inbox inbox = Test_Inboxs.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);

      FolderManager manager = new FolderManager(this.DataStore);
      Folder record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, inbox, this.Random);

      Folder recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetFolder(record.FolderId);

        string errors = string.Empty;
        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, record);
      Test_Inboxs.Delete(this.DataStore, inbox);
    }
    #endregion
  }
}
