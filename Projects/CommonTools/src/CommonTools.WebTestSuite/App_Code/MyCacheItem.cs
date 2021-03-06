﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CommonTools.Components.Caching;
using System.Xml;

namespace CommonTools.TestApp.Components
{
    /// <summary>
    /// Summary description for MyCacheItem
    /// </summary>
    [Serializable]
    public class MyCacheItem : ICacheItem
    {
        private System.Web.Caching.CacheItemPriority _CacheItemPriority = System.Web.Caching.CacheItemPriority.Normal;
        private string _CacheKey = null;
        private bool _Enabled = false;
        private bool _IsIterating = false;
        private int _Minutes = -1;
        private int _Seconds = -1;
        private string _Name = null;
        private string _Suffix = null;
        private bool _IsClustered = false;

        #region ICacheItem Members
        public System.Web.Caching.CacheItemPriority CacheItemPriority
        {
            get { return this._CacheItemPriority; }
            set { this._CacheItemPriority = value; }
        }
        public string CacheKey
        {
            get { return this._CacheKey; }
            set { this._CacheKey = value; }
        }
        public bool Enabled
        {
            get { return this._Enabled; }
            set { this._Enabled = value; }
        }
        public bool IsIterating
        {
            get { return this._IsIterating; }
            set { this._IsIterating = value; }
        }
        public int Minutes
        {
            get { return this._Minutes; }
            set { this._Minutes = value; }
        }
        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }
        public string Suffix
        {
            get { return this._Suffix; }
            set { this._Suffix = value; }
        }
        #endregion

        #region constructor
        public MyCacheItem()
        {

        }
        #endregion

        #region ICacheItem Members


        public bool IsClustered
        {
            get { return this._IsClustered; }
            set { this._IsClustered = value; }
        }

        #endregion

        #region ICacheItem Members
        
        public int Seconds
        {
            get { return this._Seconds; }
            set { this._Seconds = value; }
        }

        #endregion

        #region ICacheItem Members

        public bool UseProtocolBufferSerialization
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsMemcached
        {
            get { throw new NotImplementedException(); }
        }

        public TimeSpan ContinuousAccessExtendedLifeSpan
        {
            get { throw new NotImplementedException(); }
        }

        public bool UseContinuousAccess
        {
            get { throw new NotImplementedException(); }
        }

        public TimeSpan LifeSpan
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
