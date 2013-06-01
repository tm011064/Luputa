using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace CommonTools.Components.Testing
{
    /// <summary>
    /// This class provides methods to create dummy data that can be used for testing. In order to work, you need
    /// to provide an XML document containing dummy values for names, streets, cities... The XSD schema for this XML file
    /// can be found at DummyData.xsd.
    /// </summary>
    public class DummyDataManager
    {
        #region constants
        private const string FIRSTNAMES_TAG = "firstname";
        private const string FIRSTNAMES_GENDER_ATTRIBUTE = "gender";
        private const string MALE_FIRSTNAMES_ATTRIBUTE = "male";
        private const string FEMALE_FIRSTNAMES_ATTRIBUTE = "female";
        private const string SURNAMES_TAG = "surname";
        private const string EMAIL_PROVIDERS_TAG = "emailProvider";
        private const string TEXT_CONTENTS_TAG = "textContent";
        private const string STREETS_TAG = "street";
        private const string CITIES_TAG = "city";
        private const string IMAGES_TAG = "image";
        private const string ICONS_TAG = "icon";
        private const string VIDEOS_TAG = "video";
        private const string SONGS_TAG = "song";
        private const string FILES_TAG = "file";

        private const string CHARACTERS = " ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string POSTCODECHARACTERS = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string USERNAME_SEPERATORS = "._-";

        private XmlDocument _Document;  // the xml document do be loaded
        private Random _Random;       // instance of the Random class used throughout this object to generate random values
        #endregion

        #region properties
        private int _MaxUsernameLength = 32;
        /// <summary>
        /// Gets or sets the maximum character length of a DummyUser's username.
        /// </summary>
        /// <value>The maximum character length of a DummyUser's username.</value>
        public int MaxUsernameLength
        {
            get { return _MaxUsernameLength; }
            set { _MaxUsernameLength = value; }
        }
        private int _MinUsernameLength = 4;
        /// <summary>
        /// Gets or sets the minimum character length of a DummyUser's username.
        /// </summary>
        /// <value>The minimum character length of a DummyUser's username.</value>
        public int MinUsernameLength
        {
            get { return _MinUsernameLength; }
            set { _MinUsernameLength = value; }
        }
        private int _MaxPasswordLength = 16;
        /// <summary>
        /// Gets or sets the maximum character length of a DummyUser's password.
        /// </summary>
        /// <value>The maximum character length of a DummyUser's password.</value>
        public int MaxPasswordLength
        {
            get { return _MaxPasswordLength; }
            set { _MaxPasswordLength = value; }
        }
        private int _MinPasswordLength = 3;
        /// <summary>
        /// Gets or sets the minimum character length of a DummyUser's password.
        /// </summary>
        /// <value>The minimum character length of a DummyUser's password.</value>
        public int MinPasswordLength
        {
            get { return _MinPasswordLength; }
            set { _MinPasswordLength = value; }
        }
        private int _MinAgeInYears = 12;
        /// <summary>
        /// Gets or sets the minimum age of a DummyUser in years.
        /// </summary>
        /// <value>The minimum age of a DummyUser in years.</value>
        public int MinAgeInYears
        {
            get { return _MinAgeInYears; }
            set { _MinAgeInYears = value; }
        }
        private int _MaxAgeInYears = 105;
        /// <summary>
        /// Gets or sets the maximum age of a DummyUser in years.
        /// </summary>
        /// <value>The maximum age of a DummyUser in years.</value>
        public int MaxAgeInYears
        {
            get { return _MaxAgeInYears; }
            set { _MaxAgeInYears = value; }
        }

        private List<string> _MaleFirstnames;
        /// <summary>
        /// Gets a list of all male firstnames.
        /// </summary>
        /// <value>A list of all male firstnames.</value>
        protected List<string> MaleFirstnames
        {
            get { return _MaleFirstnames == null ? new List<string>() : _MaleFirstnames; }
        }
        private List<string> _FemaleFirstnames;
        /// <summary>
        /// Gets a list of all female firstnames.
        /// </summary>
        /// <value>A list of all female firstnames.</value>
        protected List<string> FemaleFirstnames
        {
            get { return _FemaleFirstnames == null ? new List<string>() : _FemaleFirstnames; }
        }
        private List<string> _Surnames;
        /// <summary>
        /// Gets a list of all surnames.
        /// </summary>
        /// <value>A list of all surnames.</value>
        protected List<string> Surnames
        {
            get { return _Surnames == null ? new List<string>() : _Surnames; }
        }
        private List<string> _EmailProviders;
        /// <summary>
        /// Gets a list of all email providers.
        /// </summary>
        /// <value>A list of all email providers.</value>
        protected List<string> EmailProviders
        {
            get { return _EmailProviders == null ? new List<string>() : _EmailProviders; }
        }
        private List<string> _TextContents;
        /// <summary>
        /// Gets a list of all text contents.
        /// </summary>
        /// <value>A list of all text contents.</value>
        protected List<string> TextContents
        {
            get { return _TextContents == null ? new List<string>() : _TextContents; }
        }
        private List<string> _Streets;
        /// <summary>
        /// Gets a list of all streets.
        /// </summary>
        /// <value>A list of all streets.</value>
        protected List<string> Streets
        {
            get { return _Streets == null ? new List<string>() : _Streets; }
        }
        private List<string> _Cities;
        /// <summary>
        /// Gets a list of all cities.
        /// </summary>
        /// <value>A list of all cities.</value>
        protected List<string> Cities
        {
            get { return _Cities == null ? new List<string>() : _Cities; }
        }
        private List<string> _ImageLocations;
        /// <summary>
        /// Gets a list of all image locations.
        /// </summary>
        /// <value>A list of all image locations.</value>
        protected List<string> ImageLocations
        {
            get { return _ImageLocations == null ? new List<string>() : _ImageLocations; }
        }
        private List<string> _IconLocations;
        /// <summary>
        /// Gets a list of all icon locations.
        /// </summary>
        /// <value>A list of all icon locations.</value>
        protected List<string> IconLocations
        {
            get { return _IconLocations == null ? new List<string>() : _IconLocations; }
        }
        private List<string> _VideoLocations;
        /// <summary>
        /// Gets a list of all video locations.
        /// </summary>
        /// <value>A list of all video locations.</value>
        protected List<string> VideoLocations
        {
            get { return _VideoLocations == null ? new List<string>() : _VideoLocations; }
        }
        private List<string> _FileLocations;
        /// <summary>
        /// Gets a list of all file locations.
        /// </summary>
        /// <value>A list of all file locations.</value>
        protected List<string> FileLocations
        {
            get { return _FileLocations == null ? new List<string>() : _FileLocations; }
        }
        private List<string> _SongLocations;
        /// <summary>
        /// Gets a list of all song locations.
        /// </summary>
        /// <value>A list of all song locations.</value>
        protected List<string> SongLocations
        {
            get { return _SongLocations == null ? new List<string>() : _SongLocations; }
        }
        #endregion

        #region private/protected methods
        /// <summary>
        /// This method returns a random username which can be used for a DummyUser's username or email address, based on the 
        /// DummyUser's first and last name
        /// </summary>
        /// <param name="firstname">The first name of the DummyUser</param>
        /// <param name="surname">The surname of the DummyUser</param>
        /// <returns>A random username which can be used for a DummyUser's username or email address, based on the 
        /// DummyUser's first and last name</returns>
        protected virtual string GetRandomUsername(string firstname, string surname)
        {
            switch ((UsernameType)_Random.Next(0, 10))
            {
                case UsernameType.F_Lastname:
                    return firstname[0].ToString() + USERNAME_SEPERATORS[_Random.Next(0, USERNAME_SEPERATORS.Length)] + surname;

                case UsernameType.Firstname_L:
                    return firstname + USERNAME_SEPERATORS[_Random.Next(0, USERNAME_SEPERATORS.Length)] + surname[0].ToString();

                case UsernameType.Firstname_Lastname:
                    return firstname + USERNAME_SEPERATORS[_Random.Next(0, USERNAME_SEPERATORS.Length)] + surname;

                case UsernameType.FirstnameLastname:
                    return firstname + surname;

                case UsernameType.FLastname:
                    return firstname[0].ToString() + surname;

                case UsernameType.L_Firstname:
                    return surname[0].ToString() + USERNAME_SEPERATORS[_Random.Next(0, USERNAME_SEPERATORS.Length)] + firstname;

                case UsernameType.Lastname_F:
                    return surname + USERNAME_SEPERATORS[_Random.Next(0, USERNAME_SEPERATORS.Length)] + firstname[0].ToString();

                case UsernameType.Lastname_Firstname:
                    return surname + USERNAME_SEPERATORS[_Random.Next(0, USERNAME_SEPERATORS.Length)] + firstname;

                case UsernameType.LastnameFirstname:
                    return surname + firstname;

                case UsernameType.LFirstname:
                    return surname[0].ToString() + firstname;

            }
            return string.Empty;
        }

        /// <summary>
        /// Creates a random street address for a DummyUser.
        /// </summary>
        /// <returns>Returns a random street address for a DummyUser</returns>
        protected virtual string CreateStreetAddress()
        {
            int randomLength = _Random.Next(0, 3);
            string houseNumber = string.Empty;
            if (randomLength == 0)
            {
                houseNumber = _Random.Next(1, 120).ToString() + CHARACTERS[_Random.Next(0, 12)].ToString();
                houseNumber = houseNumber.Trim();
            }
            else
            {
                for (int i = 0; i < randomLength; i++)
                {
                    houseNumber += _Random.Next(1, 80).ToString() + "/";
                }
                houseNumber = houseNumber.Remove(houseNumber.Length - 1);
            }
            return houseNumber + " " + _Streets[_Random.Next(0, _Streets.Count)];
        }

        /// <summary>
        /// Creates a random mobile number.
        /// </summary>
        /// <returns>A random mobile number.</returns>
        protected virtual string CreateMobile()
        {
            return _Random.Next(100000000, 999999999).ToString();
        }

        /// <summary>
        /// Creates a random password.
        /// </summary>
        /// <returns>A random password.</returns>
        protected virtual string CreatePassword()
        {
            return CommonTools.Components.Security.RandomPassword.Generate(MinPasswordLength, MaxPasswordLength);
        }

        /// <summary>
        /// Creates a random post code.
        /// </summary>
        /// <returns>A random post code.</returns>
        protected virtual string CreatePostCode()
        {
            string returnString = string.Empty;
            for (int i = 0; i < _Random.Next(5, 8); i++)
            {
                returnString += POSTCODECHARACTERS[_Random.Next(0, POSTCODECHARACTERS.Length)];
            }
            return returnString;
        }

        /// <summary>
        /// Creates a random telephone number.
        /// </summary>
        /// <returns>A random telephone number</returns>
        protected virtual string CreateTelephone()
        {
            return _Random.Next(100000000, 999999999).ToString();
        }

        /// <summary>
        /// Creates a random username based on the given first name and surname.
        /// </summary>
        /// <param name="firstname">The first name of the DummyUser</param>
        /// <param name="surname">The surname of the DummyUser</param>
        /// <returns>A random username based on the given first name and surname</returns>
        protected virtual string CreateUsername(string firstname, string surname)
        {
            string returnString = GetRandomUsername(firstname, surname);
            if (returnString.Length > MaxUsernameLength)
                returnString.Remove(MaxUsernameLength - 1);
            else if (returnString.Length < MinUsernameLength)
            {
                for (int i = 0; i < MinUsernameLength - returnString.Length; i++)
                    returnString += _Random.Next(0, 10).ToString();
            }

            return returnString;
        }

        /// <summary>
        /// Creates a random email address based on the given first name and surname.
        /// </summary>
        /// <param name="firstname">The first name of the DummyUser</param>
        /// <param name="surname">The surname of the DummyUser</param>
        /// <returns>A random email address based on the given first name and surname</returns>
        protected virtual string CreateEmail(string firstname, string surname)
        {
            return GetRandomUsername(firstname, surname) + "@" + _EmailProviders[_Random.Next(0, _EmailProviders.Count)];
        }
        #endregion

        #region methods
        /// <summary>
        /// This method returns a DummyUser object with random property values.
        /// </summary>
        /// <returns>A DummyUser object with random property values</returns>
        public DummyUser GetDummy()
        {
            DummyUser dummy = new DummyUser();

            dummy.Gender = (Gender)_Random.Next(0, 2);
            dummy.Firstname = (dummy.Gender == Gender.Male) ? _MaleFirstnames[_Random.Next(0, _MaleFirstnames.Count)] : _FemaleFirstnames[_Random.Next(0, _FemaleFirstnames.Count)];
            dummy.Surname = _Surnames[_Random.Next(0, _Surnames.Count)];
            dummy.City = _Cities[_Random.Next(0, _Cities.Count)];
            dummy.DateOfBirth = new DateTime(DateTime.Now.Year - _Random.Next(MinAgeInYears, MaxAgeInYears), _Random.Next(1, 13), 1);
            dummy.DateOfBirth.AddDays((double)_Random.Next(0, 31));

            dummy.Street = CreateStreetAddress();
            dummy.Mobile = CreateMobile();
            dummy.Password = CreatePassword();
            dummy.Postcode = CreatePostCode();
            dummy.Telephone = CreateTelephone();
            dummy.Username = CreateUsername(dummy.Firstname, dummy.Surname);
            dummy.Email = CreateEmail(dummy.Firstname, dummy.Surname);

            return dummy;
        }

        /// <summary>
        /// Gets a random file path.
        /// </summary>
        /// <returns>A random file path.</returns>
        public string GetDummyFilePath()
        {
            if (_FileLocations.Count == 0)
                return null;

            return _FileLocations[_Random.Next(0, _FileLocations.Count)];
        }
        /// <summary>
        /// Gets a random icon file path.
        /// </summary>
        /// <returns>A random icon file path.</returns>
        public string GetDummyIconFilePath()
        {
            if (_IconLocations.Count == 0)
                return null;

            return _IconLocations[_Random.Next(0, _IconLocations.Count)];
        }
        /// <summary>
        /// Gets a random image file path.
        /// </summary>
        /// <returns>A random image file path.</returns>
        public string GetDummyImageFilePath()
        {

            if (_ImageLocations.Count == 0)
                return null;

            return _ImageLocations[_Random.Next(0, _ImageLocations.Count)];
        }
        /// <summary>
        /// Gets a random song file path.
        /// </summary>
        /// <returns>A random song file path.</returns>
        public string GetDummySongFilePath()
        {

            if (_SongLocations.Count == 0)
                return null;

            return _SongLocations[_Random.Next(0, _SongLocations.Count)];
        }
        /// <summary>
        /// Gets a random video file path.
        /// </summary>
        /// <returns>A random video file path.</returns>
        public string GetDummyVideoFilePath()
        {
            if (_VideoLocations.Count == 0)
                return null;

            return _VideoLocations[_Random.Next(0, _VideoLocations.Count)];
        }
        /// <summary>
        /// Gets a random text.
        /// </summary>
        /// <param name="minLength">The minimum character length of the text.</param>
        /// <param name="maxLength">The maximum character length of the text.</param>
        /// <returns>A random text.</returns>
        public string GetDummyText(int minLength, int maxLength)
        {
            if (_TextContents.Count == 0)
                return null;

            string text = _TextContents[_Random.Next(0, _TextContents.Count)];
            if (text.Length < maxLength)
            {
                StringBuilder sb = new StringBuilder(text);
                bool doConcat = true;
                while (doConcat)
                {
                    sb.Append(" " + text);
                    if (sb.Length >= maxLength)
                    {
                        text = sb.ToString();
                        doConcat = false;
                    }
                }
            }
            return text.Substring((text.Length - maxLength) - _Random.Next(0, text.Length - maxLength - 1), _Random.Next(minLength, maxLength));
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DummyDataManager"/> class.
        /// </summary>
        /// <param name="xmlDummyData">An XML document containing dummy values. The XSD schema for this XML file
        /// can be found at DummyData.xsd</param>
        public DummyDataManager(string xmlDummyData)
        {
            _Document = new XmlDocument();
            _Random = new Random();

            _Document.Load(xmlDummyData);

            _MaleFirstnames = new List<string>();
            _FemaleFirstnames = new List<string>();
            foreach (XmlNode node in _Document.GetElementsByTagName(FIRSTNAMES_TAG))
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    if (node.Attributes[FIRSTNAMES_GENDER_ATTRIBUTE].Value == MALE_FIRSTNAMES_ATTRIBUTE)
                    {
                        _MaleFirstnames.Add(node.InnerText);
                    }
                    else
                    {
                        _FemaleFirstnames.Add(node.InnerText);
                    }
                }
            }
            _Surnames = new List<string>();
            foreach (XmlNode node in _Document.GetElementsByTagName(SURNAMES_TAG))
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    _Surnames.Add(node.InnerText);
                }
            }
            _EmailProviders = new List<string>();
            foreach (XmlNode node in _Document.GetElementsByTagName(EMAIL_PROVIDERS_TAG))
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    _EmailProviders.Add(node.InnerText);
                }
            }
            _TextContents = new List<string>();
            foreach (XmlNode node in _Document.GetElementsByTagName(TEXT_CONTENTS_TAG))
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    _TextContents.Add(node.InnerText);
                }
            }
            _Streets = new List<string>();
            foreach (XmlNode node in _Document.GetElementsByTagName(STREETS_TAG))
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    _Streets.Add(node.InnerText);
                }
            }
            _Cities = new List<string>();
            foreach (XmlNode node in _Document.GetElementsByTagName(CITIES_TAG))
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    _Cities.Add(node.InnerText);
                }
            }
            _ImageLocations = new List<string>();
            foreach (XmlNode node in _Document.GetElementsByTagName(IMAGES_TAG))
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    _ImageLocations.Add(node.InnerText);
                }
            }
            _IconLocations = new List<string>();
            foreach (XmlNode node in _Document.GetElementsByTagName(ICONS_TAG))
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    _IconLocations.Add(node.InnerText);
                }
            }
            _VideoLocations = new List<string>();
            foreach (XmlNode node in _Document.GetElementsByTagName(VIDEOS_TAG))
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    _VideoLocations.Add(node.InnerText);
                }
            }
            _SongLocations = new List<string>();
            foreach (XmlNode node in _Document.GetElementsByTagName(SONGS_TAG))
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    _SongLocations.Add(node.InnerText);
                }
            }
            _FileLocations = new List<string>();
            foreach (XmlNode node in _Document.GetElementsByTagName(FILES_TAG))
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    _FileLocations.Add(node.InnerText);
                }
            }
        }
        #endregion
    }
}
