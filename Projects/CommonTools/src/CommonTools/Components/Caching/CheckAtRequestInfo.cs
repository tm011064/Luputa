using System;
using System.Data.SqlClient;

namespace CommonTools.Components.Caching
{
    /// <summary>
    /// 
    /// </summary>
    internal struct CheckAtRequestInfo
    {
        private long _Ticks;
        public long Ticks
        {
            get { return _Ticks; }
            set { _Ticks = value; }
        }
        private DateTime? _LastChecked;
        public DateTime? LastChecked
        {
            get { return _LastChecked; }
            set { _LastChecked = value; }
        }

        public CheckAtRequestInfo(long ticks)
        {
            this._Ticks = ticks;
            this._LastChecked = null;
        }
        public CheckAtRequestInfo(long ticks, DateTime lastChecked)
        {
            this._Ticks = ticks;
            this._LastChecked = lastChecked;
        }
    }
}
