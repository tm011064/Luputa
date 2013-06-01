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

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS
{
  [TestFixture]
  public class Test_CMSThreads : TestSetup
  {
    internal static CMSThread Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application, CMSSection section, Random random)
    {
      CMSThreadManager manager = new CMSThreadManager(dataStore);

      CMSThread thread = new CMSThread(
        section
        , true
        , -1);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(thread);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.Greater(thread.CMSThreadId, 0);

      CMSThread dsThread = manager.GetThread(section.CMSSectionType, thread.CMSThreadId);
      Assert.IsNotNull(dsThread);

      return dsThread;
    }
    internal static void Delete(IDataStore dataStore, CMSThread thread, CMSSectionType sectionType)
    {
      CMSThreadManager manager = new CMSThreadManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(thread, true);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetThread(sectionType, thread.CMSThreadId));

      Trace.WriteLine("Successfully deleted thread " + thread.CMSName);
    }

    internal static void PopulateWithRandomValues(CMSThread record, DummyDataManager dtm, Random random)
    {
      record.CMSIsSticky = DebugUtility.FlipCoin(random);
      record.CMSLastViewedDateUtc = DateTime.UtcNow.AddMilliseconds(random.Next(-1000000, 1000000));
      record.CMSName = "TestThread " + random.Next(1000000, 10000000);
      record.CMSStickyDateUtc = DebugUtility.FlipCoin(random) ? null : (DateTime?)DateTime.UtcNow.AddMilliseconds(random.Next(-1000000, 1000000));
      record.CMSThreadStatus = random.Next(-1000000, 1000000);
      record.IsApproved = DebugUtility.FlipCoin(random);
      record.IsLocked = DebugUtility.FlipCoin(random);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteThread()
    {
      CMSSection section = Test_CMSSections.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, null, this.Random);

      CMSThreadManager manager = new CMSThreadManager(this.DataStore);
      CMSThread record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, section, this.Random);

      CMSThread recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetThread(section.CMSSectionType, record.CMSThreadId);

        string errors = string.Empty;
        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, record, section.CMSSectionType);
      Test_CMSSections.Delete(this.DataStore, section);
    }
    [Test]
    public void Test_Gets()
    {
      CMSSection section = Test_CMSSections.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, null, this.Random);

      List<CMSThread> records = new List<CMSThread>();
      CMSThreadManager manager = new CMSThreadManager(this.DataStore);

      for (int i = 0; i < 10; i++)
        records.Add(Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, section, this.Random));

      List<CMSThread> dsRecords = manager.GetAllThreads(this.Application.ApplicationId, section.CMSSectionType);
      Assert.GreaterOrEqual(dsRecords.Count, records.Count);

      foreach (CMSThread record in records)
        Assert.AreEqual(1, dsRecords.Count(c => c.CMSThreadId == record.CMSThreadId));

      foreach (CMSThread record in records)
        Delete(this.DataStore, record, section.CMSSectionType);

      Test_CMSSections.Delete(this.DataStore, section);
    }
    #endregion
  }
}
