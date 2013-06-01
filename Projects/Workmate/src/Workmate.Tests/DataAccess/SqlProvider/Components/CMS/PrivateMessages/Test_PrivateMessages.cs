using System;
using System.Diagnostics;
using CommonTools.Components.BusinessTier;
using CommonTools.Components.Testing;
using NUnit.Framework;
using Workmate.Components.CMS.PrivateMessages;
using Workmate.Components.Contracts.CMS.PrivateMessages;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS.PrivateMessages;
using Workmate.Components.Entities.Membership;
using Workmate.Configuration;
using Workmate.Data;
using Workmate.Tests.DataAccess.SqlProvider.Components.Membership;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS.PrivateMessages
{
  [TestFixture]
  public class Test_PrivateMessages : TestSetup
  {
    internal static PrivateMessage Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application
      , IUserBasic author, Folder folder, Random random)
    {
      PrivateMessageManager manager = new PrivateMessageManager(dataStore);

      PrivateMessage privateMessage = new PrivateMessage(
        author
        , folder
        , DebugUtility.GetRandomEnum<MessageStatus>(random)
        , DebugUtility.GetRandomEnum<MessageType>(random)
        , "PrivateMessage Subject" + random.Next(1000000, 10000000)
        , "PrivateMessage Body" + random.Next(1000000, 10000000));

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(privateMessage);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.Greater(privateMessage.PrivateMessageId, 0);

      PrivateMessage dsPrivateMessage = manager.GetPrivateMessage(privateMessage.PrivateMessageId);
      Assert.IsNotNull(dsPrivateMessage);

      return dsPrivateMessage;
    }
    internal static void Delete(IDataStore dataStore, PrivateMessage privateMessage)
    {
      PrivateMessageManager manager = new PrivateMessageManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(privateMessage);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetPrivateMessage(privateMessage.PrivateMessageId));

      Trace.WriteLine("Successfully deleted privateMessage " + privateMessage.Subject);
    }

    internal static void PopulateWithRandomValues(PrivateMessage record, DummyDataManager dtm, Random random)
    {
      record.MessageStatus = DebugUtility.GetRandomEnum<MessageStatus>(random);
      record.MessageType = DebugUtility.GetRandomEnum<MessageType>(random);
      record.FormattedBody = "PrivateMessage Body" + random.Next(1000000, 10000000);
      record.IsApproved = DebugUtility.FlipCoin(random);
      record.IsLocked = DebugUtility.FlipCoin(random);
      record.Subject = "PrivateMessage Status" + random.Next(1000000, 10000000);
      record.UrlFriendlyName = record.Subject;
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeletePrivateMessage()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      Inbox inbox = Test_Inboxs.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);
      Folder folder = Test_Folders.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, inbox, this.Random);

      PrivateMessageManager manager = new PrivateMessageManager(this.DataStore);
      PrivateMessage record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic, folder, this.Random);

      PrivateMessage recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetPrivateMessage(record.PrivateMessageId);

        string errors = string.Empty;
        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, record);
      Test_Inboxs.Delete(this.DataStore, inbox);
    }
    [Test]
    public void Test_Delete_Inbox()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      Inbox inbox = Test_Inboxs.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);
      Folder folder = Test_Folders.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, inbox, this.Random);
      PrivateMessage privateMessage = Test_PrivateMessages.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic, folder, this.Random);

      Test_Inboxs.Delete(this.DataStore, inbox);

      FolderManager folderManager = new FolderManager(this.DataStore);
      PrivateMessageManager privateMessageManager = new PrivateMessageManager(this.DataStore);

      Assert.IsNull(folderManager.GetFolder(folder.FolderId));
      Assert.IsNull(privateMessageManager.GetPrivateMessage(privateMessage.PrivateMessageId));
    }
    [Test]
    public void Test_Delete_Thread()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      Inbox inbox = Test_Inboxs.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);
      Folder folder = Test_Folders.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, inbox, this.Random);
      PrivateMessage privateMessage = Test_PrivateMessages.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic, folder, this.Random);

      Test_Folders.Delete(this.DataStore, folder);

      FolderManager folderManager = new FolderManager(this.DataStore);
      PrivateMessageManager privateMessageManager = new PrivateMessageManager(this.DataStore);

      Assert.IsNull(folderManager.GetFolder(folder.FolderId));
      Assert.IsNull(privateMessageManager.GetPrivateMessage(privateMessage.PrivateMessageId));

      Test_Inboxs.Delete(this.DataStore, inbox);
    }
    #endregion
  }
}
