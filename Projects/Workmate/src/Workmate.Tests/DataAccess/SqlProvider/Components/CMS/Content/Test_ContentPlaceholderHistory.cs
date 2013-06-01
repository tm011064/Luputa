using System;
using System.Diagnostics;
using CommonTools.Components.BusinessTier;
using CommonTools.Components.Testing;
using NUnit.Framework;
using Workmate.Components.CMS.Content;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS.Content;
using Workmate.Configuration;
using Workmate.Data;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS.ContentBlocks
{
  [TestFixture]
  public class Test_ContentPlaceholderHistorys : TestSetup
  {
    internal static ContentPlaceholderHistory Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application, ContentPlaceholder contentPlaceholder, Random random)
    {
      ContentPlaceholderHistoryManager manager = new ContentPlaceholderHistoryManager(dataStore);

      ContentPlaceholderHistory contentPlaceholderHistory = new ContentPlaceholderHistory(
        contentPlaceholder);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(contentPlaceholderHistory);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.Greater(contentPlaceholderHistory.ContentPlaceholderHistoryId, 0);

      ContentPlaceholderHistory dsThread = manager.GetContentPlaceholderHistory(contentPlaceholderHistory.ContentPlaceholderHistoryId);
      Assert.IsNotNull(dsThread);

      return dsThread;
    }
    internal static void Delete(IDataStore dataStore, ContentPlaceholderHistory contentPlaceholderHistory)
    {
      ContentPlaceholderHistoryManager manager = new ContentPlaceholderHistoryManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(contentPlaceholderHistory);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetContentPlaceholderHistory(contentPlaceholderHistory.ContentPlaceholderHistoryId));

      Trace.WriteLine("Successfully deleted contentPlaceholderHistory " + contentPlaceholderHistory.ContentPlaceholderHistoryId);
    }

    internal static void PopulateWithRandomValues(ContentPlaceholderHistory record, DummyDataManager dtm, Random random)
    {
      record.IsApproved = DebugUtility.FlipCoin(random);
      record.IsLocked = DebugUtility.FlipCoin(random);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteThread()
    {
      ContentPlaceholderGroup contentPlaceholderGroup = Test_ContentPlaceholderGroup.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);
      ContentPlaceholder contentPlaceholder = Test_ContentPlaceholders.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, contentPlaceholderGroup, this.Random);

      ContentPlaceholderHistoryManager manager = new ContentPlaceholderHistoryManager(this.DataStore);
      ContentPlaceholderHistory record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, contentPlaceholder, this.Random);

      ContentPlaceholderHistory recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetContentPlaceholderHistory(record.ContentPlaceholderHistoryId);

        string errors = string.Empty;
        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, record);
      Test_ContentPlaceholders.Delete(this.DataStore, contentPlaceholder);
      Test_ContentPlaceholderGroup.Delete(this.DataStore, contentPlaceholderGroup);
    }
    #endregion
  }
}
