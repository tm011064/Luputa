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
using Workmate.Components.Contracts.Company;
using Workmate.Components.Company;
using Workmate.Components.Entities.Company;
using Workmate.Configuration;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.Company
{
  [TestFixture]
  public class Test_Offices : TestSetup
  {
    internal static IOfficeModel Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application
      , DummyDataManager dtm, Random random)
    {
      OfficeManager manager = new OfficeManager(dataStore);

      IOfficeModel office = new OfficeModel(
        application.ApplicationId
        , "OfficeName" + random.Next(1000000, 10000000));

      PopulateWithRandomValues(office, dtm, random);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(application.ApplicationId, office);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);

      IOfficeModel dsOfficeModel = manager.GetOffice(office.ApplicationId, office.OfficeId);
      Assert.IsNotNull(dsOfficeModel);

      return dsOfficeModel;
    }
    internal static void Delete(IDataStore dataStore, IOfficeModel office)
    {
      OfficeManager manager = new OfficeManager(dataStore);

      int num = manager.Delete(office.ApplicationId, office.OfficeId);

      Assert.AreEqual(1, num);
      Assert.IsNull(manager.GetOffice(office.ApplicationId, office.OfficeId));

      Trace.WriteLine("Successfully deleted office " + office.Name);
    }

    internal static void PopulateWithRandomValues(IOfficeModel record, DummyDataManager dtm, Random random)
    {
      record.AddressLine1 = DebugUtility.FlipCoin(random) ? dtm.GetDummyText(5, 32) : null;
      record.AddressLine2 = DebugUtility.FlipCoin(random) ? dtm.GetDummyText(5, 32) : null;
      record.AddressLine3 = DebugUtility.FlipCoin(random) ? dtm.GetDummyText(5, 32) : null;
      record.AddressLine4 = DebugUtility.FlipCoin(random) ? dtm.GetDummyText(5, 32) : null;

      record.ContactNumber = DebugUtility.FlipCoin(random) ? dtm.GetDummyText(5, 32) : null;
      record.Country = DebugUtility.FlipCoin(random) ? dtm.GetDummyText(5, 32) : null;
      record.Description = DebugUtility.FlipCoin(random) ? dtm.GetDummyText(5, 32) : null;
      record.Email = DebugUtility.FlipCoin(random) ? dtm.GetDummyText(5, 32) : null;
      record.Fax = DebugUtility.FlipCoin(random) ? dtm.GetDummyText(5, 32) : null;
      record.Location = DebugUtility.FlipCoin(random) ? dtm.GetDummyText(5, 32) : null;
      record.PostCode = DebugUtility.FlipCoin(random) ? dtm.GetDummyText(5, 8) : null;

      record.Name = "TestOfficeModel " + random.Next(1000000, 10000000);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteOffice()
    {
      OfficeManager manager = new OfficeManager(this.DataStore);
      IOfficeModel record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application
        , this.DummyDataManager, this.Random);

      IOfficeModel recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetOffice(record.ApplicationId, record.OfficeId);

        string errors = string.Empty;

        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, record);
    }
    #endregion
  }
}
