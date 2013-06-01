using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Components.Contracts.Emails
{
  public enum EmailStatus : byte
  {
    Unsent = 1,
    Queued = 2,
    SendFailed = 3,
    Undelivered = 4,
    Sent = 5
  }
}
