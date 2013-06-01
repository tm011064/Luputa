using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;
using System.Web;
using System.Collections;
using CommonTools.Components.Caching;

namespace CommonTools.Web.UI
{
    internal class CachedElement
    {
        #region members
        private Cache _Cache = HttpRuntime.Cache;
        #endregion

        #region properties
        private string _Name = string.Empty;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private bool _Enabled = true;
        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }
        private int _Minutes = 15;
        public int Minutes
        {
            get { return _Minutes; }
            set { _Minutes = value; }
        }
        private int _Seconds = 15;
        public int Seconds
        {
            get { return _Seconds; }
            set { _Seconds = value; }
        }
        private string _Suffix = string.Empty;
        public string Suffix
        {
            get { return _Suffix; }
            set { _Suffix = value; }
        }
        private bool _IsIterating;
        public bool IsIterating
        {
            get { return _IsIterating; }
            set { _IsIterating = value; }
        }
        private CacheItemPriority _CacheItemPriority = CacheItemPriority.Default;
        public CacheItemPriority CacheItemPriority
        {
            get { return _CacheItemPriority; }
            set { _CacheItemPriority = value; }
        }

        public int NumberOfObjects
        {
            get
            {
                if (IsIterating)
                {
                    if (EnumeratedElements != null)
                        return EnumeratedElements.Count;
                    else
                        return 0;
                }
                else
                {
                    return _IsUsed ? 1 : 0;
                }
            }
        }
        private bool _IsUsed = false;
        public bool IsUsed
        {
            get { return _IsUsed; }
        }
        private Type _Type = typeof(System.Object);
        public Type Type
        {
            get { return _Type; }
        }

        private List<string> _EnumeratedElements = new List<string>();
        public List<string> EnumeratedElements
        {
            get { return _EnumeratedElements; }
        }
        #endregion

        #region constructor
        public CachedElement(ICacheItem item, ICacheController controller)
        {
            this._CacheItemPriority = item.CacheItemPriority;
            this._Enabled = item.Enabled;
            this._IsIterating = item.IsIterating;
            this._Minutes = item.Minutes < 0 ? controller.Minutes : item.Minutes;
            this._Seconds = item.Seconds;            
            this._Name = item.Name;
            this._Suffix = item.Suffix;

            #region define whether object is currently used in cache
            _IsUsed = false;
            if (item.IsIterating)
            {
                _EnumeratedElements = new List<string>();

                IDictionaryEnumerator enumerator = _Cache.GetEnumerator();
                bool typeIsInitialized = false;
                while (enumerator.MoveNext())
                {
                    if (enumerator.Key.ToString().ToLower().EndsWith(this.Suffix.ToLower()))
                    {
                        _EnumeratedElements.Add(enumerator.Key.ToString());
                        _IsUsed = true;

                        if (!typeIsInitialized)
                        {
                            this._Type = _Cache[enumerator.Key.ToString()].GetType();
                            typeIsInitialized = true;
                        }
                    }
                }
            }
            else
            {
                object obj = _Cache[this.Name];
                if (obj != null)
                {
                    _IsUsed = true;
                    this._Type = obj.GetType();
                }
                else
                {
                    _IsUsed = false;
                }
            }
            #endregion
        }
        #endregion
    }
}
