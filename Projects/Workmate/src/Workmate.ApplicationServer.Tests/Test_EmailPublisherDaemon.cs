using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Workmate.ApplicationServer.Configuration;
using NSubstitute;
using System.Net.Mail;
using System.Configuration;
using Workmate.Components.Contracts.Emails;
using Workmate.Components.Contracts;
using Workmate.Data;

namespace Workmate.ApplicationServer.Tests
{
  [TestFixture]
  public class Test_EmailPublisherDaemon : BaseTestSetup
  {
    protected override IDataStore DataStore { get { return null; } }

    [TestFixtureSetUp]
    public virtual void Setup()
    {
      IApplicationSettings applicationSettings = new ApplicationSettings(ConfigurationManager.AppSettings, ConfigurationManager.ConnectionStrings);

      var emailManager = Substitute.For<IEmailManager>();

      emailManager.PutInSendQueue(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).ReturnsForAnyArgs(new List<IEmail>());
      emailManager.SetToSent(Arg.Any<int>(), Arg.Any<EmailStatus>(), Arg.Any<EmailPriority>()).Returns(DataRepositoryActionStatus.Success);

      InstanceContainer.Initialize(applicationSettings, emailManager);
    }

    [Test]
    public void Test_SimpleSend()
    {
      EmailPublisherDaemon emailPublisherDaemon = new EmailPublisherDaemon(InstanceContainer.ApplicationSettings);

      var email = Substitute.For<IEmail>();

      email.ApplicationId.Returns(-1);
      email.Body.Returns("BODY");
      email.CreatedByUserId.Returns(-1);
      email.DateCreatedUtc.Returns(DateTime.UtcNow);
      email.EmailId.Returns(-1);
      email.EmailType.Returns(EmailType.UserCreated);
      email.Priority.Returns(EmailPriority.SendImmediately);
      email.Recipients.Returns("workmate.test.emailpublisher@gmail.com,workmate.test.user1@gmail.com");
      email.Sender.Returns("workmate.test.emailpublisher@gmail.com");
      email.Status.Returns(EmailStatus.Queued);
      email.TotalSendAttempts.Returns(0);

      Assert.IsTrue(emailPublisherDaemon.SendEmail(email));
    }
  }
}