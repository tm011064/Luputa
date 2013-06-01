using Workmate.Components.Entities.CMS;
using System;
using Workmate.Components.Contracts.CMS;
namespace Workmate.Components.Entities.CMS.Content
{
  public class ContentPlaceholder
  {
    private CMSSection _CMSSection;
    internal CMSSection CMSSection { get { return _CMSSection; } }

    #region standard properties
    public string Name
    {
      get { return _CMSSection.Name; }
      set { _CMSSection.Name = value; }
    }
    public string Description
    {
      get { return _CMSSection.Description; }
      set { _CMSSection.Description = value; }
    }
    public bool IsActive
    {
      get { return _CMSSection.IsActive; }
      set { _CMSSection.IsActive = value; }
    }
    public bool IsModerated
    {
      get { return _CMSSection.IsModerated; }
      set { _CMSSection.IsModerated = value; }
    }
    #endregion

    public int ContentPlaceholderId
    {
      get { return _CMSSection.CMSSectionId; }
      private set { _CMSSection.CMSSectionId = value; }
    }
    public int ContentPlaceholderGroupId
    {
      get { return _CMSSection.CMSGroupId.Value; }
      set { _CMSSection.CMSGroupId = value; }
    }

    public ContentPlaceholder(int applicationId, ContentPlaceholderGroup contentPlaceholderGroup, string name)
    {
      _CMSSection = new CMSSection(applicationId, name, true, true, CMSSectionType.Content);

      _CMSSection.CMSGroupId = contentPlaceholderGroup.ContentPlaceholderGroupId;
    }
    internal ContentPlaceholder(CMSSection record)
    {
      _CMSSection = record;
    }
  }
}
