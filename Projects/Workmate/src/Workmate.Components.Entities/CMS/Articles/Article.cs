using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CommonTools.Components.BusinessTier;
using CommonTools.Extensions;
using Workmate.Components.Contracts.CMS.Articles;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Entities.CMS.Articles
{
  public class Article
  {
    private CMSContent _CMSContent;
    internal CMSContent CMSContent { get { return _CMSContent; } }

    private List<KeyValuePair<string, string>> _TripleTags;
    private List<string> _Tags;
    private List<string> _ContentLevelNodes;    

    public int ArticleId
    {
      get { return _CMSContent.CMSContentId; }
      internal set { _CMSContent.CMSContentId = value; }
    }
    public int ArticleGroupThreadId
    {
      get { return _CMSContent.CMSThreadId; }
      internal set { _CMSContent.CMSThreadId = value; }
    }
    public ArticleType ArticleType
    {
      get { return (ArticleType)_CMSContent.CMSContentType; }
      set { _CMSContent.CMSContentType = (byte)value; }
    }
    public ArticleStatus ArticleStatus
    {
      get { return (ArticleStatus)_CMSContent.CMSContentStatus; }
      set { _CMSContent.CMSContentStatus = (byte)value; }
    }
    public List<string> Tags
    {
      get { return this._Tags; }
      set { this._Tags = value; }
    }
    public List<string> ContentLevelNodeNames
    {
      get { return this._ContentLevelNodes; }
      set { this._ContentLevelNodes = value; }
    }

    #region standard properties
    public int AuthorUserId
    {
      get { return _CMSContent.AuthorUserId; }
      set { _CMSContent.AuthorUserId = value; }
    }
    [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)]
    public string Subject
    {
      get { return _CMSContent.Subject; }
      set { _CMSContent.Subject = value; }
    }
    [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true, DefuseScriptTags = false, RemoveBadSQLCharacters = false)]
    public string FormattedBody
    {
      get { return _CMSContent.FormattedBody; }
      set { _CMSContent.FormattedBody = value; }
    }

    public DateTime DateCreatedUtc
    {
      get { return _CMSContent.DateCreatedUtc; }
    }
    public bool IsApproved
    {
      get { return _CMSContent.IsApproved; }
      set { _CMSContent.IsApproved = value; }
    }
    public bool IsLocked
    {
      get { return _CMSContent.IsLocked; }
      set { _CMSContent.IsLocked = value; }
    }
    public int TotalViews
    {
      get { return _CMSContent.TotalViews; }
      internal set { _CMSContent.TotalViews = value; }
    }
    public string UrlFriendlyName
    {
      get { return _CMSContent.UrlFriendlyName; }
      set { _CMSContent.UrlFriendlyName = value; }
    }
    #endregion

    public ArticleGroupThread ArticleGroupThread { get; private set; }

    public List<KeyValuePair<string, string>> GetTripleTags()
    {
      if (this._TripleTags == null)
      {
        this._TripleTags = this.GetTripleTags(this.Tags);
      }
      return this._TripleTags;
    }
    public void SetTripleTags(List<KeyValuePair<string, string>> tripleTags)
    {
      this._TripleTags = tripleTags;
      this.RemoveTripleTagsFromTagCollection();
      this.AddTripleTagsToTagCollection(this._TripleTags);
    }
    private string GetFormattedTripleTag(string key, string value)
    {
      return TagHelper.GetFormattedTripleTag(key, value);
    }
    private void RemoveTripleTagsFromTagCollection()
    {
      foreach (string current in
        from c in this.Tags
        where TagHelper.TripleTagRegex.IsMatch(c)
        select c)
      {
        this.Tags.Remove(current);
      }
    }
    private void AddTripleTagsToTagCollection(List<KeyValuePair<string, string>> tripleTags)
    {
      foreach (KeyValuePair<string, string> current in tripleTags)
      {
        string formattedTripleTag = this.GetFormattedTripleTag(current.Key, current.Value);
        if (TagHelper.TripleTagRegex.IsMatch(formattedTripleTag) && !this.Tags.Contains(formattedTripleTag))
        {
          this.Tags.Add(formattedTripleTag);
        }
      }
    }
    private List<KeyValuePair<string, string>> GetTripleTags(List<string> tags)
    {
      List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
      foreach (string current in tags)
      {
        KeyValuePair<string, string>? tripleTagKeyValuePair = TagHelper.GetTripleTagKeyValuePair(current);
        if (tripleTagKeyValuePair.HasValue)
        {
          list.Add(tripleTagKeyValuePair.Value);
        }
      }
      return list;
    }
    public Article(IUserBasic author, ArticleGroupThread articleGroupThread, ArticleStatus articleStatus, ArticleType articleType, string subject, string formattedBody, string urlFriendlyName, bool isApproved)
      : this(author, articleGroupThread, articleStatus, articleType, subject, formattedBody, urlFriendlyName, isApproved, null, null, null)
    { }

    public Article(IUserBasic author, ArticleGroupThread articleGroupThread, ArticleStatus articleStatus, ArticleType articleType, string subject, string formattedBody, string urlFriendlyName, bool isApproved, List<string> tags, List<KeyValuePair<string, string>> tripleTags, List<string> contentLevelNodeNames)
    {
      this._CMSContent = new CMSContent(author.UserId, articleGroupThread.CMSThread, (byte)articleStatus
        , (byte)articleType, subject, formattedBody, isApproved);

      this.ArticleGroupThread = articleGroupThread;

      _CMSContent.CMSExtraInfo = new XElement("i");
      _CMSContent.CMSParentContentId = null;
      _CMSContent.CMSContentLevel = 0;
      _CMSContent.UrlFriendlyName = urlFriendlyName;

      this.ContentLevelNodeNames = (contentLevelNodeNames ?? new List<string>());
      this.Tags = (tags ?? new List<string>());
      if (tripleTags != null)
      {
        this.AddTripleTagsToTagCollection(tripleTags);
      }
    }
    internal Article(CMSContent cmsContent, CMSThread cmsThread, CMSSection cmsSection, List<string> tags, List<string> contentLevelNodeNames)
    {
      this._CMSContent = cmsContent;

      if (cmsThread != null)
        this.ArticleGroupThread = new ArticleGroupThread(cmsThread, cmsSection);

      if (cmsContent.CMSExtraInfo == null)
      {
        cmsContent.CMSExtraInfo = new XElement("i");
      }

      this.ContentLevelNodeNames = (contentLevelNodeNames ?? new List<string>());
      this.Tags = (tags ?? new List<string>());
      List<KeyValuePair<string, string>> tripleTags = new List<KeyValuePair<string, string>>();
      List<string> tags2 = new List<string>();
      TagHelper.FormatTags(this.Tags, out tags2, out tripleTags);
      this.Tags = tags2;
      this._TripleTags = tripleTags;
      this.AddTripleTagsToTagCollection(this._TripleTags);
    }
  }
}
