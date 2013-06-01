using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Contracts.Emails;
using Workmate.ApplicationServer.Configuration;

namespace Workmate.ApplicationServer
{
  static class InstanceContainer
  {
    private static IApplicationSettings _ApplicationSettings;
    public static IApplicationSettings ApplicationSettings { get { return _ApplicationSettings; } }
    private static IEmailManager _EmailManager;
    public static IEmailManager EmailManager { get { return _EmailManager; } }

    public static void Initialize(
      IApplicationSettings applicationSettings
      , IEmailManager emailManager
      )
    {
      _ApplicationSettings = applicationSettings;
      _EmailManager = emailManager;
    }
  }
}
