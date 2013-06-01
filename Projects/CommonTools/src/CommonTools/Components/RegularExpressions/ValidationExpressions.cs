using System.Text.RegularExpressions;

namespace CommonTools.Components.RegularExpressions
{
    /// <summary>
    /// This class contains regular expression validation strings
    /// </summary>
    public static class ValidationExpressions
    {
        /// <summary>
        /// Validates a valid email
        /// </summary>
        public const string EmailAddress = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        /// <summary>
        /// 
        /// </summary>
        public const string UKMobilePhoneNumber = @"^447([\d]{9})$";

        #region public methods
        /// <summary>
        /// Determines whether [is valid UK mobile phone number] [the specified phone number].
        /// </summary>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid UK mobile phone number] [the specified phone number]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidUKMobilePhoneNumber(string phoneNumber)
        {
            return new Regex(UKMobilePhoneNumber).IsMatch(phoneNumber);
        }
        /// <summary>
        /// Determines whether [the specified email] [is valid] .
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid email] [the specified email]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmail(string email)
        {
            return new Regex(EmailAddress).IsMatch(email);
        }

        /// <summary>
        /// Determines whether the specified email addresses, provided with a specified delimiter are valid.
        /// </summary>
        /// <param name="emails">The emails in the following format: email1@test.com{delimiter}email2@test.com{delimiter}...</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static bool AreValidEmails(string emails, char delimiter)
        {
            foreach (string email in emails.Split(delimiter))
            {
                if (!IsValidEmail(email.Trim()))
                    return false;
            }
            return true;
        }
        #endregion
    }
}
