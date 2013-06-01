Author: Roman Majewski

Description:
The TextResources component acts as an interface to text content stored at xml files. It expects xml tags of the
format <MyResourceTag MyResourceKey="resourceKey">This is some content</MyResourceTag> and
offers methods to easily access these keys. 

This component supports localizeable resource content with a gracefull fallback mechanism in case a 
certain resource is not present at all languages. To achieve this, you have to store your resource file
in the following folder structure: 

        { RootFolder }\{ culture }\{ file path },
        
(e.g.:  C:\MyProject\TextResources\en-gb\Alerts\MyAlerts.xml, 
        C:\MyProject\TextResources\de-at\Alerts\MyAlerts.xml,
        C:\MyProject\TextResources\en-gb\CommonContent.xml ...)
        
When a resourcekey can't be found at a specified culture, the resourcekey will be looked up at the 
specified 'DefaultCulture'.

The component comes with a handy literl web control to easily integrate text resources into aspx
files. E.g.:

Xml Resource file (C:\test\en-gb\Resources.TwoCultures.xml):
<?xml version="1.0" encoding="utf-8" ?>
<Resources>
  <resource key="resource1">First resource</resource>
  <resource key="resource2">Second resource</resource>
  <resource key="resource3">Third resource</resource>  
</Resources>

Code behind:
public class WebResourceManager : TextResourceManager
{
    protected override string GetResourceNotFoundString(string key)
    {
            return "<strong>Resource " + key + " not found</strong>";
    }

    public WebResourceManager()
        : base("tr_Resources.TwoCultures.xml"
                , @"C:\test\"
                , "Resources.TwoCultures.xml"
                , "en-gb"
                , "resource"
                , "key")
    { }
}

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

WebPage:
<ct:CommonToolsTextResource id="ctr" runat="server" ResourceKey="resource1" />
 