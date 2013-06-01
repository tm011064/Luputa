Author: Roman Majewski

Description:

The CommonTools.Xml.XmlSettingsManager is a helper class to load/save serializable classes as xml and store the values at
HttpRuntime cache for better performance. Using xml representation makes it easy for the developer to switch between database and
file storage for common settings files. The object to store must be serializable, additionally the client developer can use the
IXmlSerializable interface on his settings object to fully control the xml output.

Examples: 

(1) Database:
This example uses a common settings class serialized as an xml string stored at a database. 

Settings class:

	[Serializable]
    public class Settings
    {
        #region members
        private int _SettingsID = 1000;
        private int _MaxAllowedFileTransferTimeoutInSeconds = 60;        
        private string _ApplicationPath = "C:\\";        
        private bool _IsFileTransferEnabled = false;
        #endregion

        #region properties
        /// <summary>
        /// Gets or sets the settings ID.
        /// </summary>
        /// <value>The settings ID.</value>
        [XmlIgnore]
        public int SettingsID
        {
            get { return _SettingsID; }
            set { _SettingsID = value; }
        }

        /// <summary>
        /// Gets or sets the application path.
        /// </summary>
        /// <value>The application path.</value>
        public string ApplicationPath
        {
            get { return _ApplicationPath; }
            set { _ApplicationPath = value; }
        }

        /// <summary>
        /// Gets or sets the max allowed file transfer timeout in seconds.
        /// </summary>
        /// <value>The max allowed file transfer timeout in seconds.</value>
        public int MaxAllowedFileTransferTimeoutInSeconds
        {
            get { return _MaxAllowedFileTransferTimeoutInSeconds; }
            set { _MaxAllowedFileTransferTimeoutInSeconds = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is file transfer enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is file transfer enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsFileTransferEnabled
        {
            get { return _IsFileTransferEnabled; }
            set { _IsFileTransferEnabled = value; }
        }
        #endregion

        #region constructors
        public Settings() { }
        #endregion
	}

SettingsManager class:

	public static class SettingsManager
    {
        #region private static methods
        /// <summary>
        /// Gets the current settings from SQL provider.
        /// </summary>
        /// <returns></returns>
        private static Settings GetCurrentSettingsFromSqlProvider()
        {
			// implement logic for retrieving the xml representation stored at database
			// ...
			
			/*
            ApplicationSettingsTableAdapter adapter = new ApplicationSettingsTableAdapter();
            tdsApplicationSettings.ApplicationSettingsDataTable dt = adapter.GetData(GlobalValues.CurrentSettingsID);
            if (dt.Count > 0)
            {
                return XmlSerializationHelper<Settings>.ConvertFromXml(dt[0].SettingsXML);
            }

            throw new Exception("Current settings with SettingsID '" + GlobalValues.CurrentSettingsID.ToString()
                + "' could not be loaded from database.");
			*/
        }
        #endregion

        #region private static properties
        private static object _XmlSettingsManagerLock = new object();
        private static XmlSettingsManager<Settings> _XmlSettingsManager;
        /// <summary>
        /// Gets the XML settings manager instance.
        /// </summary>
        /// <value>The XML settings manager instance.</value>
        private static XmlSettingsManager<Settings> XmlSettingsManagerInstance
        {
            get
            {
                lock (_XmlSettingsManagerLock)
                {
                    if (_XmlSettingsManager == null)
                    {
                        _XmlSettingsManager = new XmlSettingsManager<Settings>(GlobalValues.CurrentSettingsCacheKey, new TimeSpan(14, 0, 0, 0)
                            , CacheItemPriority.NotRemovable, GetCurrentSettingsFromSqlProvider);
                    }
                }

                return _XmlSettingsManager;
            }
        }
        #endregion

        #region current settings methods
        /// <summary>
        /// Gets the static current settings defined at web.config (CurrentSettingsID).
        /// </summary>
        /// <returns>The current Settings</returns>
        public static Settings GetCurrentSettings()
        {
            return XmlSettingsManagerInstance.Settings;
        }
        /// <summary>
        /// This method loads the current settings from the database into cache and sets it at our static object. Call
        /// this method on application startup.
        /// </summary>
        public static void ReloadCurrentSettingsFromExternalSource()
        {
            XmlSettingsManagerInstance.PurgeItemFromCache();
        }
        /// <summary>
        /// Updates the current settings.
        /// </summary>
        /// <param name="settings">The Settings object to set as our new static and cached Settings object.</param>
        public static void UpdateCurrentSettings(Settings settings)
        {
            if (settings.SettingsID != GlobalValues.CurrentSettingsID)
                throw new Exception("Given Settings object did not have the same SettingsID as defined at web.config.");

            if (UpdateSettings(settings))
            {
                XmlSettingsManagerInstance.LoadFromObject(settings);
            }
        }
        #endregion
    }
	

(2) Xml file: Put these lines of code into a codebehind aspx.cs file and run in debug mode to see how the XmlSettingsManager
works with an external xml file as source:

	public partial class _Default : System.Web.UI.Page
	{
		protected void Page_PreRender(object sender, EventArgs e)
		{
			// create the xml file if it doesn't exist yet...
			if (!File.Exists(Server.MapPath("~/test.xml")))
				btnSerialize_Click(this, EventArgs.Empty);

			XmlSettingsManager<MyTestClass> manager = new XmlSettingsManager<MyTestClass>("__MyCacheKey", Server.MapPath("~/test.xml"));

			MyTestClass mySettingsObject = manager.Settings;
		}

		protected void btnSerialize_Click(object sender, EventArgs e)
		{
			Random r = new Random();

			MyTestClass testClass = new MyTestClass();
			testClass.MyElement = r.Next(0, 100).ToString();
			testClass.MyFirstAttribute = r.Next(0, 100).ToString();
			testClass.MySecondAttribute = r.Next(0, 100).ToString();
			testClass.MySecondElement = r.Next(0, 100).ToString();

			XmlSerializationHelper<MyTestClass>.SaveAsXmlFile(testClass, Server.MapPath("~/test.xml"));
		}
	}

	public class MyTestClass : IXmlSerializable
	{
		#region members
		private string _MyElement = "MyTestElement";
		private string _MyFirstAttribute = "MyFirstAttribute";
		private string _MySecondAttribute = "SecondAttribute";
		private string _MySecondElement = "SecondElement";
		#endregion

		#region properties
		public string MyElement
		{
			get { return _MyElement; }
			set { _MyElement = value; }
		}
		public string MyFirstAttribute
		{
			get { return _MyFirstAttribute; }
			set { _MyFirstAttribute = value; }
		}
		public string MySecondAttribute
		{
			get { return _MySecondAttribute; }
			set { _MySecondAttribute = value; }
		}
		public string MySecondElement
		{
			get { return _MySecondElement; }
			set { _MySecondElement = value; }
		}
		#endregion

		#region IXmlSerializable Members

		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(System.Xml.XmlReader reader)
		{
			while (reader.Read())
			{
				XmlNodeType type = reader.MoveToContent();
				if (type == XmlNodeType.Element && reader.Name == "MyElement")
				{
					if (reader.HasAttributes)
					{
						reader.MoveToAttribute("MyFirstAttribute");
						_MyFirstAttribute = reader.Value;
						reader.MoveToAttribute("MySecondAttribute");
						_MySecondAttribute = reader.Value;
					}
					_MyElement = reader.ReadElementString();
				}
				if (type == XmlNodeType.Element && reader.Name == "MySecondElement")
				{
					_MySecondElement = reader.ReadElementString();
				}
			}
		}

		public void WriteXml(System.Xml.XmlWriter writer)
		{
			writer.WriteStartElement("MyTestClass");
			{
				writer.WriteStartElement("MyElement");
				{
					writer.WriteAttributeString("MyFirstAttribute", MyFirstAttribute);
					writer.WriteAttributeString("MySecondAttribute", MySecondAttribute);
					writer.WriteString(MyElement);
				}
				writer.WriteEndElement();

				writer.WriteElementString("MySecondElement", MySecondElement);
			}
			writer.WriteEndElement();
		}

		#endregion
	}