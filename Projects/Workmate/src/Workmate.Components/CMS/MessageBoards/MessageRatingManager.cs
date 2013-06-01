using CommonTools.Components.BusinessTier;
using Workmate.Components.Entities.CMS;
using System;
using System.Collections.Generic;
using Workmate.Components.Entities.CMS.MessageBoards;
using Workmate.Components.Entities.Membership;
using log4net;
using Workmate.Data;
using Workmate.Components.Contracts;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Contracts.Membership;
namespace Workmate.Components.CMS.MessageBoards
{
	public class MessageRatingManager
  {
    #region members
    protected CMSContentRatingManager _CMSContentRatingManager;
    protected IDataStore _DataStore;

    protected ILog _Log = LogManager.GetLogger("MessageRatingManager");
    #endregion

		public MessageRating GetMessageRating(Message message, IUserBasic userBasic)
		{
			CMSContentRating contentRating = _CMSContentRatingManager.GetContentRating(message.CMSContent, userBasic);
			if (contentRating != null)
			{
				return new MessageRating(contentRating);
			}
			return null;
		}
		public List<MessageRating> GetAllRatingsForMessage(Message message)
		{
			List<MessageRating> list = new List<MessageRating>();
			foreach (CMSContentRating current in _CMSContentRatingManager.GetRatingsForContent(message.CMSContent))
			{
				list.Add(new MessageRating(current));
			}
			return list;
		}
		public BusinessObjectActionReport<RatingDataRepositoryActionStatus> Create(MessageRating messageRating, out BaseRatingInfo baseRatingInfo)
		{
			return this.CreateOrUpdateExisting(messageRating, out baseRatingInfo);
		}
		public BusinessObjectActionReport<RatingDataRepositoryActionStatus> CreateOrUpdateExisting(MessageRating messageRating, out BaseRatingInfo baseRatingInfo)
		{
			return _CMSContentRatingManager.Create(messageRating.CMSContentRating, true, false, out baseRatingInfo);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Update(MessageRating messageRating)
		{
			return _CMSContentRatingManager.Update(messageRating.CMSContentRating);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(MessageRating messageRating)
		{
			return _CMSContentRatingManager.Delete(messageRating.CMSContentRating);
		}
    

    #region constructors
    public MessageRatingManager(IDataStore dataStore)
    {
      _DataStore = dataStore;

      _CMSContentRatingManager = new CMSContentRatingManager(dataStore);
    }
    #endregion
	}
}
