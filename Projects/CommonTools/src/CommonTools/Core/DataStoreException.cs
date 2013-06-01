using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class DataStoreException : BaseException
    {
        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStoreException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="isLogged">if set to <c>true</c> [is logged].</param>
        public DataStoreException(Exception innerException, bool isLogged) : base(innerException, isLogged) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataStoreException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="isLogged">if set to <c>true</c> [is logged].</param>
        public DataStoreException(string message, bool isLogged) : base(message, isLogged) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataStoreException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="isLogged">if set to <c>true</c> [is logged].</param>
        public DataStoreException(string message, Exception innerException, bool isLogged) : base(message, innerException, isLogged) { }

        #endregion
    }
}
