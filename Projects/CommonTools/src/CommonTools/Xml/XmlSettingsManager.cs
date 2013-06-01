using System;
using System.Xml;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;
using System.Runtime.Serialization;

namespace CommonTools.Xml
{
    /// <summary>
    /// This class handles the serialization/deserialization and caching of common settings classes that are [Serializable]. The object has to 
    /// be serializable, optionally you can use the IXmlSerializable interface on your object to take full control of the final xml representation.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    public class XmlSettingsManager<T>
        where T : new()
    {
        #region globals
        private string _CacheKey = string.Empty;                                    // the identification key used at HttpRuntime.Cache to store the Settings object
        private TimeSpan _CacheDuration;
        private CacheItemPriority _CacheItemPriority = CacheItemPriority.Default;
        private bool _SetCachedependencyOnFilename = false;
        private string _Filename;

        private Cache _Cache = HttpRuntime.Cache;
        private object _CacheLock = new object();                                   // object used for thread-safety
        #endregion

        #region delegates
        private GetSettingsObjectDelegate _SettingsObjectDelegate;
        /// <summary>
        /// This delegate gets called when caching is enabled and the cached object can't be retrieved from cache.  
        /// </summary>
        /// <returns>The T object.</returns>
        public delegate T GetSettingsObjectDelegate();
        #endregion

        #region private methods
        /// <summary>
        /// This method removes the currently cashed object (if still present) from cache and assignes the current T Settings to the
        /// HttpRuntime.Cache.
        /// </summary>
        private void RefreshCache()
        {
            if (_Settings != null)
            {// we can't insert a null object...

                // remove if the key was already used with an object of another type
                //
                _Cache.Remove(_CacheKey);

                // Insert new dictionary
                //
                if (_SetCachedependencyOnFilename == true && !string.IsNullOrEmpty(_Filename))
                {
                    _Cache.Insert(_CacheKey, _Settings, new CacheDependency(_Filename));
                }
                else
                {
                    _Cache.Insert(_CacheKey, _Settings, null, Cache.NoAbsoluteExpiration, _CacheDuration, _CacheItemPriority, null);
                }
            }
        }
        #endregion

        #region properties
        private T _Settings;
        /// <summary>
        /// Gets the currently loaded object of this XmlSettingsManager instance. This value is updated when calling one of the following methods:
        ///     LoadFromFile(string, bool);
        ///     LoadFromXml(string, bool);
        ///     RegisterItemAtCache(T);
        /// </summary>
        /// <value>The T Settings if already loaded, otherwise null.</value>
        public T Settings
        {
            get
            {
                // we need to be thread safe here
                lock (_CacheLock)
                {
                    // this flag is used when the cached object referring to our cachekey is not of type T
                    bool isUnexpectedObjectType = false;

                    // first, check whether we have a cached version of the desired object
                    if (_Cache[_CacheKey] != null)
                    {
                        if (_Cache[_CacheKey] is T)
                            _Settings = (T)_Cache[_CacheKey];
                        else
                            isUnexpectedObjectType = true;
                    }

                    if (_Settings == null || isUnexpectedObjectType)
                    {// we didn't have a cached version or the cached object was not of type T

                        if (_SetCachedependencyOnFilename && !string.IsNullOrEmpty(_Filename))
                        {// get the object from file
                            string errorMessage = string.Empty;
                            _Settings = XmlSerializationHelper<T>.ConvertFromFile(_Filename, out errorMessage);
                            if (_Settings == null)
                                throw new XmlSettingsManagerException(errorMessage);
                        }
                        else
                        {// get object from delegate
                            _Settings = (T)_SettingsObjectDelegate();
                        }

                        // insert the newly retrieved object into cache
                        RefreshCache();
                    }
                    return _Settings;
                }
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Loads the current Settings property from a specified xml file. The xml file must be a serialized version of the defined T object.
        /// </summary>
        /// <param name="filename">The filename (including the absolute path: C:\my\directory\filename.xml)</param>
        public void LoadFromFile(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {

                lock (_CacheLock)
                {
                    string errorMessage = string.Empty;
                    _Settings = XmlSerializationHelper<T>.ConvertFromFile(filename, out errorMessage);

                    if (!string.IsNullOrEmpty(errorMessage) || _Settings == null)
                        throw new XmlSettingsManagerException(errorMessage);

                    _Filename = filename;
                    _SetCachedependencyOnFilename = true;

                    RefreshCache();

                }
            }
        }
        /// <summary>
        /// Loads the current Settings property from a specified xml string. The xml be a serialized version of the defined T object.
        /// </summary>
        /// <param name="xml">The serialized xml version of the T object to load</param>
        public void LoadFromXml(string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                lock (_CacheLock)
                {
                    string errorMessage = string.Empty;
                    _Settings = XmlSerializationHelper<T>.ConvertFromXml(xml, out errorMessage);

                    if (!string.IsNullOrEmpty(errorMessage) || _Settings == null)
                        throw new XmlSettingsManagerException(errorMessage);

                    _SetCachedependencyOnFilename = false;
                    _Filename = string.Empty;

                    RefreshCache();
                }
            }
        }
        /// <summary>
        /// Loads this instance's T settings object from a given object.
        /// </summary>
        /// <param name="settings">The T settings object to load.</param>
        public void LoadFromObject(T settings)
        {
            if (settings != null)
            {
                lock (_CacheLock)
                {
                    _Settings = settings;

                    _SetCachedependencyOnFilename = false;
                    _Filename = string.Empty;

                    RefreshCache();
                }
            }
        }
        /// <summary>
        /// Saves the currently loaded T Settings object as a serialized XML file.
        /// </summary>
        /// <param name="filename">The filename to save the serialized representation to (including the absolute path: C:\my\directory\filename.xml)</param>
        /// <param name="errorMessage">The errormessage if not successfull.</param>
        /// <returns>True if successfull, otherwise false.</returns>
        public bool SaveAsXmlFile(string filename, out string errorMessage)
        {
            errorMessage = "T Settings object was null and therefore could not be serialized.";
            if (this.Settings != null)
            {
                if (XmlSerializationHelper<T>.SaveAsXmlFile(this.Settings, filename, out errorMessage))
                {
                    lock (_CacheLock)
                    {
                        _Filename = filename;
                        _SetCachedependencyOnFilename = true;

                        RefreshCache();
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Removes the T object from cache. If caching is enabled, the next T Settings property get call will load the object from the specified source.
        /// </summary>
        public void PurgeItemFromCache()
        {
            lock (_CacheLock)
            {
                _Cache.Remove(_CacheKey);
            }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSettingsManager&lt;T&gt;"/> class. If this constructor is called, caching
        /// is enabled.
        /// </summary>
        /// <param name="cacheKey">The cache key to identify the object at the HttpRuntime.Cache</param>
        /// <param name="cacheDuration">The cache duration</param>
        /// <param name="cacheItemPriority">The cache item priority</param>
        /// <param name="settingsObject">The delegate to call when the object can't be found at HttpRuntime.Cache and therefor needs to be inserted again.</param>
        public XmlSettingsManager(string cacheKey, TimeSpan cacheDuration, CacheItemPriority cacheItemPriority, GetSettingsObjectDelegate settingsObject)
        {
            if (settingsObject == null)
                throw new XmlSettingsManagerException("GetSettingsObjectDelegate must not be null.");

            _SettingsObjectDelegate -= settingsObject;
            _SettingsObjectDelegate += settingsObject;

            _CacheDuration = cacheDuration;
            _CacheItemPriority = cacheItemPriority;

            _CacheKey = cacheKey;

            _Filename = string.Empty;
            _SetCachedependencyOnFilename = false;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSettingsManager&lt;T&gt;"/> class. If this constructor is called, a cache dependency is set
        /// to the specified file holding an xml serialized version of the T Settings object.
        /// </summary>
        /// <param name="cacheKey">The cache key to identify the object at the HttpRuntime.Cache</param>
        /// <param name="filename">The filename to load the object from and set a cache dependency on. Be aware that the System.Web.Caching.CacheDependency()
        /// class won't work when your local XmlSettingsManager&lt;T&gt; object instance is static (Don't know why, caching works fine, it's just the
        /// cachedependency that doesn't work...).</param>
        public XmlSettingsManager(string cacheKey, string filename)
        {
            _CacheKey = cacheKey;
            _Filename = filename;

            _SetCachedependencyOnFilename = true;
        }
        #endregion
    }
}

