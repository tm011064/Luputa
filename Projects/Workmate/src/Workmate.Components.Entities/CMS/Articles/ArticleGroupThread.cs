using Workmate.Components.Entities.CMS;
using System;
using Workmate.Components.Contracts.CMS.Articles;
namespace Workmate.Components.Entities.CMS.Articles
{
  public class ArticleGroupThread
  {
    private CMSThread _CMSThread;
    internal CMSThread CMSThread { get { return _CMSThread; } }

    #region standard properties
    public DateTime DateCreatedUtc
    {
      get { return _CMSThread.DateCreatedUtc; }
    }
    public bool IsLocked
    {
      get { return _CMSThread.IsLocked; }
      set { _CMSThread.IsLocked = value; }
    }
    public bool IsApproved
    {
      get { return _CMSThread.IsApproved; }
      set { _CMSThread.IsApproved = value; }
    }
    #endregion

    public int ArticleGroupThreadId
    {
      get { return _CMSThread.CMSThreadId; }
      internal set { _CMSThread.CMSThreadId = value; }
    }
    public string Name
    {
      get { return _CMSThread.CMSName; }
      internal set { _CMSThread.CMSName = value; }
    }
    public int ArticleGroupId
    {
      get { return _CMSThread.CMSSectionId; }
      internal set { _CMSThread.CMSSectionId = value; }
    }
    public int TotalViews
    {
      get { return _CMSThread.CMSTotalViews; }
      internal set { _CMSThread.CMSTotalViews = value; }
    }
    public ArticleGroupThreadStatus ArticleGroupThreadStatus
    {
      get { return (ArticleGroupThreadStatus)_CMSThread.CMSThreadStatus; }
      set { _CMSThread.CMSThreadStatus = (int)value; }
    }

    public ArticleGroup ArticleGroup { get; private set; }

    public ArticleGroupThread(ArticleGroup articleGroup, ArticleGroupThreadStatus status, string name)
    {
      this._CMSThread = new CMSThread(articleGroup.CMSSection, false, (byte)status);

      this.Name = name;
    }
    internal ArticleGroupThread(CMSThread cmsThread, CMSSection cmsSection)
    {
      this._CMSThread = cmsThread;

      if (cmsSection != null)
        this.ArticleGroup = new ArticleGroup(cmsSection);
    }
  }
}
