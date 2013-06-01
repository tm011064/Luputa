using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Workmate.Components.Contracts
{
  public interface IApplication
  {
    int ApplicationId { get; set; }
    string ApplicationName { get; }
    string Description { get; }

    string DefaultAdminSenderEmailAddress { get; set;  }
    
    XElement GetExtraInfoClone();
  }
}
