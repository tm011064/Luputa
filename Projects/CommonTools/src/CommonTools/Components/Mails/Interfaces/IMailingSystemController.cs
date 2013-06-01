using System;
using System.Configuration;
using System.Collections.Generic;

namespace CommonTools.Components.Mails
{
	/// <summary>
	/// Provides the base interface for the MailingSystem configuration section.
	/// </summary>
	public interface IMailingSystemController
	{
        /// <summary>
        /// Gets the execution interval in minutes for all IMailingSystemItems at this object's IMailingSystemItem collection. This value can be overwritten
        /// by the IMailingSystemItem itself.
        /// </summary>
        /// <value>The minutes.</value>
        int Minutes { get; }
        /// <summary>
        /// Creates an instance of this object's default controller.
        /// </summary>
        /// <returns></returns>
        IMailingSystemController CreateMailingSystemControllerInstance();
        /// <summary>
        /// Gets the mail notification messages.
        /// </summary>
        List<IMailNotificationMessage> MailNotificationMessages { get; } 
	}
}
