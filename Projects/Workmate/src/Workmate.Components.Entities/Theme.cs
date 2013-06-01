using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Contracts;

namespace Workmate.Components.Entities
{
  public class Theme : ITheme
  {
    #region properties

    public int ThemeId { get; private set; }

    public string Name { get; private set; }

    public HashSet<string> LoweredDomainNames { get; private set; }

    public int ApplicationId { get; private set; }

    #endregion

    #region constructors
    public Theme(int themeId, string name, string loweredDomainNames, int applicationId)
    {
      this.ThemeId = themeId;
      this.Name = name;

      this.LoweredDomainNames = new HashSet<string>();
      foreach (string domainName in loweredDomainNames.Split(';'))
        this.LoweredDomainNames.Add(domainName.Trim().ToLowerInvariant());

      this.ApplicationId = applicationId;
    }
    #endregion
  }
}
