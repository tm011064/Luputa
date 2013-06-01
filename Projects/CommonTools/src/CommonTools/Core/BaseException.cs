using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseException : System.Exception
    {
        #region properties
        /// <summary>
        /// Gets or sets a value indicating whether this exception is logged.
        /// </summary>
        /// <value><c>true</c> if this instance is logged; otherwise, <c>false</c>.</value>
        public virtual bool IsLogged { get; set; }
        #endregion

        #region methods
        /// <summary>
        /// Gets the formatted error message.
        /// </summary>
        /// <param name="textFormat">The text format.</param>
        /// <returns></returns>
        public virtual string GetFormattedErrorMessage(TextFormat textFormat)
        {
            return ConversionHelper.GetFormattedException(string.Empty, this, textFormat);
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="isLogged">if set to <c>true</c> [is logged].</param>
        public BaseException(Exception innerException, bool isLogged)
            : base("An exception was thrown in your application. See inner exception for further details.", innerException)
        {
            this.IsLogged = IsLogged;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="isLogged">if set to <c>true</c> [is logged].</param>
        public BaseException(string message, bool isLogged)
            : base(message)
        {
            this.IsLogged = IsLogged;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="isLogged">if set to <c>true</c> [is logged].</param>
        public BaseException(string message, Exception innerException, bool isLogged)
            : base(message, innerException)
        {
            this.IsLogged = IsLogged;
        }
        #endregion
    }
}
