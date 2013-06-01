using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Components.BusinessTier;
using Workmate.Components.Contracts.CMS;

namespace Workmate.Components.Entities.CMS
{
  public class CMSGroup
  {
    protected internal int CMSGroupId { get; set; }
    protected internal CMSGroupType CMSGroupType { get; set; }
   
    [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)]
    protected internal string Name { get; set; }
    [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)]
    protected internal string Description { get; set; }

    private CMSGroup()
    {
    }
    internal CMSGroup(int groupId, string name, CMSGroupType cmsGroupType)
    {
      this.CMSGroupId = groupId;
      this.Name = name;
      this.CMSGroupType = cmsGroupType;
    }
    public CMSGroup(int groupId, string name, string description, CMSGroupType groupType)
    {
      this.CMSGroupId = groupId;
      this.Name = name;
      this.Description = description;
      this.CMSGroupType = groupType;
    }
  }
}
