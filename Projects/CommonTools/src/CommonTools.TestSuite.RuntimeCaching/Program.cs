using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.TestSuite.RuntimeCaching.Tests;
using System.Diagnostics;
using CommonTools.Components.Combinatorics;
using System.Threading;

namespace CommonTools.TestSuite.RuntimeCaching
{
    class Program
    {
        static void Main(string[] args)
        {
            TestContinuousAccess testContinuousAccess = new TestContinuousAccess();
            testContinuousAccess.Test_CrossThreadAccess();

            //TestThreadSafety testThreadSafety = new TestThreadSafety();
            //testThreadSafety.Test_SimultaneousAccess();
        }
    }
}
