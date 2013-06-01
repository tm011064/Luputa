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
using Workmate.Components.Entities.Membership;
using CommonTools.Components.Security;

namespace Workmate.Web.Controllers
{
  public class AccountController : BaseController
  {
    public ActionResult ViewAccount()
    {
      return View(new ViewAccountModel(InstanceContainer.WorkmateMembershipProvider.GetUserModel(this.User.UserId)));
    }
    public ActionResult ViewAccountUser(int? userId)
    {
      IUserModel user = null;
      if (userId.HasValue)
        user = InstanceContainer.WorkmateMembershipProvider.GetUserModel(userId.Value);
      else
        user = InstanceContainer.WorkmateMembershipProvider.GetUserModel(this.User.UserId);

      return View(new ViewAccountModel(user));
    }
    //
    // GET: /Account/
    public ActionResult Search()
    {
      return View(new SearchAccountModel());
    }
    [HttpPost]
    public ActionResult Search(SearchAccountModel searchAccountModel)
    {
      int pageIndex = searchAccountModel.PageIndex;
      int rowCount;

      searchAccountModel.BaseUserModels = InstanceContainer.WorkmateMembershipProvider.GetBaseUserModels(
        this.RequestContextData.ApplicationThemeInfo.ApplicationId
        , searchAccountModel.Search
        , ref pageIndex
        , 50
        , out rowCount);

      searchAccountModel.RowCount = rowCount;
      searchAccountModel.PageIndex = pageIndex;

      ModelState.Remove("Search");

      return View(searchAccountModel);
    }

    //
    // GET: /Account/Login
    [AllowAnonymous]
    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult Login(LoginDetailsModel loginDetailsModel)
    {
      if (ModelState.IsValid)
      {
        // do validation
        IUserBasic userBasic = null;
        ValidateUserStatus status = InstanceContainer.TicketManager.LogUserIn(
          this.RequestContextData.ApplicationThemeInfo.ApplicationId
          , loginDetailsModel.UserName
          , loginDetailsModel.Password
          , loginDetailsModel.RememberMe
          , out userBasic);

        if (status == ValidateUserStatus.Valid)
        {
          Response.Redirect(System.Web.Security.FormsAuthentication.GetRedirectUrl(userBasic.UserName, false));
          return null;
        }
        else
        {
          ModelState.AddModelError("", "The user name or password provided is incorrect.");
        }
      }

      return View(loginDetailsModel);
    }

    [AllowAnonymous]
    public ActionResult Logout()
    {
      InstanceContainer.TicketManager.Signout();
      return RedirectToAction("Index", "Home");
    }


    //
    // GET: /Account/Create
    public ActionResult Create()
    {
      return View(new CreateAccountModel());
    }
    //
    // GET: /Account/Create
    [HttpPost]
    public ActionResult Create(CreateAccountModel model)
    {
      if (ModelState.IsValid)
      {
        int profileImageId = -1;
        switch (model.Gender)
        {
          case Gender.Male: profileImageId = this.RequestContextData.ApplicationThemeInfo.Images.MaleSystemProfileImageId;break;
          case Gender.Female: profileImageId = this.RequestContextData.ApplicationThemeInfo.Images.FemaleSystemProfileImageId;break;
          default: throw new NotImplementedException();
        }

        SimplePassword  simplePassword = new SimplePassword();
        string password = simplePassword.Generate(8, CommonTools.Components.Security.CharacterType.NumbersUpperLowerCase);

        IUserBasic userBasic = new UserBasic(
          model.Email
          , model.Email
          , profileImageId);
        
        Guid uniqueId;

        var report = InstanceContainer.WorkmateMembershipProvider.CreateUser(
          ref userBasic
          , password
          , new List<UserRole>() { UserRole.Registered }
          , UserNameDisplayMode.FullName
          , model.FirstName
          , model.LastName
          , model.Gender
          , out uniqueId
          , this.RequestContextData.ApplicationThemeInfo.ApplicationId);

        switch (report.Status)
        {
          case UserCreateStatus.Success:

            InstanceContainer.EmailPublisher.EnqueueUserCreatedEmail(
              this.RequestContextData.ApplicationThemeInfo.ApplicationId
              , this.RequestContextData.Theme
              , this.RequestContextData.ApplicationThemeInfo.Application.DefaultAdminSenderEmailAddress
              , this.RequestContextData.StaticContentLookup
              , model.FirstName
              , model.LastName
              , model.Email
              , userBasic.UserId             
              , password);

            return RedirectToAction("ViewAccountUser", "Account", new { userId = userBasic.UserId }); // TODO (Roman): congratulations message informing that login details have been sent

          default:
            model.UserCreateStatus = report.Status;
            model.ValidationFailedErrorMessage = report.Message;
            return View(model);
        }
      }

      return View();
    }
  }
}
