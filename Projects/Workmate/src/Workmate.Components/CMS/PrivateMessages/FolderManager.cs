using CommonTools.Components.BusinessTier;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS;
using Workmate.Components.Entities.CMS.PrivateMessages;
using Workmate.Data;

namespace Workmate.Components.CMS.PrivateMessages
{
  public class FolderManager : BaseCMSManager
  {
    #region public methods
    public Folder GetFolder(int folderId)
    {
      CMSThread thread = _CMSThreadManager.GetThread(CMSSectionType.PrivateMessageInbox, folderId);
      if (thread != null)
      {
        return new Folder(thread, _CMSSectionManager.GetSection(CMSSectionType.PrivateMessageInbox, thread.CMSSectionId));
      }
      return null;
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Create(Folder folder)
    {
      return _CMSThreadManager.Create(folder.CMSThread);
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Update(Folder folder)
    {
      return _CMSThreadManager.Update(folder.CMSThread);
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(Folder folder)
    {
      return _CMSThreadManager.Delete(folder.CMSThread, false);
    }
    #endregion

    #region constructors
    internal FolderManager(IDataStore dataStore)
      : base(dataStore)
    {
    }
    #endregion
  }
}
