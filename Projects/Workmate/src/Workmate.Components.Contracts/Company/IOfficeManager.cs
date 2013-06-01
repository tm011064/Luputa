using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Components.BusinessTier;

namespace Workmate.Components.Contracts.Company
{
  public interface IOfficeManager
  {
    BusinessObjectActionReport<DataRepositoryActionStatus> Create(int applicationId, IOfficeModel office);
    int Delete(int applicationId, int officeId);
    IOfficeModel GetOffice(int applicationId, int officeId);
    IEnumerable<IOfficeModel> GetOffices(int applicationId);
    BusinessObjectActionReport<DataRepositoryActionStatus> Update(IOfficeModel office);
  }
}
