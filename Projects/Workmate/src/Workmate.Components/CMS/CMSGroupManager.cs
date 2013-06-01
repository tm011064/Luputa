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
  public class CMSGroupManager
  {
    #region members
    protected IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("CMSGroupManager");
    #endregion

    #region CRUD
    internal CMSGroup GetGroup(int groupId)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          return dataStoreContext.cms_Groups_Get(groupId);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Groups_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal List<CMSGroup> GetAllGroups()
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          return dataStoreContext.cms_Groups_GetAll().ToList();
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Groups_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal List<CMSGroup> GetAllGroups(CMSGroupType groupType)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          return dataStoreContext.cms_Groups_Get(groupType).ToList();
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Groups_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Create(CMSGroup cmsGroup)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsGroup);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
            businessObjectActionReport.Status = (DataRepositoryActionStatus)dataStoreContext.cms_Groups_InsertOrUpdate(cmsGroup.CMSGroupId, cmsGroup.Name, cmsGroup.Description, cmsGroup.CMSGroupType);
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_Groups_InsertOrUpdate", ex);
          throw new DataStoreException(ex, true);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSGroup {0} was not inserted at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsGroup)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Update(CMSGroup cmsGroup)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsGroup);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
            businessObjectActionReport.Status = (DataRepositoryActionStatus)dataStoreContext.cms_Groups_InsertOrUpdate(cmsGroup.CMSGroupId, cmsGroup.Name, cmsGroup.Description, cmsGroup.CMSGroupType);

          return businessObjectActionReport;
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_Groups_InsertOrUpdate", ex);
          throw new DataStoreException(ex, true);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSGroup {0} was not updated at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsGroup)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Delete(CMSGroup cmsGroup)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsGroup);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
            num = dataStoreContext.cms_Groups_Delete(cmsGroup.CMSGroupId);
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_Groups_Delete", ex);
          throw new DataStoreException(ex, true);
        }
        if (num == 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.WarnFormat("CMSGroup {0} was not deleted from the database (ErrorCode: {1})."
            , DebugUtility.GetObjectString(cmsGroup)
            , num);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSGroup {0} was not deleted from the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsGroup)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    #endregion

    #region constructors
    internal CMSGroupManager(IDataStore dataStore)
    {
      _DataStore = dataStore;
    }
    #endregion
  }
}
