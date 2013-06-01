using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Web.Components.Application.Sitemaps;
using Workmate.Messaging;
using Workmate.Configuration;
using Workmate.Components.Contracts.CMS.Articles;

namespace Workmate.Web.Components.Application
{
  public interface IApplicationThemeInfoImages
  {
    int MaleSystemProfileImageId { get; set; }
    int FemaleSystemProfileImageId { get; set; }

    string ImageFolderServerPath { get; }
    string ImageFolderRootUrl { get; }
    string DefaultThemeImageFolderServerPath { get; }
    string DefaultThemeImageFolderRootUrl { get; }

    bool HasCustomImageFolderRootUrl { get; }
    bool HasCustomDefaultThemeImageFolderRootUrl { get; }
  }
}
