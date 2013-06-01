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
  public class CMSFileManager
  {
    #region members
    protected IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("CMSFileManager");
    #endregion

    internal CMSFile GetFile(int fileId)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Files_Get(fileId);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Files_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    internal bool MoveTemporaryFileToFiles(int tempFileId, string fileName, string friendlyFileName, string[] tags, out int newFileId)
    {
      return this.MoveTemporaryFileToFiles(tempFileId, null, fileName, friendlyFileName, tags, out newFileId);
    }
    internal bool MoveTemporaryFileToFiles(int tempFileId, int? contentId, out int newFileId)
    {
      return this.MoveTemporaryFileToFiles(tempFileId, contentId, null, null, null, out newFileId);
    }
    internal bool MoveTemporaryFileToFiles(int tempFileId, int? contentId, string fileName, string friendlyFileName, string[] tags, out int newFileId)
    {
      newFileId = -1;
      bool value = !string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(friendlyFileName);
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          newFileId = dataStoreContext.cms_Files_MoveTempFile(tempFileId, contentId, value, fileName, friendlyFileName, tags);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Files_MoveTempFile", ex);
        throw new DataStoreException(ex, true);
      }
      if (newFileId <= 0)
      {
        _Log.ErrorFormat("Temporary file with id {0} was not moved to the files table (ErrorCode: {1}).", tempFileId, newFileId);
        return false;
      }
      return true;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Create(CMSFile cmsFile)
    {
      return this.Create(cmsFile, null);
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Create(CMSFile cmsFile, string[] tags)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsFile);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = -1;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_Files_Insert(cmsFile.ApplicationId, cmsFile.CMSUserId, cmsFile.CMSFileType, cmsFile.FileName, cmsFile.Content, cmsFile.ContentType, cmsFile.ContentSize, cmsFile.FriendlyFileName, cmsFile.CMSHeight, cmsFile.CMSWidth, cmsFile.ContentId, tags);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_Files_Insert", ex);
          throw new DataStoreException(ex, true);
        }

        if (num > 0)
        {
          cmsFile.CMSFileId = num;
          cmsFile.DateCreatedUtc = DateTime.UtcNow;
        }
        else if (num == -501)
        {// this is a special condition for system profile images to ensure thread safety on inserts
          businessObjectActionReport.Status = DataRepositoryActionStatus.UniqueKeyConstraint;
        }
        else
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.ErrorFormat("CMSFile was not inserted at the database (ErrorCode: {0}).", num);
        }

      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSFile {0} was not inserted at the database because the validation failed.\nReport: {1}", cmsFile.CMSFileId
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Update(CMSFile cmsFile)
    {
      return this.Update(cmsFile, null);
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Update(CMSFile cmsFile, string[] tags)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsFile);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        int num = 0;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_Files_Update(cmsFile.CMSFileId, cmsFile.CMSUserId, cmsFile.CMSFileType, cmsFile.FileName
              , cmsFile.Content, cmsFile.ContentType, cmsFile.ContentSize, cmsFile.FriendlyFileName, cmsFile.CMSHeight
              , cmsFile.CMSWidth, cmsFile.ContentId, tags);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_Files_Update", ex);
          throw new DataStoreException(ex, true);
        }
        if (num != 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.ErrorFormat("CMSFile {0} was not updated from the database (ErrorCode: {1}).", cmsFile.CMSFileId, num);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("CMSFile {0} was not updated at the database because the validation failed.\nReport: {1}", cmsFile.CMSFileId
          , businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Delete(CMSFile cmsFile)
    {
      return Delete(cmsFile.CMSFileId);
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Delete(int fileId)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);

      int num = 0;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          num = dataStoreContext.cms_Files_Delete(fileId);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Files_Delete", ex);
        throw new DataStoreException(ex, true);
      }

      if (num == 0)
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
        _Log.ErrorFormat("CMSFile {0} was not deleted from the database (ErrorCode: {1}).", fileId, num);
      }

      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> CreateTemporaryFile(CMSFile cmsFile)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      businessObjectActionReport.ValidationResult = BusinessObjectManager.Validate(cmsFile);
      if (businessObjectActionReport.ValidationResult.IsValid)
      {
        DateTime utcNow = DateTime.UtcNow;
        int num = -1;
        try
        {
          using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
          {
            num = dataStoreContext.cms_FilesTemp_Insert(cmsFile.ApplicationId, cmsFile.CMSUserId, cmsFile.CMSFileType, cmsFile.FileName, cmsFile.Content
              , cmsFile.ContentType, cmsFile.ContentSize, cmsFile.FriendlyFileName, cmsFile.CMSHeight, cmsFile.CMSWidth);
          }
        }
        catch (Exception ex)
        {
          _Log.Error("Error at cms_FilesTemp_Insert", ex);
          throw new DataStoreException(ex, true);
        }
        if (num > 0)
        {
          cmsFile.CMSFileId = num;
          cmsFile.IsTemporary = true;
          cmsFile.DateCreatedUtc = DateTime.UtcNow;
        }
        else
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.ErrorFormat("Temporary CMSFile {0} was not inserted at the database (ErrorCode: {1}).", cmsFile.CMSFileId, num);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.ValidationFailed;
        _Log.WarnFormat("Temporary CMSFile {0} was not inserted at the database because the validation failed.\nReport: {1}", cmsFile.CMSFileId, businessObjectActionReport.ValidationResult.ToString(TextFormat.ASCII));
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> DeleteTemporaryFiles(IUserBasic userBasic, FileType fileType)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      int num = 0;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          num = dataStoreContext.cms_FilesTemp_DeleteByUserId(userBasic.UserId, fileType);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_FilesTemp_DeleteByUserId", ex);
        throw new DataStoreException(ex, true);
      }
      if (num != -1002)
      {
        if (num != 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.ErrorFormat("Temporary profile images of user with id {0} were not deleted from the database (ErrorCode: {1}).", userBasic.UserId
            , num);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.NoRecordRowAffected;
        _Log.ErrorFormat("Temporary profile images of user with id {0} were not deleted from the database (ErrorCode: {1}).", userBasic.UserId
          , num);
      }
      return businessObjectActionReport;
    }
    internal BusinessObjectActionReport<DataRepositoryActionStatus> DeleteTemporaryFile(int fileId, int userId)
    {
      BusinessObjectActionReport<DataRepositoryActionStatus> businessObjectActionReport = new BusinessObjectActionReport<DataRepositoryActionStatus>(DataRepositoryActionStatus.Success);
      int num = 0;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          num = dataStoreContext.cms_FilesTemp_Delete(fileId, userId);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_FilesTemp_Delete", ex);
        throw new DataStoreException(ex, true);
      }
      if (num != -1002)
      {
        if (num != 0)
        {
          businessObjectActionReport.Status = DataRepositoryActionStatus.SqlError;
          _Log.ErrorFormat("Temporary CMSFile with id {0} was not deleted from the database (ErrorCode: {1}).", fileId, num);
        }
      }
      else
      {
        businessObjectActionReport.Status = DataRepositoryActionStatus.NoRecordRowAffected;
        _Log.InfoFormat("Temporary CMSFile with id {0} was not deleted from the database (ErrorCode: {1}).", fileId, num);
      }
      return businessObjectActionReport;
    }

    #region constructors
    internal CMSFileManager(IDataStore dataStore)
    {
      _DataStore = dataStore;
    }
    #endregion
  }
}
