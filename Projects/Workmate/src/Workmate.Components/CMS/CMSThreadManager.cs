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
  public class CMSThreadManager
  {
    #region members
    protected IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("CMSThreadManager");
    #endregion

    #region CRUD

    internal CMSThread GetThread(CMSSectionType sectionType, int threadId)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Threads_Get(sectionType, threadId);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Threads_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal CMSThread GetThread(CMSSectionType sectionType, string name)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Threads_Get(sectionType, name);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Threads_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal List<CMSThread> GetAllThreads(CMSSectionType sectionType)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Threads_Get(sectionType).ToList();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Threads_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal List<CMSThread> GetAllThreads(int applicationId)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Threads_Get(applicationId).ToList();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Threads_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal List<CMSThread> GetAllThreads(int applicationId, CMSSectionType sectionType)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Threads_Get(applicationId, sectionType).ToList();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Threads_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Create(CMSThread cmsThread)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsThread);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = -1;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_Threads_Insert(
              cmsThread.CMSSectionId
              , cmsThread.CMSName
              , cmsThread.CMSStickyDateUtc
              , cmsThread.IsLocked
              , cmsThread.CMSIsSticky
              , cmsThread.IsApproved
              , cmsThread.CMSThreadStatus);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_Threads_Insert", ex);
          throw new DataStoreException(ex, true);
        }
        if (num > 0)
        {
          cmsThread.CMSThreadId = num;
          cmsThread.DateCreatedUtc = DateTime.UtcNow;
        }
        else
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.NoRecordRowAffected;
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSThread {0} was not inserted at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsThread)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Update(CMSThread cmsThread)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsThread);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_Threads_Update(
              cmsThread.CMSThreadId
              , cmsThread.CMSSectionId
              , cmsThread.CMSName
              , cmsThread.CMSLastViewedDateUtc
              , cmsThread.CMSStickyDateUtc
              , cmsThread.IsLocked
              , cmsThread.CMSIsSticky
              , cmsThread.IsApproved
              , cmsThread.CMSThreadStatus);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_Threads_Update", ex);
          throw new DataStoreException(ex, true);
        }
        if (num <= 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.NoRecordRowAffected;
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSThread {0} was not updated at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsThread)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Delete(CMSThread cmsThread, bool deleteLinkedThreads)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsThread);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_Threads_Delete(cmsThread.CMSThreadId, deleteLinkedThreads);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at XXX", ex);
          throw new DataStoreException(ex, true);
        }

        if (num <= 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.NoRecordRowAffected;
          _Log.WarnFormat("CMSThread {0} was not deleted from the database (ErrorCode: {1})."
            , DebugUtility.GetObjectString(cmsThread)
            , num);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSThread {0} was not deleted from the database because the validation failed.\nReport: {1}", DebugUtility.GetObjectString(cmsThread), businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }

    #endregion

    #region constructors
    internal CMSThreadManager(IDataStore dataStore)
    {
      _DataStore = dataStore;
    }
    #endregion
  }
}
