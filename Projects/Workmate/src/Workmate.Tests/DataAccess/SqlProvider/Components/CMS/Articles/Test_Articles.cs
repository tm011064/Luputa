using System;
using System.Diagnostics;
using CommonTools.Components.BusinessTier;
using CommonTools.Components.Testing;
using NUnit.Framework;
using Workmate.Components.CMS.Articles;
using Workmate.Components.Contracts.CMS.Articles;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS.Articles;
using Workmate.Components.Entities.Membership;
using Workmate.Configuration;
using Workmate.Data;
using Workmate.Tests.DataAccess.SqlProvider.Components.Membership;
using Workmate.Components.Contracts.Membership;
using Workmate.Components.Entities.CMS.MessageBoards;
using Workmate.Tests.DataAccess.SqlProvider.Components.CMS.MessageBoards;
using Workmate.Components.CMS.MessageBoards;
using System.Text;
using Workmate.Components.CMS;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS.Articles
{
  [TestFixture]
  public class Test_Articles : TestSetup
  {
    internal static Article Create(IDataStore dataStore, IApplicationSettings applicationSettings, IApplication application
      , IUserBasic author, ArticleGroupThread articleGroupThread
      , MessageBoard messageBoard
      , Random random)
    {
      ArticleManager manager = new ArticleManager(dataStore);

      Article article = new Article(
        author
        , articleGroupThread
        , DebugUtility.GetRandomEnum<ArticleStatus>(random)
        , DebugUtility.GetRandomEnum<ArticleType>(random)
        , "Article Subject" + random.Next(1000000, 10000000)
        , "Article Body" + random.Next(1000000, 10000000)
        , "URLName" + random.Next(1000000, 10000000)
        , DebugUtility.FlipCoin(random));
            
      int count = random.Next(0, 10);
      for (int i = 0; i < count; i++)
        article.ContentLevelNodeNames.Add("Category " + i + (DebugUtility.FlipRiggedCoin(random, .2) ? " " + random.Next(10000, 100000) : string.Empty));

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Create(article, messageBoard.MessageBoardId, DebugUtility.FlipCoin(random));
      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.Greater(article.ArticleId, 0);

      Article dsArticle = manager.GetArticle(article.ArticleId);
      Assert.IsNotNull(dsArticle);

      IArticleModel articelModel = manager.GetArticleModel(dsArticle.ArticleId);
      Assert.AreEqual(article.ContentLevelNodeNames.Count, articelModel.ContentLevelNodes.Count);
      for (int i = 0; i < article.ContentLevelNodeNames.Count; i++)
      {
        Assert.AreEqual(article.ContentLevelNodeNames[i], articelModel.ContentLevelNodes[i]);
      }

      return dsArticle;
    }
    internal static void Delete(IDataStore dataStore, Article article)
    {
      ArticleManager manager = new ArticleManager(dataStore);

      BusinessObjectActionReport<DataRepositoryActionStatus> report = manager.Delete(article);

      Assert.AreEqual(DataRepositoryActionStatus.Success, report.Status);
      Assert.IsNull(manager.GetArticle(article.ArticleId));

      Trace.WriteLine("Successfully deleted article " + article.Subject);
    }

    internal static void PopulateWithRandomValues(Article record, DummyDataManager dtm, Random random)
    {
      record.ArticleStatus = DebugUtility.GetRandomEnum<ArticleStatus>(random);
      record.ArticleType = DebugUtility.GetRandomEnum<ArticleType>(random);
      record.FormattedBody = "Article Body" + random.Next(1000000, 10000000);
      record.IsApproved = DebugUtility.FlipCoin(random);
      record.IsLocked = DebugUtility.FlipCoin(random);
      record.Subject = "Article Status" + random.Next(1000000, 10000000);
      record.UrlFriendlyName = record.Subject;
    }

    internal static IArticleAttachmentModel AddAttachment(IDataStore dataStore, int applicationId, IArticleModel articleModel, IUserBasic userBasic, Random random)
    {
      ArticleAttachmentManager articleAttachmentManager = new ArticleAttachmentManager(dataStore);
      ArticleAttachment attachment = new ArticleAttachment(applicationId, userBasic);
      ArticleManager manager = new ArticleManager(dataStore);

      attachment.Content = Encoding.Unicode.GetBytes("HELLO");
      attachment.ContentSize = attachment.Content.Length;
      attachment.ContentType = "text";
      attachment.FileName = "myfile " + random.Next(1000, 10000) + ".txt";
      attachment.FriendlyFileName = "My File " + random.Next(1000, 10000);

      Assert.AreEqual(DataRepositoryActionStatus.Success, articleAttachmentManager.CreateTemporaryFile(attachment).Status);

      articleModel = manager.GetArticleModel(articleModel.ArticleId);
      int totalAttachments = articleModel.Attachments.Count;

      int articleAttachmentId;
      Assert.IsTrue(articleAttachmentManager.MoveTemporaryFileToFiles(attachment.ArticleAttachmentId, articleModel.ArticleId
        , attachment.FileName, attachment.FriendlyFileName, out articleAttachmentId));
      Assert.Greater(articleAttachmentId, 0);

      articleModel = manager.GetArticleModel(articleModel.ArticleId);
      Assert.AreEqual(totalAttachments + 1, articleModel.Attachments.Count);

      IArticleAttachmentModel articleAttachmentModel = articleModel.Attachments.Find(c => c.AttachmentId == articleAttachmentId);
      Assert.IsNotNull(articleAttachmentModel);

      Assert.AreEqual(attachment.ContentSize, articleAttachmentModel.ContentSize);
      Assert.AreEqual(attachment.ContentType, articleAttachmentModel.ContentType);
      Assert.AreEqual(attachment.FileName, articleAttachmentModel.FileName);
      Assert.AreEqual(attachment.FriendlyFileName, articleAttachmentModel.FriendlyFileName);
      Assert.AreEqual(attachment.UserId, articleAttachmentModel.UserId);

      return articleAttachmentModel;
    }
    internal static void DeleteAttachment(IDataStore dataStore, IArticleModel articleModel, int attachmentId, Random random)
    {
      ArticleManager manager = new ArticleManager(dataStore);
      ArticleAttachmentManager articleAttachmentManager = new ArticleAttachmentManager(dataStore);

      articleModel = manager.GetArticleModel(articleModel.ArticleId);
      int totalAttachments = articleModel.Attachments.Count;
      IArticleAttachmentModel articleAttachmentModel = articleModel.Attachments.Find(c => c.AttachmentId == attachmentId);
      Assert.IsNotNull(articleAttachmentModel);
      articleAttachmentManager.Delete(attachmentId);

      articleModel = manager.GetArticleModel(articleModel.ArticleId);
      Assert.AreEqual(totalAttachments + -1, articleModel.Attachments.Count);
    }

    #region tests
    [Test]
    public void Test_CreateUpdateDeleteArticle()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      MessageBoard messageBoard = Test_MessageBoards.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);
      ArticleGroup articleGroup = Test_ArticleGroups.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);
      ArticleGroupThread articleGroupThread = Test_ArticleGroupThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, articleGroup, this.Random);

      ArticleManager manager = new ArticleManager(this.DataStore);
      Article record = Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic
        , articleGroupThread, messageBoard, this.Random);

      MessageBoardThreadManager messageBoardThreadManager = new MessageBoardThreadManager(this.DataStore);

      Article recordToCompare;
      for (int i = 0; i < this.DefaultUpdateTestIterations; i++)
      {
        PopulateWithRandomValues(record, this.DummyDataManager, this.Random);
        recordToCompare = record;

        manager.Update(record);
        record = manager.GetArticle(record.ArticleId);

        string errors = string.Empty;
        // TODO (Roman): relax datetime comparisons
        Assert.IsTrue(DebugUtility.ArePropertyValuesEqual(record, recordToCompare, out errors), errors);
        Trace.WriteLine("Update test successfull.");
      }

      IArticleModel articleModel = manager.GetArticleModel(record.ArticleId);

      Assert.AreEqual(0, articleModel.TotalComments);

      #region messageboards
      MessageBoardThread messageBoardThread = messageBoardThreadManager.GetMessageBoardThread(articleModel.MessageBoardThreadId);
      Assert.IsNotNull(messageBoardThread);

      MessageManager messageManager = new MessageManager(this.DataStore);

      messageManager.Create(new Message(userBasic, messageBoardThread, "Test Message Subject 1", "Body1"));
      messageManager.Create(new Message(userBasic, messageBoardThread, "Test Message Subject 2", "Body2"));

      Message message = new Message(userBasic, messageBoardThread, "Test Message Subject 3", "Body3");
      messageManager.Create(message);

      articleModel = manager.GetArticleModel(record.ArticleId);
      Assert.AreEqual(3, articleModel.TotalComments);

      Assert.AreEqual(DataRepositoryActionStatus.Success, messageManager.Delete(message).Status);
      articleModel = manager.GetArticleModel(record.ArticleId);
      Assert.AreEqual(2, articleModel.TotalComments);
      #endregion

      #region attachments
      IArticleAttachmentModel articleAttachmentModel = AddAttachment(this.DataStore, this.Application.ApplicationId, articleModel, userBasic, this.Random);
      AddAttachment(this.DataStore, this.Application.ApplicationId, articleModel, userBasic, this.Random);

      DeleteAttachment(this.DataStore, articleModel, articleAttachmentModel.AttachmentId, this.Random);

      AddAttachment(this.DataStore, this.Application.ApplicationId, articleModel, userBasic, this.Random);
      AddAttachment(this.DataStore, this.Application.ApplicationId, articleModel, userBasic, this.Random);

      articleAttachmentModel = AddAttachment(this.DataStore, this.Application.ApplicationId, articleModel, userBasic, this.Random);
      DeleteAttachment(this.DataStore, articleModel, articleAttachmentModel.AttachmentId, this.Random);

      AddAttachment(this.DataStore, this.Application.ApplicationId, articleModel, userBasic, this.Random);
      #endregion

      #region contentnodelevels
      if (articleModel.ContentLevelNodeId.HasValue)
      {
        string newName = "Some Name" + this.Random.Next(10000, 100000);
        CMSContentLevelNodeManager contentLevelNodeManager = new CMSContentLevelNodeManager(this.DataStore);
        contentLevelNodeManager.RenameContentLevelNode(articleModel.ContentLevelNodeId.Value, newName);

        articleModel = manager.GetArticleModel(record.ArticleId);
        Assert.AreEqual(articleModel.ContentLevelNodes[articleModel.ContentLevelNodes.Count - 1], newName);
      }
      #endregion

      Delete(this.DataStore, record);
      Test_ArticleGroups.Delete(this.DataStore, articleGroup);

      Assert.IsNull(messageBoardThreadManager.GetMessageBoardThread(articleModel.MessageBoardThreadId));

      ArticleAttachmentManager articleAttachmentManager = new ArticleAttachmentManager(this.DataStore);
      Assert.IsEmpty(articleAttachmentManager.GetArticleAttachments(articleModel.ArticleId));

    }
    [Test]
    public void Test_Delete_ArticleGroup()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);
      MessageBoard messageBoard = Test_MessageBoards.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);

      ArticleGroup articleGroup = Test_ArticleGroups.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);
      ArticleGroupThread articleGroupThread = Test_ArticleGroupThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, articleGroup, this.Random);
      Article article = Test_Articles.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic
        , articleGroupThread, messageBoard, this.Random);

      Test_ArticleGroups.Delete(this.DataStore, articleGroup);

      ArticleGroupThreadManager articleGroupThreadManager = new ArticleGroupThreadManager(this.DataStore);
      ArticleManager articleManager = new ArticleManager(this.DataStore);

      Assert.IsNull(articleGroupThreadManager.GetArticleGroupThread(articleGroupThread.ArticleGroupThreadId));
      Assert.IsNull(articleManager.GetArticle(article.ArticleId));
    }
    [Test]
    public void Test_Delete_Thread()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);

      MessageBoard messageBoard = Test_MessageBoards.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);

      ArticleGroup articleGroup = Test_ArticleGroups.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);
      ArticleGroupThread articleGroupThread = Test_ArticleGroupThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, articleGroup, this.Random);
      Article article = Test_Articles.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic
        , articleGroupThread, messageBoard, this.Random);

      Test_ArticleGroupThreads.Delete(this.DataStore, articleGroupThread);

      ArticleGroupThreadManager articleGroupThreadManager = new ArticleGroupThreadManager(this.DataStore);
      ArticleManager articleManager = new ArticleManager(this.DataStore);

      Assert.IsNull(articleGroupThreadManager.GetArticleGroupThread(articleGroupThread.ArticleGroupThreadId));
      Assert.IsNull(articleManager.GetArticle(article.ArticleId));

      Test_ArticleGroups.Delete(this.DataStore, articleGroup);
    }
    #endregion
  }
}
