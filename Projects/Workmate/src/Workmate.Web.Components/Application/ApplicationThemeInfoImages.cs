using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Web.Components.Application
{
  public class ApplicationThemeInfoImages : IApplicationThemeInfoImages
  {
    public int MaleSystemProfileImageId { get; set; }
    public int FemaleSystemProfileImageId { get; set; }

    public string ImageFolderServerPath { get; set; }

    private string _ImageFolderRootUrl;
    public string ImageFolderRootUrl
    {
      get { return _ImageFolderRootUrl; }
      set
      {
        _ImageFolderRootUrl = value;
        this.HasCustomImageFolderRootUrl = !string.IsNullOrWhiteSpace(value);
      }
    }
    public bool HasCustomImageFolderRootUrl { get; private set; }

    public string DefaultThemeImageFolderServerPath { get; set; }

    private string _DefaultThemeImageFolderRootUrl;
    public string DefaultThemeImageFolderRootUrl
    {
      get { return _DefaultThemeImageFolderRootUrl; }
      set
      {
        _DefaultThemeImageFolderRootUrl = value;
        this.HasCustomDefaultThemeImageFolderRootUrl = !string.IsNullOrWhiteSpace(value);
      }
    }
    public bool HasCustomDefaultThemeImageFolderRootUrl { get; private set; }
  }
}
