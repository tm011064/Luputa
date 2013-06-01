using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Components.Contracts.CMS.Articles
{
  public interface IArticleAttachment
  {
    int ArticleAttachmentId { get; }
    int ArticleId { get; }
    byte[] Content { get; set; }
    int ContentSize { get; set; }
    string ContentType { get; set; }
    DateTime DateCreatedUtc { get; }
    string FileName { get; set; }
    string FriendlyFileName { get; set; }
    bool IsTemporary { get; set; }
    int UserId { get; }
  }
}
