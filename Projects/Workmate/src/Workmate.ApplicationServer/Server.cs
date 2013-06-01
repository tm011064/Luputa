using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using log4net;
using Workmate.Messaging;
using Workmate.Data;
using System.Reflection;
using System.Configuration;
using System.IO;
using Workmate.ApplicationServer.Configuration;
using Workmate.Components.Emails;

namespace Workmate.ApplicationServer
{
  public class Server : IDisposable
  {
    private ILog _Log = LogManager.GetLogger("Server");
    private ServiceHost _ServiceHost;
    private Exchange _Exchange;
    private EmailPublisherDaemon _EmailPublisherDaemon;

    public void Start()
    {
      _EmailPublisherDaemon.Start();

      try { _ServiceHost.Open(TimeSpan.FromSeconds(5)); }
      catch (Exception ex)
      {
        _Log.Error("Unable to open server exchange. See inner exception for further details.", ex);
        Dispose();
      }      
    }

    #region IDisposable Members
    private bool _IsDisposed = false;
    public void Dispose()
    {
      if (!_IsDisposed)
      {
        _IsDisposed = true;

        if (_ServiceHost != null)
        {
          try { _ServiceHost.Close(TimeSpan.FromSeconds(5)); }
          catch { }

          _ServiceHost = null;
        }
      }
    }

    #endregion

    #region constructors
    public Server(Exchange exchange, Uri baseAddress)
    {
      IApplicationSettings applicationSettings = new ApplicationSettings(ConfigurationManager.AppSettings, ConfigurationManager.ConnectionStrings);

      log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(applicationSettings.Log4NetConfigPath));

      IDataStore dataStore = null;

      try
      {
        string dataStoreContextTypeAssemblyName = applicationSettings.DataStoreContextType.Substring(applicationSettings.DataStoreContextType.LastIndexOf(',') + 1).Trim();
        string dataStoreContextTypeName = applicationSettings.DataStoreContextType.Substring(0, applicationSettings.DataStoreContextType.IndexOf(','));

        Assembly assembly = Assembly.Load(dataStoreContextTypeAssemblyName); // load into default load context
        Type type = assembly.GetType(dataStoreContextTypeName);
        dataStore = Activator.CreateInstance(type) as IDataStore;
      }
      catch (Exception ex)
      {
        throw new ConfigurationErrorsException("Error loading data store object, see inner exception for further details.", ex);
      }
      if (dataStore == null)
        throw new ConfigurationErrorsException("Datastore is not provided.");

      dataStore.Initialize(applicationSettings.DefaultConnectionString);

      InstanceContainer.Initialize(
        applicationSettings
        , new EmailManager(dataStore)
        );

      _Exchange = exchange;
      _ServiceHost = new ServiceHost(exchange, baseAddress);
      _EmailPublisherDaemon = new EmailPublisherDaemon(applicationSettings);
    }
    #endregion
  }
}
