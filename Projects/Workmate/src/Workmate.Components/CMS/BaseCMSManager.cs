using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Data;
using log4net;

namespace Workmate.Components.CMS
{
  public class BaseCMSManager
  {
    #region members
    protected CMSContentLevelNodeManager _CMSContentLevelNodeManager;
    protected CMSGroupManager _CMSGroupManager;
    protected CMSContentManager _CMSContentManager;
    protected CMSThreadManager _CMSThreadManager;
    protected CMSSectionManager _CMSSectionManager;
    protected CMSTagManager _CMSTagManager;
    protected IDataStore _DataStore;

    protected ILog _Log = LogManager.GetLogger("BaseCMSManager");
    #endregion

    #region constructors
    internal BaseCMSManager(IDataStore dataStore)
    {
      _DataStore = dataStore;

      _CMSContentManager = new CMSContentManager(dataStore);
      _CMSThreadManager = new CMSThreadManager(dataStore);
      _CMSSectionManager = new CMSSectionManager(dataStore);
      _CMSGroupManager = new CMSGroupManager(dataStore);
      _CMSTagManager = new CMSTagManager(dataStore);
      _CMSContentLevelNodeManager = new CMSContentLevelNodeManager(dataStore);
    }
    #endregion
  }
}
