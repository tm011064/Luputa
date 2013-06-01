using CommonTools.Components.BusinessTier;
using Workmate.Components.Entities.CMS;
using System;
using System.Collections.Generic;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS.Articles;
using Workmate.Data;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Contracts.CMS.Articles;

namespace Workmate.Components.CMS.Articles
{
  public interface IArticleGroupManager
  {
    ArticleGroup GetArticleGroup(int applicationId, string name);
    BusinessObjectActionReport<DataRepositoryActionStatus> Create(ArticleGroup articleGroup);
  }

  public class ArticleGroupManager : BaseCMSManager, IArticleGroupManager
	{
		public ArticleGroup GetArticleGroup(int articleGroupId)
		{
			CMSSection section = _CMSSectionManager.GetSection(CMSSectionType.Article, articleGroupId);
			if (section != null)
			{
				return new ArticleGroup(section);
			}
			return null;
		}
		public ArticleGroup GetArticleGroup(int applicationId, string name)
		{
      CMSSection section = _CMSSectionManager.GetSection(applicationId, CMSSectionType.Article, name);
			if (section != null)
			{
				return new ArticleGroup(section);
			}
			return null;
		}

		public List<ArticleGroup> GetAllArticleGroups(int applicationId)
		{
			List<ArticleGroup> list = new List<ArticleGroup>();
			foreach (CMSSection current in _CMSSectionManager.GetAllSections(applicationId, CMSSectionType.Article))
			{
				list.Add(new ArticleGroup(current));
			}
			return list;
		}
		public Dictionary<int, ArticleGroup> GetAllArticleGroupsDictionary(int applicationId)
		{
			Dictionary<int, ArticleGroup> dictionary = new Dictionary<int, ArticleGroup>();
			foreach (CMSSection current in _CMSSectionManager.GetAllSections(applicationId, CMSSectionType.Article))
			{
				dictionary[current.CMSSectionId] = new ArticleGroup(current);
			}
			return dictionary;
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Create(ArticleGroup articleGroup)
		{
			return _CMSSectionManager.Create(articleGroup.CMSSection);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Update(ArticleGroup articleGroup)
		{
      return _CMSSectionManager.Update(articleGroup.CMSSection);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(ArticleGroup articleGroup)
		{
      return _CMSSectionManager.Delete(articleGroup.CMSSection, true);
		}

    #region constructors
    public ArticleGroupManager(IDataStore dataStore)
      : base(dataStore)
    {
    }
    #endregion
	}
}
