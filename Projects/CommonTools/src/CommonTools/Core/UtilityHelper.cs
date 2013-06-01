using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Components.Testing;

namespace CommonTools.Core
{
    /// <summary>
    /// 
    /// </summary>
    public delegate bool CompareDelegate<T>(T a, T b);

    /// <summary>
    /// This class contains general utility methods
    /// </summary>
    public static class UtilityHelper
    {
        #region public static methods
        /// <summary>
        /// Gets a list of enums of a specified enum type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Throws a System.ArgumentException if the specified generic type is not an enum.</exception>
        public static List<T> GetEnums<T>()
        {
            return DebugUtility.GetEnums<T>();
        }

        /// <summary>
        /// Merges two ordered lists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static List<T> MergeOrderedList<T>(List<T> a, List<T> b, CompareDelegate<T> comparison)
        {
            if (a == null)
                return b;

            if (b == null)
                return a;

            List<T> merged = new List<T>();

            int aCount = a.Count;
            int bCount = b.Count;
            int bIndex = 0;
            int aIndex = 0;

            for (; ; )
            {
                if (aIndex == aCount)
                {
                    merged.AddRange(b.GetRange(bIndex, bCount - bIndex));
                    break;
                }
                else if (bIndex == bCount)
                {
                    merged.AddRange(a.GetRange(aIndex, aCount - aIndex));
                    break;
                }
                else if (comparison(a[aIndex], b[bIndex]))
                {
                    merged.Add(a[aIndex]);
                    aIndex++;
                }
                else
                {
                    merged.Add(b[bIndex]);
                    bIndex++;
                }
            }

            return merged;
        }
        #endregion


    }
}
