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
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.CMS
{
  public class CMSThreadRatingManager
  {
    #region members
    protected IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("CMSThreadRatingManager");
    #endregion

    #region CRUD

    internal CMSThreadRating GetThreadRating(CMSThread thread, IUserBasic userBasic)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_ThreadRatings_Get(userBasic.UserId, thread.CMSThreadId);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_ThreadRatings_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal List<CMSThreadRating> GetRatingsForThread(CMSThread thread)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_ThreadRatings_Get(thread.CMSThreadId).ToList();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_ThreadRatings_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal BusinessObjectActionReport<RatingDataRepositoryActionStatus> Create(CMSThreadRating cmsThreadRating)
    {
      BusinessObjectActionReport<RatingDataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<RatingDataRepositoryActionStatus>(RatingDataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsThreadRating);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_ThreadRatings_InsertOrUpdate(cmsThreadRating.Rating, cmsThreadRating.CMSThreadId, cmsThreadRating.CMSUserId);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_ThreadRatings_InsertOrUpdate", ex);
          throw new DataStoreException(ex, true);
        }
        switch (num)
        {
          case -1:
            businessObjectActionReport.Status = RatingDataRepositoryActionStatus.SelfRatingNotAllowed;
            break;
          case 0:
            cmsThreadRating.DateCreatedUtc = DateTime.UtcNow;
            break;
          default:
            businessObjectActionReport.Status = RatingDataRepositoryActionStatus.SqlError;
            _Log.ErrorFormat("CMSThreadRating {0} was not inserted at the database (ErrorCode: {1})."
              , DebugUtility.GetObjectString(cmsThreadRating), num);
            break;
        }
      }
      else
      {
        businessObjectActionReport.Status = RatingDataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSThreadRating {0} was not inserted at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsThreadRating)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Update(CMSThreadRating cmsThreadRating)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsThreadRating);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_ThreadRatings_InsertOrUpdate(cmsThreadRating.Rating, cmsThreadRating.CMSThreadId, cmsThreadRating.CMSUserId);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_ThreadRatings_InsertOrUpdate", ex);
          throw new DataStoreException(ex, true);
        }
        if (num == 0)
        {
          cmsThreadRating.DateCreatedUtc = DateTime.UtcNow;
        }
        else
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.ErrorFormat("CMSThreadRating {0} was not updated at the database (ErrorCode: {1})."
            , DebugUtility.GetObjectString(cmsThreadRating), num);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSThreadRating {0} was not updated at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsThreadRating), businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Delete(CMSThreadRating cmsThreadRating)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsThreadRating);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_ThreadRatings_Delete(cmsThreadRating.CMSUserId, cmsThreadRating.CMSThreadId);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_ThreadRatings_Delete", ex);
          throw new DataStoreException(ex, true);
        }
        if (num != 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.WarnFormat("CMSThreadRating {0} was not deleted from the database (ErrorCode: {1})."
            , DebugUtility.GetObjectString(cmsThreadRating), num);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSThreadRating {0} was not deleted from the database because the validation failed.\nReport: {1}", DebugUtility.GetObjectString(cmsThreadRating), businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    #endregion

    #region constructors
    internal CMSThreadRatingManager(IDataStore dataStore)
    {
      _DataStore = dataStore;
    }
    #endregion
  }
}
