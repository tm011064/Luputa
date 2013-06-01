using System;
using CommonTools.Components.BusinessTier;
using Workmate.Components.Contracts.CMS.PrivateMessages;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Entities.CMS.PrivateMessages
{
  public class PrivateMessage
  {
    private CMSContent _CMSContent;
    internal CMSContent CMSContent { get { return _CMSContent; } }

    public int PrivateMessageId
    {
      get { return this._CMSContent.CMSContentId; }
      internal set { this._CMSContent.CMSContentId = value; }
    }
    public int FolderId
    {
      get { return this._CMSContent.CMSThreadId; }
      internal set { this._CMSContent.CMSThreadId = value; }
    }
    public int? ParentMessageId
    {
      get { return this._CMSContent.CMSParentContentId; }
      private set { this._CMSContent.CMSParentContentId = value; }
    }
    public short MessageLevel
    {
      get { return this._CMSContent.CMSContentLevel; }
      private set { this._CMSContent.CMSContentLevel = value; }
    }
    public MessageType MessageType
    {
      get { return (MessageType)this._CMSContent.CMSContentType; }
      set { this._CMSContent.CMSContentType = (byte)value; }
    }
    public MessageStatus MessageStatus
    {
      get { return (MessageStatus)this._CMSContent.CMSContentStatus; }
      set { this._CMSContent.CMSContentStatus = (byte)value; }
    }
    public int TotalRatings
    {
      get { return this._CMSContent.CMSTotalRatings; }
      set { this._CMSContent.CMSTotalRatings = value; }
    }
    public int TotalThumbsUp
    {
      get { return this._CMSContent.CMSRatingSum; }
    }
    public int TotalThumbsDown
    {
      get { return this.TotalRatings - this._CMSContent.CMSRatingSum; }
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

    public Folder Folder { get; private set; }

    public PrivateMessage(IUserBasic author, Folder folder, MessageStatus messageStatus, MessageType messageType, string subject, string formattedBody)
    {
      this._CMSContent = new CMSContent(
        author.UserId
        , folder.CMSThread
        , (byte)messageStatus
        , (byte)messageType
        , subject
        , formattedBody
        , true);

      this.Folder = folder;
    }
    public PrivateMessage(PrivateMessage parentMessage, UserBasic author, Folder folder, MessageStatus messageStatus, MessageType messageType, string subject, string formattedBody)
    {
      this._CMSContent = new CMSContent(
        author.UserId
        , folder.CMSThread
        , (byte)messageStatus
        , (byte)messageType
        , subject
        , formattedBody
        , true);

      this.ParentMessageId = parentMessage.PrivateMessageId;
      this.MessageLevel = parentMessage.MessageLevel;

      this.Folder = folder;
    }

    internal PrivateMessage(CMSContent cmsContent, CMSThread cmsThread, CMSSection cmsSection)
    {
      this._CMSContent = cmsContent;

      if (cmsThread != null)
        this.Folder = new Folder(cmsThread, cmsSection);
    }
  }
}
