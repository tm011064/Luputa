using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Workmate.Messaging.Configuration;

namespace Workmate.Messaging
{
  #region eventargs
  public class CommunicationStateChangedEventArgs : EventArgs
  {
    public CommunicationStatus CommunicationStatus { get; private set; }

    public CommunicationStateChangedEventArgs(CommunicationStatus communicationStatus)
    {
      this.CommunicationStatus = communicationStatus;
    }
  }

  public class MessageReceivedEventArgs : EventArgs
  {
    public string RoutingKey { get; private set; }
    public byte[] MessageBody { get; private set; }

    public MessageReceivedEventArgs(byte[] messageBody, string routingKey)
    {
      this.MessageBody = messageBody;
      this.RoutingKey = routingKey;
    }
  }
  #endregion

  /// <summary>
  /// 
  /// </summary>
  public class ClientMessageHandlerProxy : IChannelMessageHandler, IClientMessageHandlerProxy
  {
    #region members
    private ILog _Log = LogManager.GetLogger("ClientMessageHandlerProxy");

    IChannel _PipeProxy = null;
    DuplexChannelFactory<IChannel> _PipeFactory;

    private IMessageClientConfigurationSection _MessageClientConfigurationSection;

    private Timer _ReconnectTimer;
    private Timer _PingTimer;

    private string _SenderId;

    private HashSet<string> _Subscriptions = new HashSet<string>();
    private readonly object _SubscriptionsLock = new object();
    bool _Disposing = false;
    #endregion

    #region events
    public event EventHandler<CommunicationStateChangedEventArgs> CommunicationStateChanged;
    public event EventHandler<MessageReceivedEventArgs> MessageReceived;
    #endregion

    #region properties
    public CommunicationState CommunicationState { get; private set; }
    #endregion

    #region private methods
    private void InvokeChannelAction(Action<IChannel> action)
    {
      InvokeChannelAction(action, true);
    }
    private void InvokeChannelAction(Action<IChannel> action, bool checkOpenConnection)
    {
      if (checkOpenConnection && this.CommunicationState != CommunicationState.Opened)
      {
        _Log.Info("Unable to invoke action because connection is down");
        return;
      }
      try
      {
        action.Invoke(_PipeProxy);
      }
      catch (CommunicationObjectFaultedException ex)
      {
        _Log.Error("An error occurred while communicating with the server", ex);

        ChangeCommunicationState(CommunicationState.Closed);
        AttemptReconnect();
      }
      catch (EndpointNotFoundException ex)
      {
        _Log.Error("An error occurred while communicating with the server", ex);

        ChangeCommunicationState(CommunicationState.Closed);
        AttemptReconnect();
      }
      catch (Exception ex)
      {
        _Log.Error("An error occurred while communicating with the server", ex);
      }
    }
    private void ChangeCommunicationState(CommunicationState communicationState)
    {
      this.CommunicationState = communicationState;
      if (this.CommunicationStateChanged != null)
        this.CommunicationStateChanged(this, new CommunicationStateChangedEventArgs((CommunicationStatus)communicationState));

      _Log.Info("Communication state changed to " + communicationState.ToString());
      if (communicationState == CommunicationState.Opened)
      {
        Resubscribe();
        _PingTimer = new Timer(new TimerCallback(Ping), null, this._MessageClientConfigurationSection.PingInterval, this._MessageClientConfigurationSection.PingInterval);
      }
    }

    private void AttemptReconnect()
    {
      if (!_Disposing
          && this._MessageClientConfigurationSection.AutoReconnect
          && _ReconnectTimer == null)
      {
        _Log.Info("Starting reconnect timer, next attempt in " + this._MessageClientConfigurationSection.ReconnectInterval.TotalMilliseconds + " ms");
        _ReconnectTimer = new Timer(new TimerCallback(Connect), null, this._MessageClientConfigurationSection.ReconnectInterval, this._MessageClientConfigurationSection.ReconnectInterval);
      }
    }

    private void Ping(object state)
    {
      _PingTimer.Change(Timeout.Infinite, Timeout.Infinite);
      if (this.CommunicationState != CommunicationState.Opened)
        return;

      InvokeChannelAction(p => p.Ping());
      _PingTimer.Change(this._MessageClientConfigurationSection.PingInterval, this._MessageClientConfigurationSection.PingInterval);
    }
    private void Connect(object state)
    {
      if (_ReconnectTimer != null)
      {
        _ReconnectTimer.Dispose();
        _ReconnectTimer = null;
      }

      if (this.CommunicationState == CommunicationState.Opened)
        return;

      _PipeFactory = new DuplexChannelFactory<IChannel>(new InstanceContext(this), this._MessageClientConfigurationSection.NetTcpBinding, this._MessageClientConfigurationSection.EndpointAddress);

      _PipeProxy = _PipeFactory.CreateChannel();

      ChangeCommunicationState(CommunicationState.Opening);
      InvokeChannelAction(p => p.Connect(), false);
    }
    #endregion

    #region public methods
    /// <summary>
    /// Connects this instance.
    /// </summary>
    public void Connect()
    {
      Task.Factory.StartNew(() =>
      {
        Connect(null);
      });
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      if (!_Disposing)
      {
        _Disposing = true;

        ChangeCommunicationState(CommunicationState.Closing);
        InvokeChannelAction(p => p.Disconnect(), false);
        ChangeCommunicationState(CommunicationState.Closed);

        if (this._ReconnectTimer != null)
          this._ReconnectTimer.Dispose();

        if (this._PingTimer != null)
          this._PingTimer.Dispose();        
      }
    }

