using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Web.Components.Application;

namespace Workmate.Web.Components
{
  public static class Singletons
  {
    private static IApplicationContext _ApplicationContext;
    public static IApplicationContext ApplicationContext { get { return _ApplicationContext; } }

    public static void Initialize(IApplicationContext applicationContext)
    {
      _ApplicationContext = applicationContext;
    }
  }
}
