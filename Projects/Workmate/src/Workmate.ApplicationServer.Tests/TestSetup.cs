using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Data.SqlClient;
using System.Configuration;
using Workmate.Data.SqlProvider;
using Workmate.Data;
using System.Reflection;
using System.IO;
using CommonTools.Components.Testing;
using Workmate.Components.Membership;
using System.Collections.Specialized;
using Workmate.Components;
using Workmate.Components.Contracts.Membership;
using Workmate.Components.Contracts;
using Workmate.ApplicationServer.Configuration;

namespace Workmate.ApplicationServer.Tests
{

  public class TestSetup : BaseTestSetup
  {
    #region members

    #endregion

    #region properties
    protected IApplication Application { get; private set; }
    protected string ConnectionString { get; set; }
    protected DummyDataManager DummyDataManager { get; set; }
    protected int DefaultUpdateTestIterations { get; set; }
    protected Random Random { get; set; }

    private SqlServerDataStore _SqlServerDataStore;
    protected override IDataStore DataStore { get { return _SqlServerDataStore; } }
    #endregion

    #region methods

    #endregion

    [TestFixtureSetUp]
    public virtual void Setup()
    {
      // run the testsetupscript...
      this.ConnectionString = ConfigurationManager.ConnectionStrings["WorkmateDatabase"].ConnectionString;

      using (SqlConnection connection = new SqlConnection(this.ConnectionString))
      {
        connection.Open(); // this will throw an exception if we can't connect
      }

      string path = Path.Combine(this.ApplicationPath, @"DataAccess\SqlProvider\SetupScript.sql");
      string sql = File.ReadAllText(path);

      using (SqlConnection connection = new SqlConnection(this.ConnectionString))
      {
        connection.Open(); // this will throw an exception if we can't connect

        using (SqlCommand command = connection.CreateCommand())
        {
          command.CommandText = sql;
          command.CommandType = System.Data.CommandType.Text;

          command.ExecuteNonQuery();
        }
      }

      _SqlServerDataStore = new SqlServerDataStore(this.ConnectionString);
      this.DummyDataManager = new DummyDataManager(Path.Combine(this.ApplicationPath, @"DataAccess\DummyData.xml"));

      this.DefaultUpdateTestIterations = 10;
      this.Random = new Random();

      ApplicationSettings applicationSettings = new ApplicationSettings(
        ConfigurationManager.AppSettings
        , ConfigurationManager.ConnectionStrings);           

      Workmate.Components.ApplicationManager applicationManger = new Workmate.Components.ApplicationManager(this.DataStore);
      this.Application = applicationManger.GetApplication("debug_test_setup");
      if (this.Application == null)
      {
        this.Application = new Workmate.Components.Entities.Application("debug_test_setup", "Auto generated at " + DateTime.UtcNow.ToString() + " (UTC)");

        var report = applicationManger.Create(this.Application);
        if (report.Status != DataRepositoryActionStatus.Success)
        {
          throw new ApplicationException("Unable to create debug test setup");
        }
      }
    }
  }
}
