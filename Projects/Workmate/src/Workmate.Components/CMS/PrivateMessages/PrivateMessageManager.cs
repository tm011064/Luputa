using CommonTools.Components.BusinessTier;
using CommonTools.Extensions;
using CommonTools.Web;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS;
using Workmate.Components.Entities.CMS.PrivateMessages;
using Workmate.Data;
namespace Workmate.Components.CMS.PrivateMessages
{
  public class PrivateMessageManager : BaseCMSManager
  {
    #region public methods
    public PrivateMessage GetPrivateMessage(int privateMessageId)
    {
      CMSContent content = _CMSContentManager.GetContent(privateMessageId);

      PrivateMessage record = null;
      if (content != null)
      {
        CMSThread thread = null;
        CMSSection section = null;

        thread = _CMSThreadManager.GetThread(CMSSectionType.PrivateMessageInbox, content.CMSThreadId);
        section = _CMSSectionManager.GetSection(CMSSectionType.PrivateMessageInbox, thread.CMSSectionId);

        record = new PrivateMessage(content, thread, section);
      }

      return record;
    }

    public BusinessObjectActionReport<DataRepositoryActionStatus> Create(PrivateMessage privateMessage)
    {
      privateMessage.FormattedBody = privateMessage.FormattedBody.RemoveScriptTags().RemoveMaliciousTags();
      string empty = string.Empty;

      if (!HtmlValidator.ValidateHtml(privateMessage.FormattedBody, out empty))
      {
        return new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.InvalidHtml);
      }

      return _CMSContentManager.Create(privateMessage.CMSContent, null, null, false, false, null, null, null);
    }

    public BusinessObjectActionReport<DataRepositoryActionStatus> Update(PrivateMessage privateMessage)
    {
      privateMessage.FormattedBody = privateMessage.FormattedBody.RemoveScriptTags().RemoveMaliciousTags();
      string empty = string.Empty;
      if (!HtmlValidator.ValidateHtml(privateMessage.FormattedBody, out empty))
      {
        return new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.InvalidHtml);
      }
      return _CMSContentManager.Update(privateMessage.CMSContent);
    }

    public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(PrivateMessage privateMessage)
    {
      return _CMSContentManager.Delete(privateMessage.CMSContent, false);
    }
    #endregion

    #region constructors
    internal PrivateMessageManager(IDataStore dataStore) : base(dataStore)
    {
    }
    #endregion
  }
}
