using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Components.Contracts
{
  public enum DataRepositoryActionStatus
  {
    Success,
    SqlError = -1000,
    ValidationFailed = -1001,
    NoRecordRowAffected = -1002,
    UnknownError = -1099,
    NameNotUnique = -1003,
    PrimaryKeyNotUnique = -1004,
    UniqueKeyConstraint = -1005,
    InvalidHtml = -2000
  }
}
