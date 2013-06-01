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
  public class Test_CMSSections : TestSetup
  {
    internal static CMSSection Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application, CMSGroup group, Random random)
    {
      CMSSectionManager manager = new CMSSectionManager(dataStore);

      CMSSectionType sectionType = DebugUtility.GetRandomEnum<CMSSectionType>(random);

      CMSSection section = new CMSSection(
        application.ApplicationId
        , "TestSection " + random.Next(1000000, 10000000)
        , true
        , false
        , sectionType);

      if (group != null)
        section.CMSGroupId = group.CMSGroupId;

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(section);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.Greater(section.CMSSectionId, 0);

      CMSSection dsSection = manager.GetSection(sectionType, section.CMSSectionId);
      Assert.IsNotNull(dsSection);
      if (group != null)
        Assert.AreEqual(section.CMSGroupId, group.CMSGroupId);

      return dsSection;
    }
    internal static void Delete(IDataStore dataStore, CMSSection section)
    {
      CMSSectionManager manager = new CMSSectionManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(section, true);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetSection(section.CMSSectionType, section.CMSSectionId));

      Trace.WriteLine("Successfully deleted section " + section.Name);
    }
    internal static void PopulateWithRandomValues(CMSSection record, DummyDataManager dtm, Random random)
    {
      record.Name = "TestSection " + random.Next(1000000, 10000000);
      record.Description = "Description " + random.Next(1000000, 10000000);
      record.CMSSectionType = DebugUtility.GetRandomEnum<CMSSectionType>(random);
      record.IsActive = DebugUtility.FlipCoin(random);
      record.IsModerated = DebugUtility.FlipCoin(random);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteSection()
    {
      CMSSectionManager manager = new CMSSectionManager(this.DataStore);
      CMSSection record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, null, this.Random);

      CMSSection recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetSection(record.CMSSectionType, record.CMSSectionId);

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
      List<CMSSection> records = new List<CMSSection>();
      CMSSectionManager manager = new CMSSectionManager(this.DataStore);

      CMSSectionType sectionType = DebugUtility.GetRandomEnum<CMSSectionType>(this.Random);
      for (int i = 0; i < 10; i++)
        records.Add(Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, null, this.Random));
      foreach (CMSSection record in records)
      {
        record.CMSSectionType = sectionType;
        manager.Update(record);
      }

      List<CMSSection> dsRecords = manager.GetAllSections(this.Application.ApplicationId, sectionType).ToList();
      Assert.GreaterOrEqual(dsRecords.Count, records.Count);

      foreach (CMSSection record in records)
        Assert.AreEqual(1, dsRecords.Count(c => c.CMSSectionId == record.CMSSectionId));

      Assert.AreEqual(0, dsRecords.Count(c => c.CMSSectionType != sectionType));

      foreach (CMSSection record in records)
        Delete(this.DataStore, record);
    }
    #endregion
  }
}
