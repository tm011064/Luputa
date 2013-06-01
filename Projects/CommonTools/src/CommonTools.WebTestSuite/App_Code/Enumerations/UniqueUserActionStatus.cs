﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.TestApp.Components
{
    public enum UniqueUserActionStatus
    {
        Success,
        SqlException,
        ValidationFailed,
        NoRecordRowAffected,
        UnknownError
    }
}
