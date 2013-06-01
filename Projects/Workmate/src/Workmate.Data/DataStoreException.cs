using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Data
{
  public class DataStoreException : System.Exception
  {
    /// <summary>
    /// Gets or sets a value indicating whether this exception is logged.
    /// </summary>
    /// <value><c>true</c> if this instance is logged; otherwise, <c>false</c>.</value>
    public bool IsLogged { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CatastrophicException"/> class.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    /// <param name="isLogged">if set to <c>true</c> [is logged].</param>
    public DataStoreException(Exception innerException, bool isLogged)
      : base("A catastrophic exception was thrown in your application. See inner exception for further details.", innerException)
    {
      this.IsLogged = IsLogged;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="CatastrophicException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="isLogged">if set to <c>true</c> [is logged].</param>
    public DataStoreException(string message, bool isLogged)
      : base(message)
    {
      this.IsLogged = IsLogged;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="CatastrophicException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    /// <param name="isLogged">if set to <c>true</c> [is logged].</param>
    public DataStoreException(string message, Exception innerException, bool isLogged)
      : base(message, innerException)
    {
      this.IsLogged = IsLogged;
    }
  }   
}
