using System;
using System.Collections.Generic;

namespace CommonTools.Components.Mails
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMailNotificationMessage
    {
        bool Enabled { get; set; }
        TimeSpan Interval { get; set; }
        TimeSpan PreventResendInterval { get; set; }
        string Recipients { get; set; }
        string Sender { get; set; }
        string Name { get; set; }
    }
}
