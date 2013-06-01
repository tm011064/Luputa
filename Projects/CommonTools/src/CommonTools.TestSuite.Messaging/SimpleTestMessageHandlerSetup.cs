using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.TestSuite.Messaging.Stubs;
using CommonTools.Messaging;
using CommonTools.Messaging.Handlers;
using System.Threading;
using System.Diagnostics;

namespace CommonTools.TestSuite.Messaging
{
    public static class SimpleTestMessageHandlerSetup
    {
        static int _Counter;

        static void server_MessageReceived(object sender, MessageEventArgs<ClientRequestType> e)
        {
            switch (e.MessageType)
            {
                case ClientRequestType.SomeObject:
                    SimpleObject obj = e.GetEmbeddedObject<SimpleObject>();
                    break;

                case ClientRequestType.Plain:
                    Console.WriteLine("Client request received");
                    break;
            }
        }

        static void client_MessageReceived(object sender, MessageEventArgs<ServerMessageType> e)
        {
            switch (e.MessageType)
            {
                case ServerMessageType.SomeObject:
                    SimpleObject obj = e.GetEmbeddedObject<SimpleObject>();
                    break;

                case ServerMessageType.Plain:
                    Interlocked.Increment(ref  _Counter);
                    Console.WriteLine("Server message received");
                    break;
            }
        }

        public static void RunSimpleTwoWayMessageHandlerTest()
        {
            SimpleTwoWayMessageHandler<ClientRequestType, ServerMessageType> server = new SimpleTwoWayMessageHandler<ClientRequestType, ServerMessageType>(
                "localhost"
                , "CommonTools.Testing.Server"
                , "direct"
                , "CommonTools.Testing.Client"
                , "direct"
                , new ConsoleLogger());

            SimpleTwoWayMessageHandler<ServerMessageType, ClientRequestType> client = new SimpleTwoWayMessageHandler<ServerMessageType, ClientRequestType>(
                "localhost"
                , "CommonTools.Testing.Client"
                , "direct"
                , "CommonTools.Testing.Server"
                , "direct"
                , new ConsoleLogger());

            server.MessageReceived += new EventHandler<MessageEventArgs<ClientRequestType>>(server_MessageReceived);
            client.MessageReceived += new EventHandler<MessageEventArgs<ServerMessageType>>(client_MessageReceived);

            server.Connect(false);
            client.Connect(false);

            client.SendMessage(ClientRequestType.SomeObject, new SimpleObject(99));
            client.SendMessage(ClientRequestType.Plain);

            server.SendMessage(ServerMessageType.Plain);

            Console.ReadLine();

            client.Dispose();
            server.Dispose();

            Console.ReadLine();
        }


        public static void RunSimplePublisherReceiverTest()
        {
            SimpleMessagePublisher<ServerMessageType> server = new SimpleMessagePublisher<ServerMessageType>(
                "localhost"
                , "CommonTools.Testing.Publisher"
                , "direct"
                , false
                , new ConsoleLogger());

            SimpleMessageReceiver<ServerMessageType> client = new SimpleMessageReceiver<ServerMessageType>(
                "localhost"
                , "CommonTools.Testing.Publisher"
                , "direct"
                , false
                , new ConsoleLogger());

            server.Open(false);
            client.Open(false);

            client.MessageReceived += new EventHandler<MessageEventArgs<ServerMessageType>>(client_MessageReceived);

            server.SendMessage(ServerMessageType.Plain);
            server.SendMessage(ServerMessageType.SomeObject, new SimpleObject(99));

            Console.ReadLine();

            client.Dispose();
            server.Dispose();
        }

        public static void RunSimplePublisherReceiverShutdownTest()
        {
            SimpleMessagePublisher<ServerMessageType> server = new SimpleMessagePublisher<ServerMessageType>(
                "localhost"
                , "CommonTools.Testing.Publisher"
                , "direct"
                , false
                , new ConsoleLogger());

            SimpleMessageReceiver<ServerMessageType> client = new SimpleMessageReceiver<ServerMessageType>(
                "localhost"
                , "CommonTools.Testing.Publisher"
                , "direct"
                , false
                , new ConsoleLogger());

            server.Open(false);
            client.Open(false);

            client.MessageReceived += new EventHandler<MessageEventArgs<ServerMessageType>>(client_MessageReceived);

            server.SendMessage(ServerMessageType.Plain);
            server.SendMessage(ServerMessageType.SomeObject, new SimpleObject(99));

            Console.WriteLine("Connection established and test messages sent, waiting for manual RabbitMQ service shutdown");
            Console.ReadLine();

            Console.WriteLine("Trying to send message...");
            server.SendMessage(ServerMessageType.Plain);

            Console.WriteLine("Manually restart and wait...");
            Console.ReadLine();

            server.SendMessage(ServerMessageType.Plain);
            server.SendMessage(ServerMessageType.SomeObject, new SimpleObject(99));

            client.Dispose();
            server.Dispose();
        }




