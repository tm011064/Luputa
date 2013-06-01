using Workmate.Components.Entities.CMS;
using System;
using Workmate.Components.Entities.Membership;
using CommonTools.Components.BusinessTier;
using Workmate.Components.Contracts.CMS.MessageBoards;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Entities.CMS.MessageBoards
{
  public class Message
  {
    private CMSContent _CMSContent;
    internal CMSContent CMSContent { get { return _CMSContent; } }

    public int MessageId
    {
      get { return _CMSContent.CMSContentId; }
      internal set { _CMSContent.CMSContentId = value; }
    }
    public int? ParentMessageId
    {
      get { return _CMSContent.CMSParentContentId; }
      set { _CMSContent.CMSParentContentId = value; }
    }
    public int MessageBoardThreadId
    {
      get { return _CMSContent.CMSThreadId; }
      internal set { _CMSContent.CMSThreadId = value; }
    }
    public short MessageLevel
    {
      get { return _CMSContent.CMSContentLevel; }
      set { _CMSContent.CMSContentLevel = value; }
    }
    public MessageStatus MessageStatus
    {
      get { return (MessageStatus)_CMSContent.CMSContentStatus; }
      internal set { _CMSContent.CMSContentStatus = (byte)value; }
    }
    public int TotalRatings
    {
      get { return _CMSContent.CMSTotalRatings; }
      set { _CMSContent.CMSTotalRatings = value; }
    }
    public int TotalThumbsUp
    {
      get { return _CMSContent.CMSRatingSum; }
    }
    public int TotalThumbsDown
    {
      get { return this.TotalRatings - _CMSContent.CMSRatingSum; }
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

    public MessageBoardThread MessageBoardThread { get; private set; }

    public Message(IUserBasic author, MessageBoardThread messageBoardThread, string subject, string formattedBody, Message parentMessage)
      : this(author, messageBoardThread, subject, formattedBody
      , (parentMessage == null ? null : (int?)parentMessage.MessageId)
      , (short)(parentMessage == null ? 0 : (parentMessage.MessageLevel + 1)))
    { }
    public Message(IUserBasic author, MessageBoardThread messageBoardThread, string subject, string formattedBody)
      : this(author, messageBoardThread, subject, formattedBody, null, 0)
    { }
    public Message(IUserBasic author, MessageBoardThread messageBoardThread, string subject, string formattedBody, int? parentMessageId, short messageLevel)
    {
      this._CMSContent = new CMSContent(author.UserId, messageBoardThread.CMSThread, 0, 0, subject, formattedBody, true);

      this.MessageBoardThread = messageBoardThread;

      this.ParentMessageId = parentMessageId;
      this.MessageLevel = messageLevel;
    }
    internal Message(CMSContent cmsContent, CMSThread cmsThread, CMSSection cmsSection)
    {
      this._CMSContent = cmsContent;

      if (cmsThread != null)
        this.MessageBoardThread = new MessageBoardThread(cmsThread, cmsSection);
    }
  }
}
