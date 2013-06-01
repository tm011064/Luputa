Author: Roman Majewski

Description:
CommonTools.Components.Caching is a namespace that lets you easily manage your cached objects through a web configuration section or
a custom provider.
Basically, cachable objects for this framework can be divided into two types:
	
	(a) objects that need an iterationkey (primary key...):
			myApplicationUser user1 = new myApplicationUser(15);    // where the passed parameter '15' is the ID of the application user
			myApplicationUser user2 = new myApplicationUser(367);   // ...
	(b) objects that don't need an identifier:
			myBasicSettings settings = new myBasicSettings();

CommonTools.Components.Caching can handle both types, it also gives you the ability to purge all cached items
of type (a) with one line of code.

Example:
	[1]
	Syntax for (a)
    public class ObjectWithIdentifier :
        CacheObject<TestNamespace.TestObjectToCache>
    {
        #region abstract implementation

        protected override object GetObject()
        {
            return new TestNamespace.TestObjectToCache(_Index);
        }

        #endregion

        #region globals
        private int _Index;
        #endregion

        #region constructor and instancer
        public static ObjectWithIdentifier Instance(int index)
        {
            return new cboSecondObject(index);
        }
        private ObjectWithIdentifier(int index)
            : base(index.ToString())
        {
            this._Index = index;
        }
        #endregion
    }
    
    Syntax for (b)	
	public class ObjectWithoutIdentifier :
        CacheObject<TestNamespace.TestObjectToCache>
    {
        #region abstract implementation

        protected override object GetObject()
        {
            return new TestNamespace.TestObjectToCache();
        }

        #endregion

        #region constructor and instancer
        public static ObjectWithoutIdentifier Instance()
        {
            return new ObjectWithoutIdentifier();
        }
        private ObjectWithoutIdentifier() { }
        #endregion
    }
    
    Description: Everything you need to do to use the CommonTools.Components.Caching namespace is to derive
    your object from CommonTools.Components.Caching.CacheObject<T>, where T is the type of the business object
    to cache.
    The actual object to be cached must be returned at the abstract method 'GetObject()'. The only
    difference between objects of type (a) and (b) is that you need to instanciate the cache identification
    key at the base constructor of your (b) object.

	[2] web.config:
	
	Section definition:
	
	<configSections>
		<section name="Cache" type="CommonTools.Components.Caching.CacheSection, CommonTools" />
	</configSections>

	Section (web.config model):
		
	<Cache>
		<objects minutes="5" enabled="true">
			<add name="ObjectWithoutIdentifier" enabled="true" cacheItemPriority="High" />
			<add name="ObjectWithIdentifier" enabled="true" minutes="400" IsIterating="true" suffix="_BO" />
		</objects>
	</Cache>
	
	Section (custom provider model):
	
	<Cache cacheControllerProviderType="DummyDatabase.MyCacheController, App_Code" />
	
	
	Description:	If you choose to use web.config for managing your cache objects, the following attributes/elements must 
					be defined:
	
					<objects> attributes:
						minutes (int) -> the default value for the cache duration of the object (in minutes)
						enabled (bool) -> when set to false, the cache won't be used for any object
				
					<elements of objects> attributes:
						name (string, required) -> The name of the class that manages the cached object
						enabled (bool, default=true) -> when set to false, the cache won't be used for this object
						minutes (int) -> overrides the default minutes setting
						IsIterating (bool, default=false) -> defines whether this object needs an identifier (case (a))
						suffix (string, default={ name attribute }) -> identification suffix used for an enumerating object's cache key
						cacheItemPriority (System.Web.Caching.CacheItemPriority, default=System.Web.Caching.CacheItemPriority.Normal) -> The System.Web.Caching.CacheItemPriority of this object
		
					Alternatively, you can use a custom provider which has to implement the CommonTools.Components.Caching.ICacheController
					interface. Here is a code example of creating your own custom provider:
					
					-------------------------------------------------------------------------------------------------------------------------
					1) ICacheController object
					================================================
					
					[Serializable]
					public class MyCacheController : ICacheController, IXmlSerializable
					{
						#region members
						private bool _Enabled = false;
						private int _Minutes = 15;
						private List<ICacheItem> _CacheItems = new List<CommonTools.Components.Caching.ICacheItem>();
						#endregion

						#region ICacheController Members
						public ICacheController CreateCacheControllerInstance()
						{
							string errormessage = string.Empty;
							MyCacheController c = CommonTools.Xml.XmlSerializationHelper<MyCacheController>.ConvertFromFile(
								HttpContext.Current.Server.MapPath("~/cacheconfig.xml"), out errormessage);

							if (!string.IsNullOrEmpty(errormessage))
								throw new Exception(errormessage);

							return c;
						}

						public bool Enabled
						{
							get { return this._Enabled; }
							set { this._Enabled = value; }
						}

						public CommonTools.Components.Caching.ICacheItem GetCacheItem(string key)
						{
							foreach (ICacheItem item in CacheItems)
								if (item.Name == key)
									return item;

							return null;
						}

						public int Minutes
						{
							get { return this._Minutes; }
							set { this._Minutes = value; }
						}

						public System.Collections.Generic.List<ICacheItem> CacheItems
						{
							get { return this._CacheItems; }
							set { this._CacheItems = value; }
						}

						#endregion

						#region IXmlSerializable Members

						public System.Xml.Schema.XmlSchema GetSchema()
						{
							return null;
						}

						public void ReadXml(System.Xml.XmlReader reader)
						{
							_CacheItems = new List<ICacheItem>();
							MyCacheItem item;
							while (reader.Read())
							{
								XmlNodeType type = reader.MoveToContent();
								if (type == XmlNodeType.Element && reader.Name == "MyCacheSection")
								{
									if (reader.HasAttributes)
									{
										reader.MoveToAttribute("enabled");
										_Enabled = bool.Parse(reader.Value);
										reader.MoveToAttribute("minutes");
										_Minutes = int.Parse(reader.Value);
									}
								}
								if (type == XmlNodeType.Element && reader.Name == "MyCacheItem")
								{
									if (reader.HasAttributes)
									{
										item = new MyCacheItem();

										if (reader.MoveToAttribute("cacheItemPriority"))
											item.CacheItemPriority = (CacheItemPriority)Enum.Parse(typeof(CacheItemPriority), reader.Value);
										if (reader.MoveToAttribute("cacheKey"))
											item.CacheKey = reader.Value;
										if (reader.MoveToAttribute("enabled"))
											item.Enabled = bool.Parse(reader.Value);
										if (reader.MoveToAttribute("isIterating"))
											item.IsIterating = bool.Parse(reader.Value);
										if (reader.MoveToAttribute("minutes"))
											item.Minutes = int.Parse(reader.Value);
										if (reader.MoveToAttribute("name"))
											item.Name = reader.Value;
										if (reader.MoveToAttribute("suffix"))
											item.Suffix = reader.Value;

										_CacheItems.Add(item);
									}
								}
							}
						}

						public void WriteXml(System.Xml.XmlWriter writer)
						{
							writer.WriteStartElement("MyCacheSection");
							{
								writer.WriteAttributeString("enabled", _Enabled.ToString());
								writer.WriteAttributeString("minutes", _Minutes.ToString());

								foreach (ICacheItem item in this.CacheItems)
								{
									writer.WriteStartElement("MyCacheItem");
									{
										writer.WriteAttributeString("cacheItemPriority", item.CacheItemPriority.ToString());
										writer.WriteAttributeString("cacheKey", item.CacheKey);
										writer.WriteAttributeString("enabled", item.Enabled.ToString());
										writer.WriteAttributeString("isIterating", item.IsIterating.ToString());
										writer.WriteAttributeString("minutes", item.Minutes.ToString());
										writer.WriteAttributeString("name", item.Name);
										writer.WriteAttributeString("suffix", item.Suffix);
									}
									writer.WriteEndElement();
								}
							}
							writer.WriteEndElement();
						}

						#endregion

						#region constructor
						public MyCacheController()
						{

						}
						#endregion
					}
					
					-------------------------------------------------------------------------------------------------------------------------
					2) ICacheItem object
					================================================
					
					public class MyCacheItem : ICacheItem
					{
						private System.Web.Caching.CacheItemPriority _CacheItemPriority = System.Web.Caching.CacheItemPriority.Normal;
						private string _CacheKey = null;
						private bool _Enabled = false;
						private bool _IsIterating = false;
						private int _Minutes = -1;
						private string _Name = null;
						private string _Suffix = null;

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
					}
					
	
	[3] Use in code:
	In order to see the effect of using this namespace, create a default webpage and copy/paste the code (1) below. Then create
	a class file in App_Code and copy/paste the class code (2). Finally, create a web.config file and copy/paste the given code (3).
	
	-------------------------------------------------------------------------------------------------------------------------
	1) Webpage (default.aspx)
	================================================
	<%@ Page Language="C#" AutoEventWireup="true" %>

	<%@ Register TagPrefix="CV" Namespace="CommonTools.Components.Caching" Assembly="CommonTools" %>
	<%@ Import Namespace="TestCache" %>
	<%@ Import Namespace="TestCache.MyObjects" %>
	<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
	<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title>Untitled Page</title>
	</head>

	<script runat="server">
		protected void Page_Load(object sender, EventArgs e)
		{
			TestObjectToCache firstObject = cached_TestObjectToCache5.Instance().Fetch();
			TestObjectToCache secondObject = cached_TestObjectToCache.Instance(14).Fetch();

			TestObjectToCache firstObject1 = cached_TestObjectToCache5.Instance().Fetch(false);
			TestObjectToCache secondObject1 = cached_TestObjectToCache.Instance(14).Fetch(false);

			TestObjectToCache secondObject2 = cached_TestObjectToCache.Instance(12).Fetch(true);
			TestObjectToCache secondObject3 = cached_TestObjectToCache.Instance(11).Fetch(true);

			lblOutput.Text = "";
			lblOutput.Text += "First object: " + firstObject.InstanceTime.ToString("HH:mm:ss");
			lblOutput.Text += "<br/>";
			lblOutput.Text += "Second object: " + secondObject.InstanceTime.ToString("HH:mm:ss");
			lblOutput.Text += "<br/>";
			lblOutput.Text += "First object no cache: " + firstObject1.InstanceTime.ToString("HH:mm:ss");
			lblOutput.Text += "<br/>";
			lblOutput.Text += "Second object no cache: " + secondObject1.InstanceTime.ToString("HH:mm:ss");
			lblOutput.Text += "<br/>";
		}
	</script>

	<body>
		<form id="form1" runat="server">
			<div>
				<asp:Label ID="lblOutput" runat="server" Text=""></asp:Label>
			<CV:CacheView ID="cv1" runat="server" Theme="Default" Font-Names="Courier New"
				Font-Size="13px" /></div>
		</form>
	</body>
	</html>

	-------------------------------------------------------------------------------------------------------------------------
	2) Class file 
	================================================
	using System;
	using CommonTools.Components.Caching;

	namespace TestCache.MyObjects
	{
		public class cached_TestObjectToCache5 :
			CacheObject<TestCache.TestObjectToCache>
		{
			#region abstract implementation

			protected override object GetObject()
			{
				return new TestCache.TestObjectToCache();
			}

			#endregion

			#region constructor and instancer
			public static cached_TestObjectToCache5 Instance()
			{
				return new cached_TestObjectToCache5();
			}
			private cached_TestObjectToCache5() { }
			#endregion
		}

		public class cached_TestObjectToCache :
			CacheObject<TestCache.TestObjectToCache>
		{
			#region abstract implementation

			protected override object GetObject()
			{
				return new TestCache.TestObjectToCache(_Index);
			}

			#endregion

			#region globals
			private int _Index;
			#endregion

			#region constructor and instancer
			public static cached_TestObjectToCache Instance(int index)
			{
				return new cached_TestObjectToCache(index);
			}
			private cached_TestObjectToCache(int index)
				: base(index.ToString())
			{
				this._Index = index;
			}
			#endregion
		}
	}

	namespace TestCache
	{
		public class TestObjectToCache
		{
			private DateTime _DateTime;
			public DateTime InstanceTime { get { return _DateTime; } }

			public TestObjectToCache()
			{
				_DateTime = DateTime.Now;
			}

			public TestObjectToCache(int addMinutes)
			{
				_DateTime = DateTime.Now.AddMinutes(addMinutes);
			}
		}

		public class TestObjectToCache1
		{
			private DateTime _DateTime;
			public DateTime InstanceTime { get { return _DateTime; } }

			public TestObjectToCache1()
			{
				_DateTime = DateTime.Now;
			}

			public TestObjectToCache1(int addMinutes)
			{
				_DateTime = DateTime.Now.AddMinutes(addMinutes);
			}
		}
	}

	-------------------------------------------------------------------------------------------------------------------------
	3) web.config
	================================================
	<?xml version="1.0"?>
	<configuration>
	  <configSections>
		<section name="Cache" type="CommonTools.Components.Caching.CacheSection, CommonTools" />
	  </configSections>
	  <Cache>
		<objects minutes="20" enabled="true">
		  <add name="cached_TestObjectToCache5" enabled="true" minutes="1" />
		  <add name="cached_TestObjectToCache" enabled="true" minutes="400" IsIterating="true" suffix="_BO" cacheItemPriority="AboveNormal" />
		  <add name="cached_TestObjectToCache1" enabled="true" IsIterating="true" suffix="_DO" cacheItemPriority="High" />
		  <add name="cached_TestObjectToCache2" enabled="true" cacheItemPriority="NotRemovable" />
		  <add name="cached_TestObjectToCache3" enabled="false" minutes="16" IsIterating="true" suffix="_SO" cacheItemPriority="Low" />
		  <add name="cached_TestObjectToCache4" enabled="true" />
		</objects>
	  </Cache>
	</configuration>