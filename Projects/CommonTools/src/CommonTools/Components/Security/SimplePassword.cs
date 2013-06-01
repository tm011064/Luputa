using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.Components.Security
{
    /// <summary>
    /// Class to help you generate a simple alpha-numeric password (or just alphabetical
    /// if you know how to use CharacterType).
    /// </summary>
    public class SimplePassword : RandomCharactersGenerator
    {
        /// <summary>
        /// Gets the CHAR s_ LCASE.
        /// </summary>
        /// <value>The CHAR s_ LCASE.</value>
        protected override string CHARS_LCASE 
        {
            get { return "abcdefgijkmnopqrstwxyz"; }
        }
        /// <summary>
        /// Gets the CHAR s_ UCASE.
        /// </summary>
        /// <value>The CHAR s_ UCASE.</value>
        protected override string CHARS_UCASE
        {
            get { return "ABCDEFGHJKLMNPQRSTWXYZ"; }
        }
        /// <summary>
        /// Gets the CHAR s_ NUMERIC.
        /// </summary>
        /// <value>The CHAR s_ NUMERIC.</value>
        protected override string CHARS_NUMERIC
        {
            get { return "23456789"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimplePassword"/> class.
        /// </summary>
        public SimplePassword() { }
    }
}
