using System;

namespace CommonTools.Components.Testing
{
    /// <summary>
    /// internally used enumeration for random username generation patterns
    /// </summary>
    internal enum UsernameType
    {
        /// <summary>
        /// Firstname_Lastname
        /// </summary>
        Firstname_Lastname = 0,
        /// <summary>
        /// F_Lastname
        /// </summary>
        F_Lastname = 1,
        /// <summary>
        /// Firstname_L
        /// </summary>
        Firstname_L = 2,
        /// <summary>
        /// Lastname_Firstname
        /// </summary>
        Lastname_Firstname = 3,
        /// <summary>
        /// L_Firstname
        /// </summary>
        L_Firstname = 4,
        /// <summary>
        /// Lastname_F
        /// </summary>
        Lastname_F = 5,
        /// <summary>
        /// FirstnameLastname
        /// </summary>
        FirstnameLastname = 6,
        /// <summary>
        /// LastnameFirstname
        /// </summary>
        LastnameFirstname = 7,
        /// <summary>
        /// FLastname
        /// </summary>
        FLastname = 8,
        /// <summary>
        /// LFirstname
        /// </summary>
        LFirstname = 9
    }
}
