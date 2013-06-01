using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CommonTools.Extensions;
using Workmate.Components.Contracts.Company;

namespace Workmate.Components.Entities.Company
{
  public class DepartmentWithOfficesModel : DepartmentModel, IDepartmentWithOfficesModel
  {
    #region properties

    public List<IOfficeModel> Offices { get; set; }
    
    #endregion

    #region constructors
    public DepartmentWithOfficesModel(IDepartmentModel department, IEnumerable<IOfficeModel> offices)
      : base(department.ApplicationId, department.DepartmentId, department.ParentDepartmentId, department.Name, department.OfficeId)
    {
      this.Office = department.Office;
      this.ParentDepartment = department.ParentDepartment;
      this.Offices = new List<IOfficeModel>(offices);
    } 
    #endregion
  }
}
