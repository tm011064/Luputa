using Workmate.Components.Entities.CMS;
using System;
using Workmate.Components.Contracts.CMS;

namespace Workmate.Components.Entities.CMS.Articles
{
  public class ArticleGroup
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

    public int ArticleGroupId
    {
      get { return _CMSSection.CMSSectionId; }
      set { _CMSSection.CMSSectionId = value; }
    }
    public int TotalArticles
    {
      get { return _CMSSection.CMSTotalContents; }
      set { _CMSSection.CMSTotalContents = value; }
    }
    public int TotalArticleGroupThreads
    {
      get { return _CMSSection.CMSTotalThreads; }
      set { _CMSSection.CMSTotalThreads = value; }
    }

    public ArticleGroup(int applicationId, string name, bool isActive)
    {
      _CMSSection = new CMSSection(applicationId, name, isActive, true, CMSSectionType.Article);
    }
    internal ArticleGroup(CMSSection record)
    {
      _CMSSection = record;
    }
  }
}
