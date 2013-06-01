using System;
using System.Configuration;

namespace CommonTools.Components.Mails
{
	/// <summary>
	/// This is the class that's capable of enumerating the MailingSystem section of your app.config.
	/// </summary>
    public static class MailingSystemSectionManager
    {
		/// <summary>
		/// Gets the MailingSystem section settings of the app.config
		/// </summary>
        public static MailingSystemSection MailingSystemSection
        {
            get
			{
                return (MailingSystemSection)ConfigurationManager.GetSection(MailingSystemControllerFactory.SECTION_NAME);
			}
        }
    }
}
