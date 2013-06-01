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

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS
{
  [TestFixture]
  public class Test_CMSGroups : TestSetup
  {
    internal CMSGroup Create(IDataStore dataStore, Random random)
    {
      CMSGroupManager manager = new CMSGroupManager(dataStore);

      int groupId = random.Next(int.MinValue + 1, -100000);

      CMSGroup group = new CMSGroup(groupId, "TestGroup " + random.Next(1000000, 10000000), "Description " + random.Next(1000000, 10000000), DebugUtility.GetRandomEnum<CMSGroupType>(random));

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(group);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);

      CMSGroup dsGroup = manager.GetGroup(groupId);
      Assert.IsNotNull(dsGroup);

      return dsGroup;
    }
    internal static void Delete(IDataStore dataStore, CMSGroup group)
    {
      CMSGroupManager manager = new CMSGroupManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(group);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetGroup(group.CMSGroupId));

      Trace.WriteLine("Successfully deleted group " + group.Name);
    }

    internal static void PopulateWithRandomValues(CMSGroup record, DummyDataManager dtm, Random random)
    {
      record.Name = "TestGroup " + random.Next(1000000, 10000000);
      record.Description = "Description " + random.Next(1000000, 10000000);
      record.CMSGroupType = DebugUtility.GetRandomEnum<CMSGroupType>(random);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteGroup()
    {
      CMSGroupManager manager = new CMSGroupManager(this.DataStore);
      CMSGroup record = Create(this.DataStore, this.Random);

      CMSGroup recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetGroup(record.CMSGroupId);

        string errors = string.Empty;
        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, record);
    }
    [Test]
    public void Test_Gets()
    {
      List<CMSGroup> records = new List<CMSGroup>();
      CMSGroupManager manager = new CMSGroupManager(this.DataStore);

      CMSGroupType groupType = DebugUtility.GetRandomEnum<CMSGroupType>(this.Random);
      for (int i = 0; i < 10; i++)
        records.Add(Create(this.DataStore, this.Random));
      foreach (CMSGroup record in records)
      {
        record.CMSGroupType = groupType;
        manager.Update(record);
      }

      List<CMSGroup> dsRecords = manager.GetAllGroups(groupType);
      Assert.GreaterOrEqual(dsRecords.Count, records.Count);

      foreach (CMSGroup record in records)
        Assert.AreEqual(1, dsRecords.Count(c => c.CMSGroupId == record.CMSGroupId));

      Assert.AreEqual(0, dsRecords.Count(c => c.CMSGroupType != groupType));
      
      foreach (CMSGroup record in records)
        Delete(this.DataStore, record);      
    }
    #endregion
  }
}
