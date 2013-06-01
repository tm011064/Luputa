using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSubstitute;
using Workmate.Web.Components.Application;
using Workmate.Web.Components;

namespace Workmate.Tests.Components.Web
{
  [TestFixture]
  public class Test_ContentFormatter
  {
    [Test]
    public void Test_Format()
    {
      var staticContentLookup = Substitute.For<IStaticContentLookup>();
      var themeFolderLookup = Substitute.For<IThemeFolderLookup>();

      string theme = "test";

      staticContentLookup.GetContent(theme, "string1").Returns<string>("This is ");
      staticContentLookup.GetContent(theme, "string2").Returns<string>("my string");

      themeFolderLookup.GetVirtualThemePath(theme, "url1").Returns<string>("This is");
      themeFolderLookup.GetVirtualThemePath(theme, "url2").Returns<string>(" my image");

      string text = ContentFormatUtility.Format(theme, "start##content(string1)####content(string2)####content(url1)####content(url2)##end"
                , staticContentLookup, themeFolderLookup
                , ContentFormatUtility.FormatMode.None);

      Assert.AreEqual(text, "startThis is my stringThis is my imageend");
    }
  }
}
