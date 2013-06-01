using CommonTools.Components.BusinessTier;
using Workmate.Components.Entities.CMS;
using System;
using System.Collections.Generic;
using Workmate.Data;
using Workmate.Components.Entities.CMS.Content;
using Workmate.Components.Contracts;
using Workmate.Components.Contracts.CMS;
namespace Workmate.Components.CMS.Content
{
  public class ContentPlaceholderHistoryManager : BaseCMSManager
	{
		public ContentPlaceholderHistory GetContentPlaceholderHistory(int contentPlaceholderHistoryId)
		{
			CMSThread thread = _CMSThreadManager.GetThread(CMSSectionType.Content, contentPlaceholderHistoryId);
			if (thread != null)
			{
        return new ContentPlaceholderHistory(thread, _CMSSectionManager.GetSection(CMSSectionType.Content, thread.CMSSectionId));
			}
			return null;
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Create(ContentPlaceholderHistory contentPlaceholderHistory)
		{
			return _CMSThreadManager.Create(contentPlaceholderHistory.CMSThread);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Update(ContentPlaceholderHistory contentPlaceholderHistory)
		{
      return _CMSThreadManager.Update(contentPlaceholderHistory.CMSThread);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(ContentPlaceholderHistory contentPlaceholderHistory)
		{
      return _CMSThreadManager.Delete(contentPlaceholderHistory.CMSThread, false);
		}

    #region constructors
    internal ContentPlaceholderHistoryManager(IDataStore dataStore)
      : base(dataStore)
    {
    }
    #endregion
	}
}
