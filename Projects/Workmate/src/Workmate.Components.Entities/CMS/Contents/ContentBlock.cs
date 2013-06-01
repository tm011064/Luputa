using Workmate.Components.Entities.CMS;
using System;
using Workmate.Components.Entities.Membership;
using CommonTools.Components.BusinessTier;
using Workmate.Components.Contracts.CMS.Content;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Entities.CMS.Content
{
  public class ContentBlock
  {
    private CMSContent _CMSContent;
    internal CMSContent CMSContent { get { return _CMSContent; } }

    public int ContentBlockId
    {
      get { return _CMSContent.CMSContentId; }
      internal set { _CMSContent.CMSContentId = value; }
    }
    public int ContentPlaceholderHistoryId
    {
      get { return _CMSContent.CMSThreadId; }
      internal set { _CMSContent.CMSThreadId = value; }
    }

    public ContentBlockStatus ContentBlockStatus
    {
      get { return (ContentBlockStatus)_CMSContent.CMSContentStatus; }
      set { _CMSContent.CMSContentStatus = (byte)value; }
    }    

    #region standard properties
    public int AuthorUserId
    {
      get { return _CMSContent.AuthorUserId; }
      set { _CMSContent.AuthorUserId = value; }
    }
    [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)]
    public string Subject
    {
      get { return _CMSContent.Subject; }
      set { _CMSContent.Subject = value; }
    }
    [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)]
    public string FormattedBody
    {
      get { return _CMSContent.FormattedBody; }
      set { _CMSContent.FormattedBody = value; }
    }

    public DateTime DateCreatedUtc
    {
      get { return _CMSContent.DateCreatedUtc; }
    }
    public bool IsApproved
    {
      get { return _CMSContent.IsApproved; }
      set { _CMSContent.IsApproved = value; }
    }
    public bool IsLocked
    {
      get { return _CMSContent.IsLocked; }
      set { _CMSContent.IsLocked = value; }
    }
    public int TotalViews
    {
      get { return _CMSContent.TotalViews; }
      internal set { _CMSContent.TotalViews = value; }
    }
    public string UrlFriendlyName
    {
      get { return _CMSContent.UrlFriendlyName; }
      set { _CMSContent.UrlFriendlyName = value; }
    }
    #endregion

    public ContentPlaceholderHistory ContentPlaceholderHistory { get; private set; }

    public ContentBlock(IUserBasic author, ContentPlaceholderHistory contentPlaceholderHistory, string formattedBody, ContentBlockStatus contentBlockStatus)
    {
      this._CMSContent = new CMSContent(author.UserId, contentPlaceholderHistory.CMSThread, (byte)contentBlockStatus, 0, string.Empty, formattedBody, true);

      this.ContentPlaceholderHistory = contentPlaceholderHistory;
    }
    internal ContentBlock(CMSContent cmsContent, CMSThread cmsThread, CMSSection cmsSection)
    {
      this._CMSContent = cmsContent;

      if (cmsThread != null)
        this.ContentPlaceholderHistory = new ContentPlaceholderHistory(cmsThread, cmsSection);
    }
  }
}
