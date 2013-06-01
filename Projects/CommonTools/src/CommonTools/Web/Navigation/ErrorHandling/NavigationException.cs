using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public NavigationException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public NavigationException(string message, Exception inner)
            : base(message, inner) { }
    }
}
