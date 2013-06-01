using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.TestSuite.Messaging.Stubs;
using CommonTools.Components.Localization;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CommonTools.TestSuite.Messaging
{
    class Program
    {
        static void Main(string[] args)
        {
            //BasicTestMessageHandlerSetup.RunServerTest();

            SimpleTestMessageHandlerSetup.RunSimpleTwoWayMessageHandlerTest();
        }
    }
}
