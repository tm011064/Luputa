using Workmate.Components.Entities.CMS;
using System;
using Workmate.Components.Contracts.CMS.Articles;
using System.Xml.Linq;
using System.Collections.Generic;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Entities.CMS.Articles
{
  public class ArticleModel : BaseArticleInfo, IArticleModel
  {
    public virtual int AuthorUserId { get; set; }
    public virtual ArticleStatus ArticleStatus { get; private set; }
    public virtual string Subject { get; private set; }
    public virtual string FormattedBody { get; private set; }

    public virtual int MessageBoardThreadId { get; private set; }
    public virtual bool IsMessageBoardEnabled { get; private set; }

    public virtual IUserBasic Author { get; set; }
    public virtual List<IArticleAttachmentModel> Attachments { get; set; }

    #region IArticleModel Members

    public int? ContentLevelNodeId { get; private set; }

    public List<string> ContentLevelNodes { get; private set; }

    public List<string> Tags
    {
      get { throw new NotImplementedException(); }
    }

    #endregion

    public ArticleModel(
      int articleId
      , int articleGroupThreadId
      , int articleGroupId
      , string friendlyName
      , DateTime dateCreatedUtc
      , int totalComments
      , XElement extraInfo
      , int authorUserId
      , ArticleStatus articleStatus
      , string subject
      , string formattedBody      
      , int messageBoardThreadId
      , bool isMessageBoardEnabled
      , int? contentLevelNodeId
      , string breadCrumbs
      , string breadCrumbsSplitIndexes
      )
      : base(
        articleId
      , articleGroupThreadId
      , articleGroupId
      , friendlyName
      , dateCreatedUtc
      , totalComments
      , extraInfo)
    {
      this.ArticleStatus = articleStatus;
      this.AuthorUserId = authorUserId;
      this.Subject = subject;
      this.FormattedBody = formattedBody;
      this.MessageBoardThreadId = messageBoardThreadId;
      this.IsMessageBoardEnabled = isMessageBoardEnabled;
      this.Attachments = new List<IArticleAttachmentModel>();

      this.ContentLevelNodeId = contentLevelNodeId;
      this.ContentLevelNodes = new List<string>();
      if (!string.IsNullOrEmpty(breadCrumbs)
          && !string.IsNullOrEmpty(breadCrumbsSplitIndexes))
      {
        int index;
        int lastIndex = 0;
        foreach (string split in breadCrumbsSplitIndexes.Split(','))
        {
          index = int.Parse(split);
          this.ContentLevelNodes.Add(breadCrumbs.Substring(lastIndex, index));
          lastIndex += index;
        }
      }
    }
  }
}
