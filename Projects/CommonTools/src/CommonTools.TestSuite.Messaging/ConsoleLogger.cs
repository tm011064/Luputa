using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Messaging;

namespace CommonTools.TestSuite.Messaging
{
    public class ConsoleLogger : ILogManager
    {
        #region ILogManager Members

        public void Log(Type type, string message, LogLevel logLevel)
        {
            this.Log(type, message, null, logLevel);
        }

        public void Log(Type type, string message, Exception exception, LogLevel logLevel)
        {
            string output = type.Name.ToString() + ": " + message;
            if (exception != null)
                output += " -> " + exception.Message;

            System.Console.WriteLine(output);
        }

        #endregion
    }
}
