using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Entities.CMS.Membership
{
  public class ProfileImage : SystemProfileImage
  {
    public int UserId
    {
      get { return _CMSFile.CMSUserId.Value; }
    }

    public bool IsSystemProfileImage
    {
      get { return base._CMSFile.CMSFileType == FileType.SystemProfileImage; }
    }

    public ProfileImage(int applicationId, IUserBasic user) : base(applicationId, user, FileType.ProfileImage) { }
    public ProfileImage(CMSFile cmsFile) : base(cmsFile) { }
  }
}
