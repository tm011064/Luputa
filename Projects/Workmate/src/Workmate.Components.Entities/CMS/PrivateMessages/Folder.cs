using System;

namespace Workmate.Components.Entities.CMS.PrivateMessages
{
  public class Folder
  {
    private CMSThread _CMSThread;
    internal CMSThread CMSThread { get { return _CMSThread; } }
    
    public int FolderId
    {
      get { return _CMSThread.CMSThreadId; }
      internal set { _CMSThread.CMSThreadId = value; }
    }
    public int InboxId
    {
      get { return _CMSThread.CMSSectionId; }
      internal set { _CMSThread.CMSSectionId = value; }
    }
    public string FolderName
    {
      get { return _CMSThread.CMSName; }
      set { _CMSThread.CMSName = value; }
    }

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

    public Inbox Inbox { get; private set; }

    public Folder(Inbox inbox, string folderName)
    {
      this._CMSThread = new CMSThread(inbox.CMSSection, false, 0);
      this.FolderName = folderName;

      this.Inbox = inbox;
    }
    public Folder(CMSThread cmsThread, CMSSection cmsSection)
    {
      this._CMSThread = cmsThread;

      if (cmsSection != null)
        this.Inbox = new Inbox(cmsSection);
    }
  }
}
