using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Entities.CMS
{
  public class CMSThreadRating
  {
    protected internal int CMSUserId { get; set; }
    protected internal int CMSThreadId { get; set; }
    protected internal short Rating { get; set; }
    protected internal DateTime DateCreatedUtc { get; set; }

    internal CMSThreadRating(IUserBasic ratingUser, int ratedThreadId, short rating)
    {
      this.CMSUserId = ratingUser.UserId;
      this.CMSThreadId = ratedThreadId;
      this.Rating = rating;
    }
    internal CMSThreadRating(IUserBasic ratingUser, CMSThread ratedThread, short rating)
      : this(ratingUser, ratedThread.CMSThreadId, rating)
    {
    }
    public CMSThreadRating(int userId, int threadId, short rating, DateTime dateCreatedUtc)
    {
      this.CMSUserId = userId;
      this.CMSThreadId = threadId;
      this.Rating = rating;
      this.DateCreatedUtc = dateCreatedUtc;
    }
  }
}
