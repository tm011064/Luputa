using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Data;
using log4net;
using Workmate.Components.Contracts.CMS;
using System.Web;

namespace Workmate.Components.CMS
{
  public class CMSContentLevelNodeManager
  {
    #region members
    protected IDataStore _DataStore;
    private ILog _Log = LogManager.GetLogger("CMSContentLevelNodeManager");
    #endregion

    #region public methods
    /// <summary>
    /// Gets the content level nodes.
    /// </summary>
    /// <param name="level">The level.</param>
    /// <param name="parentContentLevelNodeId">The parent content level node id.</param>
    /// <returns></returns>
    public List<ICMSContentLevelNode> GetContentLevelNodes(int level, int? parentContentLevelNodeId, int? threadId, int? sectionId)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          return dataStoreContext.cms_ContentLevelNodes_Get(level, parentContentLevelNodeId, threadId, sectionId).ToList();
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_ContentLevelNodes_Get", ex);
        throw new DataStoreException(ex, true);
      }
    }
    /// <summary>
    /// Gets the content level nodes.
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, ICMSContentLevelNode> GetContentLevelNodes()
    {
      Dictionary<int, ICMSContentLevelNode> records = new Dictionary<int, ICMSContentLevelNode>();
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          foreach (ICMSContentLevelNode record in dataStoreContext.cms_ContentLevelNodes_Get())
            records[record.ContentLevelNodeId] = record;
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_ContentLevelNodes_Get", ex);
        throw new DataStoreException(ex, true);
      }

      foreach (ICMSContentLevelNode record in records.Values)
      {
        if (record.ParentContentLevelNodeId.HasValue)
        {
          record.Parent = records[record.ParentContentLevelNodeId.Value];
          records[record.ParentContentLevelNodeId.Value].Children.Add(record);
        }
      }

      return records;
    }

    /// <summary>
    /// Creates the content level nodes.
    /// </summary>
    /// <param name="contentLevelNodeNames">The content level node names.</param>
    /// <returns></returns>
    public bool CreateContentLevelNodes(IEnumerable<string> contentLevelNodeNames, int? threadId, int? sectionId, out int topContentLevelNodeId)
    {
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          topContentLevelNodeId = dataStoreContext.cms_ContentLevelNodes_Insert(contentLevelNodeNames, threadId, sectionId);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_ContentLevelNodes_Insert", ex);
        throw new DataStoreException(ex, true);
      }

      return topContentLevelNodeId > 0;
    }

    /// <summary>
    /// Deletes the content level node.
    /// </summary>
    /// <param name="contentLevelNodeId">The content level node id.</param>
    /// <returns></returns>
    public bool DeleteContentLevelNode(int contentLevelNodeId)
    {
      int num;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          num = dataStoreContext.cms_ContentLevelNodes_Delete(contentLevelNodeId);
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_ContentLevelNodes_Delete", ex);
        throw new DataStoreException(ex, true);
      }

      return num > 0;
    }

    /// <summary>
    /// Renames the content level node.
    /// </summary>
    /// <param name="contentLevelNodeId">The content level node id.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public bool RenameContentLevelNode(int contentLevelNodeId, string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        return false;

      int num;
      try
      {
        using (IDataStoreContext dataStoreContext = this._DataStore.CreateContext())
        {
          num = dataStoreContext.cms_ContentLevelNodes_Update(contentLevelNodeId, HttpUtility.HtmlEncode(name));
        }
      }
      catch (Exception ex)
      {
        _Log.Error("Error at cms_ContentLevelNodes_Update", ex);
        throw new DataStoreException(ex, true);
      }

      if (num != 0)
      {
        _Log.ErrorFormat("Conten level node {0} was not renamed to {2} (ErrorCode: {1})."
          , contentLevelNodeId
          , num
          , name);
        return false;
      }

      return true;
    }
    #endregion

    #region constructors
    internal CMSContentLevelNodeManager(IDataStore dataStore)
    {
      _DataStore = dataStore;
    }
    #endregion
  }
}
