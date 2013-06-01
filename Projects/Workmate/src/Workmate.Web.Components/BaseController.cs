using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Workmate.Web.Components;
using Workmate.Components.Contracts.Membership;

namespace Workmate.Web.Components
{
  public class BaseController : Controller
  {
    private IUserBasic _User;
    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    /// <value>The user.</value>
    public new IUserBasic User
    {
      get
      {
        if (_User == null)
        {
          _User = base.User.Identity as IUserBasic;
        }

        return _User;
      }
    }

    private RequestContextData _RequestContextData;
    protected RequestContextData RequestContextData
    {
      get
      {
        if (_RequestContextData == null)
          _RequestContextData = InstanceContainer.RequestHelper.GetRequestContextData(this.Request);

        return _RequestContextData;
      }
    }

    protected int ApplicationId { get { return this.RequestContextData.ApplicationThemeInfo.ApplicationId; } }

    protected override void Initialize(System.Web.Routing.RequestContext requestContext)
    {
      base.Initialize(requestContext);
    }
  }
}
