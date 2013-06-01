using System;
using System.Web.Caching;
using System.Xml;
using System.Collections.Generic;
using System.Web;

namespace CommonTools.Xml
{
    /// <summary>
    /// This class handles xml resource files and uses the HttpContext.Current.Cache object to store them.
    /// </summary>
    /// <example>
    /// 
    /// Xml File MyResource.xml:
    /// 
    /// &lt;RootNode&gt;
    ///     &lt;Tag TagIdentifier="key1"&gt;my first resource text&lt;/Tag&gt;
    ///     &lt;Tag TagIdentifier="key2"&gt;my second resource text&lt;/Tag&gt;
    ///     &lt;Tag TagIdentifier="key3"&gt;my third resource text&lt;/Tag&gt;
    /// &lt;/RootNode&gt;
    /// 
    /// Object usage:
    /// 
    /// XmlConfigurationFileManager resourceManager = new XmlConfigurationFileManager("MyResource.xml", "LD_", "Tag", "TagIdentifier", new TimeSpan(0, 10, 0));
    /// string myResource = ResourceManager.GetString("key1");
    /// 
    /// </example>
    public class XmlResourceFileManager
    {
        #region globals
        private string _Filename = string.Empty;
        private string _CachePrefix = string.Empty;
        private string _TagName = string.Empty;
        private string _NodeIdentifier = string.Empty;

        private Cache _Cache = HttpRuntime.Cache;
        #endregion

        #region properties
        private Dictionary<string, string> _Resources;
        private Dictionary<string, string> Resources
        {
            get
            {
                if (_Resources == null)
                {
                    // try to get the resources from cache
                    //
                    _Resources = _Cache[this.CacheKey] as Dictionary<string, string>;

                    if (_Resources == null)
                    {
                        // first update the resource dictionary
                        //
                        _Resources = LoadResource();

                        // Remove the cache key item if it was something else than a dictionary
                        //
                        _Cache.Remove(this.CacheKey);

                        // Now insert the dictionary into cache
                        //
                        CacheDependency dependency = new CacheDependency(_Filename);
                        _Cache.Insert(this.CacheKey, _Resources, dependency);
                    }
                }

                return _Resources;
            }
        }

        /// <summary>
        /// Gets the cache key used to cache the resource dictionary.
        /// </summary>
        /// <value>The cache key.</value>
        public virtual string CacheKey
        {
            get
            {
                return this._CachePrefix + this._Filename;
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Gets the string from the resource dictionary.
        /// </summary>
        /// <param name="key">The identifiying key of the resource to get.</param>
        /// <param name="text">The out bound resource text of the xml attribute with the specified key.</param>
        /// <returns>true if the resource was found, false if the key was not found at the resource disctionary</returns>
        protected virtual bool GetString(string key, out string text)
        {
            if (Resources.ContainsKey(key))
            {
                text = Resources[key];
                return true;
            }

            text = "MISSING RESOURCE: " + key;
            return false;
        }


        /// <summary>
        /// This method returns the dictionary that holds all resources.
        /// </summary>
        /// <returns>The dictionary that holds all resources.</returns>
        protected virtual Dictionary<string, string> LoadResource()
        {
            Dictionary<string, string> returnDictionary = new Dictionary<string, string>();

            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(_Filename);
            }
            catch (Exception err)
            {
                throw new XmlResourceFileManagerException(
@"Could not load file '" + _Filename + @"':
" + err.Message, err);
            }
            foreach (XmlNode node in document.GetElementsByTagName(_TagName))
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    XmlAttribute nameAttribute = node.Attributes[_NodeIdentifier];
                    if (nameAttribute != null)
                    {
                        returnDictionary.Add(nameAttribute.Value, node.InnerText);
                    }
                }
            }

            return returnDictionary;
        }
        #endregion

        #region public methods
        /// <summary>
        /// Determines whether the specified key is present as a resource.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if the specified key contains resource; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsResource(string key)
        {
            return _Resources.ContainsKey(key);
        }

        /// <summary>
        /// Gets the resource text with the given key.
        /// </summary>
        /// <param name="key">The key of the resource text to get.</param>
        /// <returns>The resource text with the given key</returns>
        public string GetString(string key)
        {
            string returnString = string.Empty;

            GetString(key, out returnString);

            return returnString;
        }

        /// <summary>
        /// Gets the resource text with the given key.
        /// </summary>
        /// <param name="key">The key of the resource text to get.</param>
        /// <param name="args">An System.Object array containing zero or more format items.</param>
        /// <returns>The resource text with the given key</returns>
        public string GetString(string key, params object[] args)
        {
            string returnString = string.Empty;

            if (GetString(key, out returnString))
            {
                return String.Format(returnString, args);
            }

            return returnString;
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonTools.Xml.XmlResourceFileManager"/> class.
        /// </summary>
        /// <param name="fileName">Absolute system filename of the file to load.</param>
        /// <param name="cachePrefix">The cachePrefix to use for this resource dictionary</param>
        /// <param name="tagName">Name of the tag being the parent of the resource xml nodes.</param>
        /// <param name="nodeIdentifier">The name of the xml node attribute that identifies the xml element.</param>
        public XmlResourceFileManager(string fileName, string cachePrefix, string tagName, string nodeIdentifier)
        {
            _Filename = fileName;
            _CachePrefix = cachePrefix;
            _TagName = tagName;
            _NodeIdentifier = nodeIdentifier;
        }
        #endregion
    }
}

