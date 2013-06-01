using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Components.Contracts.CMS
{
  public interface ICMSContentLevelNode
  {
    int ContentLevelNodeId { get; }
    int Level { get; }
    int? ThreadId { get; }
    int? SectionId { get; }
    string Name { get; }

    string BreadCrumbs { get; }
    string BreadCrumbsSplitIndexes { get; }

    ICMSContentLevelNode Parent { get; set; }
    List<ICMSContentLevelNode> Children { get; }

    int? ParentContentLevelNodeId { get; }
  }
}
