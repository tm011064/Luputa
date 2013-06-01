using System;
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
using CommonTools.Web.UI;
using CommonTools.Components.TextResources;

namespace CommonTools.TestApp.Controls
{
    public class WebResourceManager : TextResourceManager
    {
        protected override string GetResourceNotFoundString(string key)
        {
            return "<strong>Resource " + key + " not found</strong>";
        }

        public WebResourceManager()
            : base("tr_Resources.TwoCultures.xml"
                , @"C:\VSS\CommonTools.3.5.root\CommonTools.TestSuite\Languages\"
                , "Resources.TwoCultures.xml"
                , "en-gb"
                , "resource"
                , "key")
        {
        }
    }

    /// <summary>
    /// Summary description for CommonToolsTextResource
    /// </summary>
    public class CommonToolsTextResource : TextResourceLiteral
    {
        private TextResourceManager _Resources;
        protected override TextResourceManager TextResourceManager
        {
            get
            {
                return _Resources;
            }
        }

        public CommonToolsTextResource()
        {
            _Resources = new WebResourceManager();
        }
    }
}