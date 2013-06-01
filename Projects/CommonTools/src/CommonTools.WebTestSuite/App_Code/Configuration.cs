using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DummyDatabase.SqlProvider;
using System.Configuration;

namespace CommonTools.TestApp.Components
{
    public static class Configuration
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DummyDatabase.SqlProvider.Properties.Settings.DummyBaseConnectionString"].ConnectionString;
            }
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
