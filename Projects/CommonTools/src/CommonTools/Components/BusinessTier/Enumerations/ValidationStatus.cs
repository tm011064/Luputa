using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Components.BusinessTier
{
    /// <summary>
    /// The validation status.
    /// </summary>
    public enum ValidationStatus
    {
        /// <summary>
        /// Valid
        /// </summary>
        Valid,
        /// <summary>
        /// Not all properties were valid.
        /// </summary>        
        NotAllPropertiesValid,
        /// <summary>
        /// Fields marked with BusinessObjectPropertyAttribute.IsMandatoryForInstance = true 
        /// need to be set before the object is allowed to be created at database
        /// </summary>  
        NotAllMandatoryFieldsProvided,
        /// <summary>
        /// 
        /// </summary>
        NullReference,
        /// <summary>
        /// used with custom errors
        /// </summary>
        ValidationFailed
    }
}
