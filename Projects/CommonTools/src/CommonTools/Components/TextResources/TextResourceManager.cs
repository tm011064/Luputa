using System;
using System.Collections.Generic;
using System.Web.Caching;
using System.IO;
using System.Xml;
using System.Web;

namespace CommonTools.Components.TextResources
{
    /// <summary>
    /// This class acts as an interface to text content stored at xml files. It expects xml tags of the
    /// format <![CDATA[<MyResourceTag MyResourceKey="resourceKey">This is some content</MyResourceTag>]]> and
    /// offers methods to easily access these keys. 
    /// 
    /// This component supports localizeable resource content with a gracefull fallback mechanism in case a 
    /// certain resource is not present at all languages. To achieve this, you have to store your resource file
    /// in the following folder structure: 
    /// 
    ///         { RootFolder }\{ culture }\{ file path },
    ///         
    /// (e.g.:  C:\MyProject\TextResources\en-gb\Alerts\MyAlerts.xml, 
    ///         C:\MyProject\TextResources\de-at\Alerts\MyAlerts.xml,
    ///         C:\MyProject\TextResources\en-gb\CommonContent.xml ...)
    ///         
    /// When a resourcekey can't be found at a specified culture, the resourcekey will be looked up at the 
    /// specified 'DefaultCulture'.
    /// 
    /// The component comes with a handy literl web control to easily integrate text resources into aspx
    /// files. E.g.:
    /// 
    /// Code behind:
    /// <![CDATA[
    /// public class WebResourceManager : TextResourceManager
    /// {
    ///     protected override string GetResourceNotFoundString(string key)
    ///     {
    ///             return "<strong>Resource " + key + " not found</strong>";
    ///     }
    /// 
    ///     public WebResourceManager()
    ///         : base("tr_Resources.TwoCultures.xml"
    ///                 , @"C:\VSS\CommonTools.3.5.root\CommonTools.TestSuite\Languages\"
    ///                 , "Resources.TwoCultures.xml"
    ///                 , "en-gb"
    ///                 , "resource"
    ///                 , "key")
    ///     { }
    /// }
    /// 
    /// public class CommonToolsTextResource : TextResourceLiteral
    /// {
    ///     private TextResourceManager _Resources;
    ///     protected override TextResourceManager TextResourceManager
    ///     {
    ///         get
    ///         {
    ///             return _Resources;
    ///         }
    ///     }
    /// 
    ///     public CommonToolsTextResource()
    ///     {
    ///         _Resources = new WebResourceManager();
    ///     }
    /// }]]>
    /// 
    /// WebPage:
    /// <![CDATA[<ct:CommonToolsTextResource id="ctr" runat="server" ResourceKey="resource1" />]]>
    /// 
    /// </summary>
    public class TextResourceManager : ITextResourceManager
    {
        #region constants
        private const string SEPARATOR = "$";
        #endregion

        #region members
        private string _ResourcesCacheKey = string.Empty;
        private Cache _Cache = HttpRuntime.Cache;
        private object _CacheLock = new object();
        private string _AbsoluteResourceRootFolder = string.Empty;
        private string _DefaultCulture = string.Empty;
        private string _RelativeResourceFilepath = string.Empty;
        private string _TagName = string.Empty;
        private string _NodeIdentifier = string.Empty;
        #endregion

