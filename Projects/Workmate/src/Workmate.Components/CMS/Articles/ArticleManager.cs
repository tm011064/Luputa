using CommonTools;
using CommonTools.Components.BusinessTier;
using CommonTools.Extensions;
using CommonTools.Web;
using Workmate.Components.Entities.CMS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Workmate.Data;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS.Articles;
using Workmate.Components.Contracts.CMS.Articles;
using Workmate.Components.Contracts.CMS;

namespace Workmate.Components.CMS.Articles
{
  public class ArticleManager : BaseCMSManager, IArticleManager
  {
    public Article GetArticle(int articleId)
    {
      CMSContent content = _CMSContentManager.GetContent(articleId);

      Article record = null;
      if (content != null)
      {
        CMSThread thread = null;
        CMSSection section = null;

        thread = _CMSThreadManager.GetThread(CMSSectionType.Article, content.CMSThreadId);
        section = _CMSSectionManager.GetSection(CMSSectionType.Article, thread.CMSSectionId);

        // this should only ever be used for testing
        List<string> contentLevelNodeNames = new List<string>();
        if (content.ContentLevelNodeId.HasValue)
        {
          var lookup = _CMSContentLevelNodeManager.GetContentLevelNodes();
          if (lookup.ContainsKey(content.ContentLevelNodeId.Value))
          {
            ICMSContentLevelNode node = lookup[content.ContentLevelNodeId.Value];
            contentLevelNodeNames.Add(node.Name);
            while (node.Parent != null)
            {
              node = node.Parent;
              contentLevelNodeNames.Insert(0, node.Name);
            }
          }
        }
        record = new Article(content, thread, section, _CMSTagManager.GetTagsByContentId(articleId), contentLevelNodeNames);
      }

      return record;
    }
    public Article GetArticle(string friendlyName, string articleGroupName, string articleGroupThreadName)
    {
      CMSContent content = _CMSContentManager.GetContent(friendlyName, articleGroupThreadName, articleGroupName, null);

      Article record = null;
      if (content != null)
      {
        CMSThread thread = null;
        CMSSection section = null;

        thread = _CMSThreadManager.GetThread(CMSSectionType.Article, content.CMSThreadId);
        section = _CMSSectionManager.GetSection(CMSSectionType.Article, thread.CMSSectionId);

        // this should only ever be used for testing
        List<string> contentLevelNodeNames = new List<string>();
        if (content.ContentLevelNodeId.HasValue)
        {
          var lookup = _CMSContentLevelNodeManager.GetContentLevelNodes();
          if (lookup.ContainsKey(content.ContentLevelNodeId.Value))
          {
            ICMSContentLevelNode node = lookup[content.ContentLevelNodeId.Value];
            contentLevelNodeNames.Add(node.Name);
            while (node.Parent != null)
            {
              node = node.Parent;
            contentLevelNodeNames.Insert(0, node.Name);
            }
          }
        }
        record = new Article(content, thread, section, _CMSTagManager.GetTagsByContentId(content.CMSContentId), contentLevelNodeNames);
      }

      return record;
    }
    public List<BaseArticleInfo> GetBaseArticleInfoPage(int? articleGroupId, int? articleGroupThreadId, List<string> tags
      , ref int pageIndex, int pageSize, out int rowCount)
    {
      List<BaseArticleInfo> list;

      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          list = dataStoreContext.cms_Contents_GetBaseArticleInfoPage(articleGroupThreadId, articleGroupId, tags
            , ref pageIndex, pageSize, out rowCount);
        }
      }
      catch (SqlException ex)
      {
        _Log.Error("Error at cms_Contents_GetBaseArticleInfoPage", ex);
        throw new DataStoreException(ex, true);
      }
      return list;
    }
    public IArticleModel GetArticleModel(int articleId)
    {
      IArticleModel articleModel;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          articleModel = dataStoreContext.cms_Contents_GetArticleModel(articleId);
        }
      }
      catch (SqlException ex)
      {
        _Log.Error("Error at cms_Contents_GetArticleModel", ex);
        throw new DataStoreException(ex, true);
      }

      return articleModel;
    }
    
    public void IncreaseTotalViews(Article article, int viewsToAdd)
    {
      this._CMSContentManager.IncreaseTotalViews(article.CMSContent, viewsToAdd);
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Create(Article article, int messageBoardId, bool isMessageBoardEnabled)
    {
      article.FormattedBody = article.FormattedBody.RemoveScriptTags().RemoveMaliciousTags("abbr|acronym|address|applet|area|base|basefont|bdo|big|body|button|caption|center|cite|code|col|colgroup|dd|del|dir|dfn|dl|dt|embed|fieldset|font|form|frame|frameset|head|html|iframe|img|input|ins|isindex|kbd|legend|link|map|menu|meta|noframes|noscript|object|optgroup|option|param|pre|q|s|samp|script|select|small|strike|style|textarea|title|tt|var|xmp");
      string empty = string.Empty;
      if (!HtmlValidator.ValidateHtml(article.FormattedBody, out empty))
      {
        return new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.InvalidHtml);
      }
      return _CMSContentManager.Create(article.CMSContent, article.Tags.ToArray(), article.ContentLevelNodeNames.ToArray()
        , true, true, messageBoardId, isMessageBoardEnabled, LinkedThreadRelationshipType.ArticleMessageBoard);
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Update(Article article)
    {
      article.FormattedBody = article.FormattedBody.RemoveScriptTags().RemoveMaliciousTags("abbr|acronym|address|applet|area|base|basefont|bdo|big|body|button|caption|center|cite|code|col|colgroup|dd|del|dir|dfn|dl|dt|embed|fieldset|font|form|frame|frameset|head|html|iframe|img|input|ins|isindex|kbd|legend|link|map|menu|meta|noframes|noscript|object|optgroup|option|param|pre|q|s|samp|script|select|small|strike|style|textarea|title|tt|var|xmp");
      string empty = string.Empty;
      if (!HtmlValidator.ValidateHtml(article.FormattedBody, out empty))
      {
        return new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.InvalidHtml);
      }
      return _CMSContentManager.Update(article.CMSContent, article.Tags.ToArray());
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(Article article)
    {
      return _CMSContentManager.Delete(article.CMSContent, true);
    }
    
    #region IArticleManager Members

    public IArticleModel GetArticleModel(string friendlyName, string articleGroupName, string articleGroupThreadName)
    {
      IArticleModel articleModel;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          articleModel = dataStoreContext.cms_Contents_GetArticleModel(friendlyName, articleGroupName, articleGroupThreadName);
        }
      }
      catch (SqlException ex)
      {
        _Log.Error("Error at cms_Contents_GetArticleModel", ex);
        throw new DataStoreException(ex, true);
      }

      return articleModel;
    }

    public List<IArticleCategoryModel> GetArticleCategories(int level, int? parentContentLevelNodeId, int? articleGroupThreadId, int? articleGroupId)
    {
      List<IArticleCategoryModel> list = new List<IArticleCategoryModel>();
      foreach (ICMSContentLevelNode item in _CMSContentLevelNodeManager.GetContentLevelNodes(level, parentContentLevelNodeId, articleGroupThreadId, articleGroupId))
        list.Add(new ArticleCategoryModel(item.ContentLevelNodeId, item.Name, item.Level, item.ParentContentLevelNodeId));
      return list;
    }

    #endregion

    #region constructors
    public ArticleManager(IDataStore dataStore)
      : base(dataStore)
    {
    }
    #endregion
  }
}
