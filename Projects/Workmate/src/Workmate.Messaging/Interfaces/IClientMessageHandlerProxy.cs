using System.ServiceModel;
using System;

namespace Workmate.Messaging
{
  public interface IClientMessageHandlerProxy : IDisposable
  {
    event EventHandler<MessageReceivedEventArgs> MessageReceived;
    event EventHandler<CommunicationStateChangedEventArgs> CommunicationStateChanged;

    void Connect();

    void Subscribe(string routingKey);
    void Unsubscribe(string routingKey);

    void Publish<TMessageType>(TMessageType messageType, string routingKey, byte[] body);

    byte[] CreateSerializedMessage<TMessageType>(TMessageType messageType, byte[] body);
    Message<TMessageType> DeserializeMessage<TMessageType>(byte[] body);
  }
}
