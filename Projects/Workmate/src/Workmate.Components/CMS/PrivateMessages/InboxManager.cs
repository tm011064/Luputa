using CommonTools.Components.BusinessTier;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS;
using Workmate.Components.Entities.CMS.PrivateMessages;
using Workmate.Data;

namespace Workmate.Components.CMS.PrivateMessages
{
  public class InboxManager : BaseCMSManager
  {
    #region public methods
    public Inbox GetInbox(int inboxId)
    {
      CMSSection section = _CMSSectionManager.GetSection(CMSSectionType.PrivateMessageInbox, inboxId);
      if (section != null)
      {
        return new Inbox(section);
      }
      return null;
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Create(Inbox inbox)
    {
      return _CMSSectionManager.Create(inbox.CMSSection);
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Update(Inbox inbox)
    {
      return _CMSSectionManager.Update(inbox.CMSSection);
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(Inbox inbox)
    {
      return _CMSSectionManager.Delete(inbox.CMSSection, false);
    }
    #endregion

    #region constructors
    internal InboxManager(IDataStore dataStore)
      : base(dataStore)
    {
    }
    #endregion
  }
}
