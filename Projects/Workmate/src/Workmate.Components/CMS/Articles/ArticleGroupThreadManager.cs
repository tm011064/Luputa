using CommonTools.Components.BusinessTier;
using Workmate.Components.Entities.CMS;
using System;
using System.Collections.Generic;
using Workmate.Data;
using Workmate.Components.Entities.CMS.Articles;
using Workmate.Components.Contracts;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Contracts;
using Workmate.Components.Contracts.CMS.Articles;

namespace Workmate.Components.CMS.Articles
{
  public interface IArticleGroupThreadManager
  {
    ArticleGroupThread GetArticleGroupThread(string name);
    BusinessObjectActionReport<DataRepositoryActionStatus> Create(ArticleGroupThread articleGroupThread);
  }

  public class ArticleGroupThreadManager : BaseCMSManager, IArticleGroupThreadManager
	{
		public ArticleGroupThread GetArticleGroupThread(int articleGroupThreadId)
		{
			CMSThread thread = _CMSThreadManager.GetThread(CMSSectionType.Article, articleGroupThreadId);
			if (thread != null)
			{
        return new ArticleGroupThread(thread, _CMSSectionManager.GetSection(CMSSectionType.Article, thread.CMSSectionId));
			}
			return null;
		}
		public ArticleGroupThread GetArticleGroupThread(string name)
		{
			CMSThread thread = _CMSThreadManager.GetThread(CMSSectionType.Article, name);
			if (thread != null)
			{
        return new ArticleGroupThread(thread, _CMSSectionManager.GetSection(CMSSectionType.Article, thread.CMSSectionId));
			}
			return null;
		}
    //public Dictionary<int, ArticleGroupThread> GetAllArticleGroupThreadsDictionary(int applicationId)
    //{
    //  Dictionary<int, ArticleGroupThread> dictionary = new Dictionary<int, ArticleGroupThread>();
    //  foreach (CMSThread current in _CMSThreadManager.GetAllThreads(applicationId, CMSSectionType.Article))
    //  {
    //    dictionary[current.CMSThreadId] = new ArticleGroupThread(current);
    //  }
    //  return dictionary;
    //}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Create(ArticleGroupThread articleGroupThread)
		{
			return _CMSThreadManager.Create(articleGroupThread.CMSThread);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Update(ArticleGroupThread articleGroupThread)
		{
      return _CMSThreadManager.Update(articleGroupThread.CMSThread);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(ArticleGroupThread articleGroupThread)
		{
      return _CMSThreadManager.Delete(articleGroupThread.CMSThread, true);
		}

    #region constructors
    public ArticleGroupThreadManager(IDataStore dataStore)
      : base(dataStore)
    {
    }
    #endregion
	}
}
