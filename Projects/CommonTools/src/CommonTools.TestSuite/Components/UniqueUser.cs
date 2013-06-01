using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using CommonTools.Components.BusinessTier;

namespace CommonTools.TestSuite.Components
{
    /// <summary>
    /// Summary description for UniqueUser
    /// </summary>
    public class UniqueUser
    {
        #region globals
        protected CommonTools.TestSuite.DummyDatabase.SqlProvider.UsersDataContext _DataContext;
        protected CommonTools.TestSuite.DummyDatabase.SqlProvider.UniqueUser _Record;
        #endregion

        #region Properties
        [BusinessObjectProperty(IsMandatoryForInstance = true)]
        public Guid UserID
        {
            get { return _Record.UserID; }
            internal set { _Record.UserID = value; }
        }
        [BusinessObjectProperty(IsMandatoryForInstance = true)]
        [BusinessObjectValidation(IsRequired = true, MinimumValue = "0"
           , MaximumValue = "12", OutOfRangeErrorMessage = "Please provide a value between 0 and 12")]
        public byte AccountStatus
        {
            get { return _Record.AccountStatus; }
            set { _Record.AccountStatus = value; }
        }
        [BusinessObjectValidation(MinLength = 1, MaxLength = 256, OutOfRangeErrorMessage = "Please enter a value with 1 to 256 characters.")]
        [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true)]
        public string City
        {
            get { return _Record.City; }
            set { _Record.City = value; }
        }
        [BusinessObjectProperty(IsMandatoryForInstance = true)]
        public DateTime DateOfBirth
        {
            get { return _Record.DateOfBirth; }
            set { _Record.DateOfBirth = value; }
        }
        [BusinessObjectValidation(MinLength = 1, MaxLength = 256, OutOfRangeErrorMessage = "Please enter a value with 1 to 256 characters.")]
        [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true)]
        public string Firstname
        {
            get { return _Record.Firstname; }
            set { _Record.Firstname = value; }
        }
        [BusinessObjectProperty(IsMandatoryForInstance = true)]
        public bool IsNewletterSubscriber
        {
            get { return _Record.IsNewletterSubscriber; }
            set { _Record.IsNewletterSubscriber = value; }
        }
        [BusinessObjectValidation(MinLength = 1, MaxLength = 256, OutOfRangeErrorMessage = "Please enter a value with 1 to 256 characters.")]
        [BusinessObjectStringSecurity(RemoveScriptTags = true, RemoveBadHtmlTags = true)]
        public string Lastname
        {
            get { return _Record.Lastname; }
            set { _Record.Lastname = value; }
        }
        [BusinessObjectProperty(IsMandatoryForInstance = true)]
        [BusinessObjectValidation(IsRequired = true, MinimumValue = "-12"
          , MaximumValue = "12", OutOfRangeErrorMessage = "Please enter a value between -12 and 12")]
        public double Timezone
        {
            get { return _Record.Timezone; }
            set { _Record.Timezone = value; }
        }
        #endregion

        #region constructors
        public UniqueUser()
        {
            _IsLoadedFromDatabase = false;

            _DataContext = Configuration.GetUsersDataContext();
            _Record = new CommonTools.TestSuite.DummyDatabase.SqlProvider.UniqueUser() { UserID = Guid.NewGuid(), AccountStatus = 0, IsNewletterSubscriber = false };
        }

        internal UniqueUser(CommonTools.TestSuite.DummyDatabase.SqlProvider.UniqueUser userRecord)
        {
            _IsLoadedFromDatabase = true;

            _DataContext = Configuration.GetUsersDataContext();
            _Record = userRecord;
        }
        #endregion

        #region IBusinessObject Members

        public bool IsCreateAble
        {
            get { return !IsLoadedFromDatabase; }
        }

        public bool IsDeleteAble
        {
            get { return IsLoadedFromDatabase; }
        }

        private bool _IsLoadedFromDatabase;
        public bool IsLoadedFromDatabase
        {
            get { return _IsLoadedFromDatabase; }
        }

        public bool IsUpdateAble
        {
            get { return IsLoadedFromDatabase; }
        }

        #endregion
    }
}