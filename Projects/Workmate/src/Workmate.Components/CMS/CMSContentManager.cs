using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Entities.CMS;
using Workmate.Components.Contracts.CMS;
using CommonTools.Components.BusinessTier;
using Workmate.Components.Contracts;
using Workmate.Data;
using log4net;
using CommonTools.Components.Testing;
using CommonTools;

namespace Workmate.Components.CMS
{
  public class CMSContentManager
  {
    #region members
    protected IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("CMSContentManager");
    #endregion

    #region CRUD

    internal CMSContent GetContent(int contentId)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Contents_Get(contentId);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Contents_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }

    internal CMSContent GetContent(string urlFriendlyName, string threadName, string sectionName, string groupName)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Contents_Get(urlFriendlyName, threadName, sectionName, groupName);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Contents_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal List<CMSContent> GetContents(CMSSectionType sectionType)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Contents_Get(sectionType).ToList();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Contents_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal List<CMSContent> GetContents(CMSGroupType groupType)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Contents_Get(groupType).ToList();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Contents_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal List<CMSContent> GetContents(int applicationId, CMSSectionType sectionType)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Contents_Get(applicationId, sectionType).ToList();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Contents_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal List<CMSContent> GetContents(int applicationId, CMSGroupType groupType)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Contents_Get(applicationId, groupType).ToList();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Contents_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal bool IncreaseTotalViews(CMSContent content, int viewsToAdd)
    {
      int num = 0;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          num = dataStoreContext.cms_Contents_IncreaseTotalViews(content.CMSContentId, viewsToAdd);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Contents_IncreaseTotalViews", ex);
        throw new DataStoreException(ex, true);
      }

      if (num == 1)
      {
        content.TotalViews += viewsToAdd;
        return true;
      }
      else
      {
        _Log.WarnFormat("Unable to increase total views for ConentId {0}. No record rows affected", content.CMSBaseContentId);
        return false;
      }
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Create(CMSContent cmsContent, string[] tags, string[] contentLevelNodeNames
      , bool checkUrlFriendlyName
      , bool createLinkedThread, int? linkedThreadSectionId, bool? isLinkedThreadEnabled, LinkedThreadRelationshipType? linkedThreadRelationshipType)
    {
      // TODO (Roman): checkUrlFriendlyName
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsContent);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = -1;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_Contents_Insert(
              cmsContent.CMSThreadId
              , cmsContent.CMSParentContentId
              , cmsContent.AuthorUserId
              , cmsContent.CMSContentLevel
              , cmsContent.Subject
              , cmsContent.FormattedBody
              , cmsContent.IsApproved
              , cmsContent.IsLocked
              , cmsContent.CMSContentType
              , cmsContent.CMSContentStatus
              , cmsContent.CMSExtraInfo
              , cmsContent.UrlFriendlyName
              , tags
              , contentLevelNodeNames
              , createLinkedThread
              , linkedThreadSectionId
              , isLinkedThreadEnabled ?? false
              , linkedThreadRelationshipType);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_Contents_Insert", ex);
          throw new DataStoreException(ex, true);
        }
        if (num > 0)
        {
          cmsContent.CMSContentId = num;
        }
        else
        {
          if (num == -1003)
          {
            businessObjectActionReport.Status = DataRepositoryActionStatus.NameNotUnique;
          }
          else
          {
            businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
            _Log.ErrorFormat("CMSContent {0} was not inserted at the database (ErrorCode: {1})."
              , DebugUtility.GetObjectString(cmsContent)
              , num);
          }
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSContent {0} was not inserted at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsContent)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Update(CMSContent cmsContent)
    {
      return this.Update(cmsContent, null);
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Update(CMSContent cmsContent, string[] tags)
    {
      return this.Update(cmsContent, tags, false);
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Update(CMSContent cmsContent, string[] tags, bool doReindex)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsContent);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_Contents_Update(
              cmsContent.CMSContentId
              , cmsContent.CMSThreadId
              , cmsContent.CMSParentContentId
              , cmsContent.AuthorUserId
              , cmsContent.CMSContentLevel
              , cmsContent.Subject
              , cmsContent.FormattedBody
              , cmsContent.IsApproved
              , cmsContent.IsLocked
              , cmsContent.CMSContentType
              , cmsContent.CMSContentStatus
              , cmsContent.CMSExtraInfo
              , cmsContent.CMSBaseContentId
              , cmsContent.UrlFriendlyName
              , tags);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_Contents_Update", ex);
          throw new DataStoreException(ex, true);
        }
        if (num != 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.ErrorFormat("CMSContent {0} was not updated from the database (ErrorCode: {1})."
            , DebugUtility.GetObjectString(cmsContent)
            , num);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSContent {0} was not updated at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsContent)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal int Delete(int contentId, bool deleteLinkedThreads)
    {
      int result;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          result = dataStoreContext.cms_Contents_Delete(contentId, deleteLinkedThreads);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Contents_Delete", ex);
        throw new DataStoreException(ex, true);
      }
      return result;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Delete(CMSContent cmsContent, bool deleteLinkedThreads)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsContent);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = this.Delete(cmsContent.CMSContentId, deleteLinkedThreads);
        if (num == 0 || num == -1003)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.NoRecordRowAffected;
        }
        else if (num < 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.WarnFormat("CMSContent {0} was not deleted from the database (ErrorCode: {1})."
            , DebugUtility.GetObjectString(cmsContent)
            , num);
        }
        else
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.Success;
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSContent {0} was not deleted from the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsContent)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }

    #endregion

    #region constructors
    internal CMSContentManager(IDataStore dataStore)
    {
      _DataStore = dataStore;
    }
    #endregion
  }
}
