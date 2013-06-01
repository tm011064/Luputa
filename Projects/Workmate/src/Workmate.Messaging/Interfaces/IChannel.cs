using System.ServiceModel;

namespace Workmate.Messaging
{
  /// <summary>
  /// 
  /// </summary>
  [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IChannelMessageHandler))]
  public interface IChannel
  {
    /// <summary>
    /// Connects a client to an exchange.
    /// </summary>
    [OperationContract(IsOneWay = true)]
    void Connect();

    /// <summary>
    /// Disconnects a client from an exchange
    /// </summary>
    [OperationContract(IsOneWay = true)]
    void Disconnect();

    /// <summary>
    /// Pings the exchange
    /// </summary>
    [OperationContract(IsOneWay = true)]
    void Ping();

    /// <summary>
    /// Subscribes a client to a specified routing key. Messages for this routing key will only be received once the client has subscribed
    /// </summary>
    /// <param name="routingKey">The routing key.</param>
    [OperationContract(IsOneWay = true)]
    void Subscribe(string routingKey);

    /// <summary>
    /// Unsubscribes a client from a specified routing key.
    /// </summary>
    /// <param name="routingKey">The routing key.</param>
    [OperationContract]
    void Unsubscribe(string routingKey);

    /// <summary>
    /// Publishes a message via the exchange.
    /// </summary>
    /// <param name="routingKey">The routing key. Can be null if you want to publish to all clients</param>
    /// <param name="body">The body.</param>
    [OperationContract(IsOneWay = true)]
    void Publish(string routingKey, byte[] body);
  }
}
