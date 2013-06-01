using Workmate.Components.Entities.CMS;
using System;
using Workmate.Components.Contracts.CMS.MessageBoards;
namespace Workmate.Components.Entities.CMS.MessageBoards
{
  public class MessageInfo
  {
    public virtual int AuthorUserId { get; private set; }
    public virtual int MessageId { get; private set; }
    public virtual DateTime DateCreatedUtc { get; private set; }
    public virtual MessageStatus MessageStatus { get; private set; }
    public virtual string Subject { get; private set; }
    public virtual string FormattedBody { get; private set; }
    public virtual int TotalThumbsUp { get; private set; }
    public virtual int TotalThumbsDown { get; private set; }
    public virtual int? ParentMessageId { get; private set; }
    public virtual short MessageLevel { get; private set; }

    public MessageInfo(
      int authorUserId
      , int messageId
      , DateTime dateCreatedUtc
      , MessageStatus messageStatus
      , string subject
      , string formattedBody
      , int ratingSum
      , int totalRatings
      , int? parentMessageId
      , short messageLevel
      )
    {
      this.AuthorUserId = authorUserId;
      this.MessageId = messageId;
      this.MessageStatus = messageStatus;
      this.DateCreatedUtc = dateCreatedUtc;
      this.FormattedBody = formattedBody;
      this.Subject = subject;
      this.TotalThumbsUp = ratingSum;
      this.TotalThumbsDown = totalRatings - ratingSum;
      this.ParentMessageId = parentMessageId;
      this.MessageLevel = messageLevel;
    }
  }
}
