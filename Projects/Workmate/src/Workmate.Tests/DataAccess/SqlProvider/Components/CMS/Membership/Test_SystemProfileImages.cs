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
  public class Test_SystemProfileImages : TestSetup
  {
    internal static SystemProfileImage CreateSystemProfileImage(IDataStore dataStore, int applicationId, Random random)
    {
      SystemProfileImageManager manager = new SystemProfileImageManager(dataStore);

      SystemProfileImage file = new SystemProfileImage(applicationId);

      ASCIIEncoding encoding = new ASCIIEncoding();
      string contentString = "Some String " + random.Next(1000000, 10000000);

      file.Height = random.Next(10, 1000);
      file.Width = random.Next(10, 1000);

      file.Content = encoding.GetBytes(contentString);
      file.ContentSize = file.Content.Length;
      file.ContentType = "TEXT " + random.Next(1000, 10000);
      file.FriendlyFileName = "Some Name " + random.Next(1000000, 10000000);
      file.IsTemporary = false;

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(file);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      
      SystemProfileImage dsFile = manager.GetSystemProfileImage(file.ImageId);
      Assert.IsNotNull(dsFile);
      Assert.AreEqual(contentString, encoding.GetString(dsFile.Content));

      return dsFile;
    }
    internal static void Delete(IDataStore dataStore, SystemProfileImage file)
    {
      SystemProfileImageManager manager = new SystemProfileImageManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(file);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetSystemProfileImage(file.ImageId));

      Trace.WriteLine("Successfully deleted file " + file.FriendlyFileName ?? string.Empty);
    }
    internal static void PopulateWithRandomValues(SystemProfileImage record, DummyDataManager dtm, Random random)
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
    public void Test_CreateUpdateDeleteSystemProfileImageFile()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);

      SystemProfileImageManager manager = new SystemProfileImageManager(this.DataStore);
      SystemProfileImage record = Test_SystemProfileImages.CreateSystemProfileImage(this.DataStore, this.Application.ApplicationId, this.Random);

      SystemProfileImage recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetSystemProfileImage(record.ImageId);

        string errors = string.Empty;
        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Delete(this.DataStore, record);
    }
  }
}
