using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Messaging;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Net.Security;
using log4net.Config;
using log4net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
using System.Configuration;
using Workmate.Messaging.Configuration;

namespace Workmate.Messaging.Tests.MessageClient
{
  class Program
  {
    private static ILog _Log = LogManager.GetLogger("ClientMessageHandlerProxy");

    enum TestMessageType
    {
      Normal,
      Bulk,
      StopBulk
    }

    static bool _KeepBulkSending = false;
    static AutoResetEvent _BulkSendFinished = new AutoResetEvent(false);

    static void Main(string[] args)
    {
      XmlConfigurator.Configure();

      IMessageClientConfigurationSection section = (IMessageClientConfigurationSection)ConfigurationManager.GetSection("clientSettings");

      using (ClientMessageHandlerProxy clientMessageHandlerProxy = new ClientMessageHandlerProxy(section))
      {
        clientMessageHandlerProxy.MessageReceived += new EventHandler<MessageReceivedEventArgs>(clientMessageHandlerProxy_MessageReceived);

        clientMessageHandlerProxy.Connect();

        Console.WriteLine("Possible Keys:");
        Console.WriteLine("\nsrk RoutingKey".PadRight(78) + " - subscribe to routing key".PadRight(40) + "\n   -> srk Orders");
        Console.WriteLine("\nurk RoutingKey".PadRight(78) + " - unsubscribe to routing key".PadRight(40) + "\n   -> urk Orders");
        Console.WriteLine("\ns rk=RoutingKey m=\"Message\"".PadRight(78) + " - send message to routing key".PadRight(40) + "\n   -> s rk=Orders m=\"Hellow World\" OR s m=\"Hello World\"");
        Console.WriteLine("\nbs rk=RoutingKey m=\"Message\" s=TotalMessagesPerBulk t=SleepBetweenBulks".PadRight(78) + " - push messages continously to routing key".PadRight(40) + "\n   -> bs rk=Orders m=\"Hellow World\" s=100 t=10");
        Console.WriteLine("\nsbs".PadRight(78) + " - stop pushing messages");
        Console.WriteLine("\nexit".PadRight(78) + " - stop client");
        Console.WriteLine("\n\n");

        bool running = true;
        Regex routingKeyRegex = new Regex(@" rk=([a-zA-Z]*)");
        Regex messageRegex = new Regex(@" m=""([a-zA-Z ]*)""");
        Regex bulkSizeRegex = new Regex(@" s=([0-9]*)");
        Regex sleepThrottleRegex = new Regex(@" t=([0-9]*)");

        string cmd, routingKey, message, str;
        Match match;
        int bulkSize = 10;
        int sleepThrottle = 10;

        while (running)
        {
          string line = Console.ReadLine();

          if (line.Contains(" "))
            cmd = line.Substring(0, line.IndexOf(' ')).Trim();
          else
            cmd = line;

          routingKey = null;

          match = routingKeyRegex.Match(line);
          if (match.Success)
            routingKey = match.Groups[1].Value;

          message = string.Empty;
          match = messageRegex.Match(line);
          if (match.Success)
            message = match.Groups[1].Value;

          bulkSize = 10;
          match = bulkSizeRegex.Match(line);
          if (match.Success)
            bulkSize = int.Parse(match.Groups[1].Value);

          sleepThrottle = 10;
          match = sleepThrottleRegex.Match(line);
          if (match.Success)
            sleepThrottle = int.Parse(match.Groups[1].Value);

          switch (cmd)
          {
            case "srk":
              str = line.Substring(line.IndexOf(' ')).Trim();
              if (!string.IsNullOrEmpty(str))
              {
                clientMessageHandlerProxy.Subscribe(str);
              }
              break;
            case "urk":
              str = line.Substring(line.IndexOf(' ')).Trim();
              if (!string.IsNullOrEmpty(str))
              {
                clientMessageHandlerProxy.Unsubscribe(str);
              }
              break;

            case "s": clientMessageHandlerProxy.Publish<TestMessageType>(TestMessageType.Normal, routingKey, ASCIIEncoding.Default.GetBytes(message)); break;

            case "bs":

              if (_KeepBulkSending)
              {
                _KeepBulkSending = false;

                Console.WriteLine("Waiting for bulk push to stop...");
                _BulkSendFinished.WaitOne();
                Console.WriteLine("Bulk push stopped");
              }

              _KeepBulkSending = true;
              ThreadPool.QueueUserWorkItem(delegate(object state)
              {
                Console.WriteLine("Start pushing messages");
                while (_KeepBulkSending)
                {
                  for (int i = 0; i < bulkSize; i++)
                    clientMessageHandlerProxy.Publish<TestMessageType>(TestMessageType.Bulk, routingKey, ASCIIEncoding.Default.GetBytes(message));

                  Thread.Sleep(sleepThrottle);
                }
                _BulkSendFinished.Set();
              });

              break;

            case "sbs":
              if (_KeepBulkSending)
              {
                _KeepBulkSending = false;
                Console.WriteLine("Waiting for bulk push to stop...");
                _BulkSendFinished.WaitOne();

                clientMessageHandlerProxy.Publish<TestMessageType>(TestMessageType.StopBulk, routingKey, null);

                Console.WriteLine("Bulk push stopped");
              }
              break;

            case "exit": running = false; break;
          }
        }
      }
    }

    static int _BulkMessageCounter = 0;
    static long _BulkMessageLatency = 0;
    static Stopwatch _BulkStopwatch = new Stopwatch();
    static readonly object _BulkLock = new object();

    static void clientMessageHandlerProxy_MessageReceived(object sender, MessageReceivedEventArgs e)
    {
      ClientMessageHandlerProxy clientMessageHandlerProxy = sender as ClientMessageHandlerProxy;

      Message<TestMessageType> message = clientMessageHandlerProxy.DeserializeMessage<TestMessageType>(e.MessageBody);

      switch (message.MessageType)
      {
        case TestMessageType.Normal:
          _Log.Info("Received message from routing key '" + e.RoutingKey + "': " + message.SenderId + ", " + message.SentUtc + ", "
            + message.MessageType.ToString() + ", " + ASCIIEncoding.Default.GetString(message.Body));
          break;

        case TestMessageType.Bulk:

          if (!_BulkStopwatch.IsRunning)
          {
            lock (_BulkLock)
            {
              if (!_BulkStopwatch.IsRunning)
              {
                Interlocked.Exchange(ref _BulkMessageCounter, 0);
                _BulkStopwatch.Start();
              }
            }
          }

          Interlocked.Increment(ref _BulkMessageCounter);
          Interlocked.Exchange(ref _BulkMessageLatency, _BulkMessageLatency + (long)(DateTime.UtcNow - message.SentUtc).TotalMilliseconds );

          if (_BulkStopwatch.ElapsedMilliseconds >= 10000)
          {
            lock (_BulkLock)
            {
              _BulkStopwatch.Stop();
              _Log.InfoFormat("Bulk received {0} messages per second, avg latency is {1} ms"
                , (((double)_BulkMessageCounter / (double)_BulkStopwatch.ElapsedMilliseconds) * 1000).ToString("N2")
                , ((double)_BulkMessageLatency / (double)_BulkMessageCounter).ToString("N2"));

              Interlocked.Exchange(ref _BulkMessageCounter, 0);
              _BulkStopwatch.Reset();
              _BulkStopwatch.Start();
            }
          }

          break;

        case TestMessageType.StopBulk:
          _BulkStopwatch.Stop();
          _BulkStopwatch.Reset();
          Interlocked.Exchange(ref _BulkMessageCounter, 0);

          break;
      }
    }
  }
}
