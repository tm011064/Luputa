using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Components.Entities.CMS.Membership
{
  public class SystemProfileImage
  {
    protected CMSFile _CMSFile;
    internal CMSFile CMSFile { get { return _CMSFile; } }

    public int ApplicationId
    {
      get { return _CMSFile.ApplicationId; }
      private set { _CMSFile.ApplicationId = value; }
    }
    public int ImageId
    {
      get { return _CMSFile.CMSFileId; }
      private set { _CMSFile.CMSFileId = value; }
    }
    public DateTime DateCreatedUtc
    {
      get { return _CMSFile.DateCreatedUtc; }
      private set { _CMSFile.DateCreatedUtc = value; }
    }
    public byte[] Content
    {
      get { return _CMSFile.Content; }
      set { _CMSFile.Content = value; }
    }
    public string ContentType
    {
      get { return _CMSFile.ContentType; }
      set { _CMSFile.ContentType = value; }
    }
    public int ContentSize
    {
      get { return _CMSFile.ContentSize; }
      set { _CMSFile.ContentSize = value; }
    }
    public bool IsTemporary
    {
      get { return _CMSFile.IsTemporary; }
      set { _CMSFile.IsTemporary = value; }
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
    public string FriendlyFileName
    {
      get { return _CMSFile.FriendlyFileName; }
      set { _CMSFile.FriendlyFileName = value; }
    }
    
    public SystemProfileImage(int applicationId)
    {
      this._CMSFile = new CMSFile(applicationId, null, Contracts.CMS.FileType.SystemProfileImage);

      this.IsTemporary = true;
    }
    protected SystemProfileImage(int applicationId, IUserBasic user, FileType fileType)
    {
      this._CMSFile = new CMSFile(applicationId, user, fileType);

      this.IsTemporary = true;
    }
    internal SystemProfileImage(CMSFile cmsFile)
    {
      this._CMSFile = cmsFile;
    }
  }
}
