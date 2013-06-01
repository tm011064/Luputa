using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Messaging;
using Workmate.Messaging.Contracts;

namespace Workmate.Web.Components
{
  public class ApplicationMessageHandler : IApplicationMessageHandler
  {
    #region const members
    private static readonly string WEB_APPLICATION_MESSAGE_ROUTING_KEY = RoutingKey.WebClientChannel.ToString("d");
    #endregion

    #region members
    private IClientMessageHandlerProxy _ClientMessageHandlerProxy;
    #endregion

    #region event handlers
    public event EventHandler<EventArgs> RefreshApplicationDataRequest;
    public event EventHandler<CommunicationStateChangedEventArgs> CommunicationStateChanged;
    #endregion

    #region events
    void _ClientMessageHandlerProxy_MessageReceived(object sender, MessageReceivedEventArgs e)
    {
      IClientMessageHandlerProxy clientMessageHandlerProxy = sender as IClientMessageHandlerProxy;
      Message<MessageType> message = clientMessageHandlerProxy.DeserializeMessage<MessageType>(e.MessageBody);

      switch (message.MessageType)
      {
        case MessageType.RefreshApplicationDataRequest:

          if (this.RefreshApplicationDataRequest != null)
            this.RefreshApplicationDataRequest(this, EventArgs.Empty);

          break;
      }
    }
    void _ClientMessageHandlerProxy_CommunicationStateChanged(object sender, CommunicationStateChangedEventArgs e)
    {
      IClientMessageHandlerProxy clientMessageHandlerProxy = sender as IClientMessageHandlerProxy;
      switch (e.CommunicationStatus)
      {
        case CommunicationStatus.Opened:
          // we have a connection, subscribe to web application events
          clientMessageHandlerProxy.Subscribe(WEB_APPLICATION_MESSAGE_ROUTING_KEY);

          if (this.CommunicationStateChanged != null)
            this.CommunicationStateChanged(this, e);

          break;
      }
    }
    #endregion

    #region public methods
    public void Connect()
    {
      _ClientMessageHandlerProxy.Connect();
    }

    public void Publish(MessageType messageType)
    {
      _ClientMessageHandlerProxy.Publish<MessageType>(messageType, WEB_APPLICATION_MESSAGE_ROUTING_KEY, null);
    }
    public void Publish(MessageType messageType, byte[] body)
    {
      _ClientMessageHandlerProxy.Publish<MessageType>(messageType, WEB_APPLICATION_MESSAGE_ROUTING_KEY, body);
    }
    #endregion

    #region constructors
    public ApplicationMessageHandler(IClientMessageHandlerProxy clientMessageHandlerProxy)
    {
      _ClientMessageHandlerProxy = clientMessageHandlerProxy;
      
      _ClientMessageHandlerProxy.MessageReceived += new EventHandler<MessageReceivedEventArgs>(_ClientMessageHandlerProxy_MessageReceived);
      _ClientMessageHandlerProxy.CommunicationStateChanged += new EventHandler<CommunicationStateChangedEventArgs>(_ClientMessageHandlerProxy_CommunicationStateChanged);
    }
    #endregion

    #region IDisposable Members

    public void Dispose()
    {
      try { _ClientMessageHandlerProxy.Dispose(); }
      catch { }
    }

    #endregion
  }
}
