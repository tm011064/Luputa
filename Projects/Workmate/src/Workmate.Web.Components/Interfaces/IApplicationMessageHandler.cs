using System;
using Workmate.Messaging.Contracts;

namespace Workmate.Web.Components
{
  public interface IApplicationMessageHandler : IDisposable
  {
    event EventHandler<EventArgs> RefreshApplicationDataRequest;

    void Publish(MessageType messageType);
    void Publish(MessageType messageType, byte[] body);
  }
}
