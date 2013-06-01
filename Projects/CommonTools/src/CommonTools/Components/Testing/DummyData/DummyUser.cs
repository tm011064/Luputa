using System;

namespace CommonTools.Components.Testing
{
    /// <summary>
    /// A simple user profile class
    /// </summary>
    public class DummyUser
    {
        private string _Username;
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        private string _Password;
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private string _Surname;
        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>The surname.</value>
        public string Surname
        {
            get { return _Surname; }
            set { _Surname = value; }
        }

        private string _Firstname;
        /// <summary>
        /// Gets or sets the firstname.
        /// </summary>
        /// <value>The firstname.</value>
        public string Firstname
        {
            get { return _Firstname; }
            set { _Firstname = value; }
        }

        private DateTime _DateOfBirth;
        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>The date of birth.</value>
        public DateTime DateOfBirth
        {
            get { return _DateOfBirth; }
            set { _DateOfBirth = value; }
        }

        private string _Street;
        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        /// <value>The street.</value>
        public string Street
        {
            get { return _Street; }
            set { _Street = value; }
        }

        private string _City;
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }

        private string _Postcode;
        /// <summary>
        /// Gets or sets the postcode.
        /// </summary>
        /// <value>The postcode.</value>
        public string Postcode
        {
            get { return _Postcode; }
            set { _Postcode = value; }
        }

        private string _Telephone;
        /// <summary>
        /// Gets or sets the telephone.
        /// </summary>
        /// <value>The telephone.</value>
        public string Telephone
        {
            get { return _Telephone; }
            set { _Telephone = value; }
        }

        private string _Mobile;
        /// <summary>
        /// Gets or sets the mobile.
        /// </summary>
        /// <value>The mobile.</value>
        public string Mobile
        {
            get { return _Mobile; }
            set { _Mobile = value; }
        }

        private string _Email;
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private Gender _Gender;
        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>The gender.</value>
        public Gender Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }

        /// <summary>
        /// Returns this dummy user properties in a string.
        /// </summary>
        /// <returns></returns>
        public string ToDebugString()
        {
            return String.Format("DummyUser -> Username: {0}, Password: {1}, Surname: {2}, Firstname: {3}, DateOfBirth: {4}, Street: {5}, City: {6}, PostCode: {7}, Telephone: {8}, Mobile: {9}, Email: {10}, Gender: {11}",
                this.Username, this.Password, this.Surname, this.Firstname, this.DateOfBirth.ToString(), this.Street, this.City, this.Postcode, this.Telephone, this.Mobile, this.Email, this.Gender.ToString());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DummyUser"/> class.
        /// </summary>
        internal DummyUser()
        {

        }
    }
}
