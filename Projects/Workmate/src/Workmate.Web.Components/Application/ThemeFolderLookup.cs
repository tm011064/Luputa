using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Workmate.Web.Components.Application
{
  public class ThemeFolderLookup : IThemeFolderLookup
  {
    #region members
    private Dictionary<string, string> _ViewLookup = new Dictionary<string, string>();
    private Dictionary<string, string> _ThemeLookup = new Dictionary<string, string>();

    private Dictionary<string, string> _ThemeByUrlLookup = new Dictionary<string, string>();
    private HashSet<string> _ThemeNames = new HashSet<string>();
    #endregion

    #region public methods
    public string[] ThemeNames
    {
      get { return _ThemeNames.ToArray(); }
    }

    public string GetViewPath(string theme, string viewPath)
    {
      try { return _ViewLookup[(theme + "_" + viewPath).ToLowerInvariant().Trim()]; }
      catch (KeyNotFoundException)
      {
        throw new ApplicationException(string.Format("Theme '{0}' or folder '{1}' not found. Please verify wheter both exist.", theme, viewPath));
      }
    }
    public bool TryGetVirtualThemePath(string theme, string filePath, out string path)
    {
      if (_ThemeLookup.ContainsKey((theme + "_" + filePath).ToLowerInvariant().Trim()))
      {
        path = _ThemeLookup[(theme + "_" + filePath).ToLowerInvariant().Trim()];
        return true;
      }

      path = null;
      return false;
    }
    public string GetVirtualThemePath(string theme, string filePath)
    {
      try { return _ThemeLookup[(theme + "_" + filePath).ToLowerInvariant().Trim()]; }
      catch (KeyNotFoundException)
      {
        throw new ApplicationException(string.Format("Theme '{0}' or folder '{1}' not found. Please verify wheter both exist.", theme, filePath));
      }
    }
    public string GetAbsoluteThemePath(string theme, string filePath)
    {
      try { return _ThemeLookup[(theme + "_" + filePath).ToLowerInvariant().Trim()].Substring(1); }
      catch (KeyNotFoundException)
      {
        throw new ApplicationException(string.Format("Theme '{0}' or folder '{1}' not found. Please verify wheter both exist.", theme, filePath));
      }
    }
    public bool DoesFileExist(string theme, string filePath)
    {
      return _ThemeLookup.ContainsKey((theme + "_" + filePath).ToLowerInvariant().Trim());
    }
    public HashSet<string> GetAllDomainNames()
    {
      return new HashSet<string>(_ThemeByUrlLookup.Keys);
    }

    public void Initialize(
      string physicalApplicationPath
      , string applicationName
      , string defaultThemeName
      , Dictionary<string, string> themeByUrlLookup)
    {
      Dictionary<string, string> dict = new Dictionary<string, string>();
      foreach (string theme in themeByUrlLookup.Keys)
        dict[theme.ToLowerInvariant()] = themeByUrlLookup[theme].ToLowerInvariant();
      _ThemeByUrlLookup = dict;

      defaultThemeName = defaultThemeName.Trim().ToLowerInvariant();

      // views are stored at the View path. This is a hardcoded restriction by MVC, so we can't put them into the theme folder
      RegisterUrls(physicalApplicationPath, "Views", "themes", "*.cshtml", applicationName, defaultThemeName, ref _ViewLookup);

      RegisterUrls(physicalApplicationPath, null, "themes", "*", applicationName, defaultThemeName, ref _ThemeLookup);

      _ThemeNames = new HashSet<string>(_ThemeByUrlLookup.Values);
    }
    #endregion

    #region private methods
    private void RegisterUrls(string physicalApplicationPath
      , string initialFolder
      , string themeFolder
      , string fileFilter
      , string applicationName
      , string defaultThemeName
      , ref Dictionary<string, string> lookupReference)
    {
      if (string.IsNullOrWhiteSpace(initialFolder))
        initialFolder = string.Empty;
      else
        initialFolder = initialFolder.Trim().ToLowerInvariant().Trim('\\').Trim('/') + @"\";

      if (string.IsNullOrWhiteSpace(themeFolder))
        themeFolder = string.Empty;
      else
        themeFolder = themeFolder.Trim().ToLowerInvariant().Trim('\\').Trim('/') + @"\";

      string rootPath = Path.Combine(physicalApplicationPath, initialFolder + themeFolder + applicationName + @"\").ToLowerInvariant();
      DirectoryInfo folder = new DirectoryInfo(rootPath);
      if (!folder.Exists)
        throw new ApplicationException("Folder " + folder.FullName + "doesn't exist");

      string defaultThemePath = Path.Combine(rootPath, defaultThemeName);
      if (!Directory.Exists(defaultThemePath))
        throw new ApplicationException("Default theme folder " + defaultThemePath + "doesn't exist");

      List<string> viewPaths = new List<string>();

      if (string.IsNullOrWhiteSpace(fileFilter))
        fileFilter = "*";

      foreach (FileInfo fileInfo in new DirectoryInfo(defaultThemePath).GetFiles(fileFilter, SearchOption.AllDirectories))
        viewPaths.Add(fileInfo.FullName.ToLowerInvariant());

      Dictionary<string, string> lookup = new Dictionary<string, string>();

      string path;
      string key;
      string themePath;

      HashSet<string> themeKeys = new HashSet<string>(_ThemeByUrlLookup.Values);
      foreach (DirectoryInfo directoryInfo in folder.GetDirectories())
        themeKeys.Add(directoryInfo.Name.ToLowerInvariant());

      foreach (string theme in themeKeys)
      {
        themePath = Path.Combine(rootPath, theme);

        foreach (string record in viewPaths)
        {
          path = record.Replace(defaultThemePath, themePath);
          key = theme + "_" + path.Replace(themePath + @"\", string.Empty)
                                  .Replace(@"\", "/");

          if (File.Exists(path))
            lookup.Add(key, path.Replace(themePath, "~/" + initialFolder + themeFolder + applicationName + "/" + theme).Replace(@"\", "/"));
          else
            lookup.Add(key, record.Replace(defaultThemePath, "~/" + initialFolder + themeFolder + applicationName + "/" + defaultThemeName).Replace(@"\", "/"));
        }
      }

      lookupReference = lookup;
    }
    #endregion

    #region constructors
    public ThemeFolderLookup() { }
    #endregion
  }
}
