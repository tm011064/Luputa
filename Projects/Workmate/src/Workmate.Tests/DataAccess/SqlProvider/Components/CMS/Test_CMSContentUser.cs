using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Workmate.Components.CMS;
using Workmate.Components.Entities.CMS;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Contracts;
using CommonTools.Components.BusinessTier;
using Workmate.Data;
using CommonTools.Components.Testing;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Workmate.Components.Entities.Membership;
using Workmate.Tests.DataAccess.SqlProvider.Components.Membership;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS
{
  [TestFixture]
  public class Test_CMSContentUsers : TestSetup
  {
    internal static void Delete(IDataStore dataStore, CMSContent content, IUserBasic userBasic)
    {
      CMSContentUserManager manager = new CMSContentUserManager(dataStore);
      
      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(new CMSContentUser(userBasic, content));

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Trace.WriteLine("Successfully deleted contentUser " + content.CMSContentId + " -> " + userBasic.UserId);
    }
    
    #region tests
    [Test]
    public void Test_CreateUpdateDeleteContentUser()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      CMSSection section = Test_CMSSections.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, null, this.Random);
      CMSThread thread = Test_CMSThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, section, this.Random);
      CMSContent content = Test_CMSContents.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic.UserId, thread, this.Random);

      CMSContentUserManager manager = new CMSContentUserManager(this.DataStore);

      CMSContentUser record = new CMSContentUser(userBasic, content);
      
      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(record);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      
      Delete(this.DataStore, content, userBasic);
      Test_CMSSections.Delete(this.DataStore, section); // deleting the section should also delete the file
    }

    #endregion
  }
}
