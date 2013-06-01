using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using CommonTools.Components.Testing;
using System.Collections.Generic;
using CommonTools.Components.Caching;
using System.Web.Caching;
using System.Xml.Serialization;
using System.Globalization;
using CommonTools.Components.Logging;
using System.Xml;
using System.Text;
using DummyDatabase;
using CommonTools.Extensions;
using CommonTools.TestApp.Components;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnCreateUnique_Click(object sender, EventArgs e)
    {
        //DummyDataManager dtm = new DummyDataManager(Server.MapPath("~/DummyData.xml"));
        //DummyUser dummy;
        //Random random = new Random();
        //DateTime now = DateTime.UtcNow;
        //string filename = @"C:\Temp\DummyData\dummy.UniqueUsers." + txtAmount.Text + ".txt";

        //using (StreamWriter sw = new StreamWriter(filename))
        //{
        //    for (int i = 0; i < int.Parse(txtAmount.Text); i++)
        //    {
        //        dummy = dtm.GetDummy();
        //        sw.WriteLine(Guid.NewGuid() + "," + random.Next(0, 6) + "," + (random.NextDouble() * 10).ToString("0.##")
        //            + "," + dummy.Firstname + "," + dummy.Surname + "," + dummy.DateOfBirth.ToString("dd.MM.yyyy")
        //            + "," + dummy.City + "," + random.Next(0, 2));
        //    }
        //}

        //litStatus.Text = "Successfully created file " + filename + " with " + txtAmount.Text + " unique user records.<br/>Execution time: "
        //    + ((TimeSpan)(DateTime.UtcNow - now)).ToString();
    }
    protected void btnCreateIncrementing_Click(object sender, EventArgs e)
    {
        //DummyDataManager dtm = new DummyDataManager(Server.MapPath("~/DummyData.xml"));
        //DummyUser dummy;
        //Random random = new Random();
        //DateTime now = DateTime.UtcNow;
        //string filename = @"C:\Temp\DummyData\dummy.IncrementingUsers." + txtAmount.Text + ".txt";

        //using (StreamWriter sw = new StreamWriter(filename))
        //{
        //    for (int i = 0; i < int.Parse(txtAmount.Text); i++)
        //    {
        //        dummy = dtm.GetDummy();
        //        sw.WriteLine("0," + random.Next(0, 6) + "," + (random.NextDouble() * 10).ToString("0.##")
        //            + "," + dummy.Firstname + "," + dummy.Surname + "," + dummy.DateOfBirth.ToString("dd.MM.yyyy")
        //            + "," + dummy.City + "," + random.Next(0, 2));
        //    }
        //}

        //litStatus.Text = "Successfully created file " + filename + " with " + txtAmount.Text + " incrementing user records.<br/>Execution time: "
        //    + ((TimeSpan)(DateTime.UtcNow - now)).ToString();
    }

    protected void btnPostback_Click(object sender, EventArgs e)
    {

    }

    protected void btnLoadGridview_Click(object sender, EventArgs e)
    {
        //grvUserPage.DataSource = UniqueUserManager.GetUserPage(int.Parse(txtPageIndex.Text), int.Parse(txtPageSize.Text));
        //grvUserPage.DataBind();
    }

    protected void btnWriteXML_Click(object sender, EventArgs e)
    {
        MyCacheController dcc = new MyCacheController()
        {
            Enabled = true,
            Minutes = 15,
            CacheItems = new List<ICacheItem>() { 
                new MyCacheItem() { CacheItemPriority = CacheItemPriority.Normal, CacheKey = "ck1", Suffix = "sf1", Name="n1", IsIterating = false, Enabled=false},
                new MyCacheItem() { CacheItemPriority = CacheItemPriority.Normal, CacheKey = "ck2", Suffix = "sf2", Name="n2", IsIterating = false, Enabled=true},
                new MyCacheItem() { CacheItemPriority = CacheItemPriority.Normal, CacheKey = "ck3", Suffix = "sf3", Name="n3", IsIterating = false, Enabled=true}                
            }
        };

        XElement xCacheSection =
                new XElement("MyCacheSection",
                        new XAttribute("enabled", dcc.Enabled.ToString()),
                        new XAttribute("minutes", dcc.Minutes.ToString()),
                            from c in dcc.CacheItems
                            select new XElement("MyCacheItem",
                                new XAttribute("cacheItemPriority", c.CacheItemPriority.ToString()),
                                new XAttribute("cacheKey", c.CacheKey),
                                new XAttribute("enabled", c.Enabled.ToString()),
                                new XAttribute("isIterating", c.IsIterating.ToString()),
                                new XAttribute("minutes", c.Minutes.ToString()),
                                new XAttribute("name", c.Name),
                                new XAttribute("suffix", c.Suffix)));


        xCacheSection.Save(Server.MapPath("~/test.xml"));



        XElement el = XElement.Load(Server.MapPath("~/test.xml"));

        var cacheItemNodes = (from c in el.Nodes().OfType<XElement>()
                              where c.Name == "MyCacheItem"
                              select c);
        List<ICacheItem> list = new List<ICacheItem>();
        foreach (XElement element in cacheItemNodes)
        {
            list.Add(new MyCacheItem()
            {
                Suffix = (element.Attribute("suffix") == null ? "" : element.Attribute("suffix").Value),
                CacheItemPriority = (element.Attribute("cacheItemPriority") == null ? CacheItemPriority.Normal : (CacheItemPriority)Enum.Parse(typeof(CacheItemPriority), element.Attribute("cacheItemPriority").Value)),
                Enabled = (element.Attribute("enabled") == null ? true : bool.Parse(element.Attribute("enabled").Value)),
                IsIterating = (element.Attribute("isIterating") == null ? false : bool.Parse(element.Attribute("isIterating").Value)),
                Minutes = (element.Attribute("minutes") == null ? -1 : int.Parse(element.Attribute("minutes").Value)),
                Name = (element.Attribute("name") == null ? "" : element.Attribute("name").Value),
                CacheKey = (element.Attribute("cacheKey") == null ? "" : element.Attribute("cacheKey").Value)
            });
        }



        dcc = new MyCacheController()
        {
            Minutes = (el.Attribute("minutes").Value == null ? 15 : int.Parse(el.Attribute("minutes").Value)),
            Enabled = (el.Attribute("enabled").Value == null ? true : bool.Parse(el.Attribute("enabled").Value)),
            CacheItems = list
        };
    }
}
