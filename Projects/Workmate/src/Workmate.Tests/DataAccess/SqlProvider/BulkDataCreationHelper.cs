using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Tests.DataAccess.SqlProvider.Components.Membership;
using Workmate.Components.Entities.Membership;
using NUnit.Framework;
using Workmate.Components;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Tests.DataAccess.SqlProvider
{
  class BulkDataCreationHelper : TestSetup
  {
    List<IUserBasic> GenerateUsers(int totalUsersToCreate)
    {
      List<IUserBasic> records = new List<IUserBasic>();
      for (int i = 0; i < totalUsersToCreate; i++)
      {
        records.Add(Test_WorkmateMembershipProvider.CreateUser(this.DataStore
          , InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager));
      }
      return records;
    }

    void GenerateUsers()
    {
      Setup();

      GenerateUsers(1000);
    }
  }
}
