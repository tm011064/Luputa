using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Web.Components.Application.Sitemaps;

namespace Workmate.Web.Components.Application
{
  public interface IThemeFolderLookup
  {
    string GetViewPath(string theme, string viewPath);
    string GetVirtualThemePath(string theme, string filePath);
    string GetAbsoluteThemePath(string theme, string filePath);
    bool DoesFileExist(string theme, string filePath);
    string[] ThemeNames { get; }
  }
}
