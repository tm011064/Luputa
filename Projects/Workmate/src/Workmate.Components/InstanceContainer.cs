using Workmate.Components.Contracts.Membership;
using Workmate.Configuration;
using Workmate.Data;

namespace Workmate.Components
{
  public static class InstanceContainer
  {
    private static IApplicationSettings _ApplicationSettings;
    public static IApplicationSettings ApplicationSettings { get { return _ApplicationSettings; } }

    private static IDataStore _DataStore;
    public static IDataStore DataStore { get { return _DataStore; } }

    private static IMembershipSettings _MembershipSettings;
    public static IMembershipSettings MembershipSettings { get { return _MembershipSettings; } }
    
    public static void Initialize(IDataStore dataStore, IApplicationSettings applicationSettings
      , IMembershipSettings membershipSettings)
    {
      _ApplicationSettings = applicationSettings;
      _DataStore = dataStore;
      _MembershipSettings = membershipSettings;
    }
  }
}