        #region properties
        /// <summary>
        /// Gets a dictionary with all resources.
        /// </summary>
        /// <value>The resources.</value>
        private Dictionary<string, Dictionary<string, string>> Resources
        {
            get
            {
                if (_Cache[_ResourcesCacheKey] != null)
                {
                    return (Dictionary<string, Dictionary<string, string>>)_Cache[_ResourcesCacheKey];
                }
                else
                {

                    Dictionary<string, Dictionary<string, string>> resources = GetResourceDictionaries();
                    SetCache(resources);
                    return resources;
                }
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Gets the resource file path.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        private string GetResourceFilePath(string culture)
        {
            return _AbsoluteResourceRootFolder + culture + @"\" + _RelativeResourceFilepath;
        }
        /// <summary>
        /// Gets all available cultures.
        /// </summary>
        /// <returns></returns>
        private string[] GetAllAvailableCultures()
        {
            string[] directories = Directory.GetDirectories(_AbsoluteResourceRootFolder);

            string[] cultures = new string[directories.Length];
            DirectoryInfo info;
            for (int i = 0; i < directories.Length; i++)
            {
                info = new DirectoryInfo(directories[i]);
                cultures[i] = info.Name;
            }
            return cultures;
        }
        /// <summary>
        /// Gets the resource file paths from all cultures.
        /// </summary>
        /// <returns></returns>
        private string[] GetResourceFilePathsFromAllCultures()
        {
            List<string> filePaths = new List<string>();
            string path = string.Empty;

            foreach (string culture in GetAllAvailableCultures())
            {
                path = GetResourceFilePath(culture);
                if (File.Exists(path))
                    filePaths.Add(path);
            }

            return filePaths.ToArray();
        }
        /// <summary>
        /// Sets the cache.
        /// </summary>
        /// <param name="resources">The resources.</param>
        private void SetCache(Dictionary<string, Dictionary<string, string>> resources)
        {
            _Cache.Insert(_ResourcesCacheKey, resources
                , new CacheDependency(GetResourceFilePathsFromAllCultures())
                , Cache.NoAbsoluteExpiration
                , Cache.NoSlidingExpiration
                , CacheItemPriority.NotRemovable
                , null);
        }
        /// <summary>
        /// Purges the dictionary from cache.
        /// </summary>
        private void PurgeFromCache()
        {
            _Cache.Remove(_ResourcesCacheKey);
        }
        /// <summary>
        /// This method returns a dictionary with all localized resource text dictionaries. Format:
        /// <![CDATA[Dictionary<{ culture }, Dictionary<{ resourceKey }, { content }>>]]>
        /// </summary>
        /// <returns>A dictionary with all localized resource text dictionaries</returns>
        /// <exception cref="TextResourceManagerException">Thrown when there is an error loading
        /// an xml file or there are resource keys are not unique at the resource file.</exception>
        private Dictionary<string, Dictionary<string, string>> GetResourceDictionaries()
        {
            Dictionary<string, Dictionary<string, string>> resources = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> resource;

            string path = string.Empty;
            foreach (string culture in GetAllAvailableCultures())
            {
                path = GetResourceFilePath(culture);
                if (File.Exists(path))
                {
                    resource = new Dictionary<string, string>();
                    XmlDocument document = new XmlDocument();
                    try
                    {
                        document.Load(path);
                    }
                    catch (Exception err)
                    {
                        throw new TextResourceManagerException(
                            @"Could not load file '" + path + @"': " + err.Message, err);
                    }
                    foreach (XmlNode node in document.GetElementsByTagName(_TagName))
                    {
                        if (node.NodeType != XmlNodeType.Comment)
                        {
                            XmlAttribute nameAttribute = node.Attributes[_NodeIdentifier];
                            if (nameAttribute != null)
                            {
                                if (resource.ContainsKey(nameAttribute.Value))
                                    throw new TextResourceManagerException("Resource key " + nameAttribute.Value + " was not unique at file " + path);

                                resource.Add(nameAttribute.Value, node.InnerText);
                            }
                        }
                    }
                    resources.Add(culture.ToLower(), resource);
                }
            }

            return resources;
        }
        #endregion

        #region protected methods
        /// <summary>
        /// This method returns a generic resource string if the spefied resource can't be found. It
        /// can be overwritten in deriving classes.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected virtual string GetResourceNotFoundString(string key)
        {
            return "MISSING RESOURCE: " + key;
        }
        #endregion

        #region public methods
        /// <summary>
        /// Gets the formatted resource text.
        /// </summary>
        /// <param name="key">The resource key.</param>
        /// <param name="args">An System.Object array containing zero or more objects to format.</param>
        /// <returns></returns>
        public string GetFormattedResourceText(string key, params object[] args)
        {
            return string.Format(GetResourceText(key, _DefaultCulture), args);
        }
        /// <summary>
        /// Gets the formatted resource text.
        /// </summary>
        /// <param name="key">The resource key.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="args">An System.Object array containing zero or more objects to format.</param>
        /// <returns></returns>
        public string GetFormattedResourceText(string key, string culture, params object[] args)
        {
            return string.Format(GetResourceText(key, culture), args);
        }
        /// <summary>
        /// Gets the resource text.
        /// </summary>
        /// <param name="key">The resource key.</param>
        /// <returns></returns>
        public string GetResourceText(string key)
        {
            return GetResourceText(key, _DefaultCulture);
        }
        /// <summary>
        /// Gets the resource text.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public string GetResourceText(string key, string culture)
        {
            if (string.IsNullOrEmpty(culture))
                culture = _DefaultCulture;

            culture = culture.ToLower();
            if (this.Resources.ContainsKey(culture))
            {
                if (this.Resources[culture].ContainsKey(key))
                    return this.Resources[culture][key];
                else
                {
                    // we didn't find the key, so test whether we've got it at the default culture...
                    if (culture != _DefaultCulture)
                    {
                        if (this.Resources.ContainsKey(_DefaultCulture))
                        {
                            if (this.Resources[_DefaultCulture].ContainsKey(key))
                                return this.Resources[_DefaultCulture][key];
                        }
                    }
                }
            }
            else if (this.Resources.ContainsKey(_DefaultCulture))
            {
                if (this.Resources[_DefaultCulture].ContainsKey(key))
                    return this.Resources[_DefaultCulture][key];
            }

            // we didn't find the key, so return the resource not found message...
            return GetResourceNotFoundString(key);
        }
        #endregion

        #region constructor
        /// <summary>
        /// This constructor creates a new instance of the TextResourceManager class.
        /// </summary>
        /// <param name="cacheKey">The key for storing resource files as dictionary at the HttpRuntime.Cache</param>
        /// <param name="absoluteResourceFolder">The root folder of all resources. Subfolders of this
        /// path must be folders representing culture keys. E.g: The absolute resource folder of the resource
        /// file 'C:\MyProject\TextResources\en-gb\Alerts\MyAlerts.xml' is ''C:\MyProject\TextResources\'</param>
        /// <param name="relativeFilePath">The filepath relative to the culture folder. E.g: The relative resource
        /// file path of the resource file 'C:\MyProject\TextResources\en-gb\Alerts\MyAlerts.xml' is 'Alerts\MyAlerts.xml'</param>
        /// <param name="defaultCulture">The default culture to use for gracefull fallbacks when a resource key is not
        /// found at a specific culture.</param>
        /// <param name="tagname">The name of a resource tag at the resource xml file. E.g.: The tag name of
        /// the xml document schema
        /// <![CDATA[
        /// <Resources>
        ///     <resource key="mykey1">some content</resource>
        ///     <resource key="mykey2">some other content</resource>
        /// </Resources>
        /// ]]>
        /// is 'resource'.
        /// </param>
        /// <param name="nodeIdentifier">The name of a resource tag identification attribute. E.g.: The nodeIdentifier of
        /// the xml document schema
        /// <![CDATA[
        /// <Resources>
        ///     <resource key="mykey1">some content</resource>
        ///     <resource key="mykey2">some other content</resource>
        /// </Resources>
        /// ]]>
        /// is 'key'.</param>
        public TextResourceManager(string cacheKey, string absoluteResourceFolder, string relativeFilePath
            , string defaultCulture, string tagname, string nodeIdentifier)
        {
            if (!absoluteResourceFolder.EndsWith(@"\"))
                absoluteResourceFolder += @"\";
            if (relativeFilePath.StartsWith(@"\"))
                relativeFilePath = relativeFilePath.Remove(0, 1);

            _ResourcesCacheKey = cacheKey;
            _AbsoluteResourceRootFolder = absoluteResourceFolder;
            _DefaultCulture = defaultCulture.ToLower();
            _RelativeResourceFilepath = relativeFilePath;
            _TagName = tagname;
            _NodeIdentifier = nodeIdentifier;
        }
        #endregion
    }
}