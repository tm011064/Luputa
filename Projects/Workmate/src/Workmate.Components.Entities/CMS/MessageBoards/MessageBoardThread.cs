using Workmate.Components.Entities.CMS;
using System;
namespace Workmate.Components.Entities.CMS.MessageBoards
{
  public class MessageBoardThread
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

    public int MessageBoardThreadId
    {
      get { return _CMSThread.CMSThreadId; }
      internal set { _CMSThread.CMSThreadId = value; }
    }

    public int MessageBoardId
    {
      get { return _CMSThread.CMSSectionId; }
      internal set { _CMSThread.CMSSectionId = value; }
    }

    public MessageBoard MessageBoard { get; private set; }

    public MessageBoardThread(MessageBoard messageBoard)
    {
      this._CMSThread = new CMSThread(messageBoard.CMSSection, false, 0);
    }
    internal MessageBoardThread(CMSThread cmsThread, CMSSection cmsSection)
    {
      this._CMSThread = cmsThread;

      if (cmsSection != null)
        this.MessageBoard = new MessageBoard(cmsSection);
    }
  }
}
