using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Entities.CMS
{
  public class CMSContentUser
  {
    protected internal int CMSContentId { get; set; }
    protected internal int CMSReceivingUserId { get; set; }
    protected internal DateTime DateReceivedUtc { get; set; }

    internal CMSContentUser(IUserBasic receivingUser, CMSContent receivedContent)
    {
      this.CMSReceivingUserId = receivingUser.UserId;
      this.CMSContentId = receivedContent.CMSContentId;
    }
    public CMSContentUser(int contentId, int receivingUserId, DateTime dateReceivedUtc)
    {
      this.CMSContentId = contentId;
      this.CMSReceivingUserId = receivingUserId;
      this.DateReceivedUtc = dateReceivedUtc;
    }
  }
}
