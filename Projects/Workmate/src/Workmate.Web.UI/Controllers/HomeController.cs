using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Workmate.Web.Components;
using System.Web.Routing;
using Workmate.Web.Components.Security;
using Workmate.Web.UI.Models;

namespace Workmate.Web.Controllers
{
  public class HomeController : BaseController
  {
    //
    // GET: /MyHomePage/
    [AllowAnonymous]
    public ActionResult Index()
    {
      return View();
    }
    
    [AllowAnonymous]
    public ActionResult AccessDenied()
    {
      return View();
    }

    [AllowAnonymous]
    public ActionResult PageNotFound()
    {
      return View();
    } 

    [AllowAnonymous]
    [HttpPost]
    public ActionResult ClearCache()
    {
      // TODO (Roman): remove this later on
      InstanceContainer.ApplicationContext.ApplicationMessageHandler.Publish(
        Messaging.Contracts.MessageType.RefreshApplicationDataRequest);

      return Index();
    }


    [HttpPost]
    public ActionResult FindUser(TopMenuModel model)
    {
      return Index();
    }
  }
}
