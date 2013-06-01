using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Entities.CMS.MessageBoards;
using Workmate.Tests.DataAccess.SqlProvider.Components.Membership;
using Workmate.Components.CMS.MessageBoards;
using Workmate.Components.Contracts.CMS.MessageBoards;
using Workmate.Components.Entities.CMS;
using CommonTools.Components.BusinessTier;
using Workmate.Components.Contracts.CMS;
using CommonTools.Components.Testing;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Tests.DataAccess.SqlProvider.Components.CMS.MessageBoards
{
  [TestFixture]
  public class Test_MessageBoardRatings : TestSetup
  {
    [Test]
    public void Test_CreateUpdateDelete()
    {
      IUserBasic userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);

      MessageBoard messageBoard = Test_MessageBoards.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.Random);
      MessageBoardThread messageBoardThread = Test_MessageBoardThreads.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, messageBoard, this.Random);
      Message message = Test_Messages.Create(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, userBasic, messageBoardThread, this.Random);

      MessageRatingManager messageRatingManager = new MessageRatingManager(this.DataStore);
      
      BaseRatingInfo baseRatingInfo;
      int totalThumbsUp = 0;
      for (int i = 0; i < 10; i++)
      {
        userBasic = Test_WorkmateMembershipProvider.CreateUser(this.DataStore, Workmate.Components.InstanceContainer.ApplicationSettings, this.Application, this.DummyDataManager);

        MessageRating record = new MessageRating(userBasic, message, DebugUtility.GetRandomEnum<MessageRatingType>(this.Random));

        BusinessObjectActionReport<RatingDataRepositoryActionStatus> report = messageRatingManager.Create(record, out baseRatingInfo);
        Assert.AreEqual(RatingDataRepositoryActionStatus.Success, report.Status);

        totalThumbsUp += (short)record.MessageRatingType;
        Assert.AreEqual(totalThumbsUp, baseRatingInfo.GetTotalThumbsUp());
        Assert.AreEqual(i + 1 - totalThumbsUp, baseRatingInfo.GetTotalThumbsDown());

        record = new MessageRating(userBasic, message, DebugUtility.GetRandomEnum<MessageRatingType>(this.Random, record.MessageRatingType));
        messageRatingManager.CreateOrUpdateExisting(record, out baseRatingInfo);
        Assert.AreEqual(RatingDataRepositoryActionStatus.Success, report.Status);

        totalThumbsUp += record.MessageRatingType == MessageRatingType.ThumbsUp ? 1 : -1;
        Assert.AreEqual(totalThumbsUp, baseRatingInfo.GetTotalThumbsUp());
        Assert.AreEqual(i + 1 - totalThumbsUp, baseRatingInfo.GetTotalThumbsDown());
      }
      
      Test_MessageBoardThreads.Delete(this.DataStore, messageBoardThread);

      MessageBoardThreadManager messageBoardThreadManager = new MessageBoardThreadManager(this.DataStore);
      MessageManager messageManager = new MessageManager(this.DataStore);

      Assert.IsNull(messageBoardThreadManager.GetMessageBoardThread(messageBoardThread.MessageBoardThreadId));
      Assert.IsNull(messageManager.GetMessage(message.MessageId));
      Assert.IsEmpty(messageRatingManager.GetAllRatingsForMessage(message));

      Test_MessageBoards.Delete(this.DataStore, messageBoard);
    }
  }
}
