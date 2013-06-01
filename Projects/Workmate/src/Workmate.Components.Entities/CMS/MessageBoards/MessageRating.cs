using Workmate.Components.Entities.CMS;
using System;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Contracts.CMS.MessageBoards;
using Workmate.Components.Contracts.Membership;
namespace Workmate.Components.Entities.CMS.MessageBoards
{
  public class MessageRating
  {
    private CMSContentRating _CMSContentRating;
    internal CMSContentRating CMSContentRating { get { return _CMSContentRating; } }

    public int RatingUserId
    {
      get { return _CMSContentRating.CMSUserId; }
      internal set { _CMSContentRating.CMSUserId = value; }
    }
    public int RatedMessageId
    {
      get { return _CMSContentRating.CMSContentId; }
      internal set { _CMSContentRating.CMSContentId = value; }
    }

    public MessageRatingType MessageRatingType
    {
      get { return this._CMSContentRating.Rating == 1 ? MessageRatingType.ThumbsUp : MessageRatingType.ThumbsDown; }
      set { this._CMSContentRating.Rating = (short)value; }
    }

    public Message RatedMessage { get; private set; }
    public UserBasic RatingUser { get; private set; }

    public MessageRating(IUserBasic ratingUser, int messageId, MessageRatingType messageRatingType)
    {
      this._CMSContentRating = new CMSContentRating(ratingUser, messageId, (short)(messageRatingType));
    }
    public MessageRating(IUserBasic ratingUser, Message ratedMessage, MessageRatingType messageRatingType)
    {
      this._CMSContentRating = new CMSContentRating(ratingUser, ratedMessage.CMSContent, (short)(messageRatingType));
    }
    internal MessageRating(CMSContentRating cmsContentRating)
    {
      this._CMSContentRating = cmsContentRating;
    }
  }
}
