using System;
using System.ServiceModel;

namespace Workmate.Messaging.Configuration
{
  /// <summary>
  /// 
  /// </summary>
  public interface IMessageClientConfigurationSection
  {
    /// <summary>
    /// Gets a value indicating whether [auto reconnect].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [auto reconnect]; otherwise, <c>false</c>.
    /// </value>
    bool AutoReconnect { get; }
    /// <summary>
    /// Gets the endpoint address.
    /// </summary>
    string EndpointAddress { get; }
    /// <summary>
    /// Gets the net TCP binding.
    /// </summary>
    NetTcpBinding NetTcpBinding { get; }
    /// <summary>
    /// Gets the ping interval.
    /// </summary>
    TimeSpan PingInterval { get; }
    /// <summary>
    /// Gets the reconnect interval.
    /// </summary>
    TimeSpan ReconnectInterval { get; }
  }
}
