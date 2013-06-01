using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Workmate.Components.Entities.CMS.Membership;
using Workmate.Data;
using Workmate.Components.Contracts.Membership;
using Workmate.Components.CMS;
using Workmate.Components.Entities.CMS;
using Workmate.Components.Contracts.CMS;
using CommonTools.Components.BusinessTier;
using Workmate.Components.Contracts;
using Workmate.Components.CMS.Membership;
using Workmate.Tests.DataAccess.SqlProvider.Components.Membership;
using CommonTools.Components.Testing;
using System.Diagnostics;
using Workmate.Components.Membership;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS.Membership
{
  [TestFixture]
  public class Test_ProfileImages : TestSetup
  {
    internal static ProfileImage CreateProfileImage(IDataStore dataStore, int applicationId, IUserBasic fileOwner, Random random)
    {
      ProfileImageManager manager = new ProfileImageManager(dataStore);

      ProfileImage file = new ProfileImage(applicationId, fileOwner);

      ASCIIEncoding encoding = new ASCIIEncoding();
      string contentString = "Some String " + random.Next(1000000, 10000000);

      file.Height = random.Next(10, 1000);
      file.Width = random.Next(10, 1000);

      file.Content = encoding.GetBytes(contentString);
      file.ContentSize = file.Content.Length;
      file.ContentType = "TEXT " + random.Next(1000, 10000);
      file.FriendlyFileName = "Some Name " + random.Next(1000000, 10000000);
      file.IsTemporary = false;

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.CreateTemporaryImage(file);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);

      int newFileId;
      Assert.IsTrue(manager.AssignTemporaryProfileImageToUser(file.ImageId, out newFileId));

      ProfileImage dsFile = manager.GetProfileImage(newFileId);
      Assert.IsNotNull(dsFile);
      Assert.AreEqual(contentString, encoding.GetString(dsFile.Content));

      WorkmateMembershipProvider membershipProvider = new WorkmateMembershipProvider();
      fileOwner = membershipProvider.GetUserBasic(fileOwner.UserId, false);
      Assert.AreEqual(fileOwner.ProfileImageId, newFileId);

      return dsFile;
    }
    internal static void Delete(IDataStore dataStore, ProfileImage file)
    {
      ProfileImageManager manager = new ProfileImageManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(file);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetProfileImage(file.ImageId));

      Trace.WriteLine("Successfully deleted file " + file.FriendlyFileName ?? string.Empty);
    }
    internal static void PopulateWithRandomValues(ProfileImage record, DummyDataManager dtm, Random random)
    {
      ASCIIEncoding encoding = new ASCIIEncoding();
      string contentString = "Some String " + random.Next(1000000, 10000000);

      record.Height = random.Next(10, 1000);
      record.Width = random.Next(10, 1000);

      record.Content = encoding.GetBytes(contentString);
      record.ContentSize = record.Content.Length;
      record.ContentType = "TEXT " + random.Next(1000, 10000);
      record.FriendlyFileName = "Some Name " + random.Next(1000000, 10000000);
    }

    [Test]
    public void Test_CreateUpdateDeleteProfileImageFile()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);

      ProfileImageManager manager = new ProfileImageManager(this.DataStore);
      ProfileImage record = Test_ProfileImages.CreateProfileImage(this.DataStore, this.Application.ApplicationId, userBasic, this.Random);

      ProfileImage recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetProfileImage(record.ImageId);

        string errors = string.Empty;
        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, record);
    }

    [Test]
    public void Test_AssignNewProfileImage()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);

      ProfileImageManager manager = new ProfileImageManager(this.DataStore);
      ProfileImage record = Test_ProfileImages.CreateProfileImage(this.DataStore, this.Application.ApplicationId, userBasic, this.Random);

      ProfileImage record2 = Test_ProfileImages.CreateProfileImage(this.DataStore, this.Application.ApplicationId, userBasic, this.Random);
      Assert.IsNull(manager.GetProfileImage(record.ImageId));

      Delete(this.DataStore, record2);
    }
  }
}
