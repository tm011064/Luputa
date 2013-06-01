using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Workmate.Components.Entities.CMS;
using Workmate.Data;
using Workmate.Components.CMS;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Contracts.CMS;
using CommonTools.Components.Testing;
using System.Runtime.Serialization.Formatters.Binary;
using CommonTools.Components.BusinessTier;
using Workmate.Components.Contracts;
using System.Diagnostics;
using Workmate.Tests.DataAccess.SqlProvider.Components.Membership;
using Workmate.Components.Contracts.Membership;
using Workmate.Tests.DataAccess.SqlProvider.Components.CMS.Membership;
using Workmate.Components.Entities.CMS.Membership;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS
{
  [TestFixture]
  public class Test_CMSFiles : TestSetup
  {
    internal static CMSFile CreateTemporaryFile(IDataStore dataStore, int applicationId, IUserBasic fileOwner, byte[] content, Random random)
    {
      CMSFileManager manager = new CMSFileManager(dataStore);

      CMSFile file = new CMSFile(applicationId, fileOwner, FileType.PostAttachment);

      file.IsTemporary = true;

      file.CMSHeight = random.Next(10, 1000);
      file.CMSWidth = random.Next(10, 1000);

      file.Content = content;
      file.ContentSize = file.Content.Length;
      file.ContentType = "TEXT " + random.Next(1000, 10000);
      file.FileName = "Some Name " + random.Next(1000000, 10000000);
      file.FriendlyFileName = "Some Name " + random.Next(1000000, 10000000);
      file.IsTemporary = false;

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.CreateTemporaryFile(file);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);

      return file;
    }
    internal static CMSFile CreateContentFile(IDataStore dataStore, int applicationId, IUserBasic fileOwner, CMSContent content, Random random)
    {
      CMSFileManager manager = new CMSFileManager(dataStore);

      CMSFile file = new CMSFile(applicationId, fileOwner, FileType.PostAttachment);

      file.ContentId = content.CMSContentId;

      ASCIIEncoding encoding = new ASCIIEncoding();
      string contentString = "Some String " + random.Next(1000000, 10000000);

      file.CMSHeight = random.Next(10, 1000);
      file.CMSWidth = random.Next(10, 1000);

      file.Content = encoding.GetBytes(contentString);
      file.ContentSize = file.Content.Length;
      file.ContentType = "TEXT " + random.Next(1000, 10000);
      file.FileName = "Some Name " + random.Next(1000000, 10000000);
      file.FriendlyFileName = "Some Name " + random.Next(1000000, 10000000);
      file.IsTemporary = false;

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(file);
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);

      CMSFile dsFile = manager.GetFile(file.CMSFileId);
      Assert.IsNotNull(dsFile);
      Assert.AreEqual(contentString, encoding.GetString(dsFile.Content));
      Assert.AreEqual(file.ContentId, content.CMSContentId);

      return dsFile;
    }
    internal static void Delete(IDataStore dataStore, CMSFile file)
    {
      CMSFileManager manager = new CMSFileManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(file);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetFile(file.CMSFileId));

      Trace.WriteLine("Successfully deleted file " + file.FileName);
    }

    internal static void PopulateWithRandomValues(CMSFile record, DummyDataManager dtm, Random random)
    {
      ASCIIEncoding encoding = new ASCIIEncoding();
      string contentString = "Some String " + random.Next(1000000, 10000000);

      record.CMSHeight = random.Next(10, 1000);
      record.CMSWidth = random.Next(10, 1000);

      record.Content = encoding.GetBytes(contentString);
      record.ContentSize = record.Content.Length;
      record.ContentType = "TEXT " + random.Next(1000, 10000);
      record.FileName = "Some Name " + random.Next(1000000, 10000000);
      record.FriendlyFileName = "Some Name " + random.Next(1000000, 10000000);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteContentFile()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      CMSSection section = Test_CMSSections.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, null, this.Random);
      CMSThread thread = Test_CMSThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, section, this.Random);
      CMSContent content = Test_CMSContents.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic.UserId, thread, this.Random);

      CMSFileManager manager = new CMSFileManager(this.DataStore);
      CMSFile record = CreateContentFile(this.DataStore, this.Application.ApplicationId, userBasic, content, this.Random);

      CMSFile recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetFile(record.CMSFileId);

        string errors = string.Empty;
        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      Test_CMSSections.Delete(this.DataStore, section); // deleting the section should also delete the file
      Assert.IsNull(manager.GetFile(record.CMSFileId));
    }

    [Test]
    public void Test_CreateTempFileAndMove()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      CMSSection section = Test_CMSSections.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, null, this.Random);
      CMSThread thread = Test_CMSThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, section, this.Random);
      CMSContent content = Test_CMSContents.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic.UserId, thread, this.Random);

      CMSFileManager manager = new CMSFileManager(this.DataStore);

      ASCIIEncoding encoding = new ASCIIEncoding();
      string contentString = "Some String " + this.Random.Next(1000000, 10000000);

      CMSFile tempFile = CreateTemporaryFile(this.DataStore, this.Application.ApplicationId, userBasic, encoding.GetBytes(contentString), this.Random);

      int fileId;
      manager.MoveTemporaryFileToFiles(tempFile.CMSFileId, content.CMSContentId, "FileName", "FriendlyFileName", null, out fileId);
      CMSFile file = manager.GetFile(fileId);
      Assert.IsNotNull(file);

      // TODO (Roman): do all the value asserts

      Assert.AreEqual(contentString, encoding.GetString(file.Content));

      // TODO (Roman): check that tempFile doesn't exist any more
      Test_CMSSections.Delete(this.DataStore, section); // deleting the section should also delete the file
      Assert.IsNull(manager.GetFile(file.CMSFileId));
    }
    #endregion
  }
}
