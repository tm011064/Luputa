using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.TestSuite.Messaging.Stubs;
using System.Diagnostics;

namespace CommonTools.TestSuite.Messaging
{
    public static class BasicTestMessageHandlerSetup
    {
        public static void RunServerTest()
        {
            BasicTestMessageHandler server = new BasicTestMessageHandler(
                "localhost"
                , "CommonTools.Testing.Server"
                , "direct"
                , "CommonTools.Testing.Client"
                , "direct");

            server.Connect(false);
            Console.ReadLine();

            server.SendMessage("Hello World");
            
            Console.WriteLine("press enter to end test and dispose the server/client objects...");
            Console.ReadLine();

            server.SendMessage("Hello World");

            server.Dispose();
        }


        public static void RunTest()
        {
            BasicTestMessageHandler server = new BasicTestMessageHandler(
                "localhost"
                , "CommonTools.Testing.Server"
                , "direct"
                , "CommonTools.Testing.Client"
                , "direct");
            BasicTestMessageHandler client = new BasicTestMessageHandler(
                "localhost"
                , "CommonTools.Testing.Client"
                , "direct"
                , "CommonTools.Testing.Server"
                , "direct");

            server.Connect(false);
            client.Connect(false);

            server.SendMessage("Hello World");
            
            client.Dispose();
            server.Dispose();
        }


        public static void RunRestartTest()
        {
            BasicTestMessageHandler server = new BasicTestMessageHandler(
                "localhost"
                , "CommonTools.Testing.Server"
                , "direct"
                , "CommonTools.Testing.Client"
                , "direct");
            BasicTestMessageHandler client = new BasicTestMessageHandler(
                "localhost"
                , "CommonTools.Testing.Client"
                , "direct"
                , "CommonTools.Testing.Server"
                , "direct");

            server.MessageReceived += new EventHandler<TestMessageEventArgs>(server_MessageReceived);
            server.ConnectionStatusChanged += new EventHandler<CommonTools.Messaging.ConnectionStatusChangedEventArgs>(server_ConnectionStatusChanged);

            client.MessageReceived += new EventHandler<TestMessageEventArgs>(client_MessageReceived);
            client.ConnectionStatusChanged += new EventHandler<CommonTools.Messaging.ConnectionStatusChangedEventArgs>(client_ConnectionStatusChanged);

            server.Connect(false);
            client.Connect(false);

            Console.WriteLine("Sending test message...");
            server.SendMessage("Hello World");

            Console.WriteLine("Stop the rabbit server and press enter...");
            Console.ReadLine();

            Console.WriteLine("wait a bit and then restart the rabbit server...");
            Console.ReadLine();


            Console.WriteLine("check whether the server/clients have restarted");
            Console.ReadLine();

            Console.WriteLine("Sending test message...");
            server.SendMessage("Hello World");

            Console.WriteLine("press enter to end test and dispose the server/client objects...");
            Console.ReadLine();

            client.Dispose();
            server.Dispose();
        }

        static void client_ConnectionStatusChanged(object sender, CommonTools.Messaging.ConnectionStatusChangedEventArgs e)
        {
            Console.WriteLine("Server status changed to " + e.Status.ToString());
        }

        static void server_ConnectionStatusChanged(object sender, CommonTools.Messaging.ConnectionStatusChangedEventArgs e)
        {
            Console.WriteLine("Client status changed to " + e.Status.ToString());
        }

        static void client_MessageReceived(object sender, TestMessageEventArgs e)
        {
            Console.WriteLine(Environment.NewLine + "Client message received: " + Environment.NewLine + e.ToString());
        }

        static void server_MessageReceived(object sender, TestMessageEventArgs e)
        {
            Console.WriteLine(Environment.NewLine + "Server message received: " + Environment.NewLine + e.ToString());
        }
    }
}
