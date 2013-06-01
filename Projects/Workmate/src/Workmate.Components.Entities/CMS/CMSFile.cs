using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Components.BusinessTier;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Entities.CMS
{
  public class CMSFile
  {
    protected internal int ApplicationId { get; set; }
    protected internal int CMSFileId { get; set; }
    protected internal int? CMSUserId { get; set; }
    protected internal FileType CMSFileType { get; set; }
    protected internal DateTime DateCreatedUtc { get; set; }

    [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)]
    protected internal string FileName { get; set; }
    protected internal byte[] Content { get; set; }
    [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)]
    protected internal string ContentType { get; set; }

    protected internal int ContentSize { get; set; }
    [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)]
    protected internal string FriendlyFileName { get; set; }
    protected internal int CMSHeight { get; set; }
    protected internal int CMSWidth { get; set; }
    protected internal int? ContentId { get; set; }

    protected internal bool IsTemporary { get; internal set; }

    internal CMSFile(int applicationId, IUserBasic fileOwner, FileType fileType)
    {
      this.CMSUserId = fileOwner == null ? null : (int?)fileOwner.UserId;
      this.CMSFileType = fileType;
      this.ApplicationId = applicationId;

      this.IsTemporary = false;
    }
    internal CMSFile(int applicationId, int fileId, FileType fileType)
    {
      this.CMSFileId = fileId;
      this.CMSFileType = fileType;
      this.ApplicationId = applicationId;

      this.IsTemporary = false;
    }
    public CMSFile(int applicationId, int fileId, int? userId, FileType fileType, DateTime dateCreatedUtc, string fileName, byte[] content, string contentType, int contentSize, string friendlyFileName
      , int height, int width, int? contentId, bool isTemporary)
    {
      this.ApplicationId = applicationId;
      this.CMSFileId = fileId;
      this.CMSUserId = userId;
      this.CMSFileType = fileType;
      this.DateCreatedUtc = dateCreatedUtc;
      this.FileName = fileName;
      this.Content = content;
      this.ContentType = contentType;
      this.ContentSize = contentSize;
      this.FriendlyFileName = friendlyFileName;
      this.CMSHeight = height;
      this.CMSWidth = width;
      this.ContentId = contentId;

      this.IsTemporary = isTemporary;
    }
  }
}
