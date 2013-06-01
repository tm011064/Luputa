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
using Workmate.Components.Entities.Membership;

namespace Workmate.Components.CMS
{
  public class CMSContentUserManager
  {
    #region members
    protected IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("CMSContentUserManager");
    #endregion

    internal BusinessObjectActionReport<DataRepositoryActionStatus> Create(CMSContentUser cmsContentUser)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsContentUser);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_ContentUser_InsertOrUpdate(cmsContentUser.CMSContentId, cmsContentUser.CMSReceivingUserId);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_ContentUser_InsertOrUpdate", ex);
          throw new DataStoreException(ex, true);
        }
        if (num != 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.NoRecordRowAffected;
        }
        else
        {
          cmsContentUser.DateReceivedUtc = DateTime.UtcNow;
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSContentUser {0} was not updated at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsContentUser), businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Update(CMSContentUser cmsContentUser)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsContentUser);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        DateTime utcNow = DateTime.UtcNow;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {            
            num = dataStoreContext.cms_ContentUser_InsertOrUpdate(cmsContentUser.CMSContentId, cmsContentUser.CMSReceivingUserId);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_ContentUser_InsertOrUpdate", ex);
          throw new DataStoreException(ex, true);
        }
        if (num == 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.NoRecordRowAffected;
        }
        else
        {
          cmsContentUser.DateReceivedUtc = DateTime.UtcNow;
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSContentUser {0} was not updated at the database because the validation failed.\nReport: {1}", DebugUtility.GetObjectString(cmsContentUser), businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Delete(CMSContentUser cmsContentUser)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsContentUser);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_ContentUser_Delete(cmsContentUser.CMSContentId, cmsContentUser.CMSReceivingUserId);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_ContentUser_Delete", ex);
          throw new DataStoreException(ex, true);
        }
        if (num != 1)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.WarnFormat("CMSContentUser {0} was not deleted from the database (ErrorCode: {1}).", DebugUtility.GetObjectString(cmsContentUser), num);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSContentUser {0} was not deleted from the database because the validation failed.\nReport: {1}", DebugUtility.GetObjectString(cmsContentUser), businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }

    
    #region constructors
    internal CMSContentUserManager(IDataStore dataStore)
    {
      _DataStore = dataStore;
    }
    #endregion
  }
}
