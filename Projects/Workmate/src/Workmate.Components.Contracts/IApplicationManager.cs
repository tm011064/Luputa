using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Components.BusinessTier;

namespace Workmate.Components.Contracts
{
  public interface IApplicationManager
  {
    IApplication GetApplication(string name);
    BusinessObjectActionReport<DataRepositoryActionStatus> Create(IApplication application);
    BusinessObjectActionReport<DataRepositoryActionStatus> Update(IApplication application);
  }
}
