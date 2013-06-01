using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace CommonTools.Caching.Testing.Mockups
{
    [Serializable]
    [ProtoContract]
    public class SimpleProtoObject2
    {
        [ProtoMember(1)]
        public string MyString { get; set; }
        [ProtoMember(2)]
        public int MyValue { get; set; }
        [ProtoMember(3)]
        public int MyThirdValue { get; set; }

        public SimpleProtoObject2() { }
    }
}
