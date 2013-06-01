using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Contracts;

namespace Workmate.Web.Components.Application
{
  public class ApplicationThemeInfo : IApplicationThemeInfo
  {
    public string ApplicationName { get; set; }
    public string ApplicationGroup { get; set; }
    public string DomainName { get; set; }
    public int ApplicationId { get; set; }
    public string ThemeName { get; set; }
    public bool IsDefault { get; set; }
    public string DefaultThemeName { get; set; }

    public IApplication Application { get; set; }
    
    public IApplicationThemeInfoImages Images { get; set; }
  }
}
