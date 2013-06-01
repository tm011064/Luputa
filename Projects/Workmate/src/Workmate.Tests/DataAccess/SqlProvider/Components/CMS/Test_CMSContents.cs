using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Workmate.Components.CMS;
using Workmate.Components.Entities.CMS;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Contracts;
using CommonTools.Components.BusinessTier;
using Workmate.Data;
using CommonTools.Components.Testing;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Workmate.Configuration;
using Workmate.Tests.DataAccess.SqlProvider.Components.Membership;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS
{
  [TestFixture]
  public class Test_CMSContents : TestSetup
  {
    internal static CMSContent Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application
      , int authorUserId, CMSThread thread, Random random)
    {
      CMSContentManager manager = new CMSContentManager(dataStore);

      CMSContent content = new CMSContent(
        authorUserId
        , thread
        , 0
        , 0
        , "Content Status" + random.Next(1000000, 10000000)
        , "Content Body" + random.Next(1000000, 10000000)
        , true);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(content, null, null, false, false, null, null, null);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.Greater(content.CMSContentId, 0);

      CMSContent dsContent = manager.GetContent(content.CMSContentId);
      Assert.IsNotNull(dsContent);

      return dsContent;
    }
    internal static void Delete(IDataStore dataStore, CMSContent content)
    {
      CMSContentManager manager = new CMSContentManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(content, true);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetContent(content.CMSContentId));

      Trace.WriteLine("Successfully deleted content " + content.Subject);
    }

    internal static void PopulateWithRandomValues(CMSContent record, DummyDataManager dtm, Random random)
    {
      record.CMSContentStatus = (byte)random.Next(1, 256);
      record.CMSContentType = (byte)random.Next(1, 256);
      record.FormattedBody = "Content Body" + random.Next(1000000, 10000000);
      record.IsApproved = DebugUtility.FlipCoin(random);
      record.IsLocked = DebugUtility.FlipCoin(random);
      record.Subject = "Content Status" + random.Next(1000000, 10000000);
      record.UrlFriendlyName = record.Subject;
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteContent()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      CMSSection section = Test_CMSSections.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, null, this.Random);
      CMSThread thread = Test_CMSThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, section, this.Random);

      CMSContentManager manager = new CMSContentManager(this.DataStore);
      CMSContent record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic.UserId, thread, this.Random);

      CMSContent recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetContent(record.CMSContentId);

        string errors = string.Empty;
        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, record);
      Test_CMSSections.Delete(this.DataStore, section);
    }
    [Test]
    public void Test_Gets()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      CMSSection section = Test_CMSSections.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, null, this.Random);
      CMSThread thread = Test_CMSThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, section, this.Random);

      List<CMSContent> records = new List<CMSContent>();
      CMSContentManager manager = new CMSContentManager(this.DataStore);

      for (int i = 0; i < 10; i++)
        records.Add(Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic.UserId, thread, this.Random));

      List<CMSContent> dsRecords = manager.GetContents(this.Application.ApplicationId, section.CMSSectionType);
      Assert.GreaterOrEqual(dsRecords.Count, records.Count);

      foreach (CMSContent record in records)
        Assert.AreEqual(1, dsRecords.Count(c => c.CMSContentId == record.CMSContentId));

      foreach (CMSContent record in records)
        Delete(this.DataStore, record);

      Test_CMSSections.Delete(this.DataStore, section);
    }

    [Test]
    public void Test_Delete_Section()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      CMSSection section = Test_CMSSections.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, null, this.Random);
      CMSThread thread = Test_CMSThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, section, this.Random);
      CMSContent content = Test_CMSContents.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic.UserId, thread, this.Random);

      Test_CMSSections.Delete(this.DataStore, section);

      CMSThreadManager threadManager = new CMSThreadManager(this.DataStore);
      CMSContentManager contentManager = new CMSContentManager(this.DataStore);

      Assert.IsNull(threadManager.GetThread(section.CMSSectionType, thread.CMSThreadId));
      Assert.IsNull(contentManager.GetContent(content.CMSContentId));
    }
    [Test]
    public void Test_Delete_Thread()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      CMSSection section = Test_CMSSections.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, null, this.Random);
      CMSThread thread = Test_CMSThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, section, this.Random);
      CMSContent content = Test_CMSContents.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic.UserId, thread, this.Random);

      Test_CMSThreads.Delete(this.DataStore, thread, section.CMSSectionType);

      CMSThreadManager threadManager = new CMSThreadManager(this.DataStore);
      CMSContentManager contentManager = new CMSContentManager(this.DataStore);

      Assert.IsNull(threadManager.GetThread(section.CMSSectionType, thread.CMSThreadId));
      Assert.IsNull(contentManager.GetContent(content.CMSContentId));

      Test_CMSSections.Delete(this.DataStore, section);
    }
    #endregion
  }
}
