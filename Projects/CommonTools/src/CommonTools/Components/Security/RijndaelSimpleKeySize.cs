using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace CommonTools.Components.Security
{
    /// <summary>
    /// Enumeration for all possible KeySizes to be used at RijndaelSimple class
    /// </summary>
    public enum RijndaelSimpleKeySize : int
    {
        /// <summary>
        /// keysize is 128
        /// </summary>
        Small = 128,
        /// <summary>
        /// keysize is 192
        /// </summary>
        Medium = 192,
        /// <summary>
        /// keysize is 256
        /// </summary>
        Safe = 256
    }
}
