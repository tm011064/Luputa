using CommonTools.Components.BusinessTier;
using CommonTools.Web;
using Workmate.Components.Entities.CMS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Workmate.Data;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Entities.CMS.Content;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Contracts.CMS.Content;
using Workmate.Components.Contracts.Membership;
namespace Workmate.Components.CMS.Content
{
  public class ContentPlaceholderManager : BaseCMSManager
  {
    public ContentPlaceholder GetContentPlaceholder(int applicationId, string key)
    {
      CMSSection section = _CMSSectionManager.GetSection(applicationId, CMSSectionType.Content, key);
      if (section != null)
      {
        return new ContentPlaceholder(section);
      }
      return null;
    }
    public ContentPlaceholder GetContentPlaceholder(int contentPlaceholderId)
    {
      CMSSection section = _CMSSectionManager.GetSection(CMSSectionType.Content, contentPlaceholderId);
      if (section != null)
      {
        return new ContentPlaceholder(section);
      }
      return null;
    }

    public Dictionary<string, string> GetContentPlaceholderBodies(int applicationId)
    {
      Dictionary<string, string> result;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          result = dataStoreContext.cms_Contents_GetAllWithSectionName(
            applicationId
            , CMSSectionType.Content
            , (byte)ContentBlockStatus.Active);
        }
      }
      catch (SqlException ex)
      {
        _Log.Error("Error at cms_ContentRatings_InsertOrUpdate", ex);
        throw new DataStoreException(ex, true);
      }
      return result;
    }
    public ContentUpdateStatus UpdateContentBlock(int applicationId, IUserBasic author, string name, string formattedBody, out int contentBlockId)
    {
      return this.UpdateContentBlock(applicationId, author, name, formattedBody, false, out contentBlockId);
    }
    public ContentUpdateStatus UpdateContentBlock(int applicationId, IUserBasic author, string name, string formattedBody, bool createPlaceholderIfNotExists, out int contentBlockId)
    {
      contentBlockId = -1;
      string empty = string.Empty;
      if (!HtmlValidator.ValidateHtml(formattedBody, out empty))
      {
        return ContentUpdateStatus.InvalidHtml;
      }
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          contentBlockId = dataStoreContext.cms_Sections_UpdateContentBlock(applicationId, name, formattedBody, author.UserId);
        }
      }
      catch (SqlException ex)
      {
        _Log.Error("Error at cms_Sections_UpdateContentBlock", ex);
        throw new DataStoreException(ex, true);
      }
      ContentUpdateStatus contentUpdateStatus;
      if (contentBlockId > 0)
      {
        contentUpdateStatus = ContentUpdateStatus.Success;
      }
      else
      {
        if (contentBlockId == -1)
        {
          contentUpdateStatus = ContentUpdateStatus.SectionNotFound;
        }
        else
        {
          contentUpdateStatus = ContentUpdateStatus.UnknownError;
        }
        _Log.WarnFormat(string.Format("Unable to add content block for content placeholder '{0}'.\nReason: {1}", name, contentUpdateStatus));
      }

      return contentUpdateStatus;
    }
    
    public BusinessObjectActionReport<DataRepositoryActionStatus> Create(ContentPlaceholder contentPlaceholder)
    {
      return _CMSSectionManager.Create(contentPlaceholder.CMSSection);
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Update(ContentPlaceholder contentPlaceholder)
    {
      return _CMSSectionManager.Update(contentPlaceholder.CMSSection);
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(ContentPlaceholder contentPlaceholder)
    {
      return _CMSSectionManager.Delete(contentPlaceholder.CMSSection, false);
    }

    #region constructors
    internal ContentPlaceholderManager(IDataStore dataStore)
      : base(dataStore)
    {
    }
    #endregion
  }
}
