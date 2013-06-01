using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Components.BusinessTier
{
    /// <summary>
    /// Enumeration of possible validation errors at properties
    /// </summary>
    public enum PropertyValidationError
    {
        /// <summary>
        /// The value exceeded the defined maximum value of the object
        /// </summary>
        MaximumValueConstraint,
        /// <summary>
        /// The value was too low
        /// </summary>
        MinimumValueConstraint,
        /// <summary>
        /// The value didn't match the defined regular expression.
        /// </summary>
        RegexMatchConstraint
    }
}
