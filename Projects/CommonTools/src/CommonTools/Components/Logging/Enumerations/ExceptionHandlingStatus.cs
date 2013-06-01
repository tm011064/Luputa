using System;

namespace CommonTools.Components.Logging
{
    /// <summary>
    /// ExceptionHandlingStatus
    /// </summary>
    public enum ExceptionHandlingStatus : int
    {
        /// <summary>
        /// this exception might occure but is expected and handled
        /// </summary>
        Expected = 0,
        /// <summary>
        /// this exception should not be thrown but is handled in code
        /// </summary>
        HandledInCode = 1,
        /// <summary>
        /// this was an unhandled exception, further investigation is necessary
        /// </summary>
        Unhandled = 2,
        /// <summary>
        /// the error that caused this execption was resolved and should not occure any more
        /// </summary>
        Resolved = 3
    }
}
