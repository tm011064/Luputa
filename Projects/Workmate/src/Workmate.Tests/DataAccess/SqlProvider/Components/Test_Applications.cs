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
using Workmate.Components;
using Workmate.Components.Entities;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS
{
  [TestFixture]
  public class Test_Applications : TestSetup
  {
    internal IApplication Create(IDataStore dataStore, Random random)
    {
      ApplicationManager manager = new ApplicationManager(dataStore);

      int applicationId = random.Next(int.MinValue + 1, -100000);

      Application application = new Application("TestApplication " + random.Next(1000000, 10000000), "Description " + random.Next(1000000, 10000000));

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(application);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);

      IApplication dsApplication = manager.GetApplication(application.ApplicationName);
      Assert.IsNotNull(dsApplication);

      return dsApplication;
    }
    internal static void Delete(IDataStore dataStore, IApplication application)
    {
      ApplicationManager manager = new ApplicationManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(application);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetApplication(application.ApplicationName));

      Trace.WriteLine("Successfully deleted application " + application.ApplicationName);
    }


    #region tests
    [Test]
    public void Test_CreateDeleteApplication()
    {
      ApplicationManager manager = new ApplicationManager(this.DataStore);
      IApplication record = Create(this.DataStore, this.Random);
      
      Delete(this.DataStore, record);
    }
    #endregion
  }
}
