using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CommonTools.Finance.RegularExpressions
{
    /// <summary>
    /// This class contains validation expressions and methods to validate financial terminologies
    /// </summary>
    public static class ValidationExpressions
    {
        #region private methods
        private static int GetDigitSum(int value)
        {
            if (value >= 0)
            {
                if (value > 9)
                {
                    string text = value.ToString();
                    int length = text.Length;

                    value = 0;
                    for (int i = 0; i < length; i++)
                        value += Convert.ToInt32(text[i].ToString());
                }
            }
            else
            {
                return GetDigitSum(Math.Abs(value));
            }
            return value;
        }
        #endregion

        #region ISIN
        /// <summary>
        /// Determines whether the specified ISIN is valid according to the following definition -> An ISIN consists of 
        /// three parts: Generally, a two letter country code, a nine character alpha-numeric national security 
        /// identifier, and a single check digit. The country code is the ISO 3166-1 alpha-2 code for the country of 
        /// issue, which is not necessarily the country in which the issuing company is domiciled. International securities 
        /// cleared through Clearstream or Euroclear, which are Europe-wide, use "XS" as the country code.
        /// </summary>
        /// <param name="isin">The isin.</param>
        /// <returns>
        /// 	<c>true</c> if [is ISIN valid] [the specified isin]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsISINValid(string isin)
        {
            if (string.IsNullOrEmpty(isin))
                return false;

            if (Regex.IsMatch(isin, @"^[A-Z]{2}[A-Z0-9]{9}[0-9]{1}$"))
            {
                // validate checksum
                return (int.Parse(isin[11].ToString()) == CreateISINChecksum(isin));
            }

            return false;
        }

        /// <summary>
        /// This method creates the checksum for a specified ISIN. If you pass in a full 12 character ISIN, the checksum returned by this
        /// method should equal the last character.
        /// </summary>
        /// <param name="isin">The isin.</param>
        /// <returns></returns>
        public static int CreateISINChecksum(string isin)
        {
            if (string.IsNullOrEmpty(isin))
                return -1;

            int length = isin.Length;

            if (length == 12)
                length = 11;

            if (length != 11)
            {// ISINs need to be 12 characters long. The twelth character is the checksum.
                return -1;
            }

            List<int>[] lists = new List<int>[2];

            lists[0] = new List<int>();
            lists[1] = new List<int>();

            int index = 1;
            string value;
            for (int i = 0; i < length; i++)
            {
                if (Char.IsDigit(isin[i]))
                {
                    index = index == 0 ? 1 : 0;
                    lists[index].Add(Convert.ToInt32(isin[i].ToString()));
                }
                else if (Char.IsLetter(isin[i]))
                {
                    value = ((int)Char.ToUpper(isin[i]) - 55).ToString();
                    for (int j = 0; j < value.Length; j++)
                    {
                        index = index == 0 ? 1 : 0;
                        lists[index].Add(Convert.ToInt32(value[j].ToString()));
                    }
                }
                else
                    return -1;
            }

            length = lists[index].Count;
            for (int i = 0; i < length; i++)
                lists[index][i] *= 2;

            int sum = 0;

            length = lists[0].Count;
            for (int i = 0; i < length; i++)
                sum += GetDigitSum(lists[0][i]);
            length = lists[1].Count;
            for (int i = 0; i < length; i++)
                sum += GetDigitSum(lists[1][i]);

            return (10 - (sum % 10)) % 10;
        }
        #endregion

        #region SEDOL
        /// <summary>
        /// Determines whether the specified SEDOL is valid according to the following definition -&gt; SEDOLs are seven
        /// characters in length, consisting of two parts: a six-place alphanumeric code and a trailing check digit.
        /// 
        /// SEDOLs issued prior to January 26, 2004 were composed only of numbers. For those older SEDOLs, those
        /// from Asia and Africa typically begin with 6, those from the UK and Ireland (until Ireland joined the EU)
        /// typically begin with 0 or 3 those from Europe typically began with 4, 5 or 7 and those from the Americas
        /// began with 2.
        /// 
        /// After January 26, 2004, SEDOLs were changed to be alpha-numeric and are issued sequentially,
        /// beginning with B000009. At each character position numbers precede letters and vowels are never used.
        /// All new SEDOLs, therefore, begin with a letter. Ranges beginning with 9 are reserved for end user allocation.
        /// </summary>
        /// <param name="sedol">The sedol.</param>
        /// <returns>
        /// 	<c>true</c> if the SEDOL is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSedolValid(string sedol)
        {
            if (string.IsNullOrEmpty(sedol))
                return false;

            if (sedol.Length == 6)
            {
                // old format, only numbers
                return Regex.IsMatch(sedol, @"^[0-9]{6}$");
            }
            else if (sedol.Length == 7
                && Regex.IsMatch(sedol, @"^[A-Z0-9]{6}[0-9]{1}$"))
            {
                return (int.Parse(sedol[6].ToString()) == CreateSedolChecksum(sedol));
            }

            return false;
        }

        private static int[] _SedolWeights = { 1, 3, 1, 7, 3, 9 };
        /// <summary>
        /// This method creates the checksum for a specified Sedol. If you pass in a full 7 character Sedol, the checksum returned by this
        /// method should equal the last character.
        /// </summary>
        /// <param name="sedol">The sedol.</param>
        /// <returns></returns>
        public static int CreateSedolChecksum(string sedol)
        {
            if (string.IsNullOrEmpty(sedol))
                return -1;

            int length = sedol.Length;
            int sum = 0;

            if (length == 7)
                length = 6;

            if (length != 6)
            {// ISINs need to be 12 characters long. The twelth character is the checksum.
                return -1;
            }

            if (Regex.IsMatch(sedol, "[AEIOUaeiou]+")) //invalid SEDOL
                return -1;

            for (int i = 0; i < 6; i++)
            {
                if (Char.IsDigit(sedol[i]))
                    sum += (((int)sedol[i] - 48) * _SedolWeights[i]);
                else if (Char.IsLetter(sedol[i]))
                    sum += (((int)Char.ToUpper(sedol[i]) - 55) * _SedolWeights[i]);
                else
                    return -1;
            }

            return (10 - (sum % 10)) % 10;
        }
        #endregion
    }
}
