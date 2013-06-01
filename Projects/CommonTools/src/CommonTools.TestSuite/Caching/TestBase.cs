using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using ProtoBuf;
using System.Runtime.Serialization;

namespace CommonTools.Caching.Testing
{
    public class TestBase
    {

        #region trace formatting
        private const string CONFIGURATION_NEWLINE = "\n*****     ";
        private const string CONFIGURATION_HEADER = "****************************************************************" + CONFIGURATION_NEWLINE + "Configuration values:" + CONFIGURATION_NEWLINE;
        private const string CONFIGURATION_FOOTER = CONFIGURATION_NEWLINE + "\n****************************************************************\n";

        private const string GENERIC_TESTMETHOD_HEADER = "\n\n\n\n     START TEST METHOD: {0}\n________________________________________________________________";
        private const string GENERIC_TESTMETHOD_FOOTER = "________________________________________________________________\n     TEST METHOD {0} COMPLETED";

        protected string GetGenericHeader()
        {
            return string.Format(GENERIC_TESTMETHOD_HEADER, new StackTrace().GetFrame(1).GetMethod().Name);
        }
        protected string GetGenericFooter()
        {
            return string.Format(GENERIC_TESTMETHOD_FOOTER, new StackTrace().GetFrame(1).GetMethod().Name);
        }

        protected void TraceHeader()
        {
            Trace.WriteLine( string.Format("\n\n\nMethod: {0}\n", new StackTrace().GetFrame(1).GetMethod().Name));
        }
        #endregion

        protected byte[] ProtocolBufferSerialize<T>(T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }

        protected T ProtocolBufferDeserialize<T>(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Position = 0;
                return Serializer.Deserialize<T>(ms);
            }
        }

        #region stopwatch

        private Dictionary<string, Stopwatch> _Stopwatches = new Dictionary<string, Stopwatch>();

        /// <summary>
        /// Gets the stopwatch.
        /// </summary>
        /// <value>The stopwatch.</value>
        protected Stopwatch GetStopwatch(string key)
        {
            if (!this._Stopwatches.ContainsKey(key))
            {
                this._Stopwatches.Add(key, new Stopwatch());
            }
            return this._Stopwatches[key];
        }

        /// <summary>
        /// Resets the and start stopwatch.
        /// </summary>
        /// <param name="key">The key.</param>
        protected void ResetAndStartStopwatch(string key)
        {
            Stopwatch sw = GetStopwatch(key);
            sw.Reset();
            sw.Start();
        }

        protected void ResetStopwatch(string key)
        {
            Stopwatch sw = GetStopwatch(key);
            sw.Reset();
        }
        protected void StartStopwatch(string key)
        {
            Stopwatch sw = GetStopwatch(key);
            sw.Start();
        }
        protected void StopStopwatch(string key)
        {
            Stopwatch sw = GetStopwatch(key);
            sw.Stop();
        }
        /// <summary>
        /// Stops the and trace stopwatch.
        /// </summary>
        /// <param name="key">The key.</param>
        protected void StopAndTraceStopwatch(string key)
        {
            Stopwatch sw = GetStopwatch(key);
            sw.Stop();
            Trace.WriteLine(string.Format("Stopwatch {0} elapsed: {1}", key, sw.ElapsedMilliseconds.ToString()));
        }
        #endregion
    }
}
