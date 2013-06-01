using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Components.Contracts.CMS
{
  public enum RatingDataRepositoryActionStatus
  {
    Success,
    SqlError = -1000,
    ValidationFailed = -1001,
    SelfRatingNotAllowed = -1
  }
}
