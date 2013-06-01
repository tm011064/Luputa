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
  public class Test_Departments : TestSetup
  {
    internal IDepartmentModel Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application
      , DummyDataManager dtm, Random random)
    {
      DepartmentManager manager = new DepartmentManager(dataStore);

      IDepartmentModel department = new DepartmentModel(
        application.ApplicationId
        , "DepartmentName" + random.Next(1000000, 10000000));

      PopulateWithRandomValues(department, dtm, random);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(application.ApplicationId, department);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);

      IDepartmentModel dsDepartmentModel = manager.GetDepartment(department.ApplicationId, department.DepartmentId);
      Assert.IsNotNull(dsDepartmentModel);

      return dsDepartmentModel;
    }
    internal static void Delete(IDataStore dataStore, IDepartmentModel department)
    {
      DepartmentManager manager = new DepartmentManager(dataStore);

      int num = manager.Delete(department.ApplicationId, department.DepartmentId);

      Assert.GreaterOrEqual(num, 1);
      Assert.IsNull(manager.GetDepartment(department.ApplicationId, department.DepartmentId));

      Trace.WriteLine("Successfully deleted department " + department.Name);
    }

    internal static void PopulateWithRandomValues(IDepartmentModel record, DummyDataManager dtm, Random random)
    {
      record.Name = "TestDepartmentModel " + random.Next(1000000, 10000000);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteDepartment()
    {
      DepartmentManager manager = new DepartmentManager(this.DataStore);
      IDepartmentModel record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application
        , this.DummyDataManager, this.Random);

      IDepartmentModel recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetDepartment(record.ApplicationId, record.DepartmentId);

        string errors = string.Empty;

        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, record);
    }
    [Test]
    public void Test_Gets()
    {
      DepartmentManager manager = new DepartmentManager(this.DataStore);
     
      IOfficeModel office_A = Test_Offices.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application
        , this.DummyDataManager, this.Random);
      IOfficeModel office_B_And_C = Test_Offices.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application
        , this.DummyDataManager, this.Random);
      
      IDepartmentModel record_C = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application
        , this.DummyDataManager, this.Random);
      IDepartmentModel record_B = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application
        , this.DummyDataManager, this.Random);
      IDepartmentModel record_A = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application
        , this.DummyDataManager, this.Random);

      record_C.OfficeId = office_B_And_C.OfficeId;        
      record_C.ParentDepartmentId = record_B.DepartmentId;
      manager.Update(record_C);

      record_B.OfficeId = office_B_And_C.OfficeId;
      record_B.ParentDepartmentId = record_A.DepartmentId;
      manager.Update(record_B);

      record_A.OfficeId = office_A.OfficeId;
      manager.Update(record_A);

      IDepartmentWithOfficesModel record = manager.GetDepartment(this.Application.ApplicationId, record_C.DepartmentId);

      Assert.IsNotNull(record);
      Assert.IsNotNull(record.Office);
      Assert.IsNotNull(record.ParentDepartment);
      Assert.IsNotNull(record.ParentDepartment.Office);
      Assert.IsNotNull(record.ParentDepartment.ParentDepartment);
      Assert.IsNotNull(record.ParentDepartment.ParentDepartment.Office);

      Assert.IsNull(record.ParentDepartment.ParentDepartment.ParentDepartment);
      
      Assert.AreEqual(record.Office.OfficeId, office_B_And_C.OfficeId);            
      Assert.AreEqual(record_B.DepartmentId, record.ParentDepartment.DepartmentId);
      Assert.AreEqual(record_B.OfficeId, record.ParentDepartment.Office.OfficeId);

      Assert.AreEqual(record_A.DepartmentId, record.ParentDepartment.ParentDepartment.DepartmentId);
      Assert.AreEqual(record_A.OfficeId, record.ParentDepartment.ParentDepartment.Office.OfficeId);

      Assert.IsNotNull(record.Offices);
      Assert.GreaterOrEqual(record.Offices.Count, 2);
      Assert.IsNotNull(record.Offices.Find(c => c.OfficeId == office_A.OfficeId));
      Assert.IsNotNull(record.Offices.Find(c => c.OfficeId == office_B_And_C.OfficeId));

      Test_Offices.Delete(this.DataStore, office_A);
      Delete(this.DataStore, record_A);
      Test_Offices.Delete(this.DataStore, office_B_And_C);
    }
    #endregion
  }
}
