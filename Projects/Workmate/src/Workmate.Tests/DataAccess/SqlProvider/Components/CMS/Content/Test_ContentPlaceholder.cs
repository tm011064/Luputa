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
using Workmate.Components.Entities.Membership;
using Workmate.Tests.DataAccess.SqlProvider.Components.Membership;
using Workmate.Components.Contracts.CMS.Content;
using System.Collections.Generic;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS.ContentBlocks
{
  [TestFixture]
  public class Test_ContentPlaceholders : TestSetup
  {
    internal static ContentPlaceholder Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application, ContentPlaceholderGroup contentPlaceholderGroup, Random random)
    {
      ContentPlaceholderManager manager = new ContentPlaceholderManager(dataStore);

      ContentPlaceholder contentPlaceholder = new ContentPlaceholder(
        application.ApplicationId
        , contentPlaceholderGroup
        , "TestContentPlaceholder " + random.Next(1000000, 10000000));

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(contentPlaceholder);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.Greater(contentPlaceholder.ContentPlaceholderId, 0);

      ContentPlaceholder dsContentPlaceholder = manager.GetContentPlaceholder(contentPlaceholder.ContentPlaceholderId);
      Assert.IsNotNull(dsContentPlaceholder);

      return dsContentPlaceholder;
    }
    internal static void Delete(IDataStore dataStore, ContentPlaceholder contentPlaceholder)
    {
      ContentPlaceholderManager manager = new ContentPlaceholderManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(contentPlaceholder);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetContentPlaceholder(contentPlaceholder.ContentPlaceholderId));

      Trace.WriteLine("Successfully deleted contentPlaceholder " + contentPlaceholder.Name);
    }

    internal static void PopulateWithRandomValues(ContentPlaceholder record, DummyDataManager dtm, Random random)
    {
      record.Name = "TestContentPlaceholder " + random.Next(1000000, 10000000);
      record.Description = "Description " + random.Next(1000000, 10000000);
      record.IsActive = DebugUtility.FlipCoin(random);
      record.IsModerated = DebugUtility.FlipCoin(random);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteContentPlaceholder()
    {
      ContentPlaceholderGroup contentPlaceholderGroup = Test_ContentPlaceholderGroup.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);
      ContentPlaceholderManager manager = new ContentPlaceholderManager(this.DataStore);
      ContentPlaceholder record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, contentPlaceholderGroup, this.Random);

      ContentPlaceholder recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetContentPlaceholder(record.ContentPlaceholderId);

        string errors = string.Empty;

        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, record);
    }


    [Test]
    public void Test_UpdateContentBlock()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      ContentPlaceholderGroup contentPlaceholderGroup = Test_ContentPlaceholderGroup.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);
      ContentPlaceholderManager manager = new ContentPlaceholderManager(this.DataStore);
      ContentBlockManager contentBlockManager = new ContentBlockManager(this.DataStore);

      string key = "test_content_block";      

      int contentBlockId;
      Dictionary<string, string> lookup;

      ContentUpdateStatus contentUpdateStatus = manager.UpdateContentBlock(this.Application.ApplicationId, userBasic
        , key, "Body1", out contentBlockId);

      // TODO (Roman): Html testing...

      Assert.AreEqual(ContentUpdateStatus.Success, contentUpdateStatus);
      Assert.Greater(contentBlockId, 0);

      ContentBlock contentBlock1 = contentBlockManager.GetContentBlock(contentBlockId);
      Assert.IsNotNull(contentBlock1);
      Assert.AreEqual(ContentBlockStatus.Active, contentBlock1.ContentBlockStatus);
      Assert.AreEqual("Body1", contentBlock1.FormattedBody);

      lookup = manager.GetContentPlaceholderBodies(this.Application.ApplicationId);
      Assert.IsNotNull(lookup);
      Assert.IsTrue(lookup.ContainsKey(key));
      Assert.AreEqual(lookup[key], "Body1");

      contentUpdateStatus = manager.UpdateContentBlock(this.Application.ApplicationId, userBasic
        , key, "Body2", out contentBlockId);

      Assert.AreEqual(ContentUpdateStatus.Success, contentUpdateStatus);
      Assert.Greater(contentBlockId, 0);

      ContentBlock contentBlock2 = contentBlockManager.GetContentBlock(contentBlockId);

      Assert.IsNotNull(contentBlock2);
      Assert.AreEqual(ContentBlockStatus.Active, contentBlock2.ContentBlockStatus);
      Assert.AreEqual("Body2", contentBlock2.FormattedBody);

      contentBlock1 = contentBlockManager.GetContentBlock(contentBlock1.ContentBlockId);
      Assert.IsNotNull(contentBlock1);
      Assert.AreEqual(ContentBlockStatus.Inactive, contentBlock1.ContentBlockStatus);
      Assert.AreEqual("Body1", contentBlock1.FormattedBody);

      lookup = manager.GetContentPlaceholderBodies(this.Application.ApplicationId);
      Assert.IsNotNull(lookup);
      Assert.IsTrue(lookup.ContainsKey(key));
      Assert.AreEqual(lookup[key], "Body2");
    }
    #endregion
  }
}
