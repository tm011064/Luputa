using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using CommonTools.TestSuite.DummyDatabase.SqlProvider;
using System.Diagnostics;

namespace CommonTools.TestSuite.Components
{
    public static class Configuration
    {
        #region trace formatting
        private const string CONFIGURATION_NEWLINE = "\n*****     ";
        private const string CONFIGURATION_HEADER = "****************************************************************" + CONFIGURATION_NEWLINE + "Configuration values:" + CONFIGURATION_NEWLINE;
        private const string CONFIGURATION_FOOTER = CONFIGURATION_NEWLINE + "\n****************************************************************\n";

        private const string GENERIC_TESTMETHOD_HEADER = "\n\n\n\n     START TEST METHOD: {0}\n________________________________________________________________";
        private const string GENERIC_TESTMETHOD_FOOTER = "________________________________________________________________\n     TEST METHOD {0} COMPLETED";

        public static string GetGenericHeader()
        {
            return string.Format(GENERIC_TESTMETHOD_HEADER, new StackTrace().GetFrame(1).GetMethod().Name);
        }
        public static string GetGenericFooter()
        {
            return string.Format(GENERIC_TESTMETHOD_FOOTER, new StackTrace().GetFrame(1).GetMethod().Name);
        }
        #endregion

        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["CommonTools.TestSuite.DummyDatabase.SqlProvider.Properties.Settings.DummyBaseConnectionString"].ConnectionString;
            }
        }
        public static string ClusteredCacheConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["CommonTools.TestSuite.ClusteredCacheConnectionString"].ConnectionString;
            }
        }

        public static string DummyDataXmlPath
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings["XmlDummyDataPath"]; }
        }
        public static string CacheConfigXmlPath
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings["CacheConfigPath"]; }
        }
        public static string SitemapControllerXmlPath
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings["SitemapControllerPath"]; }
        }
        public static string SiteMapMenuControllerXmlPath
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings["SiteMapMenuControllerPath"]; }
        }
        public static string GenericSitemapControllerXmlPath
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings["GenericSitemapControllerPath"]; }
        }
        public static string GenericSiteMapMenuControllerXmlPath
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings["GenericSiteMapMenuControllerPath"]; }
        }

        public static string WrongConnectionString
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings["WrongConnectionString"]; }
        }
        public static string TextResourcesRootFolder
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings["TextResourcesRootFolder"]; }
        }

        public static UsersDataContext GetUsersDataContext()
        {
            UsersDataContext dc = new UsersDataContext(ConnectionString);
            dc.DeferredLoadingEnabled = false;
            dc.ObjectTrackingEnabled = false;

            return dc;
        }
    }
}
