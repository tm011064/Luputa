using System.Collections.Generic;

namespace Workmate.Data
{
  public interface IDataStore
  {
    void Initialize(string connectionString);

    IDataStoreContext CreateContext();
    IDataStoreContext CreateContext(int defaultCommandTimeout);
  }
}
