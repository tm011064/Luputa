using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.Core
{
    /// <summary>
    /// This class contains all compare helper related data
    /// </summary>
    public static class CompareHelper
    {
        /// <summary>
        /// Haves the equal items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static bool HaveEqualItems<T>(List<T> a, List<T> b) where T : IEquatable<T>
        {
            int count = b.Count
                , index;

            if (a.Count != count)
                return false;

            List<T> bClone = new List<T>();
            for (int i = 0; i < count; i++)
                bClone.Add(b[i]);

            T item;
            for (int i = count - 1; i >= 0; i--)
            {
                item = a[i];
                index = -1;
                for (int j = count - 1; j >= 0; j--)
                {
                    if (item.Equals(bClone[j]))
                    {
                        index = j;
                        break;
                    }
                }

                if (index >= 0)
                {
                    bClone.RemoveAt(index);
                    count--;
                }
                else
                    return false;
            }

            return true;
        }
    }
}
