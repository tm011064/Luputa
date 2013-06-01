using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Workmate.Components.Entities.Company;
using Workmate.Components.Contracts.Company;

namespace Workmate.Web.UI.Models.ControlPanel.Departments
{
  public class DepartmentIndexModel
  {
    public DepartmentModel Department { get; set; }
    public IEnumerable<IDepartmentModel> Departments { get; set; }
    
    public DepartmentIndexModel(IEnumerable<IDepartmentModel> departments)
    {
      this.Departments = departments;
    }
    public DepartmentIndexModel()
    {
      this.Departments = new List<IDepartmentModel>();
    }
  }
}