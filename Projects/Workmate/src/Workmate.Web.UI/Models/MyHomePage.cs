using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Workmate.Web.Models
{
  public class MyHomePage
  {
    public string RouteName { get; set; }
    public string RouteController { get; set; }
    public string RouteUrl { get; set; }    
    public string RouteAction { get; set; }
  }
}