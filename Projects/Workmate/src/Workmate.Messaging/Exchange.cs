using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using log4net;

namespace Workmate.Messaging
{
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
  public class Exchange : IChannel
  {
    #region nested classes
    class RoutedMessageHandler
    {
      #region members
      private HashSet<IChannelMessageHandler> _MessageHandlers = new HashSet<IChannelMessageHandler>();
      #endregion

      #region properties
      public string RoutingKey { get; set; }
      #endregion

      #region methods
      public List<IChannelMessageHandler> GetMessageHandlersCopy()
      {
        return new List<IChannelMessageHandler>(_MessageHandlers);
      }

      public void RemoveChannelMessageHandler(IChannelMessageHandler channelMessageHandler)
      {
        _MessageHandlers.Remove(channelMessageHandler);
      }

      public void AddChannelMessageHandler(IChannelMessageHandler channelMessageHandler)
      {
        _MessageHandlers.Add(channelMessageHandler);
      }
      #endregion

      #region constructors
      public RoutedMessageHandler(string routingKey)
      {
        this.RoutingKey = routingKey;
      }
      #endregion
    }
    #endregion

    #region members
    private ILog _Log = LogManager.GetLogger("Exchange");
    private readonly object _AccessLock = new object();

    HashSet<IChannelMessageHandler> _ConnectedEndpoints = new HashSet<IChannelMessageHandler>();
    Dictionary<string, RoutedMessageHandler> _RoutedMessageHandlers = new Dictionary<string, RoutedMessageHandler>();
    
    private long _TotalMessagesSent = 0;
    #endregion

    #region IChannel Members

    /// <summary>
    /// Connects a client to an exchange.
    /// </summary>
    public void Connect()
    {
      IChannelMessageHandler channelMessageHandler = OperationContext.Current.GetCallbackChannel<IChannelMessageHandler>();
      lock (_AccessLock)
      {        
        if (!_ConnectedEndpoints.Contains(channelMessageHandler))
        {
          ((IClientChannel)channelMessageHandler).Faulted += new EventHandler(Exchange_Closed);
          ((IClientChannel)channelMessageHandler).Closed += new EventHandler(Exchange_Closed);
          
          _ConnectedEndpoints.Add(channelMessageHandler);

          _Log.Info("Successfully connected to client " + ((IClientChannel)channelMessageHandler).LocalAddress.Uri.ToString());
        }

        InvokeChannelMessageHandlerAction(p => p.OnAdminMessageReceived(AdminMessageType.ConnectionEstablished, null), channelMessageHandler);
      }
    }

    /// <summary>
    /// Disconnects a client from an exchange
    /// </summary>
    public void Disconnect()
    {
      Disconnect(OperationContext.Current.GetCallbackChannel<IChannelMessageHandler>());
    }

    public void Unsubscribe(string routingKey)
    {
      lock (_AccessLock)
      {
        if (_RoutedMessageHandlers.ContainsKey(routingKey))
        {
          IChannelMessageHandler channelMessageHandler = OperationContext.Current.GetCallbackChannel<IChannelMessageHandler>();
          _Log.Info("Successfully removed client " + ((IClientChannel)channelMessageHandler).LocalAddress.Uri.ToString() + " from routing key " + routingKey);
          _RoutedMessageHandlers[routingKey].RemoveChannelMessageHandler(channelMessageHandler);
        }
      }
    }

    /// <summary>
    /// Subscribes a client to a specified routing key. Messages for this routing key will only be received once the client has subscribed
    /// </summary>
    /// <param name="routingKey">The routing key.</param>
    public void Subscribe(string routingKey)
    {
      lock (_AccessLock)
      {
        if (!_RoutedMessageHandlers.ContainsKey(routingKey))
          _RoutedMessageHandlers[routingKey] = new RoutedMessageHandler(routingKey);

        if (_RoutedMessageHandlers.ContainsKey(routingKey))
        {
          IChannelMessageHandler channelMessageHandler = OperationContext.Current.GetCallbackChannel<IChannelMessageHandler>();

          _Log.Info("Successfully subscribed client " + ((IClientChannel)channelMessageHandler).LocalAddress.Uri.ToString() + " to routing key " + routingKey);
          _RoutedMessageHandlers[routingKey].AddChannelMessageHandler(channelMessageHandler);
        }
      }
    }

