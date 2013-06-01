using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Data;
using System.Data.SqlClient;

namespace Workmate.Data.SqlProvider
{
  public class SqlServerDataStore : IDataStore
  {
    public string ConnectionString { get; private set; }

    #region IDataStore Members

    public IDataStoreContext CreateContext()
    {
      return new SqlServerDataStoreContext(this.ConnectionString, 30);
    }
    public IDataStoreContext CreateContext(int defaultCommandTimeout)
    {
      return new SqlServerDataStoreContext(this.ConnectionString, defaultCommandTimeout);
    }

    #endregion

    #region general
    public void Initialize(string connectionString)
    {
      this.ConnectionString = connectionString;
    }
    #endregion

    public SqlServerDataStore() { }
    public SqlServerDataStore(string connectionString)
    {
      this.ConnectionString = connectionString;
    }
  }
}
