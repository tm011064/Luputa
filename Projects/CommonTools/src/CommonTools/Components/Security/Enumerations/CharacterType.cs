using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.Components.Security
{
    /// <summary>
    /// 
    /// </summary>
    public enum CharacterType
    {
        /// <summary>
        /// Only use lower case characters.
        /// </summary>
        LowerCase,

        /// <summary>
        /// Only use upper case characters.
        /// </summary>
        UpperCase,

        /// <summary>
        /// Use only alphabetic characters.
        /// </summary>
        LowerUpperCase,

        /// <summary>
        /// Use only numbers.
        /// </summary>
        Numbers,

        /// <summary>
        /// Alphanumeric but all upper case.
        /// </summary>
        NumbersUpperCase,

        /// <summary>
        /// Alphanumeric but all lower case.
        /// </summary>
        NumbersLowerCase,

        /// <summary>
        /// Alphanumeric
        /// </summary>
        NumbersUpperLowerCase
    }
}
