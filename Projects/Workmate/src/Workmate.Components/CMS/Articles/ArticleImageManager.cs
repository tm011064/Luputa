using CommonTools.Components.BusinessTier;
using Workmate.Components.Entities.CMS;
using System;
using System.Data.SqlClient;
using System.Linq;
using Workmate.Components.Entities.CMS.Articles;
using Workmate.Components.Contracts.CMS.Articles;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.Membership;
using Workmate.Data;
using log4net;
using Workmate.Components.Contracts.CMS;

namespace Workmate.Components.CMS.Articles
{
  public class ArticleImageManager
  {
    #region members
    private CMSFileManager _CMSFileManager;
    protected IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("ArticleImageManager");
    #endregion

    public ArticleImage GetArticleImage(int imageId)
    {
      ArticleImage articleImage = null;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          CMSFile file = dataStoreContext.cms_Files_Get(imageId, Contracts.CMS.FileType.ArticleImage);
          if (file != null)
          {
            articleImage = new ArticleImage(file, dataStoreContext.cms_Tags_GetByFileId(imageId).ToList());
          }
        }
      }
      catch (SqlException ex)
      {
        _Log.Error("Error at cms_Files_Get", ex);
        throw new DataStoreException(ex, true);
      }
      return articleImage;
    }
    public ArticleImage GetTemporaryArticleImage(int imageId)
    {
      ArticleImage articleImage = null;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          CMSFile file = dataStoreContext.cms_FilesTemp_Get(imageId, FileType.ArticleImage);
          if (file != null)
          {
            articleImage = new ArticleImage(file);
          }
        }
      }
      catch (SqlException ex)
      {
        _Log.Error("Error at cms_FilesTemp_Get", ex);
        throw new DataStoreException(ex, true);
      }

      return articleImage;
    }
    public bool CreateImageFromTemporary(int temporaryImageId, string fileName, string friendlyFileName, string[] tags, out int newFileId)
    {
      return _CMSFileManager.MoveTemporaryFileToFiles(temporaryImageId, fileName, friendlyFileName, tags, out newFileId);
    }
    //public ArticleImageSize GetArticleImageSize(int width, int height, ApplicationSettings applictionSettings)
    //{
    //  if (width == applictionSettings.ArticleImagePreviewSize.Width && height == applictionSettings.ArticleImagePreviewSize.Height)
    //  {
    //    return ArticleImageSize.Preview;
    //  }
    //  if (width == applictionSettings.ArticleImageWideSize.Width && height == applictionSettings.ArticleImageWideSize.Height)
    //  {
    //    return ArticleImageSize.Wide;
    //  }
    //  return ArticleImageSize.Unknown;
    //}
    //public BusinessObjectActionReport<DataRepositoryActionStatus> CreateTemporaryImage(ArticleImage articleImage, ApplicationSettings applictionSettings)
    //{
    //  switch (ArticleImageManager.GetArticleImageSize(articleImage.Width, articleImage.Height, applictionSettings))
    //  {
    //    case ArticleImageSize.Wide:
    //      articleImage.CMSFileType = 4;
    //      goto IL_42;
    //    case ArticleImageSize.Preview:
    //      articleImage.CMSFileType = 5;
    //      goto IL_42;
    //  }
    //  articleImage.CMSFileType = 6;
    //IL_42:
    //  return CMSFileManager.CreateTemporaryFile(articleImage);
    //}
    internal BusinessObjectActionReport<DataRepositoryActionStatus> Create(ArticleImage articleImage)
    {
      return _CMSFileManager.Create(articleImage.CMSFile, articleImage.Tags.ToArray());
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Update(ArticleImage articleImage)
    {
      return _CMSFileManager.Update(articleImage.CMSFile);
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(ArticleImage articleImage)
    {
      return _CMSFileManager.Delete(articleImage.ImageId);
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> DeleteTemporaryFile(ArticleImage articleImage)
    {
      return _CMSFileManager.DeleteTemporaryFile(articleImage.ImageId, articleImage.OwnerUserId);
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> DeleteTemporaryFile(int imageId, UserBasic userBasic)
    {
      return _CMSFileManager.DeleteTemporaryFile(imageId, userBasic.UserId);
    }

    public ArticleImageManager(IDataStore dataStore)
    {
      _CMSFileManager = new CMSFileManager(dataStore);
      _DataStore = dataStore;
    }
  }
}
