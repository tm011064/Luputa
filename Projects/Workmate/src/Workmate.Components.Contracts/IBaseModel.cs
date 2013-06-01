using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Components.Contracts
{
  public interface IBaseModel
  {
    string ErrorMessage { get; }
    List<string> Errors { get; set; }
    bool HasErrors();
  }
}
