using System;
using System.Xml.Linq;
namespace Workmate.Components.Contracts.Company
{
  public interface IDepartmentModel : IBaseModel
  {
    int ApplicationId { get; set; }
    int DepartmentId { get; set; }
    string Name { get; set; }

    int? OfficeId { get; set; }
    int? ParentDepartmentId { get; set; }

    IDepartmentModel ParentDepartment { get; set; }
    IOfficeModel Office { get; set; }
  }
}
