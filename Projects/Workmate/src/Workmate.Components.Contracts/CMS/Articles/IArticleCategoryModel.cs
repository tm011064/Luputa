using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Contracts.CMS.Articles
{
  public interface IArticleCategoryModel
  {
    int ContentLevelNodeId { get; set; }
    int Level { get; set; }
    string Name { get; set; }
    int? ParentContentLevelNodeId { get; }
  }
}
