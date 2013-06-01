using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.TestSuite.RuntimeCaching
{
    public class SimpleObject
    {
        public string MyString { get; set; }
        public int MyValue { get; set; }

        public override string ToString()
        {
            return MyString + " -> " + MyValue;
        }

        public SimpleObject() { }
    }
}
