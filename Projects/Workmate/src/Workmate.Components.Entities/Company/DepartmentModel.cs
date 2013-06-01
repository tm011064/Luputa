using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CommonTools.Extensions;
using Workmate.Components.Contracts.Company;

namespace Workmate.Components.Entities.Company
{
  public class DepartmentModel : BaseModel, IDepartmentModel
  {
    #region properties

    public int ApplicationId { get; set; }

    public int DepartmentId { get; set; }

    public int? ParentDepartmentId { get; set; }

    public string Name { get; set; }

    public int? OfficeId { get; set; }
    
    public IDepartmentModel ParentDepartment { get; set; }

    public IOfficeModel Office { get; set; }
    
    #endregion

    #region constructors
    public DepartmentModel()
    {

    }

    public DepartmentModel(int applicationId, string name)
    {
      this.ApplicationId = applicationId;
      this.Name = name;
    }
    public DepartmentModel(int applicationId, int departmentId, int? parentDepartmentId, string name, int? officeId)
    {
      this.ApplicationId = applicationId;
      this.DepartmentId = departmentId;
      this.ParentDepartmentId = parentDepartmentId;
      this.Name = name;
      this.OfficeId = officeId;
    } 
    #endregion
  }
}
