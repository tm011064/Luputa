using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Data;
using log4net;
using Workmate.Components.Contracts;
using CommonTools.Components.BusinessTier;
using CommonTools.Components.Testing;
using CommonTools;

namespace Workmate.Components
{
  public class ApplicationManager : IApplicationManager
  {
    #region members
    protected IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("ApplicationManager");
    #endregion

    #region CRUD
    public IApplication GetApplication(string name)
    {
      IApplication record;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          record = dataStoreContext.wm_Applications_Get(name);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Applications_Get", ex);
        throw new DataStoreException(ex, true);
      }
      return record;
    }

    public BusinessObjectActionReport<DataRepositoryActionStatus> Create(IApplication application)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(application);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
            num = dataStoreContext.wm_Applications_InsertOrUpdate(application);
        }
        catch (Exception ex)
        {
          _Log.Error("Error at wm_Applications_InsertOrUpdate", ex);
          throw new DataStoreException(ex, true);
        }
        if (num <= 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.ErrorFormat("Application {0} was not inserted at the database (ErrorCode: {1})."
            , DebugUtility.GetObjectString(application)
            , num);
        }
        else
        {
          application.ApplicationId = num;
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("Application {0} was not inserted at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(application)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Update(IApplication application)
    {
      return Create(application);
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(IApplication application)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(application);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
            num = dataStoreContext.wm_Applications_Delete(application.ApplicationId);
        }
        catch (Exception ex)
        {
          _Log.Error("Error at wm_Applications_Delete", ex);
          throw new DataStoreException(ex, true);
        }
        if (num == 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.WarnFormat("Application {0} was not deleted from the database (ErrorCode: {1})."
            , DebugUtility.GetObjectString(application)
            , num);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("Application {0} was not deleted from the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(application)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    #endregion

    #region constructors
    public ApplicationManager(IDataStore dataStore)
    {
      _DataStore = dataStore;
    }
    #endregion
  }
}