    /// <summary>
    /// Publishes the specified body.
    /// </summary>
    /// <param name="body">The body.</param>
    public void Publish(byte[] body)
    {
      Publish(null, body);
    }
    public void Publish(string routingKey, byte[] body)
    {
      List<IChannelMessageHandler> messageHandlers;

      if (string.IsNullOrEmpty(routingKey))
      {
        lock (_AccessLock)
        {
          messageHandlers = new List<IChannelMessageHandler>(_ConnectedEndpoints);
        }
      }
      else
      {
        lock (_AccessLock)
        {
          if (!_RoutedMessageHandlers.ContainsKey(routingKey))
            return;

          messageHandlers = _RoutedMessageHandlers[routingKey].GetMessageHandlersCopy();
        }
      }

      int count = messageHandlers.Count;
      foreach (IChannelMessageHandler channelMessageHandler in messageHandlers)
      {
        try
        {
          if (_ConnectedEndpoints.Contains(channelMessageHandler))
          {
            channelMessageHandler.OnMessageReceived(routingKey, body);
            Interlocked.Increment(ref _TotalMessagesSent);
          }
        }
        catch (CommunicationException ex)
        {
          _Log.Error("An error occurred while communicating with the client", ex);
          Disconnect(channelMessageHandler);
        }
        catch (TimeoutException ex)
        {
          _Log.Error("An error occurred while communicating with the server", ex);
          Disconnect(channelMessageHandler);
        }
        catch (Exception ex)
        {
          _Log.Error("An error occurred while communicating with the server", ex);
          Disconnect(channelMessageHandler);
        }
      }
    }

    /// <summary>
    /// Pings the exchange
    /// </summary>
    public void Ping()
    {
      if (_Log.IsDebugEnabled)
      {
        _Log.Debug("Ping received from " + ((IClientChannel)OperationContext.Current.GetCallbackChannel<IChannelMessageHandler>()).LocalAddress.Uri.ToString());
      }
    }

    #endregion

    #region private methods
    private void Disconnect(IChannelMessageHandler channelMessageHandler)
    {
      if (channelMessageHandler != null)
      {
        lock (_AccessLock)
        {
          if (!_ConnectedEndpoints.Contains(channelMessageHandler))
            return;

          _Log.Info("Disconnecting client " + ((IClientChannel)channelMessageHandler).LocalAddress.Uri.ToString());

          foreach (RoutedMessageHandler RoutedMessageHandler in _RoutedMessageHandlers.Values)
            RoutedMessageHandler.RemoveChannelMessageHandler(channelMessageHandler);

          _ConnectedEndpoints.Remove(channelMessageHandler);

          _Log.Info("Client " + ((IClientChannel)channelMessageHandler).LocalAddress.Uri.ToString() + " disconnected");
        }
      }
    }

    private void Exchange_Closed(object sender, EventArgs e)
    {
      Disconnect(sender as IChannelMessageHandler);
    }

    private void InvokeChannelMessageHandlerAction(Action<IChannelMessageHandler> action, IChannelMessageHandler channelMessageHandler)
    {
      try
      {
        action.Invoke(channelMessageHandler);
      }
      catch (CommunicationException ex)
      {
        _Log.Error("An error occurred while communicating with the client", ex);
        Disconnect(channelMessageHandler);
      }
      catch (TimeoutException ex)
      {
        _Log.Error("An error occurred while communicating with the server", ex);
        Disconnect(channelMessageHandler);
      }
      catch (Exception ex)
      {
        _Log.Error("An error occurred while communicating with the server", ex);
        Disconnect(channelMessageHandler);
      }
    }
    #endregion

    #region public methods
    public long TotalMessagesSent { get { return Interlocked.Read(ref _TotalMessagesSent); } }
    #endregion

    #region constructors
    public Exchange()
    {

    }
    #endregion
  }
}
