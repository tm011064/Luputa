using System;
using System.Collections.Generic;
using CommonTools.Components.BusinessTier;

namespace Workmate.Components.Contracts.Membership
{
  public interface IWorkmateRoleProvider
  {
    void CreateRolesIfNotExist(int applicationId, string[] userRoles);
  }
}
