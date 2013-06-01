using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Data.MockDataProvider
{
  public class MockDataStore : IDataStore
  {
    public string ConnectionString { get; private set; }

    #region IDataStore Members

    public IDataStoreContext CreateContext()
    {
      return new MockDataStoreContext();
    }
    public IDataStoreContext CreateContext(int defaultCommandTimeout)
    {
      return new MockDataStoreContext();
    }

    #endregion

    #region general
    public void Initialize(string connectionString)
    {
      this.ConnectionString = connectionString;
    }
    #endregion

    public MockDataStore() { }
    public MockDataStore(string connectionString)
    {
      this.ConnectionString = connectionString;
    }
  }
}
