using Workmate.Components.CMS.Articles;
using Workmate.Components.CMS.Membership;
using Workmate.Components.Contracts;
using Workmate.Components.Contracts.CMS.Articles;
using Workmate.Components.Contracts.Membership;
using Workmate.Web.Components.Application;
using Workmate.Web.Components.Security;
using Workmate.Components.Emails;
using Workmate.Components.Contracts.Emails;
using Workmate.Web.Components.Emails;
using Workmate.Components.Contracts.Company;

namespace Workmate.Web.Components
{
  public static class InstanceContainer
  {
    private static IApplicationContext _ApplicationContext;
    public static IApplicationContext ApplicationContext { get { return _ApplicationContext; } }

    private static ITicketManager _TicketManager;
    public static ITicketManager TicketManager { get { return _TicketManager; } }

    private static IWorkmateMembershipProvider _WorkmateMembershipProvider;
    public static IWorkmateMembershipProvider WorkmateMembershipProvider { get { return _WorkmateMembershipProvider; } }
    private static IWorkmateRoleProvider _WorkmateRoleProvider;
    public static IWorkmateRoleProvider WorkmateRoleProvider { get { return _WorkmateRoleProvider; } }

    private static IProfileImageManager _ProfileImageManager;
    public static IProfileImageManager ProfileImageManager { get { return _ProfileImageManager; } }
    private static ISystemProfileImageManager _SystemProfileImageManager;
    public static ISystemProfileImageManager SystemProfileImageManager { get { return _SystemProfileImageManager; } }

    private static IApplicationManager _ApplicationManager;
    public static IApplicationManager ApplicationManager { get { return _ApplicationManager; } }

    private static IArticleManager _ArticleManager;
    public static IArticleManager ArticleManager { get { return _ArticleManager; } }
    private static IArticleGroupManager _ArticleGroupManager;
    public static IArticleGroupManager ArticleGroupManager { get { return _ArticleGroupManager; } }
    private static IArticleGroupThreadManager _ArticleGroupThreadManager;
    public static IArticleGroupThreadManager ArticleGroupThreadManager { get { return _ArticleGroupThreadManager; } }
    private static IArticleAttachmentManager _ArticleAttachmentManager;
    public static IArticleAttachmentManager ArticleAttachmentManager { get { return _ArticleAttachmentManager; } }

    private static IRequestHelper _RequestHelper;
    public static IRequestHelper RequestHelper { get { return _RequestHelper; } }

    private static IApplicationDataCache _ApplicationDataCache;
    public static IApplicationDataCache ApplicationDataCache { get { return _ApplicationDataCache; } }

    private static IEmailManager _EmailManager;
    public static IEmailManager EmailManager { get { return _EmailManager; } }

    private static IEmailPublisher _EmailPublisher;
    public static IEmailPublisher EmailPublisher { get { return _EmailPublisher; } }

    private static IOfficeManager _OfficeManager;
    public static IOfficeManager OfficeManager { get { return _OfficeManager; } }
    
    private static IDepartmentManager _DepartmentManager;
    public static IDepartmentManager DepartmentManager { get { return _DepartmentManager; } }

    public static void Initialize(IApplicationContext applicationContext, ITicketManager ticketManager
      , IWorkmateMembershipProvider workmateMembershipProvider, IWorkmateRoleProvider workmateRoleProvider
      , IArticleManager articleManager, IRequestHelper requestHelper
      , IArticleAttachmentManager articleAttachmentManager, IApplicationManager applicationManager
      , IArticleGroupManager articleGroupManager, IArticleGroupThreadManager articleGroupThreadManager
      , IApplicationDataCache applicationDataCache, IProfileImageManager profileImageManager
      , ISystemProfileImageManager systemProfileImageManager, IEmailManager emailManager
      , IEmailPublisher emailPublisher
      , IOfficeManager officeManager, IDepartmentManager departmentManager
      )
    {
      _ApplicationContext = applicationContext;
      _TicketManager = ticketManager;
      _WorkmateMembershipProvider = workmateMembershipProvider;
      _WorkmateRoleProvider = workmateRoleProvider;
      _ArticleManager = articleManager;
      _RequestHelper = requestHelper;
      _ArticleAttachmentManager = articleAttachmentManager;
      _ApplicationManager = applicationManager;
      _ArticleGroupManager = articleGroupManager;
      _ArticleGroupThreadManager = articleGroupThreadManager;
      _ApplicationDataCache = applicationDataCache;
      _ProfileImageManager = profileImageManager;
      _SystemProfileImageManager = systemProfileImageManager;
      _EmailManager = emailManager;
      _EmailPublisher = emailPublisher;
      _OfficeManager = officeManager;
      _DepartmentManager = departmentManager;
    }
  }
}
