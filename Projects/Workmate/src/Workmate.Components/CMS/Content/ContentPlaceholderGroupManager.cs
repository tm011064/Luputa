using CommonTools.Components.BusinessTier;
using Workmate.Components.Entities.CMS;
using System;
using System.Collections.Generic;
using Workmate.Data;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS.Content;
using Workmate.Components.Contracts.CMS;
namespace Workmate.Components.CMS.Content
{
  public class ContentPlaceholderGroupManager : BaseCMSManager
	{
		public ContentPlaceholderGroup GetContentPlaceholderGroup(int groupId)
		{
			CMSGroup group = _CMSGroupManager.GetGroup(groupId);
			if (group != null)
			{
				return new ContentPlaceholderGroup(group);
			}
			return null;
		}
		public List<ContentPlaceholderGroup> GetAllContentPlaceholderGroups()
		{
			List<ContentPlaceholderGroup> list = new List<ContentPlaceholderGroup>();
			foreach (CMSGroup current in _CMSGroupManager.GetAllGroups(CMSGroupType.Content))
			{
				list.Add(new ContentPlaceholderGroup(current));
			}
			return list;
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Create(ContentPlaceholderGroup group)
		{
			return _CMSGroupManager.Create(group.CMSGroup);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Update(ContentPlaceholderGroup group)
		{
      return _CMSGroupManager.Update(group.CMSGroup);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(ContentPlaceholderGroup group)
		{
      return _CMSGroupManager.Delete(group.CMSGroup);
		}

    #region constructors
    internal ContentPlaceholderGroupManager(IDataStore dataStore)
      : base(dataStore)
    {
    }
    #endregion
	}
}
