using CommonTools.Components.BusinessTier;
using CommonTools.Web;
using Workmate.Components.Entities.CMS;
using System;
using System.Collections.Generic;
using Workmate.Components.Entities.CMS.Content;
using Workmate.Components.Contracts;
using Workmate.Data;
using Workmate.Components.Contracts.CMS;
namespace Workmate.Components.CMS.Content
{
  public class ContentBlockManager : BaseCMSManager
	{
		public ContentBlock GetContentBlock(int contentBlockId)
    {
      CMSContent content = _CMSContentManager.GetContent(contentBlockId);

      ContentBlock record = null;
      if (content != null)
      {
        CMSThread thread = null;
        CMSSection section = null;

        thread = _CMSThreadManager.GetThread(CMSSectionType.Content, content.CMSThreadId);
        section = _CMSSectionManager.GetSection(CMSSectionType.Content, thread.CMSSectionId);

        record = new ContentBlock(content, thread, section);
      }

      return record;
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Create(ContentBlock contentBlock)
		{
			string empty = string.Empty;
			if (!HtmlValidator.ValidateHtml(contentBlock.FormattedBody, out empty))
			{
				return new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.InvalidHtml);
			}
			return _CMSContentManager.Create(contentBlock.CMSContent, null, null, false, false, null, null, null);
		}
		public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(ContentBlock contentBlock)
		{
      return _CMSContentManager.Delete(contentBlock.CMSContent, false);
		}

    #region constructors
    internal ContentBlockManager(IDataStore dataStore)
      : base(dataStore)
    {
    }
    #endregion
	}
}
