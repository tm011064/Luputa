using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Contracts.CMS.Articles
{
  public interface IArticleModel
  {
    ArticleStatus ArticleStatus { get; }
    string Subject { get; }
    string FormattedBody { get; }

    int MessageBoardThreadId { get; }
    bool IsMessageBoardEnabled { get; }

    IUserBasic Author { get; }
    List<IArticleAttachmentModel> Attachments { get; }

    int? ContentLevelNodeId { get; }
    List<string> ContentLevelNodes { get; }
    List<string> Tags { get; }

    int ArticleId { get; }
    int ArticleGroupThreadId { get; }
    int ArticleGroupId { get; }
    string FriendlyName { get; }
    DateTime DateCreatedUtc { get; }
    int TotalComments { get; }
  }
}
