using Workmate.Components.Contracts;
namespace Workmate.Web.Components.Application
{
  public interface IApplicationThemeInfo
  {
    IApplication Application { get; set; }
    int ApplicationId { get; set; }
    string ApplicationName { get; }

    string ApplicationGroup { get; }
    string DomainName { get; }
    string ThemeName { get; }
    bool IsDefault { get; }
    string DefaultThemeName { get; }

    IApplicationThemeInfoImages Images { get; }
  }
}
