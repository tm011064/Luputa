using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Contracts;

namespace Workmate.Components.Entities
{
  public class BaseModel : IBaseModel
  {
    #region IBaseModel Members

    public string ErrorMessage { get; set; }

    private List<string> _Errors = new List<string>();
    public List<string> Errors
    {
      get { return _Errors; }
      set { _Errors = value; }
    }

    public bool HasErrors()
    {
      return !string.IsNullOrWhiteSpace(this.ErrorMessage)
        || this.Errors.Count > 0;
    }
    
    #endregion
  }
}
