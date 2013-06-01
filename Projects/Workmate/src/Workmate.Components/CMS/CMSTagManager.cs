using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Data;
using log4net;

namespace Workmate.Components.CMS
{
  public class CMSTagManager
  {
    #region members
    protected IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("CMSTagManager");
    #endregion

    #region public methods
    public List<string> GetTagsByContentId(int contentId)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_Tags_GetByContentId(contentId).ToList();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_Threads_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    #endregion

    #region constructors
    internal CMSTagManager(IDataStore dataStore)
    {
      _DataStore = dataStore;
    }
    #endregion
  }
}
