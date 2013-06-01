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
  public class SearchAccountModel
  {
    // TODO (Roman): min length
    [Required]
    public string Search { get; set; }

    public int PageIndex { get; set; }
    public int RowCount { get; set; }

    //public string DepartmentFilter { get; set; }
    //public string LanguageFilter { get; set; }

    public List<IBaseUserModel> BaseUserModels { get; set; }

    public SearchAccountModel() : this(null) { }
    public SearchAccountModel(List<IBaseUserModel> baseUserModels)
    {
      this.BaseUserModels = baseUserModels ?? new List<IBaseUserModel>();
    }
  }
}