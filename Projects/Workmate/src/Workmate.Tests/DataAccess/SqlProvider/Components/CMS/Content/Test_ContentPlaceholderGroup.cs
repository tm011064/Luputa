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
  public class Test_ContentPlaceholderGroup : TestSetup
  {
    internal static ContentPlaceholderGroup Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application, Random random)
    {
      ContentPlaceholderGroupManager manager = new ContentPlaceholderGroupManager(dataStore);

      ContentPlaceholderGroup contentPlaceholderGroup = new ContentPlaceholderGroup(
        random.Next(1000, 2000)
        , "TestContentPlaceholderGroup " + random.Next(1000000, 10000000));

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(contentPlaceholderGroup);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.Greater(contentPlaceholderGroup.ContentPlaceholderGroupId, 0);

      ContentPlaceholderGroup dsContentPlaceholderGroup = manager.GetContentPlaceholderGroup(contentPlaceholderGroup.ContentPlaceholderGroupId);
      Assert.IsNotNull(dsContentPlaceholderGroup);

      return dsContentPlaceholderGroup;
    }
    internal static void Delete(IDataStore dataStore, ContentPlaceholderGroup contentPlaceholderGroup)
    {
      ContentPlaceholderGroupManager manager = new ContentPlaceholderGroupManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(contentPlaceholderGroup);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetContentPlaceholderGroup(contentPlaceholderGroup.ContentPlaceholderGroupId));

      Trace.WriteLine("Successfully deleted contentPlaceholderGroup " + contentPlaceholderGroup.ContentPlaceholderGroupId);
    }

    internal static void PopulateWithRandomValues(ContentPlaceholderGroup record, DummyDataManager dtm, Random random)
    {

    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteContentPlaceholderGroup()
    {
      ContentPlaceholderGroupManager manager = new ContentPlaceholderGroupManager(this.DataStore);
      ContentPlaceholderGroup record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);

      ContentPlaceholderGroup recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetContentPlaceholderGroup(record.ContentPlaceholderGroupId);

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
