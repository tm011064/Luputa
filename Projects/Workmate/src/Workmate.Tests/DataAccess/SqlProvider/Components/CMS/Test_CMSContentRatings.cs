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
  public class Test_CMSContentRatings : TestSetup
  {
    internal static void Delete(IDataStore dataStore, CMSContent content, IUserBasic userBasic)
    {
      CMSContentRatingManager manager = new CMSContentRatingManager(dataStore);

      CMSContentRating contentRating = manager.GetContentRating(content, userBasic);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(contentRating);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetContentRating(content, userBasic));

      Trace.WriteLine("Successfully deleted contentRating " + contentRating.CMSContentId + " -> " + contentRating.CMSUserId);
    }

    internal static void PopulateWithRandomValues(CMSContentRating record, DummyDataManager dtm, Random random)
    {
      record.Rating = (short)random.Next(1, 1000);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteContentRating()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      CMSSection section = Test_CMSSections.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, null, this.Random);
      CMSThread thread = Test_CMSThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, section, this.Random);
      CMSContent content = Test_CMSContents.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic.UserId, thread, this.Random);

      CMSContentRatingManager manager = new CMSContentRatingManager(this.DataStore);

      CMSContentRating record = new CMSContentRating(userBasic, content, 1);
      BaseRatingInfo baseRatingInfo;

      BusinessObjectActionReport<RatingDataRepositoryActionStatus> report = manager.Create(record, true, true, out baseRatingInfo);
      Assert.AreEqual(RatingDataRepositoryActionStatus.Success, report.Status);
      Assert.AreEqual(1, baseRatingInfo.RatingSum);
      Assert.AreEqual(1, baseRatingInfo.TotalRatings);

      CMSContentRating recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetContentRating(content, userBasic);

        string errors = string.Empty;
        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, content, userBasic);
      Test_CMSSections.Delete(this.DataStore, section); // deleting the section should also delete the file
    }

    [Test]
    public void Test_SelfRatingConstraint()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      CMSSection section = Test_CMSSections.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, null, this.Random);
      CMSThread thread = Test_CMSThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, section, this.Random);
      CMSContent content = Test_CMSContents.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic.UserId, thread, this.Random);

      CMSContentRatingManager manager = new CMSContentRatingManager(this.DataStore);

      CMSContentRating record = new CMSContentRating(userBasic, content, 1);
      BaseRatingInfo baseRatingInfo;

      BusinessObjectActionReport<RatingDataRepositoryActionStatus> report = manager.Create(record, true, false, out baseRatingInfo);
      Assert.AreEqual(RatingDataRepositoryActionStatus.SelfRatingNotAllowed, report.Status);

      Test_CMSSections.Delete(this.DataStore, section); // deleting the section should also delete the file
    }
    [Test]
    public void Test_ContentRatingCaluclations()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      CMSSection section = Test_CMSSections.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, null, this.Random);
      CMSThread thread = Test_CMSThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, section, this.Random);
      CMSContent content = Test_CMSContents.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic.UserId, thread, this.Random);

      CMSContentRatingManager manager = new CMSContentRatingManager(this.DataStore);
      
      for (int i = 0; i < 10; i++)
      {
        userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);

        CMSContentRating record = new CMSContentRating(userBasic, content, (short)i);

        BusinessObjectActionReport<RatingDataRepositoryActionStatus> report = manager.Create(record);
        Assert.AreEqual(RatingDataRepositoryActionStatus.Success, report.Status);
      }

      CMSContentManager contentManager = new CMSContentManager(this.DataStore);
      content = contentManager.GetContent(content.CMSContentId);
      Assert.AreEqual(10, content.CMSTotalRatings);
      Assert.AreEqual(45, content.CMSRatingSum);

      Test_CMSSections.Delete(this.DataStore, section); // deleting the section should also delete the file
    }
    #endregion
  }
}
