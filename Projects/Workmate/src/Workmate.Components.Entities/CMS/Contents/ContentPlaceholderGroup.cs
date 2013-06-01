using Workmate.Components.Entities.CMS;
using System;
using Workmate.Components.Contracts.CMS;

namespace Workmate.Components.Entities.CMS.Content
{
  public class ContentPlaceholderGroup
  {
    private CMSGroup _CMSGroup;
    internal CMSGroup CMSGroup { get { return _CMSGroup; } }

    public int ContentPlaceholderGroupId
    {
      get { return _CMSGroup.CMSGroupId; }
      set { _CMSGroup.CMSGroupId = value; }
    }

    public ContentPlaceholderGroup(int groupId, string name)
    {
      this._CMSGroup = new CMSGroup(groupId, name, CMSGroupType.Content);
    }
    internal ContentPlaceholderGroup(CMSGroup cmsGroup)
    {
      this._CMSGroup = cmsGroup;
    }
  }
}
