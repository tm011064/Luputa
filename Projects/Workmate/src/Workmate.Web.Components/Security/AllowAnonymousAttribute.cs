using System;

namespace Workmate.Web.Components.Security
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
  public sealed class AllowAnonymousAttribute : Attribute { }
}
