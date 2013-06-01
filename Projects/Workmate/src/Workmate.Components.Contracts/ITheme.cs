using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Components.Contracts
{
  public interface ITheme
  {
    int ThemeId { get; }
    string Name { get; }
    HashSet<string> LoweredDomainNames { get; }
    int ApplicationId { get; }
  }
}
