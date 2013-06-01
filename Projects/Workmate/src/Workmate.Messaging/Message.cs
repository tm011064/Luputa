using System;
using ProtoBuf;

namespace Workmate.Messaging
{
  [ProtoContract(SkipConstructor=true)]
  public class Message<TMessageType>
  {
    [ProtoMember(1)]
    public string SenderId { get; set; }

    [ProtoMember(2)]
    public DateTime SentUtc { get; set; }

    [ProtoMember(3)]
    public TMessageType MessageType { get; set; }

    [ProtoMember(4)]
    public byte[] Body { get; set; }

    public Message(string senderId, DateTime sentUtc, TMessageType messageType, byte[] body)
    {
      this.SenderId = senderId;
      this.SentUtc = sentUtc;
      this.MessageType = messageType;
      this.Body = body;
    }
  } 
}
