using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Messaging;
using RabbitMQ.Client;
using System.IO;
using RabbitMQ.Client.Exceptions;
using ProtoBuf;

namespace CommonTools.TestSuite.Messaging.Stubs
{
    [ProtoContract]
    public class SimpleObject
    {
        [ProtoMember(1)]
        public int Value { get; set; }

        public SimpleObject(int value)
        {
            this.Value = value;
        }
        public SimpleObject() { }
    }
}
