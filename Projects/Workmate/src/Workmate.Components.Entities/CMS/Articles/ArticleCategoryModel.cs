using Workmate.Components.Entities.CMS;
using System;
using Workmate.Components.Contracts.CMS.Articles;
using System.Xml.Linq;
using System.Collections.Generic;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Entities.CMS.Articles
{
  public class ArticleCategoryModel : IArticleCategoryModel
  {
    public virtual int ContentLevelNodeId { get; set; }
    public virtual string Name { get; set; }
    public virtual int Level { get; set; }    
    public virtual int? ParentContentLevelNodeId { get; private set; }

    public ArticleCategoryModel(int contentLevelNodeId, string name, int level, int? parentContentLevelNodeId)
    {
      this.ContentLevelNodeId = contentLevelNodeId;
      this.Name = name;
      this.Level = level;
      this.ParentContentLevelNodeId = parentContentLevelNodeId;
    }
  }
}
