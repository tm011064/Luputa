using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Contracts.CMS;

namespace Workmate.Components.Entities.CMS
{
  public class CMSContentLevelNode : ICMSContentLevelNode
  {
    #region properties

    public int ContentLevelNodeId { get; set; }

    public string Name { get; set; }

    public int Level { get; set; }

    public string BreadCrumbs { get; set; }

    public string BreadCrumbsSplitIndexes { get; private set; }

    public int? ParentContentLevelNodeId { get; private set; }
    public int? ThreadId { get; private set; }
    public int? SectionId { get; private set; }

    public ICMSContentLevelNode Parent { get; set; }
    public List<ICMSContentLevelNode> Children { get; set; }

    #endregion

    #region constructors
    public CMSContentLevelNode(int contentLevelNodeId, string name, int level, int? parentContentLevelNodeId
      , string breadCrumbs, string breadCrumbsSplitIndexes, int? threadId, int? sectionId)
    {
      this.ContentLevelNodeId = contentLevelNodeId;
      this.Name = name;
      this.Level = level;
      this.ParentContentLevelNodeId = parentContentLevelNodeId;
      this.SectionId = sectionId;
      this.ThreadId = threadId;
      this.BreadCrumbs = breadCrumbs;
      this.BreadCrumbsSplitIndexes = breadCrumbsSplitIndexes;

      this.Children = new List<ICMSContentLevelNode>();
    }
    #endregion
  }
}
