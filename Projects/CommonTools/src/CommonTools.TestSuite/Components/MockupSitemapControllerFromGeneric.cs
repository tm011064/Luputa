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
using System.Collections.Generic;
using System.Xml.Serialization;
using FantasyLeague.CommonTools.Components.Caching;
using System.Xml;
using System.Web.Caching;
using FantasyLeague.CommonTools.TestSuite.Components;
using FantasyLeague.CommonTools.Web.Navigation;
using System.Xml.Linq;

namespace FantasyLeague.CommonTools.TestSuite.Components
{
    /// <summary>
    /// Summary description for MockupCacheController
    /// </summary>
    [Serializable]
    public class MockupSitemapControllerFromGeneric : GenericUrlRewriteController<MockupSitemapPageItem, MockupSiteMapMenuControllerFromGeneric>
    {
        #region abstract implementation

        public override IUrlRewriteController CreateUrlRewriteControllerInstance()
        {
            string errormessage = string.Empty;
            MockupSitemapControllerFromGeneric c = FantasyLeague.CommonTools.Xml.XmlSerializationHelper<MockupSitemapControllerFromGeneric>.ConvertFromFile(
                Configuration.GenericSitemapControllerXmlPath, out errormessage);

            if (!string.IsNullOrEmpty(errormessage))
                throw new Exception(errormessage);

            return c;
        }

        /// <remarks>
        /// The ReadXml could be overwritten in case we want to deserialize our object from here
        /// </remarks>
        //public override void ReadXml(System.Xml.XmlReader reader)
        //{
        //    _IUrlRewriteItem = GetGenericRewriteItemsFromXml(reader);
        //}

        #endregion


        #region constructor
        public MockupSitemapControllerFromGeneric()
        {

        }
        #endregion
    }
}