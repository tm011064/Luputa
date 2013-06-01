using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.WebPages;
using System.IO;
using System.Text.RegularExpressions;
using Workmate.Web.Components.Application;
using Workmate.Web.Components.Application.Sitemaps;
using Workmate.Components.Contracts.Membership;
using Workmate.Components.Entities.Membership;
using System.Configuration;

namespace Workmate.Web.Components
{
  public abstract class BaseWebViewPage<T> : WebViewPage<T>
  {
    #region members
    protected RequestContextData _RequestContextData;
    #endregion

    #region properties
    public string Theme { get { return _RequestContextData.Theme; } }
    public string ApplicationName { get { return _RequestContextData.ApplicationThemeInfo.ApplicationName; } }
    public int ApplicationId { get { return _RequestContextData.ApplicationThemeInfo.ApplicationId; } }

    private IUserBasic _User;
    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    /// <value>The user.</value>
    public new IUserBasic User
    {
      get
      {
        if (_User == null)
        {
          _User = this.Context.User.Identity as IUserBasic;
        }

        return _User;
      }
    }

    public MenuInfo MenuInfo { get { return _RequestContextData.MenuInfo; } }

    public IApplicationContext ApplicationContext
    {
      get { return InstanceContainer.ApplicationContext; }
    }
    #endregion

    #region methods
    public string FormatRouteName(string routeName)
    {
      return MagicStrings.FormatRouteName(_RequestContextData.ApplicationThemeInfo.ApplicationGroup, routeName);
    }

    public string GetContent(string key)
    {
      return _RequestContextData.StaticContentLookup.GetContent(this.Theme, key);
    }

    public Menu GetMenu(string name)
    {
      return _RequestContextData.SitemapLookup.GetMenu(name);
    }

    public string GetThemedViewPath(string localPath)
    {
      return _RequestContextData.ThemeFolderLookup.GetViewPath(_RequestContextData.Theme, localPath);
    }

    public string GetThemedPath(string localPath)
    {
      return _RequestContextData.ThemeFolderLookup.GetVirtualThemePath(_RequestContextData.Theme, localPath);
    }

    public string GetProfileImage(int imageId)
    {
      return MagicStrings.FormatProfileImagePath(ProfileImageSize.Normal, imageId, _RequestContextData.ApplicationThemeInfo.Images.DefaultThemeImageFolderRootUrl);
    }

    public string GetTinyProfileImage(int imageId)
    {
      return MagicStrings.FormatProfileImagePath(ProfileImageSize.Tiny, imageId, _RequestContextData.ApplicationThemeInfo.Images.DefaultThemeImageFolderRootUrl);
    }

    #region headertags
    public void RenderCssIncludes()
    {
      StringWriter sw = this.Output as StringWriter;
      StringBuilder sb = sw.GetStringBuilder();

      foreach (SitemapItem sitemapItem in _RequestContextData.CssIncludes.Values)
      {
        sb.AppendLine(@"<link href=""" + this.Url.Content(this.GetThemedPath(sitemapItem.FormattedPath)) + @""" rel=""stylesheet"" type=""text/css"" />");
      }
    }
    public void RenderJavascriptIncludes()
    {
      StringWriter sw = this.Output as StringWriter;
      StringBuilder sb = sw.GetStringBuilder();

      foreach (SitemapItem sitemapItem in _RequestContextData.JavascriptIncludes.Values)
      {
        sb.AppendLine(@"<script src=""" + this.Url.Content(
          string.IsNullOrEmpty(sitemapItem.Url)
            ? this.GetThemedPath(sitemapItem.FormattedPath)
            : sitemapItem.Url) + @""" type=""text/javascript""></script>");
      }
    }

    public void RegisterCssInclude(string name)
    {
      SitemapItem sitemapItem = _RequestContextData.SitemapLookup.GetSitemapItem(SitemapItemType.CSS, name);
      if (sitemapItem != null)
      {
        SortedDictionary<int, SitemapItem> dict = _RequestContextData.CssIncludes;
        if (!dict.ContainsKey(sitemapItem.Index))
        {
          dict.Add(sitemapItem.Index, sitemapItem);
        }
      }
    }
    public void RegisterJavascriptInclude(string name)
    {
      SitemapItem sitemapItem = _RequestContextData.SitemapLookup.GetSitemapItem(SitemapItemType.Javascript, name);
      if (sitemapItem != null)
      {
        SortedDictionary<int, SitemapItem> dict = _RequestContextData.JavascriptIncludes;
        if (!dict.ContainsKey(sitemapItem.Index))
        {
          dict.Add(sitemapItem.Index, sitemapItem);
        }
      }
    }

    public void RegisterJavascript(Func<string, HelperResult> helper)
    {
      _RequestContextData.JavascriptMarkup.Append(helper(null));
    }

    public void RenderJavascriptMarkup()
    {
      StringWriter sw = this.Output as StringWriter;
      StringBuilder sb = sw.GetStringBuilder();

      sb.Append(_RequestContextData.JavascriptMarkup);
    }
    #endregion

    #endregion

    #region overrides

    protected override void InitializePage()
    {
      base.InitializePage();

      this._RequestContextData = InstanceContainer.RequestHelper.GetRequestContextData(this.Request);
    }
    #endregion
  }
}
