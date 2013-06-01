using System;
using System.Diagnostics;
using CommonTools.Components.BusinessTier;
using CommonTools.Components.Testing;
using NUnit.Framework;
using Workmate.Components.CMS.MessageBoards;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS.MessageBoards;
using Workmate.Configuration;
using Workmate.Data;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS.MessageBoards
{
  [TestFixture]
  public class Test_MessageBoardThreads : TestSetup
  {
    internal static MessageBoardThread Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application, MessageBoard messageBoard, Random random)
    {
      MessageBoardThreadManager manager = new MessageBoardThreadManager(dataStore);

      MessageBoardThread messageBoardThread = new MessageBoardThread(
        messageBoard);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(messageBoardThread);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.Greater(messageBoardThread.MessageBoardThreadId, 0);

      MessageBoardThread dsThread = manager.GetMessageBoardThread(messageBoardThread.MessageBoardThreadId);
      Assert.IsNotNull(dsThread);

      return dsThread;
    }
    internal static void Delete(IDataStore dataStore, MessageBoardThread messageBoardThread)
    {
      MessageBoardThreadManager manager = new MessageBoardThreadManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(messageBoardThread);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetMessageBoardThread(messageBoardThread.MessageBoardThreadId));

      Trace.WriteLine("Successfully deleted messageBoardThread " + messageBoardThread.MessageBoardId);
    }

    internal static void PopulateWithRandomValues(MessageBoardThread record, DummyDataManager dtm, Random random)
    {
      record.IsApproved = DebugUtility.FlipCoin(random);
      record.IsLocked = DebugUtility.FlipCoin(random);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteThread()
    {
      MessageBoard messageBoard = Test_MessageBoards.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);

      MessageBoardThreadManager manager = new MessageBoardThreadManager(this.DataStore);
      MessageBoardThread record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, messageBoard, this.Random);

      MessageBoardThread recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetMessageBoardThread(record.MessageBoardThreadId);

        string errors = string.Empty;
        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, record);
      Test_MessageBoards.Delete(this.DataStore, messageBoard);
    }
    #endregion
  }
}