    /// <summary>
    /// Resubscribes this client to all routing keys.
    /// </summary>
    public void Resubscribe()
    {
      lock (_SubscriptionsLock)
      {
        _Log.Info("Start resubscribing routing keys");
        foreach (string subscription in _Subscriptions)
        {
          Task.Factory.StartNew(() =>
          {
            _Log.Info("Subscribe to routing key " + subscription);
            InvokeChannelAction(p => p.Subscribe(subscription));
          });
        }
      }
    }

    /// <summary>
    /// Subscribes this client to the specified routing key.
    /// </summary>
    /// <param name="routingKey">Name of the exchange.</param>
    public void Subscribe(string routingKey)
    {
      lock (_SubscriptionsLock)
      {
        _Subscriptions.Add(routingKey);
      }

      Task.Factory.StartNew(() =>
      {
        _Log.Info("Subscribe to routing key " + routingKey);
        InvokeChannelAction(p => p.Subscribe(routingKey));
      });
    }
    /// <summary>
    /// Unsubscribes this client from the specified routing key.
    /// </summary>
    /// <param name="routingKey">The routing key.</param>
    public void Unsubscribe(string routingKey)
    {
      lock (_SubscriptionsLock)
      {
        _Subscriptions.Remove(routingKey);
      }

      Task.Factory.StartNew(() =>
      {
        _Log.Info("Unsubscribe from routing key " + routingKey);
        InvokeChannelAction(p => p.Unsubscribe(routingKey));
      });
    }

    /// <summary>
    /// Publishes a message from this client using the specified routing key.
    /// </summary>
    /// <param name="routingKey">The routing key (can be null to publish to all clients).</param>
    /// <param name="body">The body.</param>
    public void Publish(string routingKey, byte[] body)
    {
      Task.Factory.StartNew(() =>
      {
        InvokeChannelAction(p => p.Publish(routingKey, body));
      });
    }
    /// <summary>
    /// Publishes the specified body.
    /// </summary>
    /// <param name="body">The body.</param>
    public void Publish(byte[] body)
    {
      Task.Factory.StartNew(() =>
      {
        InvokeChannelAction(p => p.Publish(null, body));
      });
    }

    /// <summary>
    /// Publishes a message.
    /// </summary>
    /// <typeparam name="TMessageType">The type of the message type.</typeparam>
    /// <param name="messageType">Type of the message.</param>
    /// <param name="body">The body.</param>
    public void Publish<TMessageType>(TMessageType messageType, byte[] body)
    {
      Publish<TMessageType>(messageType, null, body);
    }
    /// <summary>
    /// Publishes the message.
    /// </summary>
    /// <typeparam name="TMessageType">The type of the message type.</typeparam>
    /// <param name="messageType">Type of the message.</param>
    /// <param name="routingKey">The routing key.</param>
    /// <param name="body">The body.</param>
    public void Publish<TMessageType>(TMessageType messageType, string routingKey, byte[] body)
    {
      Task.Factory.StartNew(() =>
      {
        InvokeChannelAction(p => p.Publish(routingKey, CreateSerializedMessage<TMessageType>(messageType, body)));
      });
    }
    #endregion

    #region static methods
    /// <summary>
    /// Creates a serialized message with a specified message type and body.
    /// </summary>
    /// <typeparam name="TMessageType">The type of the message type.</typeparam>
    /// <param name="messageType">Type of the message.</param>
    /// <param name="body">The body.</param>
    /// <returns></returns>
    public byte[] CreateSerializedMessage<TMessageType>(TMessageType messageType, byte[] body)
    {
      Message<TMessageType> message = new Message<TMessageType>(this._SenderId, DateTime.UtcNow, messageType, body);
      return SerializationUtility.Serialize<Message<TMessageType>>(message);
    }
    /// <summary>
    /// Deserializes the message.
    /// </summary>
    /// <typeparam name="TMessageType">The type of the message type.</typeparam>
    /// <param name="body">The body.</param>
    /// <returns></returns>
    public Message<TMessageType> DeserializeMessage<TMessageType>(byte[] body)
    {
      return SerializationUtility.Deserialize<Message<TMessageType>>(body);
    }
    #endregion

    #region IChannelMessageHandler Members

    /// <summary>
    /// Is called when a message is received from the exchange
    /// </summary>
    /// <param name="routingKey">The routing key.</param>
    /// <param name="message">The message.</param>
    public void OnMessageReceived(string routingKey, byte[] message)
    {
      if (this.MessageReceived != null)
        this.MessageReceived(this, new MessageReceivedEventArgs(message, routingKey));
    }

    /// <summary>
    /// Is called when an admin message is received from the exchange.
    /// </summary>
    /// <param name="messageType">Type of the message.</param>
    /// <param name="message">The message.</param>
    public void OnAdminMessageReceived(AdminMessageType adminMessageType, string message)
    {
      switch (adminMessageType)
      {
        case AdminMessageType.ConnectionEstablished:
          ChangeCommunicationState(CommunicationState.Opened);
          break;

        case AdminMessageType.Disconnected:
          ChangeCommunicationState(CommunicationState.Closed);
          AttemptReconnect();
          break;

        case AdminMessageType.Ping:
          _Log.Debug("Ping received");
          break;
      }
    }

    #endregion

    #region constructors
    public ClientMessageHandlerProxy(IMessageClientConfigurationSection messageClientConfigurationSection)
    {
      this._MessageClientConfigurationSection = messageClientConfigurationSection;

      this._SenderId = Environment.MachineName + ":" + Process.GetCurrentProcess().Id.ToString();

      this.CommunicationState = CommunicationState.Created;
    }
    #endregion
  }
}
