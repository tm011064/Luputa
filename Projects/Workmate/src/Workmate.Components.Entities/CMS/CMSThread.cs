using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Components.BusinessTier;

namespace Workmate.Components.Entities.CMS
{
  public class CMSThread
  {
    protected internal int CMSThreadId { get; set; }
    protected internal int CMSSectionId { get; set; }
    [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)]
    protected internal string CMSName { get; set; }
    protected internal DateTime CMSLastViewedDateUtc { get; set; }
    protected internal DateTime? CMSStickyDateUtc { get; set; }
    protected internal int CMSTotalViews { get; set; }
    protected internal int CMSTotalReplies { get; set; }
    protected internal bool CMSIsSticky { get; set; }
    protected internal int CMSRatingSum { get; set; }
    protected internal int CMSTotalRatings { get; set; }
    protected internal int CMSThreadStatus { get; set; }

    protected internal DateTime DateCreatedUtc { get; internal set; }
    protected internal bool IsLocked { get; set; }
    protected internal bool IsApproved { get; set; }

    internal CMSThread(CMSSection section, bool isSticky, int threadStatus)
    {
      this.CMSIsSticky = isSticky;
      this.CMSThreadStatus = threadStatus;
      this.CMSSectionId = section.CMSSectionId;
      this.CMSTotalRatings = 0;
      this.CMSTotalReplies = 0;
      this.CMSTotalViews = 0;
      this.CMSRatingSum = 0;
      this.IsLocked = false;
    }
    public CMSThread(int threadId, int sectionId, string name, DateTime lastViewedDateUtc, DateTime? stickyDateUtc, int totalViews, int totalReplies, bool isLocked, bool isSticky
      , bool isApproved, int ratingSum, int totalRatings, int threadStatus, DateTime dateCreatedUtc)
    {
      this.CMSThreadId = threadId;
      this.CMSSectionId = sectionId;
      this.CMSName = name;
      this.CMSLastViewedDateUtc = lastViewedDateUtc;
      this.CMSStickyDateUtc = stickyDateUtc;
      this.CMSTotalViews = totalViews;
      this.CMSTotalReplies = totalReplies;
      this.IsLocked = isLocked;
      this.CMSIsSticky = isSticky;
      this.IsApproved = isApproved;
      this.CMSRatingSum = ratingSum;
      this.CMSTotalRatings = totalRatings;
      this.CMSThreadStatus = threadStatus;
      this.DateCreatedUtc = dateCreatedUtc;
    }
  }
}
