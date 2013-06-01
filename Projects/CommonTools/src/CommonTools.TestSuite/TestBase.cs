using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using CommonTools.TestSuite.Components;
using CommonTools.Components.Testing;
using CommonTools.Components.BusinessTier;

namespace CommonTools.TestSuite.Tests
{
    

    public class TestBase
    {
        private static bool _IsInstanciated = false;

        [TestFixtureSetUp]
        public void InitializeTestSuite()
        {
            if (_IsInstanciated)
                return;

            Trace.WriteLine("////////////////////////////////////////////////////////////////////");
            Trace.WriteLine("       INIT table mappings START");
            Trace.WriteLine("////////////////////////////////////////////////////////////////////\n");

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            UniqueUserManager.GetUser(Guid.NewGuid());
            stopwatch.Stop();
            Trace.WriteLine("Tablemappings initialization time: " + stopwatch.Elapsed.ToString());

            stopwatch.Reset();
            stopwatch.Start();
            UniqueUserManager.GetUserPage(0, 10);
            stopwatch.Stop();
            Trace.WriteLine("Stored Procedure initialization time: " + stopwatch.Elapsed.ToString());


            DummyDataManager dtm = new DummyDataManager(System.Configuration.ConfigurationSettings.AppSettings["XmlDummyDataPath"]);
            Random random = new Random();

            DummyUser dummy = dtm.GetDummy();
            UniqueUser user = new UniqueUser()
            {
                AccountStatus = 0,
                City = dummy.City,
                DateOfBirth = dummy.DateOfBirth,
                Firstname = dummy.Firstname,
                IsNewletterSubscriber = (random.Next(0, 2) == 1),
                Lastname = dummy.Surname,
                Timezone = random.NextDouble() * 10
            };

            stopwatch.Reset();
            stopwatch.Start();
            BusinessObjectActionReport<UniqueUserActionStatus> report = UniqueUserManager.Create(user);
            report = UniqueUserManager.Delete(user);
            stopwatch.Stop();
            Trace.WriteLine("Stored Procedure initialization time: " + stopwatch.Elapsed.ToString());
            Trace.WriteLine("\n////////////////////////////////////////////////////////////////////");
            Trace.WriteLine("       INIT table mappings END ");
            Trace.WriteLine("////////////////////////////////////////////////////////////////////\n\n\n");
            Trace.WriteLine("       START TESTS ");
            Trace.WriteLine("////////////////////////////////////////////////////////////////////\n");

            _IsInstanciated = true;
        }
    }
}
