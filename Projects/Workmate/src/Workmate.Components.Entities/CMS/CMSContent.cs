using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Components.BusinessTier;
using System.Xml.Linq;

namespace Workmate.Components.Entities.CMS
{
  public class CMSContent
  {
    protected internal int CMSContentId { get; internal set; }
    protected internal int CMSThreadId { get; internal set; }
    protected internal int? CMSParentContentId { get; internal set; }
    protected internal int? CMSBaseContentId { get; private set; }
    protected internal short CMSContentLevel { get; internal set; }
    protected internal byte CMSContentType { get; set; }
    protected internal int CMSRatingSum { get; set; }
    protected internal int CMSTotalRatings { get; set; }
    protected internal byte CMSContentStatus { get; set; }
    protected internal XElement CMSExtraInfo { get; set; }

    protected internal int AuthorUserId { get; set; }
    [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)]
    protected internal string Subject { get; set; }
    [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)]
    protected internal string FormattedBody { get; set; }

    protected internal DateTime DateCreatedUtc { get; private set; }
    protected internal bool IsApproved { get; set; }
    protected internal bool IsLocked { get; set; }
    protected internal int TotalViews { get; internal set; }
    protected internal string UrlFriendlyName { get; set; }

    protected internal int? ContentLevelNodeId { get; set; }

    internal CMSContent(int authorUserId, CMSThread thread, byte contentStatus, byte contentType, string subject, string formattedBody, bool isApproved)
    {
      this.CMSThreadId = thread.CMSThreadId;
      this.AuthorUserId = authorUserId;
      this.CMSContentStatus = contentStatus;
      this.CMSContentType = contentType;
      this.Subject = subject;
      this.FormattedBody = (formattedBody ?? string.Empty);
      this.IsApproved = this.IsApproved;
      this.CMSContentLevel = 0;
      this.CMSTotalRatings = 0;
      this.CMSRatingSum = 0;
      this.IsLocked = false;

      this.CMSExtraInfo = new XElement("r");
    }
    public CMSContent(int contentId, int threadId, int? parentContentId, int authorUserId, short contentLevel, string subject, string formattedBody, DateTime dateCreatedUtc
      , bool isApproved, bool isLocked, int totalViews, byte contentType, int ratingSum, int totalRatings, byte contentStatus, XElement extraInfo, int? baseContentId
      , string urlFriendlyName, int? contentLevelNodeId)
    {
      this.CMSContentId = contentId;
      this.CMSThreadId = threadId;
      this.CMSParentContentId = parentContentId;
      this.AuthorUserId = authorUserId;
      this.CMSContentLevel = contentLevel;
      this.Subject = subject;
      this.FormattedBody = formattedBody;
      this.DateCreatedUtc = dateCreatedUtc;
      this.IsApproved = isApproved;
      this.IsLocked = isLocked;
      this.TotalViews = totalViews;
      this.CMSContentType = contentType;
      this.CMSRatingSum = ratingSum;
      this.CMSTotalRatings = totalRatings;
      this.CMSContentStatus = contentStatus;
      this.CMSExtraInfo = extraInfo ?? new XElement("r");
      this.CMSBaseContentId = baseContentId;
      this.UrlFriendlyName = urlFriendlyName;
      this.ContentLevelNodeId = contentLevelNodeId;
    }
  }
}
