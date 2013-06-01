using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using log4net.Config;
using System.Configuration;
using System.Diagnostics;
using System.ServiceModel.Configuration;
using log4net;
using Workmate.Messaging;

namespace Workmate.ApplicationServer.Console
{
  class Program
  {
    private static ILog _Log = LogManager.GetLogger("Server.Console");

    static void Main(string[] args)
    {
      XmlConfigurator.Configure();

      Exchange exchange = new Exchange();
      ExchangeMessageMonitor exchangeMessageMonitor = new ExchangeMessageMonitor(TimeSpan.FromSeconds(15), exchange);
      exchangeMessageMonitor.StartSnapshotLogging();

      System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
      ServiceModelSectionGroup serviceModelSectionGroup = config.GetSectionGroup("system.serviceModel") as ServiceModelSectionGroup;

      if (serviceModelSectionGroup == null)
      {
        _Log.Error("Unable to load service model section group");
        return;
      }
      ServiceElement serviceElement = (from c in serviceModelSectionGroup.Services.Services.Cast<ServiceElement>()
                                       where c.Name == "Workmate.Messaging.Exchange"
                                       select c).FirstOrDefault();
      if (serviceElement == null)
      {
        _Log.Error("Unable to load service with name Workmate.Messaging.Exchange");
        return;
      }
      if (serviceElement.Endpoints.Count == 0)
      {
        _Log.Error("No endpoints defined for service Workmate.Messaging.Exchange");
        return;
      }

      using (Server server = new Server(exchange, serviceElement.Endpoints[0].Address))
      {
        _Log.InfoFormat("Starting server at " + serviceElement.Endpoints[0].Address);
        server.Start();

        System.Console.ReadLine();
      }
    }
  }
}
