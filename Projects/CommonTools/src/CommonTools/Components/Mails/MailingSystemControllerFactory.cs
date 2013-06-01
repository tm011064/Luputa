using System;
using System.Configuration;

namespace CommonTools.Components.Mails
{
    /// <summary>
    /// This is the class that's capable of enumerating the MailingSystem section of your app.config.
    /// </summary>
    public static class MailingSystemControllerFactory
    {
        /// <summary>
        /// The configuration's configsection key
        /// </summary>
        public const string SECTION_NAME = "MailingSystem";

        /// <summary>
        /// Gets the MailingSystem section settings of the app.config
        /// </summary>
        public static IMailingSystemController CreateMailingSystemController()
        {
            MailingSystemSection section = (MailingSystemSection)ConfigurationManager.GetSection(SECTION_NAME);

            if (!string.IsNullOrEmpty(section.MailingSystemControllerType))
            {
                IMailingSystemController customController = Activator.CreateInstance(Type.GetType(section.MailingSystemControllerType)) as IMailingSystemController;
                if (customController != null)
                {
                    return customController.CreateMailingSystemControllerInstance();
                }
            }

            return section;
        }
    }
}
