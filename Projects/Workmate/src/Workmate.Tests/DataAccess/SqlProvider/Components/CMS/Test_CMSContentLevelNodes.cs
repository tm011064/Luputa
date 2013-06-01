using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Workmate.Components.CMS;
using Workmate.Components.Contracts.CMS;
using System.Web;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS
{
  [TestFixture]
  public class Test_CMSContentLevelNodes : TestSetup
  {
    [Test]
    public void Test_CreateUpdateDelete()
    {
      int count = this.Random.Next(3, 10);
      List<string> names = new List<string>();
      List<string> names2 = new List<string>();
      ICMSContentLevelNode topContentLevelNode;
      Dictionary<int, ICMSContentLevelNode> lookup;

      #region create
      for (int i = 0; i < count; i++)
      {
        names.Add("__ &\"'%<tag></tag>TestCat " + i + " " + this.Random.Next(1000, 100000000));
      }

      CMSContentLevelNodeManager manager = new CMSContentLevelNodeManager(this.DataStore);
      int contentLevelNodeId;
      Assert.IsTrue(manager.CreateContentLevelNodes(names, null, null, out contentLevelNodeId));

      lookup = manager.GetContentLevelNodes();

      Assert.IsTrue(lookup.ContainsKey(contentLevelNodeId));

      topContentLevelNode = lookup[contentLevelNodeId];

      int index;
      int lastIndex = 0;
      foreach (string split in topContentLevelNode.BreadCrumbsSplitIndexes.Split(','))
      {
        index = int.Parse(split);
        names2.Add(topContentLevelNode.BreadCrumbs.Substring(lastIndex, index));
        lastIndex += index;
      }

      Assert.AreEqual(names.Count, names2.Count);
      for (int i = 0; i < count; i++)
        Assert.AreEqual(names[i], names2[i]);
      #endregion

      #region update
      List<ICMSContentLevelNode> allNodes;
      ICMSContentLevelNode current;

      allNodes = new List<ICMSContentLevelNode>();
      current = topContentLevelNode;
      while (current.Parent != null)
      {
        allNodes.Insert(0, current);
        current = current.Parent;
      }
      allNodes.Insert(0, current);
      string newName = "some &\"'%: name " + this.Random.Next(100, 1000);
      int changedLevel = this.Random.Next(0, count);
      int renamedContentLevelNodeId = allNodes[changedLevel].ContentLevelNodeId;
      Assert.IsTrue(
        manager.RenameContentLevelNode(
          renamedContentLevelNodeId
          , newName)
      );

      lookup = manager.GetContentLevelNodes();
      Assert.IsTrue(lookup.ContainsKey(contentLevelNodeId));
      topContentLevelNode = lookup[contentLevelNodeId];


      allNodes = new List<ICMSContentLevelNode>();
      current = topContentLevelNode;
      while (current.Parent != null)
      {
        allNodes.Insert(0, current);
        current = current.Parent;
      }
      allNodes.Insert(0, current);

      lastIndex = 0;
      names2 = new List<string>();
      foreach (string split in topContentLevelNode.BreadCrumbsSplitIndexes.Split(','))
      {
        index = int.Parse(split);
        names2.Add(topContentLevelNode.BreadCrumbs.Substring(lastIndex, index));
        lastIndex += index;
      }

      Assert.AreEqual(names.Count, names2.Count);
      for (int i = 0; i < count; i++)
      {
        if (i == changedLevel)
          Assert.AreEqual(HttpUtility.HtmlEncode(newName), names2[i]);
        else
          Assert.AreEqual(names[i], names2[i]);
      }
      #endregion

      #region delete
      Assert.IsTrue(manager.DeleteContentLevelNode(allNodes[0].ContentLevelNodeId));
      lookup = manager.GetContentLevelNodes();
      Assert.IsFalse(lookup.ContainsKey(contentLevelNodeId));
      #endregion
    }
  }
}
