using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Components.Entities.CMS
{
  public class BaseRatingInfo
  {
    public int RatingSum    {      get;      private set;    }
    public int TotalRatings   {      get;      private set;    }
    public int GetTotalThumbsUp()
    {
      return this.RatingSum;
    }
    public int GetTotalThumbsDown()
    {
      return this.TotalRatings - this.RatingSum;
    }
    public decimal? GetAverageRating()
    {
      if (this.TotalRatings == 0)
      {
        return null;
      }
      return (decimal)this.RatingSum / (decimal)this.TotalRatings;
    }

    public BaseRatingInfo(int ratingSum, int totalRatings)
    {
      this.RatingSum = ratingSum;
      this.TotalRatings = totalRatings;
    }
  }
}
