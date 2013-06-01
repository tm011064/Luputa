using System.Collections.Generic;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Web.Components.Application.Sitemaps
{
  public class MenuItem
  {
    public string Name { get; set; }
    public string RouteName { get; set; }

    public string Action { get; set; }
    public string Controller { get; set; }

    public string ContentKey { get; set; }
    public string LicenseKey { get; set; }

    public HashSet<string> AllowedRoles { get; set; }
    public bool HasUserRoleRestrictions { get; set; }

    public MenuItem Parent { get; set; }
    public List<MenuItem> Children { get; set; }

    public int Level { get; set; }
    public bool ShowInNavPills { get; set; }

    public List<MenuItem> DropdownMenuItems { get; set; }
    public bool HasDropdownMenuItems { get; set; }

    #region methods
    public bool ContainsAllowedRole(HashSet<string> roles)
    {
      foreach (string role in roles)
        if (this.AllowedRoles.Contains(role))
          return true;

      return false; 
    }
    #endregion

    #region constructors
    private static MenuItem _Empty;
    static MenuItem()
    {
      _Empty = new MenuItem();
    }
    public static MenuItem Empty { get { return _Empty; } }

    public MenuItem()
    {
      this.Children = new List<MenuItem>();
      this.DropdownMenuItems = new List<MenuItem>();
      this.AllowedRoles = new HashSet<string>();
    }
    #endregion
  }
}
