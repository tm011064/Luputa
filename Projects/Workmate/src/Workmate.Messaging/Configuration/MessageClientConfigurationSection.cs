using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ServiceModel;

namespace Workmate.Messaging.Configuration
{
  /// <summary>
  /// 
  /// </summary>
  public class MessageClientConfigurationSection : ConfigurationSection, IMessageClientConfigurationSection
  {
    /// <summary>
    /// Gets or sets the name of the net TCP binding.
    /// </summary>
    /// <value>
    /// The name of the net TCP binding.
    /// </value>
    [ConfigurationProperty("netTcpBindingName", IsRequired = true)]
    public string NetTcpBindingName
    {
      get { return ((string)(base["netTcpBindingName"])); }
      set { base["netTcpBindingName"] = value; }
    }
    /// <summary>
    /// Gets or sets the endpoint address.
    /// </summary>
    /// <value>
    /// The endpoint address.
    /// </value>
    [ConfigurationProperty("endpointAddress", IsRequired = true)]
    public string EndpointAddress
    {
      get { return ((string)(base["endpointAddress"])); }
      set { base["endpointAddress"] = value; }
    }
    /// <summary>
    /// Gets or sets a value indicating whether [auto reconnect].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [auto reconnect]; otherwise, <c>false</c>.
    /// </value>
    [ConfigurationProperty("autoReconnect", IsRequired = false, DefaultValue = "false")]
    public bool AutoReconnect
    {
      get { return ((bool)(base["autoReconnect"])); }
      set { base["autoReconnect"] = value; }
    }
    /// <summary>
    /// Gets or sets the reconnect interval.
    /// </summary>
    /// <value>
    /// The reconnect interval.
    /// </value>
    [ConfigurationProperty("reconnectInterval", IsRequired = false, DefaultValue = "00:00:15")]
    public TimeSpan ReconnectInterval
    {
      get { return ((TimeSpan)(base["reconnectInterval"])); }
      set { base["reconnectInterval"] = value; }
    }
    /// <summary>
    /// Gets or sets the ping interval.
    /// </summary>
    /// <value>
    /// The ping interval.
    /// </value>
    [ConfigurationProperty("pingInterval", IsRequired = false, DefaultValue = "00:00:05")]
    public TimeSpan PingInterval
    {
      get { return ((TimeSpan)(base["pingInterval"])); }
      set { base["pingInterval"] = value; }
    }

    /// <summary>
    /// Gets the net TCP binding.
    /// </summary>
    public NetTcpBinding NetTcpBinding
    {
      get { return new NetTcpBinding(this.NetTcpBindingName); }
    }

  }
}
