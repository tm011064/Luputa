using System;
using System.Diagnostics;
using CommonTools.Components.BusinessTier;
using CommonTools.Components.Testing;
using NUnit.Framework;
using Workmate.Components.CMS.Content;
using Workmate.Components.Contracts.CMS.Content;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS.Content;
using Workmate.Components.Entities.Membership;
using Workmate.Configuration;
using Workmate.Data;
using Workmate.Tests.DataAccess.SqlProvider.Components.Membership;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS.ContentBlocks
{
  [TestFixture]
  public class Test_ContentBlocks : TestSetup
  {
    internal static ContentBlock Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application
      , IUserBasic author, ContentPlaceholderHistory contentPlaceholderHistory, Random random)
    {
      ContentBlockManager manager = new ContentBlockManager(dataStore);

      ContentBlock contentBlock = new ContentBlock(
        author
        , contentPlaceholderHistory
        , "ContentBlock Body" + random.Next(1000000, 10000000)
        , Workmate.Components.Contracts.CMS.Content.ContentBlockStatus.Active);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(contentBlock);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.Greater(contentBlock.ContentBlockId, 0);

      ContentBlock dsContentBlock = manager.GetContentBlock(contentBlock.ContentBlockId);
      Assert.IsNotNull(dsContentBlock);

      return dsContentBlock;
    }
    internal static void Delete(IDataStore dataStore, ContentBlock contentBlock)
    {
      ContentBlockManager manager = new ContentBlockManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(contentBlock);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetContentBlock(contentBlock.ContentBlockId));

      Trace.WriteLine("Successfully deleted contentBlock " + contentBlock.Subject);
    }

    internal static void PopulateWithRandomValues(ContentBlock record, DummyDataManager dtm, Random random)
    {
      record.FormattedBody = "ContentBlock Body" + random.Next(1000000, 10000000);
      record.IsApproved = DebugUtility.FlipCoin(random);
      record.IsLocked = DebugUtility.FlipCoin(random);
      record.Subject = "ContentBlock Status" + random.Next(1000000, 10000000);
      record.UrlFriendlyName = record.Subject;
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteContentBlock()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      ContentPlaceholderGroup contentPlaceholderGroup = Test_ContentPlaceholderGroup.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);
      ContentPlaceholder contentPlaceholder = Test_ContentPlaceholders.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, contentPlaceholderGroup, this.Random);
      ContentPlaceholderHistory contentPlaceholderHistory = Test_ContentPlaceholderHistorys.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, contentPlaceholder, this.Random);

      ContentBlockManager manager = new ContentBlockManager(this.DataStore);
      ContentBlock record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic, contentPlaceholderHistory, this.Random);
      
      Delete(this.DataStore, record);
      Test_ContentPlaceholders.Delete(this.DataStore, contentPlaceholder);
      Test_ContentPlaceholderGroup.Delete(this.DataStore, contentPlaceholderGroup);
    }
    #endregion
  }
}
