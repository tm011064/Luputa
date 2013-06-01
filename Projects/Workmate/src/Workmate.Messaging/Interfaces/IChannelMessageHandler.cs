using System.ServiceModel;

namespace Workmate.Messaging
{
  /// <summary>
  /// 
  /// </summary>
  public interface IChannelMessageHandler
  {
    /// <summary>
    /// Is called when a message is received from the exchange
    /// </summary>
    /// <param name="routingKey">The routing key.</param>
    /// <param name="message">The message.</param>
    [OperationContract(IsOneWay = true)]
    void OnMessageReceived(string routingKey, byte[] message);

    /// <summary>
    /// Is called when an admin message is received from the exchange.
    /// </summary>
    /// <param name="messageType">Type of the message.</param>
    /// <param name="message">The message.</param>
    [OperationContract(IsOneWay = true)]
    void OnAdminMessageReceived(AdminMessageType adminMessageType, string message);
  }
}
