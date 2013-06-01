using Workmate.Components.Entities.CMS;
using System;
namespace Workmate.Components.Entities.CMS.Content
{
  public class ContentPlaceholderHistory
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

    public int ContentPlaceholderHistoryId
    {
      get { return _CMSThread.CMSThreadId; }
      internal set { _CMSThread.CMSThreadId = value; }
    }
    public int ContentPlaceholderId
    {
      get { return _CMSThread.CMSSectionId; }
      internal set { _CMSThread.CMSSectionId = value; }
    }

    public ContentPlaceholder ContentPlaceholder { get; private set; }

    public ContentPlaceholderHistory(ContentPlaceholder contentPlaceholder)
    {
      this._CMSThread = new CMSThread(contentPlaceholder.CMSSection, false, 0);
    }
    internal ContentPlaceholderHistory(CMSThread cmsThread, CMSSection cmsSection)
    {
      this._CMSThread = cmsThread;

      if (cmsSection != null)
        this.ContentPlaceholder = new ContentPlaceholder(cmsSection);
    }
  }
}
