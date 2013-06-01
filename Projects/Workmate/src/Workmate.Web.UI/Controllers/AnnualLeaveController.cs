using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Workmate.Web.Components;
using System.Web.Routing;
using Workmate.Web.Components.Security;

namespace Workmate.Web.Controllers
{
  public class AnnualLeaveController : BaseController
  {
    public ActionResult Index()
    {
      return View();
    }
    public ActionResult Settings()
    {
      return View();
    }
    public ActionResult History()
    {
      return View();
    }
    public ActionResult Bookings()
    {
      return View();
    }


  }
}
