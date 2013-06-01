using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Components.Contracts.CMS.Articles
{
  public interface IArticleManager
  {
    IArticleModel GetArticleModel(string friendlyName, string articleGroupName, string articleGroupThreadName);
    List<IArticleCategoryModel> GetArticleCategories(int level, int? parentContentLevelNodeId, int? articleGroupThreadId, int? articleGroupId);
  }
}
