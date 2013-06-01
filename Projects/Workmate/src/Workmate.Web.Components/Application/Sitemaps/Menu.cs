using System.Collections.Generic;

namespace Workmate.Web.Components.Application.Sitemaps
{
  public class Menu
  {
    private Dictionary<string, MenuItem> _MenuItems = new Dictionary<string, MenuItem>();
    private Dictionary<string, MenuItem> _DropdownMenuItems = new Dictionary<string, MenuItem>();

    public string Name { get; set; }
    public IEnumerable<MenuItem> MenuItems { get { return _MenuItems.Values; } }

    public MenuItem GetMenuItem(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        return MenuItem.Empty;

      if (_MenuItems.ContainsKey(name))
        return _MenuItems[name];

      if (_DropdownMenuItems.ContainsKey(name))
      {// special check for dropdowns
        return _DropdownMenuItems[name];
      }

      return MenuItem.Empty;
    }

    public void AddMenuItem(MenuItem menuItem)
    {
      _MenuItems[menuItem.Name] = menuItem;
      foreach (MenuItem dropdownMenuItem in menuItem.DropdownMenuItems)
        _DropdownMenuItems[dropdownMenuItem.Name] = dropdownMenuItem;
    }

    public Menu()
    {

    }
  }
}
