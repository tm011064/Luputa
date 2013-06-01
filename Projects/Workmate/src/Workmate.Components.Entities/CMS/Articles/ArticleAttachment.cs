using System;
using Workmate.Components.Contracts.CMS.Articles;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Entities.CMS.Articles
{
  public class ArticleAttachment : IArticleAttachment
  {
    private CMSFile _CMSFile;
    internal CMSFile CMSFile { get { return _CMSFile; } }

    public int ApplicationId
    {
      get { return _CMSFile.ApplicationId; }
      private set { _CMSFile.ApplicationId = value; }
    }
    public int ArticleId
    {
      get { return _CMSFile.ContentId.Value; }
      private set { _CMSFile.ContentId = value; }
    }
    public int ArticleAttachmentId
    {
      get { return _CMSFile.CMSFileId; }
      private set { _CMSFile.CMSFileId = value; }
    }
    public int UserId
    {
      get { return _CMSFile.CMSUserId.Value; }
      private set { _CMSFile.CMSUserId = value; }
    }
    public DateTime DateCreatedUtc
    {
      get { return _CMSFile.DateCreatedUtc; }
      private set { _CMSFile.DateCreatedUtc = value; }
    }
    public string FileName
    {
      get { return _CMSFile.FileName; }
      set { _CMSFile.FileName = value; }
    }
    public string FriendlyFileName
    {
      get { return _CMSFile.FriendlyFileName; }
      set { _CMSFile.FriendlyFileName = value; }
    }
    public byte[] Content
    {
      get { return _CMSFile.Content; }
      set { _CMSFile.Content = value; }
    }
    public string ContentType
    {
      get { return _CMSFile.ContentType; }
      set { _CMSFile.ContentType = value; }
    }
    public int ContentSize
    {
      get { return _CMSFile.ContentSize; }
      set { _CMSFile.ContentSize = value; }
    }
    public bool IsTemporary
    {
      get { return _CMSFile.IsTemporary; }
      set { _CMSFile.IsTemporary = value; }
    }

    public ArticleAttachment(int applicationId, IUserBasic user)
    {
      this._CMSFile = new CMSFile(applicationId, user, Contracts.CMS.FileType.ArticleAttachment);

      this.IsTemporary = true;
    }
    internal ArticleAttachment(CMSFile cmsFile)
    {
      this._CMSFile = cmsFile;
    }
  }
}
