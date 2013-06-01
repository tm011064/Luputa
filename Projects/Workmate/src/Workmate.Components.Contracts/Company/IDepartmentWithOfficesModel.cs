using System;
using System.Xml.Linq;
using System.Collections.Generic;
namespace Workmate.Components.Contracts.Company
{
  public interface IDepartmentWithOfficesModel : IDepartmentModel
  {
    List<IOfficeModel> Offices { get; set; }
  }
}
