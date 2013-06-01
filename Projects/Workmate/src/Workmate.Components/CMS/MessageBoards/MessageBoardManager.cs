using System.Collections.Generic;
using CommonTools.Components.BusinessTier;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS;
using Workmate.Components.Entities.CMS.MessageBoards;
using Workmate.Data;
namespace Workmate.Components.CMS.MessageBoards
{
  public class MessageBoardManager : BaseCMSManager
	{
    public MessageBoard GetMessageBoard(int applicationId, string name)
		{
			CMSSection section = _CMSSectionManager.GetSection(applicationId, CMSSectionType.MessageBoard, name);
			if (section != null)
			{
				return new MessageBoard(section);
			}
			return null;
		}
		public MessageBoard GetMessageBoard(int messageBoardId)
		{
			CMSSection section = _CMSSectionManager.GetSection(CMSSectionType.MessageBoard, messageBoardId);
			if (section != null)
			{
				return new MessageBoard(section);
			}
			return null;
		}
		internal List<MessageBoard> GetAllMessageBoards()
		{
			List<MessageBoard> list = new List<MessageBoard>();
			foreach (CMSSection current in _CMSSectionManager.GetAllSections())
			{
				list.Add(new MessageBoard(current));
			}
			return list;
		}
		public List<MessageBoard> GetAllMessageBoards(int applicationId)
		{
			List<MessageBoard> list = new List<MessageBoard>();
			foreach (CMSSection current in _CMSSectionManager.GetAllSections(applicationId, CMSSectionType.MessageBoard))
			{
				list.Add(new MessageBoard(current));
			}
			return list;
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Create(MessageBoard messageBoard)
		{
			return _CMSSectionManager.Create(messageBoard.CMSSection);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Update(MessageBoard messageBoard)
		{
      return _CMSSectionManager.Update(messageBoard.CMSSection);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(MessageBoard messageBoard)
		{
      return _CMSSectionManager.Delete(messageBoard.CMSSection, false);
		}

    #region constructors
    internal MessageBoardManager(IDataStore dataStore)
      : base(dataStore)
    {
    }
    #endregion
	}
}
