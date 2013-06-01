using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace CommonTools.Caching.Testing.Mockups
{
    [Serializable]
    public class SimpleObject
    {
        public string MyString { get; set; }
        public int MyValue { get; set; }

        public SimpleObject() { }
    }
}
