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
  public class DepartmentManager : IDepartmentManager
  {
    #region members
    protected IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("DepartmentManager");
    #endregion

    #region CRUD
    public IDepartmentWithOfficesModel GetDepartment(int applicationId, int departmentId)
    {
      IDepartmentWithOfficesModel record;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          record = dataStoreContext.wm_Departments_GetRecursive(applicationId, departmentId);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Departments_GetRecursive", ex);
        throw new DataStoreException(ex, true);
      }
      return record;
    }
    public IEnumerable<IDepartmentModel> GetDepartments(int applicationId)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.wm_Departments_GetDepartmentModels(applicationId).ToList();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Departments_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }

    public BusinessObjectActionReport<DataRepositoryActionStatus> Create(int applicationId, IDepartmentModel department)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(department);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
            num = dataStoreContext.wm_Departments_Insert(applicationId, department.ParentDepartmentId, department.Name, department.OfficeId);
        }
        catch (Exception ex)
        {
          _Log.Error("Error at wm_Departments_Insert", ex);
          throw new DataStoreException(ex, true);
        }
        if (num <= 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.ErrorFormat("Department {0} was not inserted at the database (ErrorCode: {1})."
            , DebugUtility.GetObjectString(department)
            , num);
        }
        else
        {
          department.DepartmentId = num;
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("Department {0} was not inserted at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(department)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Update(IDepartmentModel department)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(department);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
            num = dataStoreContext.wm_Departments_Update(department.DepartmentId, department.ParentDepartmentId, department.Name, department.OfficeId);
        }
        catch (Exception ex)
        {
          _Log.Error("Error at wm_Departments_Update", ex);
          throw new DataStoreException(ex, true);
        }
        if (num == 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.NoRecordRowAffected;
          _Log.ErrorFormat("Department {0} was not updated at the database, NoRecordRowAffected"
            , DebugUtility.GetObjectString(department));
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("Department {0} was not updated at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(department)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    public int Delete(int applicationId, int departmentId)
    {
      int num;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          num = dataStoreContext.wm_Departments_Delete(applicationId, departmentId);
      }
      catch (Exception ex)
      {
        _Log.Error("Error at wm_Departments_Delete", ex);
        throw new DataStoreException(ex, true);
      }
      return num;
    }
    #endregion

    #region constructors
    public DepartmentManager(IDataStore dataStore)
    {
      _DataStore = dataStore;
    }
    #endregion
  }
}
