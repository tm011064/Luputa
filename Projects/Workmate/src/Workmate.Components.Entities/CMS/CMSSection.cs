using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Components.BusinessTier;
using Workmate.Components.Contracts.CMS;

namespace Workmate.Components.Entities.CMS
{
  public class CMSSection
  {
    protected internal int ApplicationId { get; private set; }
    protected internal int CMSSectionId { get; set; }
    protected internal int? CMSParentSectionId { get; set; }
    protected internal int? CMSGroupId { get; set; }

    [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)]
    protected internal string Name { get; set; }
    [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)]
    protected internal string Description { get; set; }

    protected internal CMSSectionType CMSSectionType { get; set; }
    protected internal bool IsActive { get; set; }
    protected internal bool IsModerated { get; set; }
    protected internal int CMSTotalContents { get; set; }
    protected internal int CMSTotalThreads { get; set; }

    internal CMSSection(int applicationId, string name, bool isActive, bool isModerated, CMSSectionType sectionType)
    {
      this.ApplicationId = applicationId;
      this.IsActive = isActive;
      this.IsModerated = isModerated;
      this.CMSSectionType = sectionType;
      this.Name = name;
      this.CMSTotalContents = 0;
      this.CMSTotalThreads = 0;
    }
    public CMSSection(int applicationId, int sectionId, int? parentSectionId, int? groupId, string name, string description, CMSSectionType sectionType, bool isActive
      , bool isModerated, int totalContents, int totalThreads)
    {
      this.ApplicationId = applicationId;
      this.CMSSectionId = sectionId;
      this.CMSParentSectionId = parentSectionId;
      this.CMSGroupId = groupId;
      this.Name = name;
      this.Description = description;
      this.CMSSectionType = sectionType;
      this.IsActive = isActive;
      this.IsModerated = isModerated;
      this.CMSTotalContents = totalContents;
      this.CMSTotalThreads = totalThreads;
    }
  }
}
