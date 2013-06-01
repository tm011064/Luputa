using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Contracts.Membership;
using Workmate.Data;
using CommonTools.Components.BusinessTier;
using Workmate.Components.Contracts;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Entities.CMS.Membership;
using log4net;
using Workmate.Components.Entities.CMS;

namespace Workmate.Components.CMS.Membership
{
  public interface IProfileImageManager
  {

  }

  public class ProfileImageManager : IProfileImageManager
  {
    #region members
    CMSFileManager _CMSFileManager;
    IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("ProfileImageManager");
    #endregion

    #region methods
    /// <summary>
    /// Gets the profile image.
    /// </summary>
    /// <param name="imageId">The image id.</param>
    /// <returns></returns>
    public ProfileImage GetProfileImage(int imageId)
    {
      CMSFile cmsFile;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          cmsFile = dataStoreContext.cms_Files_Get(imageId, FileType.ProfileImage);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Files_Get", ex);
        throw new DataStoreException(ex, true);
      }

      if (cmsFile != null)
        return new ProfileImage(cmsFile);

      return null;
    }
    /// <summary>
    /// Gets the temporary profile image.
    /// </summary>
    /// <param name="userBasic">The user basic.</param>
    /// <param name="imageId">The image id.</param>
    /// <returns></returns>
    public ProfileImage GetTemporaryProfileImage(IUserBasic userBasic, int imageId)
    {
      return this.GetTemporaryProfileImage(userBasic.UserId, imageId);
    }
    /// <summary>
    /// Gets the temporary profile image.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="imageId">The image id.</param>
    /// <returns></returns>
    public ProfileImage GetTemporaryProfileImage(int userId, int imageId)
    {
      CMSFile cmsFile;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          cmsFile = dataStoreContext.cms_FilesTemp_Get(imageId, userId, FileType.ProfileImage);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_FilesTemp_Get", ex);
        throw new DataStoreException(ex, true);
      }

      if (cmsFile != null)
        return new ProfileImage(cmsFile);

      return null;
    }
    /// <summary>
    /// Gets the latest temporary profile image.
    /// </summary>
    /// <param name="userBasic">The user basic.</param>
    /// <returns></returns>
    public ProfileImage GetLatestTemporaryProfileImage(IUserBasic userBasic)
    {
      CMSFile cmsFile;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          cmsFile = dataStoreContext.cms_FilesTemp_GetLatest(userBasic.UserId, FileType.ProfileImage);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_FilesTemp_GetLatest", ex);
        throw new DataStoreException(ex, true);
      }

      if (cmsFile != null)
        return new ProfileImage(cmsFile);

      return null;
    }
    /// <summary>
    /// Gets the profile image.
    /// </summary>
    /// <param name="userBasic">The user basic.</param>
    /// <returns></returns>
    public ProfileImage GetProfileImage(IUserBasic userBasic)
    {
      ProfileImage record;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          record = dataStoreContext.cms_Files_GetProfileImage(userBasic.UserId);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_FilesTemp_GetLatest", ex);
        throw new DataStoreException(ex, true);
      }

      return record;
    }

    /// <summary>
    /// Assigns the temporary profile image to user.
    /// </summary>
    /// <param name="imageId">The image id.</param>
    /// <param name="newFileId">The new file id.</param>
    /// <returns></returns>
    public bool AssignTemporaryProfileImageToUser(int imageId, out int newFileId)
    {
      newFileId = -1;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          newFileId = dataStoreContext.cms_Files_AssignTemporaryProfileImageToUser(imageId);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Files_MoveTempFile", ex);
        throw new DataStoreException(ex, true);
      }
      if (newFileId <= 0)
      {
        _Log.ErrorFormat("Temporary file with id {0} was not moved to the files table (ErrorCode: {1}).", imageId, newFileId);
        return false;
      }
      return true;
    }

    /// <summary>
    /// Creates the temporary image.
    /// </summary>
    /// <param name="profileImage">The profile image.</param>
    /// <returns></returns>
    public BusinessObjectActionReport<DataRepositoryActionStatus> CreateTemporaryImage(ProfileImage profileImage)
    {
      return _CMSFileManager.CreateTemporaryFile(profileImage.CMSFile);
    }

    /// <summary>
    /// Updates the specified profile image.
    /// </summary>
    /// <param name="profileImage">The profile image.</param>
    /// <returns></returns>
    public BusinessObjectActionReport<DataRepositoryActionStatus> Update(ProfileImage profileImage)
    {
      if (profileImage.IsSystemProfileImage)
      {
        throw new ArgumentException("It is not allowed to update a  profile image via the ProfileImageManager.Update method. Use SystemProfileManager.Update instead.");
      }
      return _CMSFileManager.Update(profileImage.CMSFile);
    }

    /// <summary>
    /// Deletes the specified profile image.
    /// </summary>
    /// <param name="profileImage">The profile image.</param>
    /// <returns></returns>
    public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(ProfileImage profileImage)
    {
      return _CMSFileManager.Delete(profileImage.ImageId);
    }
    /// <summary>
    /// Deletes the temporary file.
    /// </summary>
    /// <param name="profileImage">The profile image.</param>
    /// <returns></returns>
    public BusinessObjectActionReport<DataRepositoryActionStatus> DeleteTemporaryFile(ProfileImage profileImage)
    {
      return _CMSFileManager.DeleteTemporaryFile(profileImage.ImageId, profileImage.UserId);
    }
    /// <summary>
    /// Deletes the temporary file.
    /// </summary>
    /// <param name="imageId">The image id.</param>
    /// <param name="userBasic">The user basic.</param>
    /// <returns></returns>
    public BusinessObjectActionReport<DataRepositoryActionStatus> DeleteTemporaryFile(int imageId, IUserBasic userBasic)
    {
      return _CMSFileManager.DeleteTemporaryFile(imageId, userBasic.UserId);
    }
    /// <summary>
    /// Deletes the temporary files.
    /// </summary>
    /// <param name="userBasic">The user basic.</param>
    /// <returns></returns>
    public BusinessObjectActionReport<DataRepositoryActionStatus> DeleteTemporaryFiles(IUserBasic userBasic)
    {
      return _CMSFileManager.DeleteTemporaryFiles(userBasic, FileType.ProfileImage);
    }
    #endregion

    #region constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileImageManager"/> class.
    /// </summary>
    /// <param name="dataStore">The data store.</param>
    public ProfileImageManager(IDataStore dataStore)
    {
      _CMSFileManager = new CMSFileManager(dataStore);
      _DataStore = dataStore;
    }
    #endregion
  }
}
