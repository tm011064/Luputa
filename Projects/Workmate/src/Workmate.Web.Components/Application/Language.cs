using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Web.Components.Application
{
  public class Language
  {
    public string ShortCode { get; private set; }
    public string Name { get; private set; }

    public Language(string shortCode, string name)
    {
      this.ShortCode = shortCode;
      this.Name = name;
    }
  }
}
