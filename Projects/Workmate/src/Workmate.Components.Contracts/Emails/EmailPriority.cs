using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Components.Contracts.Emails
{
  public enum EmailPriority : byte
  {
    SendImmediately = 1,
    CanWait = 10,
  }
}
