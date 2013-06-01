using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Entities.CMS
{
  public class CMSContentRating
  {
    protected internal int CMSUserId { get; set; }
    protected internal int CMSContentId { get; set; }
    protected internal short Rating { get; set; }
    protected internal DateTime DateCreatedUtc { get; set; }

    internal CMSContentRating(IUserBasic ratingUser, int ratedContentId, short rating)
    {
      this.CMSUserId = ratingUser.UserId;
      this.CMSContentId = ratedContentId;
      this.Rating = rating;
    }
    internal CMSContentRating(IUserBasic ratingUser, CMSContent ratedContent, short rating)
      : this(ratingUser, ratedContent.CMSContentId, rating)
    {
    }
    public CMSContentRating(int userId, int contentId, short rating, DateTime dateCreatedUtc)
    {
      this.CMSUserId = userId;
      this.CMSContentId = contentId;
      this.Rating = rating;
      this.DateCreatedUtc = dateCreatedUtc;
    }
  }
}
