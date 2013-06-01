using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Components.Contracts.CMS
{
  public enum FileType : byte
  {
    ProfileImage = 1,
    SystemProfileImage = 2,
    PostAttachment = 3,
    DeletedProfileImage = 4,
    ArticleImage = 5,
    ArticleAttachment = 6
  }
}
