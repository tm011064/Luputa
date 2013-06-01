using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Workmate.Web.Components.Validation;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Web.UI.Models.Account
{
  public class ViewAccountModel
  {
    public IUserModel User { get; set; }

    public ViewAccountModel(IUserModel user)
    {
      this.User = user;
    }
  }
}