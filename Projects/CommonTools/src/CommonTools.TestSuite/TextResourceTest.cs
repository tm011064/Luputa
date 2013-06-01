using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CommonTools.Components.Testing;
using CommonTools.Components.BusinessTier;
using CommonTools;
using System.Diagnostics;
using CommonTools.TestSuite.Components;
using CommonTools.Components.TextResources;

namespace CommonTools.TestSuite.Tests
{
    [TestFixture]
    public class Components_TextResources
    {
        #region const values

        #endregion

        #region basic tests

        [Test]
        public void Test_LoadTextResource()
        {
            Trace.WriteLine(Configuration.GetGenericHeader());

            TextResourceManager manager = new TextResourceManager("tr_Resources.TwoCultures.xml"
                , Configuration.TextResourcesRootFolder
                , "Resources.TwoCultures.xml"
                , "en-gb"
                , "resource"
                , "key");

            Assert.IsTrue(manager.GetResourceText("resource1") == "First resource");
            Assert.IsTrue(manager.GetResourceText("resource2") == "Second resource");
            Assert.IsTrue(manager.GetResourceText("resource3") == "Third resource");
            Assert.IsTrue(manager.GetResourceText("resource1", "de-at") == "Erster text");
            Assert.IsTrue(manager.GetResourceText("resource2", "de-at") == "Zweiter text");
            Assert.IsTrue(manager.GetResourceText("resource3", "de-at") == "Third resource");

            manager = new TextResourceManager("tr_Resources.OneCulture.xml"
                , Configuration.TextResourcesRootFolder
                , "Resources.OneCulture.xml"
                , "en-gb"
                , "resource"
                , "key");

            Assert.IsTrue(manager.GetResourceText("resource1") == "First resource");
            Assert.IsTrue(manager.GetResourceText("resource2") == "Second resource");
            Assert.IsTrue(manager.GetResourceText("resource3") == "Third resource");
            Assert.IsTrue(manager.GetResourceText("resource1", "de-at") == "First resource");
            Assert.IsTrue(manager.GetResourceText("resource2", "de-at") == "Second resource");
            Assert.IsTrue(manager.GetResourceText("resource3", "de-at") == "Third resource");

            Trace.WriteLine(Configuration.GetGenericFooter());
        }

        #endregion
    }
}
