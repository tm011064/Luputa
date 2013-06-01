using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Contracts.CMS.Articles;

namespace Workmate.Components.Entities.CMS.Articles
{
  public class ArticleAttachmentModel : IArticleAttachmentModel
  {
    public int AttachmentId { get; set; }
    public int ArticleId { get; set; }
    public int UserId { get; set; }
    public DateTime DateCreatedUtc { get; set; }
    public string FileName { get; set; }
    public string FriendlyFileName { get; set; }
    public string ContentType { get; set; }
    public int ContentSize { get; set; }
    
    public ArticleAttachmentModel(int attachmentId, int articleId, int userId, DateTime dateCreatedUtc, string fileName
      , string friendlyFileName, string contentType, int contentSize)
    {
      this.ArticleId = articleId;
      this.AttachmentId = attachmentId;
      this.ContentSize = contentSize;
      this.ContentType = contentType;
      this.DateCreatedUtc = dateCreatedUtc;
      this.FileName = fileName;
      this.FriendlyFileName = friendlyFileName;
      this.UserId = userId;
    }
  }
}
