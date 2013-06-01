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
    public class RandomCharactersGenerator
    {
        /// <summary>
        /// Gets the CHAR s_ LCASE.
        /// </summary>
        /// <value>The CHAR s_ LCASE.</value>
        protected virtual string CHARS_LCASE 
        {
            get { return "abcdefghijklmnopqrstuvwxyz"; }
        }
        /// <summary>
        /// Gets the CHAR s_ UCASE.
        /// </summary>
        /// <value>The CHAR s_ UCASE.</value>
        protected virtual string CHARS_UCASE
        {
            get { return "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
        }
        /// <summary>
        /// Gets the CHAR s_ NUMERIC.
        /// </summary>
        /// <value>The CHAR s_ NUMERIC.</value>
        protected virtual string CHARS_NUMERIC
        {
            get { return "0123456789"; }
        }


        /// <summary>
        /// Generates the specified length.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="characterType">Type of the character.</param>
        /// <returns></returns>
        public string Generate(int length, CharacterType characterType)
        {
            return Generate(length, characterType, new Random());
        }
        /// <summary>
        /// Generates a random word with the specified length.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="characterType">Type of the character.</param>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        public string Generate(int length, CharacterType characterType, Random r)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; ++i)
            {
                switch (characterType)
                {
                    case CharacterType.LowerCase:
                        sb.Append(CHARS_LCASE[r.Next(CHARS_LCASE.Length)]);
                        break;
                    case CharacterType.LowerUpperCase:
                        if (r.Next(0, 2) == 1)
                            sb.Append(CHARS_LCASE[r.Next(CHARS_LCASE.Length)]);
                        else
                            sb.Append(CHARS_UCASE[r.Next(CHARS_UCASE.Length)]);
                        break;
                    case CharacterType.Numbers:
                        sb.Append(CHARS_NUMERIC[r.Next(CHARS_NUMERIC.Length)]);
                        break;
                    case CharacterType.NumbersLowerCase:
                        if (r.Next(0, 2) == 1)
                            sb.Append(CHARS_LCASE[r.Next(CHARS_LCASE.Length)]);
                        else
                            sb.Append(CHARS_NUMERIC[r.Next(CHARS_NUMERIC.Length)]);
                        break;
                    case CharacterType.NumbersUpperCase:
                        if (r.Next(0, 2) == 1)
                            sb.Append(CHARS_UCASE[r.Next(CHARS_UCASE.Length)]);
                        else
                            sb.Append(CHARS_NUMERIC[r.Next(CHARS_NUMERIC.Length)]);
                        break;
                    case CharacterType.UpperCase:
                        sb.Append(CHARS_UCASE[r.Next(CHARS_UCASE.Length)]);
                        break;
                    case CharacterType.NumbersUpperLowerCase:
                        int rNumber = r.Next(0, 3);
                        if (rNumber == 0)
                            sb.Append(CHARS_LCASE[r.Next(CHARS_LCASE.Length)]);
                        else if (rNumber == 1)
                            sb.Append(CHARS_NUMERIC[r.Next(CHARS_NUMERIC.Length)]);
                        else
                            sb.Append(CHARS_UCASE[r.Next(CHARS_UCASE.Length)]);
                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomCharactersGenerator"/> class.
        /// </summary>
        public RandomCharactersGenerator() { }
    }
}
