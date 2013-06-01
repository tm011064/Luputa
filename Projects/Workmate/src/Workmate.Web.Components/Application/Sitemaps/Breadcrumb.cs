using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Web.Components.Application.Sitemaps
{
  public class Breadcrumb
  {
    public string RouteName { get; set; }

    public string Action { get; set; }
    public string Controller { get; set; }

    public string ContentKey { get; set; }

    public List<Breadcrumb> Parents { get; set; }

    #region constructors
    public Breadcrumb(string routeName, string action, string controller, string contentKey)
    {
      this.RouteName = routeName;
      this.Action = action;
      this.Controller = controller;
      this.ContentKey = contentKey;

      this.Parents = new List<Breadcrumb>();
    }
    #endregion
  }
}
