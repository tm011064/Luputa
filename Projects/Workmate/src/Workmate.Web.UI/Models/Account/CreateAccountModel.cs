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
  public class CreateAccountModel
  {
    [Email]
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

   // [Required]
    public Gender Gender { get; set; }

    //public SelectList GenderSelectList { get { return _GenderSelectList; } }
    //private SelectList _GenderSelectList = new SelectList(
    //    CommonTools.Core.UtilityHelper.GetEnums<Gender>()
    //    , "Gender"
    //    , "Gender" // TODO (Roman): this should come from a content field
    //    );

    public UserCreateStatus UserCreateStatus { get; set; }
    public string ValidationFailedErrorMessage { get; set; }

    //[Required]
    //[DataType(DataType.Password)]
    //public string Password { get; set; }

    //[Required]
    //[DataType(DataType.Password)]
    //[Compare("Password")]
    //public string ConfirmPassword { get; set; }


    public CreateAccountModel()
    {
      this.UserCreateStatus = UserCreateStatus.Success;
      this.Gender = Gender.Male;
    }
  }
}