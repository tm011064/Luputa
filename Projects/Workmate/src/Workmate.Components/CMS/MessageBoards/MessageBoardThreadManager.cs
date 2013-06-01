using CommonTools.Components.BusinessTier;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS;
using Workmate.Components.Entities.CMS.MessageBoards;
using Workmate.Data;
namespace Workmate.Components.CMS.MessageBoards
{
  public class MessageBoardThreadManager : BaseCMSManager
	{
		public MessageBoardThread GetMessageBoardThread(int messageBoardThreadId)
		{
			CMSThread thread = _CMSThreadManager.GetThread(CMSSectionType.MessageBoard, messageBoardThreadId);
			if (thread != null)
			{
        return new MessageBoardThread(thread, _CMSSectionManager.GetSection(CMSSectionType.MessageBoard, thread.CMSSectionId));
			}
			return null;
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Create(MessageBoardThread messageBoardThread)
		{
			return _CMSThreadManager.Create(messageBoardThread.CMSThread);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Create(MessageBoardThread messageBoardThread, bool isEnabled)
		{
      return _CMSThreadManager.Create(messageBoardThread.CMSThread);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Update(MessageBoardThread messageBoardThread)
		{
      return _CMSThreadManager.Update(messageBoardThread.CMSThread);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(MessageBoardThread messageBoardThread)
		{
      return _CMSThreadManager.Delete(messageBoardThread.CMSThread, false);
		}

    #region constructors
    internal MessageBoardThreadManager(IDataStore dataStore)
      : base(dataStore)
    {
    }
    #endregion
	}
}
