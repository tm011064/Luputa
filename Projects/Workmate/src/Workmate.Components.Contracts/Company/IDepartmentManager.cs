using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Components.BusinessTier;

namespace Workmate.Components.Contracts.Company
{
  public interface IDepartmentManager
  {
    BusinessObjectActionReport<DataRepositoryActionStatus> Create(int applicationId, IDepartmentModel department);
    int Delete(int applicationId, int departmentId);
    IDepartmentWithOfficesModel GetDepartment(int applicationId, int departmentId);
    IEnumerable<IDepartmentModel> GetDepartments(int applicationId);
    BusinessObjectActionReport<DataRepositoryActionStatus> Update(IDepartmentModel department);
  }
}
