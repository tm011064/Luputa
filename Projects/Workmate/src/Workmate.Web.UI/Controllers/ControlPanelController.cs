using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Workmate.Web.Components;
using System.Web.Routing;
using Workmate.Web.UI.Models;
using Workmate.Components.Contracts.Membership;
using Workmate.Web.UI.Models.Account;
using Workmate.Web.Components.Security;
using Workmate.Components.Contracts.Company;
using Workmate.Components.Entities.Company;
using Workmate.Web.UI.Models.ControlPanel.Departments;

namespace Workmate.Web.Controllers
{
  public class ControlPanelController : BaseController
  {
    public ActionResult Index()
    {
      return View();
    }
    public ActionResult Accounts()
    {
      return View();
    }
    public ActionResult AnnualLeave()
    {
      return View();
    }
    public ActionResult Wiki()
    {
      return View();
    }
    public ActionResult License()
    {
      return View();
    }

    #region departments
    public ActionResult Departments_Index()
    {
      return View(new DepartmentIndexModel(
        InstanceContainer.DepartmentManager.GetDepartments(this.RequestContextData.ApplicationThemeInfo.ApplicationId)));
    }
    [HttpPost]
    public ActionResult Departments_Index(List<IDepartmentModel> models)
    {
      var report = InstanceContainer.DepartmentManager.Create(this.ApplicationId, models[0]);
      if (report.Status == Workmate.Components.Contracts.DataRepositoryActionStatus.Success)
      {
        var departments = InstanceContainer.DepartmentManager.GetDepartments(this.RequestContextData.ApplicationThemeInfo.ApplicationId);
        return View(departments);
      }

      // TODO (Roman): error message...
      return View();
    }


    public ActionResult Departments_View(int? departmentId)
    {
      return View();
    }
    public ActionResult Departments_Edit(int? departmentId)
    {
      return View();
    }
    public ActionResult Departments_Delete(int? departmentId)
    {
      return View();
    }
    public ActionResult Departments_Create()
    {
      return View();
    }
    #endregion

    #region offices
    public ActionResult Offices_Index()
    {
      return View(InstanceContainer.OfficeManager.GetOffices(this.RequestContextData.ApplicationThemeInfo.ApplicationId));
    }
    public ActionResult Offices_View(int? officeId)
    {
      return View();
    }
    public ActionResult Offices_Edit(int? officeId)
    {
      if (!officeId.HasValue)
        return RedirectToAction("PageNotFound", "Home");

      IOfficeModel record = InstanceContainer.OfficeManager.GetOffice(this.ApplicationId, officeId.Value);
      if (record == null)
        return RedirectToAction("PageNotFound", "Home");

      return View(record);
    }
    [HttpPost]
    public ActionResult Offices_Edit(OfficeModel model)
    {
      // TODO (Roman): validation...
      InstanceContainer.OfficeManager.Update(model);

      return RedirectToAction("Offices_Index", "ControlPanel");
    }
    public ActionResult Offices_Delete(int? officeId)
    {
      if (!officeId.HasValue)
        return RedirectToAction("PageNotFound", "Home");

      IOfficeModel record = InstanceContainer.OfficeManager.GetOffice(this.ApplicationId, officeId.Value);
      if (record == null)
        return RedirectToAction("PageNotFound", "Home");

      return View(record);
    }
    [HttpPost]
    public ActionResult Offices_Delete(OfficeModel model)
    {
      // TODO (Roman): validation...
      InstanceContainer.OfficeManager.Delete(this.ApplicationId, model.OfficeId);
      return RedirectToAction("Offices_Index", "ControlPanel");
    }
    public ActionResult Offices_Create()
    {
      return View();
    }
    [HttpPost]
    public ActionResult Offices_Create(OfficeModel model)
    {
      // TODO (Roman): validation...
      var report = InstanceContainer.OfficeManager.Create(this.ApplicationId, model);
      if (report.Status == Workmate.Components.Contracts.DataRepositoryActionStatus.Success)
      {
        return RedirectToAction("Offices_Index", "ControlPanel");
      }
      else
      {
        // TODO (Roman): do this properly
        model.ErrorMessage = report.Message;
      }

      return View(model);
    }
    #endregion
  }
   
}
