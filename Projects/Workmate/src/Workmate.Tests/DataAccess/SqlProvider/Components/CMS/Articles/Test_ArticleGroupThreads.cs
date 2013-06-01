using System;
using System.Diagnostics;
using CommonTools.Components.BusinessTier;
using CommonTools.Components.Testing;
using NUnit.Framework;
using Workmate.Components.CMS.Articles;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS.Articles;
using Workmate.Configuration;
using Workmate.Data;
using Workmate.Components.Contracts.CMS.Articles;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS.Articles
{
  [TestFixture]
  public class Test_ArticleGroupThreads : TestSetup
  {
    internal static ArticleGroupThread Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application, ArticleGroup articleGroup, Random random)
    {
      ArticleGroupThreadManager manager = new ArticleGroupThreadManager(dataStore);

      ArticleGroupThread articleGroupThread = new ArticleGroupThread(
        articleGroup
        , ArticleGroupThreadStatus.Enabled
        , "TestThread " + random.Next(1000000, 10000000));

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(articleGroupThread);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.Greater(articleGroupThread.ArticleGroupThreadId, 0);

      ArticleGroupThread dsThread = manager.GetArticleGroupThread(articleGroupThread.ArticleGroupThreadId);
      Assert.IsNotNull(dsThread);

      return dsThread;
    }
    internal static void Delete(IDataStore dataStore, ArticleGroupThread articleGroupThread)
    {
      ArticleGroupThreadManager manager = new ArticleGroupThreadManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(articleGroupThread);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetArticleGroupThread(articleGroupThread.ArticleGroupThreadId));

      Trace.WriteLine("Successfully deleted articleGroupThread " + articleGroupThread.Name);
    }

    internal static void PopulateWithRandomValues(ArticleGroupThread record, DummyDataManager dtm, Random random)
    {
      record.Name = "TestThread " + random.Next(1000000, 10000000);
      record.IsApproved = DebugUtility.FlipCoin(random);
      record.IsLocked = DebugUtility.FlipCoin(random);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteThread()
    {
      ArticleGroup articleGroup = Test_ArticleGroups.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);

      ArticleGroupThreadManager manager = new ArticleGroupThreadManager(this.DataStore);
      ArticleGroupThread record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, articleGroup, this.Random);

      ArticleGroupThread recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetArticleGroupThread(record.ArticleGroupThreadId);

        string errors = string.Empty;
        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, record);
      Test_ArticleGroups.Delete(this.DataStore, articleGroup);
    }
    #endregion
  }
}
