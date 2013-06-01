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
using Workmate.Components.Contracts.Company;

namespace Workmate.Components.Company
{
  public class OfficeManager : IOfficeManager
  {
    #region members
    protected IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("OfficeManager");
    #endregion

    #region CRUD
    public IOfficeModel GetOffice(int applicationId, int officeId)
    {
      IOfficeModel record;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          record = dataStoreContext.wm_Offices_Get(applicationId, officeId);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Offices_Get", ex);
        throw new DataStoreException(ex, true);
      }
      return record;
    }
    public IEnumerable<IOfficeModel> GetOffices(int applicationId)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.wm_Offices_Get(applicationId).ToList();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Offices_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }

    public BusinessObjectActionReport<DataRepositoryActionStatus> Create(int applicationId, IOfficeModel office)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(office);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
            num = dataStoreContext.wm_Offices_Insert(applicationId, office);
        }
        catch (Exception ex)
        {
          _Log.Error("Error at wm_Offices_Insert", ex);
          throw new DataStoreException(ex, true);
        }
        if (num <= 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.ErrorFormat("Office {0} was not inserted at the database (ErrorCode: {1})."
            , DebugUtility.GetObjectString(office)
            , num);
        }
        else
        {
          office.OfficeId = num;
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("Office {0} was not inserted at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(office)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Update(IOfficeModel office)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(office);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
            num = dataStoreContext.wm_Offices_Update(office);
        }
        catch (Exception ex)
        {
          _Log.Error("Error at wm_Offices_Update", ex);
          throw new DataStoreException(ex, true);
        }
        if (num == 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.NoRecordRowAffected;
          _Log.ErrorFormat("Office {0} was not updated at the database, NoRecordRowAffected"
            , DebugUtility.GetObjectString(office));
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("Office {0} was not updated at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(office)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    public int Delete(int applicationId, int officeId)
    {
      int num;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          num = dataStoreContext.wm_Offices_Delete(applicationId, officeId);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Offices_Delete", ex);
        throw new DataStoreException(ex, true);
      }
      return num;
    }
    #endregion

    #region constructors
    public OfficeManager(IDataStore dataStore)
    {
      _DataStore = dataStore;
    }
    #endregion
  }
}
