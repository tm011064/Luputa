using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Web.Components.Application.Sitemaps
{
  public class MenuInfo
  {
    public string TopMenuName { get; set; }
    public string TopMenuItemName { get; set; }
    public string TopMenuSubItemName { get; set; }

    #region constructors
    private static MenuInfo _Empty;
    static MenuInfo()
    {
      _Empty = new MenuInfo();
    }
    public static MenuInfo Empty { get { return _Empty; } }
    #endregion
  }
}
