using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using log4net.Config;

namespace Workmate.Messaging.Tests.Server
{
  class Program
  {
    static void Main(string[] args)
    {
      XmlConfigurator.Configure();

      Uri baseAddress = new Uri("net.tcp://localhost/Workmate/Messaging");
      Exchange exchange = new Exchange();
      ExchangeMessageMonitor exchangeMessageMonitor = new ExchangeMessageMonitor(TimeSpan.FromSeconds(15), exchange);
      exchangeMessageMonitor.StartSnapshotLogging();

      using (ServiceHost host = new ServiceHost(exchange, baseAddress))
      {
        host.Open();

        Console.ReadLine();

        host.Close();
      }
    }
  }
}
