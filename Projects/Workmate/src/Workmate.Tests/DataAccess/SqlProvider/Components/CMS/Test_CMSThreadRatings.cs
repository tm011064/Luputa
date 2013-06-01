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
  public class Test_CMSThreadRatings : TestSetup
  {
    internal static void Delete(IDataStore dataStore, CMSThread thread, IUserBasic userBasic)
    {
      CMSThreadRatingManager manager = new CMSThreadRatingManager(dataStore);

      CMSThreadRating threadRating = manager.GetThreadRating(thread, userBasic);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(threadRating);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetThreadRating(thread, userBasic));

      Trace.WriteLine("Successfully deleted threadRating " + threadRating.CMSThreadId + " -> " + threadRating.CMSUserId);
    }

    internal static void PopulateWithRandomValues(CMSThreadRating record, DummyDataManager dtm, Random random)
    {
      record.Rating = (short)random.Next(1, 1000);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteThreadRating()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      CMSSection section = Test_CMSSections.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, null, this.Random);
      CMSThread thread = Test_CMSThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, section, this.Random);
     
      CMSThreadRatingManager manager = new CMSThreadRatingManager(this.DataStore);

      CMSThreadRating record = new CMSThreadRating(userBasic, thread, 1);

      BusinessObjectActionReport<RatingDataRepositoryActionStatus> report = manager.Create(record);
      Assert.AreEqual(RatingDataRepositoryActionStatus.Success, report.Status);

      CMSThreadRating recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetThreadRating(thread, userBasic);

        string errors = string.Empty;
        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, thread, userBasic);
      Test_CMSSections.Delete(this.DataStore, section); // deleting the section should also delete the file
    }
    [Test]
    public void Test_ThreadRatingCaluclations()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      CMSSection section = Test_CMSSections.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, null, this.Random);
      CMSThread thread = Test_CMSThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, section, this.Random);
      
      CMSThreadRatingManager manager = new CMSThreadRatingManager(this.DataStore);

      for (int i = 0; i < 10; i++)
      {
        userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);

        CMSThreadRating record = new CMSThreadRating(userBasic, thread, (short)i);

        BusinessObjectActionReport<RatingDataRepositoryActionStatus> report = manager.Create(record);
        Assert.AreEqual(RatingDataRepositoryActionStatus.Success, report.Status);
      }

      CMSThreadManager threadManager = new CMSThreadManager(this.DataStore);
      thread = threadManager.GetThread(section.CMSSectionType, thread.CMSThreadId);
      Assert.AreEqual(10, thread.CMSTotalRatings);
      Assert.AreEqual(45, thread.CMSRatingSum);

      Test_CMSSections.Delete(this.DataStore, section); // deleting the section should also delete the file
    }
    #endregion
  }
}
