using System;

namespace Workmate.Components.Contracts.CMS.Articles
{
  public interface IArticleAttachmentModel
  {
    int AttachmentId { get; set; }
    int ArticleId { get; set; }
    int UserId { get; set; }
    DateTime DateCreatedUtc { get; set; }
    string FileName { get; set; }
    string FriendlyFileName { get; set; }
    string ContentType { get; set; }
    int ContentSize { get; set; }
  }
}
