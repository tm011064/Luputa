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
  public class CMSContentRatingManager
  {
    #region members
    protected IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("CMSContentRatingManager");
    #endregion

    #region CRUD

    internal CMSContentRating GetContentRating(CMSContent content, IUserBasic userBasic)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_ContentRatings_Get(userBasic.UserId, content.CMSContentId);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_ContentRatings_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal List<CMSContentRating> GetRatingsForContent(CMSContent content)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_ContentRatings_Get(content.CMSContentId).ToList();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_ContentRatings_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal BusinessObjectActionReport<RatingDataRepositoryActionStatus> Create(CMSContentRating cmsContentRating)
    {
      BaseRatingInfo baseRatingInfo = null;
      return this.Create(cmsContentRating, false, false, out baseRatingInfo);
    }
    internal BusinessObjectActionReport<RatingDataRepositoryActionStatus> Create(CMSContentRating cmsContentRating, bool getBaseRatingInfo, bool allowSelfRating, out BaseRatingInfo baseRatingInfo)
    {
      baseRatingInfo = null;
      BusinessObjectActionReport<RatingDataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<RatingDataRepositoryActionStatus>(RatingDataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsContentRating);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_ContentRatings_InsertOrUpdate(cmsContentRating.Rating, cmsContentRating.CMSContentId, cmsContentRating.CMSUserId, allowSelfRating);
            if (getBaseRatingInfo)
            {
              baseRatingInfo = dataStoreContext.cms_Contents_GetBaseRatingInfo(cmsContentRating.CMSContentId);
            }
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_ContentRatings_InsertOrUpdate, cms_Contents_GetBaseRatingInfo", ex);
          throw new DataStoreException(ex, true);
        }
        switch (num)
        {
          case -1:
            businessObjectActionReport.Status = RatingDataRepositoryActionStatus.SelfRatingNotAllowed;
            break;
          case 0:
            cmsContentRating.DateCreatedUtc = DateTime.UtcNow;
            break;
          default:
            businessObjectActionReport.Status = RatingDataRepositoryActionStatus.SqlError;
            _Log.ErrorFormat("CMSContentRating {0} was not inserted at the database (ErrorCode: {1})."
              , DebugUtility.GetObjectString(cmsContentRating), num);
            break;
        }
      }
      else
      {
        businessObjectActionReport.Status = RatingDataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSContentRating {0} was not inserted at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsContentRating)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Update(CMSContentRating cmsContentRating)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsContentRating);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_ContentRatings_InsertOrUpdate(cmsContentRating.Rating, cmsContentRating.CMSContentId, cmsContentRating.CMSUserId, true);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_ContentRatings_InsertOrUpdate", ex);
          throw new DataStoreException(ex, true);
        }
        if (num == 0)
        {
          cmsContentRating.DateCreatedUtc = DateTime.UtcNow;
        }
        else
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.ErrorFormat("CMSContentRating {0} was not updated at the database (ErrorCode: {1})."
            , DebugUtility.GetObjectString(cmsContentRating), num);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSContentRating {0} was not updated at the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsContentRating), businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Delete(CMSContentRating cmsContentRating)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsContentRating);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_ContentRatings_Delete(cmsContentRating.CMSUserId, cmsContentRating.CMSContentId);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_ContentRatings_Delete", ex);
          throw new DataStoreException(ex, true);
        }
        if (num != 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.WarnFormat("CMSContentRating {0} was not deleted from the database (ErrorCode: {1})."
            , DebugUtility.GetObjectString(cmsContentRating), num);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSContentRating {0} was not deleted from the database because the validation failed.\nReport: {1}"
          , DebugUtility.GetObjectString(cmsContentRating)
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    #endregion

    #region constructors
    internal CMSContentRatingManager(IDataStore dataStore)
    {
      _DataStore = dataStore;
    }
    #endregion
  }
}
