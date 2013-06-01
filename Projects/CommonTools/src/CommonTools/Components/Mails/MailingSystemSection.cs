using System;
using System.Configuration;
using System.Web.Caching;
using System.Collections;
using System.Collections.Generic;


namespace CommonTools.Components.Mails
{
    /// <summary>
    /// Provides the handler for the MailingSystem section of app.config.
    /// </summary>
    public class MailingSystemSection : ConfigurationSection, IMailingSystemController
    {
        private Dictionary<string, IMailNotificationMessage> _MailingSystemItems;

        /// <summary>
        /// holds the collection of MailingSystem objects in the app.config.
        /// </summary>
        [ConfigurationProperty("objects", IsRequired = true)]
        public MailingSystemElements MailingSystemElementCollection
        {
            get { return (MailingSystemElements)base["objects"]; }
        }

        /// <summary>
        /// The number of minutes to hold the MailingSystemd object.
        /// </summary>
        [ConfigurationProperty("MailingSystemControllerProviderType", IsRequired = false)]
        public string MailingSystemControllerType
        {
            get
            {
                if (base["MailingSystemControllerProviderType"] != null)
                    return (string)base["MailingSystemControllerProviderType"];

                return null;
            }
        }

        #region IMailingSystemController Members
        /// <summary>
        /// Creates an instance of this object's default controller.
        /// </summary>
        /// <returns></returns>
        public IMailingSystemController CreateMailingSystemControllerInstance()
        {
            return MailingSystemSectionManager.MailingSystemSection;
        }

        /// <summary>
        /// Gets the IMailNotificationMessage collection associated with this IMailingSystemController.
        /// </summary>
        /// <value>The MailingSystem items.</value>
        public Dictionary<string, IMailNotificationMessage> MailingSystemItems
        {
            get
            {
                if (_MailingSystemItems == null)
                {
                    _MailingSystemItems = new Dictionary<string, IMailNotificationMessage>();
                    foreach (MailingSystemElement item in MailingSystemElementCollection)
                        _MailingSystemItems.Add(item.Name, (IMailNotificationMessage)item);
                }
                return _MailingSystemItems;
            }
        }

        /// <summary>
        /// Gets the MailingSystem item.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IMailNotificationMessage GetMailingSystemItem(string key)
        {
            if (MailingSystemItems.ContainsKey(key))
                return MailingSystemItems[key];

            return null;
        }

        /// <summary>
        /// Gets the amount of minutes to MailingSystem all IMailNotificationMessages at this object's IMailNotificationMessage collection. This value can be overwritten
        /// by the IMailNotificationMessage itself.
        /// </summary>
        /// <value>The minutes.</value>
        public int Minutes
        {
            get { return MailingSystemElementCollection.Minutes; }
        }

        /// <summary>
        /// Gets a value indicating whether to enable caching or not. If this is set to false, no IMailNotificationMessage at the IMailNotificationMessage collection
        /// can use the HttpRuntime MailingSystem.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return MailingSystemElementCollection.Enabled; }
        }


        /// <summary>
        /// Gets the continuous access stale key suffix for memMailingSystemd.
        /// </summary>
        /// <value>The continuous access stale key suffix for memMailingSystemd.</value>
        public string ContinuousAccessStaleKeySuffixForMemMailingSystemd
        {
            get { return MailingSystemElementCollection.ContinuousAccessStaleKeySuffixForMemMailingSystemd; }
        }
        #endregion
    }

    /// <summary>
    /// Provides the handler for each individual MailingSystem node in the MailingSystem section of app.config.
    /// </summary>
    public class MailingSystemElements : ConfigurationElementCollection
    {
        #region attributes
        /// <summary>
        /// Gets the continuous access stale key suffix for memMailingSystemd.
        /// </summary>
        /// <value>The continuous access stale key suffix for memMailingSystemd.</value>
        [ConfigurationProperty("continuousAccessStaleKeySuffixForMemMailingSystemd", DefaultValue = "~")]
        public string ContinuousAccessStaleKeySuffixForMemMailingSystemd
        {
            get { return (string)base["continuousAccessStaleKeySuffixForMemMailingSystemd"]; }
        }

        /// <summary>
        /// The number of minutes to hold the MailingSystemd object.
        /// </summary>
        [ConfigurationProperty("minutes", DefaultValue = "15")]
        public int Minutes
        {
            get { return (int)base["minutes"]; }
        }

        /// <summary>
        /// Whether the MailingSystem object is MailingSystemd.
        /// </summary>
        [ConfigurationProperty("enabled", DefaultValue = "true")]
        public bool Enabled
        {
            get { return (bool)base["enabled"]; }
        }
        #endregion

        /// <summary>
        /// Overridden. Creates a new MailingSystemElement.
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new MailingSystemElement();
        }

        /// <summary>
        /// Overridden. Retrieves the specified MailingSystem element name for the given weak typed node.
        /// </summary>
        /// <param name="element">The node in app.config</param>
        /// <returns>The MailingSystem element name located at the configuration element.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MailingSystemElement)element).Name;
        }
    }

    /// <summary>
    /// Provides the MailingSystem element object as specified through app.config.
    /// </summary>
    public class MailingSystemElement : ConfigurationElement, IMailNotificationMessage
    {
        [ConfigurationProperty("enabled", DefaultValue = "true")]
        public bool Enabled
        {
            get { return (bool)base["enabled"]; }
        }
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base["name"]; }
        }
        [ConfigurationProperty("interval", IsRequired = true, IsKey = true)]
        public TimeSpan Interval
        {
            get { return (TimeSpan)base["interval"]; }
        }

        [ConfigurationProperty("recipients", IsRequired = true, IsKey = true)]
        public string Recipients
        {
            get { return (string)base["recipients"]; }
        }
        [ConfigurationProperty("sender", IsRequired = true, IsKey = true)]
        public string Sender
        {
            get { return (string)base["sender"]; }
        }
    }
}