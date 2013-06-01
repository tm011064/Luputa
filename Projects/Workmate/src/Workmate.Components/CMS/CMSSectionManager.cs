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
  public class CMSSectionManager
  {
    #region members
    protected IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("CMSSectionManager");
    #endregion

    #region CRUD
    internal CMSSection GetSection(int applicationId, CMSSectionType sectionType, string name)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Sections_Get(applicationId, sectionType, name);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Sections_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal CMSSection GetSection(CMSSectionType sectionType, int sectionId)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Sections_Get(sectionType, sectionId);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Sections_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal List<CMSSection> GetAllSections()
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Sections_GetAll().ToList();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Sections_GetAll", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal List<CMSSection> GetAllSections(int applicationId)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Sections_Get(applicationId).ToList();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Sections_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal List<CMSSection> GetAllSections(int applicationId, CMSSectionType sectionType)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Sections_Get(applicationId, sectionType).ToList();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Sections_Get", ex);
        throw new DataStoreException(ex, true);
      }

    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Create(CMSSection cmsSection)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsSection);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = -1;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_Sections_Insert(cmsSection.ApplicationId, cmsSection.CMSParentSectionId, cmsSection.CMSGroupId
              , cmsSection.Name, cmsSection.Description, cmsSection.CMSSectionType, cmsSection.IsActive, cmsSection.IsModerated);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_Sections_Insert", ex);
          throw new DataStoreException(ex, true);
        }
        // TODO (Roman): make sure we return the right values from db
        if (num == -1003)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.NameNotUnique;
        }
        else
        {
          cmsSection.CMSSectionId = num;
          //cmsSection.DateCreatedUtc = DateTime.UtcNow;
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSSection {0} was not inserted at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsSection)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Update(CMSSection cmsSection)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsSection);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_Sections_Update(cmsSection.CMSSectionId, cmsSection.CMSParentSectionId, cmsSection.CMSGroupId
              , cmsSection.Name, cmsSection.Description, cmsSection.CMSSectionType, cmsSection.IsActive, cmsSection.IsModerated);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_Sections_Update", ex);
          throw new DataStoreException(ex, true);
        }
        // TODO (Roman): make sure we return the right values from db
        if (num != -1003)
        {
          if (num == 0)
          {
            businessObjectActionReport.Status = DataRepositoryActionStatus.NoRecordRowAffected;
          }
        }
        else
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.NameNotUnique;
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSSection {0} was not updated at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsSection)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Delete(CMSSection cmsSection, bool deleteLinkedThreads)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsSection);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_Sections_Delete(cmsSection.CMSSectionId, deleteLinkedThreads);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_Sections_Delete", ex);
          throw new DataStoreException(ex, true);
        }
        if (num < 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.ErrorFormat("CMSSection {0} was not deleted from the database (ErrorCode: {1})."
            , DebugUtility.GetObjectString(cmsSection)
            , num);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSSection {0} was not deleted from the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsSection)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    #endregion

    #region constructors
    internal CMSSectionManager(IDataStore dataStore)
    {
      _DataStore = dataStore;
    }
    #endregion
  }
}
