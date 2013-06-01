using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CommonTools.Components.BusinessTier;
using log4net;
using Workmate.Components.Contracts;
using Workmate.Components.Contracts.CMS.Articles;
using Workmate.Components.Entities.CMS.Articles;
using Workmate.Data;

namespace Workmate.Components.CMS.Articles
{
  public interface IArticleAttachmentManager
  {
    BusinessObjectActionReport<DataRepositoryActionStatus> CreateTemporaryFile(ArticleAttachment articleAttachment);
    BusinessObjectActionReport<DataRepositoryActionStatus> Delete(int fileId);
    BusinessObjectActionReport<DataRepositoryActionStatus> DeleteTemporaryFile(int attachmentId, int userId);
    List<IArticleAttachmentModel> GetArticleAttachments(int articleId);
    bool MoveTemporaryFileToFiles(int temporaryArticleAttachmentId, int articleId, string fileName, string friendlyName, out int newArticleAttachmentId);
  }

  public class ArticleAttachmentManager : IArticleAttachmentManager
  {
    #region members
    protected CMSFileManager _CMSFileManager;
    protected ILog _Log = LogManager.GetLogger("ArticleAttachmentManager");
    protected IDataStore _DataStore;
    #endregion

    public List<IArticleAttachmentModel> GetArticleAttachments(int articleId)
    {
      List<IArticleAttachmentModel> records;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          records = dataStoreContext.cms_Files_GetArticleAttachmentModels(articleId).ToList();
        }
      }
      catch (SqlException ex)
      {
        _Log.Error("Error at cms_Files_GetArticleAttachmentModels", ex);
        throw new DataStoreException(ex, true);
      }

      return records;
    }

    public bool MoveTemporaryFileToFiles(int temporaryArticleAttachmentId, int articleId, string fileName, string friendlyName, out int newArticleAttachmentId)
    {
      return _CMSFileManager.MoveTemporaryFileToFiles(temporaryArticleAttachmentId, articleId, fileName, friendlyName, null, out newArticleAttachmentId);
    }

    public BusinessObjectActionReport<DataRepositoryActionStatus> Delete(int fileId)
    {
      return _CMSFileManager.Delete(fileId);
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> CreateTemporaryFile(ArticleAttachment articleAttachment)
    {
      return _CMSFileManager.CreateTemporaryFile(articleAttachment.CMSFile);
    }
    public BusinessObjectActionReport<DataRepositoryActionStatus> DeleteTemporaryFile(int attachmentId, int userId)
    {
      return _CMSFileManager.DeleteTemporaryFile(attachmentId, userId);
    }

    #region constructors
    public ArticleAttachmentManager(IDataStore dataStore)
    {
      _DataStore = dataStore;
      _CMSFileManager = new CMSFileManager(dataStore);
    }
    #endregion
  }
}
