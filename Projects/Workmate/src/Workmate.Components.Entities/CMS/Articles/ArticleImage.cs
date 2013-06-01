using Workmate.Components.Entities.CMS;
using System;
using System.Collections.Generic;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Contracts.CMS;
namespace Workmate.Components.Entities.CMS.Articles
{
  public class ArticleImage
  {
    private CMSFile _CMSFile;
    internal CMSFile CMSFile { get { return _CMSFile; } }

    private List<string> _Tags;

    public int ApplicationId
    {
      get { return _CMSFile.ApplicationId; }
      private set { _CMSFile.ApplicationId = value; }
    }
    public int ImageId
    {
      get { return _CMSFile.CMSFileId; }
    }
    public int OwnerUserId
    {
      get { return _CMSFile.CMSUserId.Value; }
      private set { _CMSFile.CMSUserId = new int?(value); }
    }
    public int Height
    {
      get { return _CMSFile.CMSHeight; }
      set { _CMSFile.CMSHeight = value; }
    }
    public int Width
    {
      get { return _CMSFile.CMSWidth; }
      set { _CMSFile.CMSWidth = value; }
    }
    public List<string> Tags
    {
      get { return this._Tags; }
      set { this._Tags = value; }
    }

    protected internal ArticleImage(CMSFile cmsFile)
      : this(cmsFile, new List<string>())
    { }
    protected internal ArticleImage(CMSFile cmsFile, List<string> tags)
    {
      this._CMSFile = cmsFile;

      this.Tags = (tags ?? new List<string>());
    }
    public ArticleImage(int applicationId, UserBasic owner)
    {
      this._CMSFile = new CMSFile(applicationId, owner, FileType.ArticleImage);

      this.OwnerUserId = owner.UserId;
      this.Tags = new List<string>();
    }
  }
}
