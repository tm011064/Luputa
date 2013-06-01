using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;

namespace Workmate.Configuration
{
  public class ApplicationSettings : IApplicationSettings
  {
    public string Log4NetConfigPath { get; private set; }
    public string DataStoreContextType { get; private set; }

    public string DefaultConnectionString { get; private set; }
    public string DefaultConnectionStringName { get; private set; }
    
    #region constructors
    public ApplicationSettings(NameValueCollection appSettings, ConnectionStringSettingsCollection connectionStrings)
    {
      this.Log4NetConfigPath = appSettings["log4netConfigPath"] ?? "log4net.config";
      this.DataStoreContextType = appSettings["dataStoreContextType"] ?? "NOT SET";

      this.DefaultConnectionStringName = appSettings["defaultConnectionStringName"] ?? "NOT SET";
      this.DefaultConnectionString = connectionStrings[this.DefaultConnectionStringName].ConnectionString;
    }
    #endregion
  }
}
