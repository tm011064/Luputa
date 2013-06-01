using System;
using System.Diagnostics;
using CommonTools.Components.BusinessTier;
using CommonTools.Components.Testing;
using NUnit.Framework;
using Workmate.Components.CMS.MessageBoards;
using Workmate.Components.Contracts.CMS.MessageBoards;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS.MessageBoards;
using Workmate.Components.Entities.Membership;
using Workmate.Configuration;
using Workmate.Data;
using Workmate.Tests.DataAccess.SqlProvider.Components.Membership;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS.MessageBoards
{
  [TestFixture]
  public class Test_Messages : TestSetup
  {
    internal static Message Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application
      , IUserBasic author, MessageBoardThread messageBoardThread, Random random)
    {
      MessageManager manager = new MessageManager(dataStore);

      Message message = new Message(
        author
        , messageBoardThread
        , "Message Subject" + random.Next(1000000, 10000000)
        , "Message Body" + random.Next(1000000, 10000000));

      message.MessageStatus = DebugUtility.GetRandomEnum<MessageStatus>(random);


      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(message);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.Greater(message.MessageId, 0);

      Message dsMessage = manager.GetMessage(message.MessageId);
      Assert.IsNotNull(dsMessage);

      return dsMessage;
    }
    internal static void Delete(IDataStore dataStore, Message message)
    {
      MessageManager manager = new MessageManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(message);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetMessage(message.MessageId));

      Trace.WriteLine("Successfully deleted message " + message.Subject);
    }

    internal static void PopulateWithRandomValues(Message record, DummyDataManager dtm, Random random)
    {
      record.MessageStatus = DebugUtility.GetRandomEnum<MessageStatus>(random);
      record.FormattedBody = "Message Body" + random.Next(1000000, 10000000);
      record.IsApproved = DebugUtility.FlipCoin(random);
      record.IsLocked = DebugUtility.FlipCoin(random);
      record.Subject = "Message Status" + random.Next(1000000, 10000000);
      record.UrlFriendlyName = record.Subject;
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteMessage()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      MessageBoard messageBoard = Test_MessageBoards.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);
      MessageBoardThread messageBoardThread = Test_MessageBoardThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, messageBoard, this.Random);

      MessageManager manager = new MessageManager(this.DataStore);
      Message record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic, messageBoardThread, this.Random);

      Message recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetMessage(record.MessageId);

        string errors = string.Empty;
        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, record);
      Test_MessageBoards.Delete(this.DataStore, messageBoard);
    }
    [Test]
    public void Test_Delete_MessageBoard()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      MessageBoard messageBoard = Test_MessageBoards.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);
      MessageBoardThread messageBoardThread = Test_MessageBoardThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, messageBoard, this.Random);
      Message message = Test_Messages.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic, messageBoardThread, this.Random);

      Test_MessageBoards.Delete(this.DataStore, messageBoard);

      MessageBoardThreadManager messageBoardThreadManager = new MessageBoardThreadManager(this.DataStore);
      MessageManager messageManager = new MessageManager(this.DataStore);

      Assert.IsNull(messageBoardThreadManager.GetMessageBoardThread(messageBoardThread.MessageBoardThreadId));
      Assert.IsNull(messageManager.GetMessage(message.MessageId));
    }
    [Test]
    public void Test_Delete_Thread()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      MessageBoard messageBoard = Test_MessageBoards.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);
      MessageBoardThread messageBoardThread = Test_MessageBoardThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, messageBoard, this.Random);
      Message message = Test_Messages.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic, messageBoardThread, this.Random);

      Test_MessageBoardThreads.Delete(this.DataStore, messageBoardThread);

      MessageBoardThreadManager messageBoardThreadManager = new MessageBoardThreadManager(this.DataStore);
      MessageManager messageManager = new MessageManager(this.DataStore);

      Assert.IsNull(messageBoardThreadManager.GetMessageBoardThread(messageBoardThread.MessageBoardThreadId));
      Assert.IsNull(messageManager.GetMessage(message.MessageId));

      Test_MessageBoards.Delete(this.DataStore, messageBoard);
    }
    #endregion
  }
}