        static void topicTest_client_MessageReceived(object sender, MessageEventArgs<ServerMessageType> e)
        {
            switch (e.MessageType)
            {
                case ServerMessageType.SomeObject:
                    SimpleObject obj = e.GetEmbeddedObject<SimpleObject>();
                    Console.WriteLine("Message received, value -> " + obj.Value);
                    break;
            }
        }
        public static void RunSimplePublisherReceiverTopicTest()
        {
            string routingKey = "TestChannel";
            string unknownRoutingKey = "UnknownChannel";

            SimpleMessagePublisher<ServerMessageType> server = new SimpleMessagePublisher<ServerMessageType>(
                "localhost"
                , "CommonTools.Testing.Publisher.Topic"
                , "topic"
                , false
                , new ConsoleLogger());

            SimpleMessageReceiver<ServerMessageType> client = new SimpleMessageReceiver<ServerMessageType>(
                "localhost"
                , "CommonTools.Testing.Publisher.Topic"
                , "topic"
                , false
                , true
                , new ConsoleLogger());

            server.Open(false);
            client.Open(false);

            client.MessageReceived += new EventHandler<MessageEventArgs<ServerMessageType>>(topicTest_client_MessageReceived);

            server.SendRoutedMessage(ServerMessageType.SomeObject, new SimpleObject(99), routingKey);
     
            // we expect to get the message with value 99
            Console.ReadLine();

            client.Dispose();
            client = new SimpleMessageReceiver<ServerMessageType>(
                "localhost"
                , "CommonTools.Testing.Publisher.Topic"
                , "topic"
                , false
                , false
                , new ConsoleLogger());
            client.Open(false);

            client.MessageReceived += new EventHandler<MessageEventArgs<ServerMessageType>>(topicTest_client_MessageReceived);

            client.BindRoutingKey(routingKey);

            Console.WriteLine("Expecting to get value 99...");
            server.SendRoutedMessage(ServerMessageType.SomeObject, new SimpleObject(99), routingKey);
            server.SendRoutedMessage(ServerMessageType.SomeObject, new SimpleObject(55), unknownRoutingKey);

            Console.ReadLine();

            client.UnbindRoutingKey(routingKey);
            client.BindRoutingKey(unknownRoutingKey);

            Console.WriteLine("Expecting to get value 55...");
            server.SendRoutedMessage(ServerMessageType.SomeObject, new SimpleObject(99), routingKey);
            server.SendRoutedMessage(ServerMessageType.SomeObject, new SimpleObject(55), unknownRoutingKey);

            Console.ReadLine();

            Console.WriteLine("Expecting no error...");
            client.UnbindRoutingKey(unknownRoutingKey);
            client.UnbindRoutingKey(unknownRoutingKey);
            client.BindRoutingKey(unknownRoutingKey);
            client.BindRoutingKey(unknownRoutingKey);

            Console.WriteLine("Expecting to get value 55...");
            server.SendRoutedMessage(ServerMessageType.SomeObject, new SimpleObject(99), routingKey);
            server.SendRoutedMessage(ServerMessageType.SomeObject, new SimpleObject(55), unknownRoutingKey);

            Console.WriteLine("Connection established and test messages sent, waiting for manual RabbitMQ service shutdown");
            Console.ReadLine();

            Console.WriteLine("Manually restart and wait...");
            Console.ReadLine();

            Console.WriteLine("Expecting to get value 55...");
            server.SendRoutedMessage(ServerMessageType.SomeObject, new SimpleObject(99), routingKey);
            server.SendRoutedMessage(ServerMessageType.SomeObject, new SimpleObject(55), unknownRoutingKey);
            Console.ReadLine();

            client.Dispose();
            server.Dispose();
        }
    }
}
