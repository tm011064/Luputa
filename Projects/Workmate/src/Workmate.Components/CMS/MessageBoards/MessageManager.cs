using System.Collections.Generic;
using System.Data.SqlClient;
using CommonTools.Components.BusinessTier;
using CommonTools.Extensions;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS;
using Workmate.Components.Entities.CMS.MessageBoards;
using Workmate.Data;

namespace Workmate.Components.CMS.MessageBoards
{
  public class MessageManager : BaseCMSManager
	{
		public Message GetMessage(int messageId)
    {
      CMSContent content = _CMSContentManager.GetContent(messageId);

      Message record = null;
      if (content != null)
      {
        CMSThread thread = null;
        CMSSection section = null;

        thread = _CMSThreadManager.GetThread(CMSSectionType.MessageBoard, content.CMSThreadId);
        section = _CMSSectionManager.GetSection(CMSSectionType.MessageBoard, thread.CMSSectionId);

        record = new Message(content, thread, section);
      }

      return record;
		}
		public List<MessageInfo> GetMessageInfoPage(int messageBoardThreadId, ref int pageIndex, int pageSize, out int rowCount)
		{
      List<MessageInfo> list;
			try
			{
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
				{
          list = dataStoreContext.cms_Contents_GetMessageInfoPageFromThreadIndex(messageBoardThreadId, ref pageIndex, pageSize, out rowCount);
				}
			}
			catch (SqlException ex)
      {
        _Log.Error("Error at cms_Contents_GetMessageInfoPageFromThreadIndex", ex);
        throw new DataStoreException(ex, true);
			}
			return list;
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Create(Message message)
		{
			message.FormattedBody = message.FormattedBody.RemoveScriptTags().RemoveMaliciousTags();
      return _CMSContentManager.Create(message.CMSContent, null, null, false, false, null, null, null);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Update(Message message)
		{
			message.FormattedBody = message.FormattedBody.RemoveScriptTags().RemoveMaliciousTags();
      return _CMSContentManager.Update(message.CMSContent, null, true);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(Message message)
		{
			return _CMSContentManager.Delete(message.CMSContent, false);
		}
		public bool Delete(int messageId)
		{
			return _CMSContentManager.Delete(messageId, false) == 0;
		}

    #region constructors
    internal MessageManager(IDataStore dataStore)
      : base(dataStore)
    {
    }
    #endregion
	}
}
