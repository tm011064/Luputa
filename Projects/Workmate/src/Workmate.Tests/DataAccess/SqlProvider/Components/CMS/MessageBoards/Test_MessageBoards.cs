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
  public class Test_MessageBoards : TestSetup
  {
    internal static MessageBoard Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application, Random random)
    {
      MessageBoardManager manager = new MessageBoardManager(dataStore);

      MessageBoard messageBoard = new MessageBoard(
        application.ApplicationId
        , "TestMessageBoard " + random.Next(1000000, 10000000)
        , true
        , true);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(messageBoard);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.Greater(messageBoard.MessageBoardId, 0);

      MessageBoard dsMessageBoard = manager.GetMessageBoard(messageBoard.MessageBoardId);
      Assert.IsNotNull(dsMessageBoard);

      return dsMessageBoard;
    }
    internal static void Delete(IDataStore dataStore, MessageBoard messageBoard)
    {
      MessageBoardManager manager = new MessageBoardManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(messageBoard);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetMessageBoard(messageBoard.MessageBoardId));

      Trace.WriteLine("Successfully deleted messageBoard " + messageBoard.Name);
    }

    internal static void PopulateWithRandomValues(MessageBoard record, DummyDataManager dtm, Random random)
    {
      record.Name = "TestMessageBoard " + random.Next(1000000, 10000000);
      record.Description = "Description " + random.Next(1000000, 10000000);
      record.IsActive = DebugUtility.FlipCoin(random);
      record.IsModerated = DebugUtility.FlipCoin(random);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteMessageBoard()
    {
      MessageBoardManager manager = new MessageBoardManager(this.DataStore);
      MessageBoard record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);

      MessageBoard recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetMessageBoard(record.MessageBoardId);

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
