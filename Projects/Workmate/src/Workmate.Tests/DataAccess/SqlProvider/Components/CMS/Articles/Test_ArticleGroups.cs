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

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS.Articles
{
  [TestFixture]
  public class Test_ArticleGroups : TestSetup
  {
    internal static ArticleGroup Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application, Random random)
    {
      ArticleGroupManager manager = new ArticleGroupManager(dataStore);

      ArticleGroup articleGroup = new ArticleGroup(
        application.ApplicationId
        , "TestArticleGroup " + random.Next(1000000, 10000000)
        , true);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(articleGroup);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.Greater(articleGroup.ArticleGroupId, 0);

      ArticleGroup dsArticleGroup = manager.GetArticleGroup(articleGroup.ArticleGroupId);
      Assert.IsNotNull(dsArticleGroup);

      return dsArticleGroup;
    }
    internal static void Delete(IDataStore dataStore, ArticleGroup articleGroup)
    {
      ArticleGroupManager manager = new ArticleGroupManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(articleGroup);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetArticleGroup(articleGroup.ArticleGroupId));

      Trace.WriteLine("Successfully deleted articleGroup " + articleGroup.Name);
    }

    internal static void PopulateWithRandomValues(ArticleGroup record, DummyDataManager dtm, Random random)
    {
      record.Name = "TestArticleGroup " + random.Next(1000000, 10000000);
      record.Description = "Description " + random.Next(1000000, 10000000);
      record.IsActive = DebugUtility.FlipCoin(random);
      record.IsModerated = DebugUtility.FlipCoin(random);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteArticleGroup()
    {
      ArticleGroupManager manager = new ArticleGroupManager(this.DataStore);
      ArticleGroup record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);

      ArticleGroup recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetArticleGroup(record.ArticleGroupId);

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
