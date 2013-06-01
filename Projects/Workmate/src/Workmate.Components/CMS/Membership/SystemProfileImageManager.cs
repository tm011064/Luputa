using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Entities.CMS.Membership;
using CommonTools.Components.BusinessTier;
using Workmate.Components.Contracts;
using Workmate.Data;
using Workmate.Components.Entities.CMS;
using Workmate.Components.Contracts.CMS;
using log4net;

namespace Workmate.Components.CMS.Membership
{
  public interface ISystemProfileImageManager
  {
    Dictionary<string, SystemProfileImage> GetSystemProfileImages(int applicationId);
    BusinessObjectActionReport<DataRepositoryActionStatus> Create(SystemProfileImage systemProfileImage);
  }

  public class SystemProfileImageManager : ISystemProfileImageManager
  {
    #region members
    CMSFileManager _CMSFileManager;
    IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("SystemProfileImageManager");
    #endregion

    #region methods
    public SystemProfileImage GetSystemProfileImage(int imageId)
    {
      CMSFile cmsFile;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          cmsFile = dataStoreContext.cms_Files_Get(imageId, FileType.SystemProfileImage);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Files_Get", ex);
        throw new DataStoreException(ex, true);
      }

      if (cmsFile != null)
        return new SystemProfileImage(cmsFile);

      return null;
    }

    /// <summary>
    /// Gets the system profile images.
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, SystemProfileImage> GetSystemProfileImages(int applicationId)
    {
      Dictionary<string, SystemProfileImage> records = new Dictionary<string, SystemProfileImage>();

      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          foreach (CMSFile cmsFile in dataStoreContext.cms_Files_GetMultiple(applicationId, FileType.SystemProfileImage))
          {
            if (!string.IsNullOrWhiteSpace(cmsFile.FriendlyFileName))
              records[cmsFile.FriendlyFileName] = new SystemProfileImage(cmsFile);
          }
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Files_Get", ex);
        throw new DataStoreException(ex, true);
      }

      return records;
    }

    public BusinessObjectActionReport<DataRepositoryActionStatus> Create(SystemProfileImage systemProfileImage)
    {
      return this._CMSFileManager.Create(systemProfileImage.CMSFile, null);
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Update(SystemProfileImage systemProfileImage)
    {
      return this._CMSFileManager.Update(systemProfileImage.CMSFile);
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(SystemProfileImage systemProfileImage)
    {
      return this._CMSFileManager.Delete(systemProfileImage.CMSFile);
    }
    #endregion

    #region constructors
    public SystemProfileImageManager(IDataStore dataStore)
    {
      _DataStore = dataStore;
      _CMSFileManager = new CMSFileManager(dataStore);
    }
    #endregion
  }
}
