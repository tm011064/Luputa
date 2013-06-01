using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Configuration
{
  public interface IApplicationSettings
  {
    string Log4NetConfigPath { get; }
    string DataStoreContextType { get; }

    string DefaultConnectionString { get; }
    string DefaultConnectionStringName { get; }
  }
}
