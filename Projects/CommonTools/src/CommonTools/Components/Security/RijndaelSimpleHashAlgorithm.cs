using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace CommonTools.Components.Security
{
    /// <summary>
    /// Enumeration for all possible algorithms to be used at RijndaelSimple class
    /// </summary>
    public enum RijndaelSimpleHashAlgorithm
    {
        /// <summary>
        /// MD5 algorithm
        /// </summary>
        MD5,
        /// <summary>
        /// SHA1 algorithm (slower than MD5 but better results)
        /// </summary>
        SHA1
    }
}
