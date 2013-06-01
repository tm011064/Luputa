using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CommonTools.Components.Mathematics
{
    /// <summary>
    /// 
    /// </summary>
    public static class Statistics
    {
        #region median
        /// <summary>
        /// Gets the median.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        public static double GetMedian(IEnumerable<double> records)
        {
            int count = records.Count();
            if (count == 0)
                return 0;

            if (count % 2 == 0)
            {
                count = count / 2;
                var ordered = records.OrderBy(c => c).ToList();
                return (ordered[count] + ordered[count - 1]) / 2.0;
            }
            else
            {
                return records.OrderBy(c => c).ToList()[count / 2];
            }
        }
        /// <summary>
        /// Gets the median.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        public static double GetMedian(IEnumerable<int> records)
        {
            int count = records.Count();
            if (count == 0)
                return 0;

            if (count % 2 == 0)
            {
                count = count / 2;
                var ordered = records.OrderBy(c => c).ToList();
                return (ordered[count] + ordered[count - 1]) / 2.0;
            }
            else
            {
                return records.OrderBy(c => c).ToList()[count / 2];
            }
        }
        /// <summary>
        /// Gets the median.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        public static double GetMedian(IEnumerable<long> records)
        {
            int count = records.Count();
            if (count == 0)
                return 0;

            if (count % 2 == 0)
            {
                count = count / 2;
                var ordered = records.OrderBy(c => c).ToList();
                return (ordered[count] + ordered[count - 1]) / 2.0;
            }
            else
            {
                return records.OrderBy(c => c).ToList()[count / 2];
            }
        }
        /// <summary>
        /// Gets the median.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        public static decimal GetMedian(IEnumerable<decimal> records)
        {
            int count = records.Count();
            if (count == 0)
                return 0;

            if (count % 2 == 0)
            {
                count = count / 2;
                var ordered = records.OrderBy(c => c).ToList();
                return (ordered[count] + ordered[count - 1]) / 2m;
            }
            else
            {
                return records.OrderBy(c => c).ToList()[count / 2];
            }
        }
        /// <summary>
        /// Gets the median.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        public static double GetMedian(IEnumerable<float> records)
        {
            int count = records.Count();
            if (count == 0)
                return 0;

            if (count % 2 == 0)
            {
                count = count / 2;
                var ordered = records.OrderBy(c => c).ToList();
                return (ordered[count] + ordered[count - 1]) / 2.0;
            }
            else
            {
                return records.OrderBy(c => c).ToList()[count / 2];
            }
        }
        #endregion

        #region variance
        /// <summary>
        /// Calculates the variance.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateVariance(IEnumerable<double> values)
        {
            double average = values.Average();
            return values.Average(v => System.Math.Pow(v - average, 2));
        }
        /// <summary>
        /// Calculates the variance.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateVariance(IEnumerable<decimal> values)
        {
            decimal average = values.Average();
            return values.Average(v => System.Math.Pow((double)(v - average), 2));
        }
        /// <summary>
        /// Calculates the variance.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateVariance(IEnumerable<int> values)
        {
            double average = values.Average();
            return values.Average(v => System.Math.Pow(v - average, 2));
        }
        /// <summary>
        /// Calculates the variance.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateVariance(IEnumerable<long> values)
        {
            double average = values.Average();
            return values.Average(v => System.Math.Pow(v - average, 2));
        }
        /// <summary>
        /// Calculates the variance.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateVariance(IEnumerable<float> values)
        {
            double average = values.Average();
            return values.Average(v => System.Math.Pow(v - average, 2));
        }
        #endregion

        #region standard deviation
        /// <summary>
        /// Calculates the standard deviation.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateStandardDeviation(IEnumerable<double> values) { return Math.Sqrt(CalculateVariance(values)); }
        /// <summary>
        /// Calculates the standard deviation.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateStandardDeviation(IEnumerable<decimal> values) { return Math.Sqrt(CalculateVariance(values)); }
        /// <summary>
        /// Calculates the standard deviation.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateStandardDeviation(IEnumerable<int> values) { return Math.Sqrt(CalculateVariance(values)); }
        /// <summary>
        /// Calculates the standard deviation.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateStandardDeviation(IEnumerable<long> values) { return Math.Sqrt(CalculateVariance(values)); }
        /// <summary>
        /// Calculates the standard deviation.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateStandardDeviation(IEnumerable<float> values) { return Math.Sqrt(CalculateVariance(values)); }
        #endregion

        #region media variance
        /// <summary>
        /// Calculates the median variance.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateMedianVariance(IEnumerable<double> values)
        {
            double median = GetMedian(values);
            return values.Average(v => System.Math.Pow(v - median, 2));
        }
        /// <summary>
        /// Calculates the median variance.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateMedianVariance(IEnumerable<decimal> values)
        {
            decimal median = GetMedian(values);
            return values.Average(v => System.Math.Pow((double)(v - median), 2));
        }
        /// <summary>
        /// Calculates the median variance.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateMedianVariance(IEnumerable<int> values)
        {
            double median = GetMedian(values);
            return values.Average(v => System.Math.Pow(v - median, 2));
        }
        /// <summary>
        /// Calculates the median variance.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateMedianVariance(IEnumerable<long> values)
        {
            double median = GetMedian(values);
            return values.Average(v => System.Math.Pow(v - median, 2));
        }
        /// <summary>
        /// Calculates the median variance.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateMedianVariance(IEnumerable<float> values)
        {
            double median = GetMedian(values);
            return values.Average(v => System.Math.Pow(v - median, 2));
        }
        #endregion

        #region median absolute deviation
        /// <summary>
        /// Calculates the median absolute deviation.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateMedianAbsoluteDeviation(IEnumerable<double> values) { return Math.Sqrt(CalculateMedianVariance(values)); }
        /// <summary>
        /// Calculates the median absolute deviation.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateMedianAbsoluteDeviation(IEnumerable<decimal> values) { return Math.Sqrt(CalculateMedianVariance(values)); }
        /// <summary>
        /// Calculates the median absolute deviation.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateMedianAbsoluteDeviation(IEnumerable<int> values) { return Math.Sqrt(CalculateMedianVariance(values)); }
        /// <summary>
        /// Calculates the median absolute deviation.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateMedianAbsoluteDeviation(IEnumerable<long> values) { return Math.Sqrt(CalculateMedianVariance(values)); }
        /// <summary>
        /// Calculates the median absolute deviation.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double CalculateMedianAbsoluteDeviation(IEnumerable<float> values) { return Math.Sqrt(CalculateMedianVariance(values)); }
        #endregion

        #region covariance
        /// <summary>
        /// Calculates the covariance of two sets
        /// </summary>
        /// <param name="a">Set A</param>
        /// <param name="b">Set B</param>
        /// <returns></returns>
        public static double CalculateCovariance(IEnumerable<double> a, IEnumerable<double> b)
        {
            int count = a.Count();
            if (count == 0
                || count != b.Count())
            {
                return 0;
            }

            var enumerator_a = a.GetEnumerator();
            var enumerator_b = b.GetEnumerator();
            double sum = 0;

            while (enumerator_a.MoveNext())
            {
                enumerator_b.MoveNext();
                sum += enumerator_a.Current * enumerator_b.Current;
            }

            return (sum / (double)count)
                   - (a.Average() * b.Average());
        }
        /// <summary>
        /// Calculates the covariance of two sets
        /// </summary>
        /// <param name="a">Set A</param>
        /// <param name="b">Set B</param>
        /// <returns></returns>
        public static decimal CalculateCovariance(IEnumerable<decimal> a, IEnumerable<decimal> b)
        {
            int count = a.Count();
            if (count == 0
                || count != b.Count())
            {
                return 0;
            }

            var enumerator_a = a.GetEnumerator();
            var enumerator_b = b.GetEnumerator();
            decimal sum = 0;

            while (enumerator_a.MoveNext())
            {
                enumerator_b.MoveNext();
                sum += enumerator_a.Current * enumerator_b.Current;
            }

            return (sum / (decimal)count)
                   - (a.Average() * b.Average());
        }
        /// <summary>
        /// Calculates the covariance of two sets
        /// </summary>
        /// <param name="a">Set A</param>
        /// <param name="b">Set B</param>
        /// <returns></returns>
        public static double CalculateCovariance(IEnumerable<int> a, IEnumerable<int> b)
        {
            int count = a.Count();
            if (count == 0
                || count != b.Count())
            {
                return 0;
            }

            var enumerator_a = a.GetEnumerator();
            var enumerator_b = b.GetEnumerator();
            double sum = 0;

            while (enumerator_a.MoveNext())
            {
                enumerator_b.MoveNext();
                sum += enumerator_a.Current * enumerator_b.Current;
            }

            return (sum / (double)count)
                   - (a.Average() * b.Average());
        }
        /// <summary>
        /// Calculates the covariance of two sets
        /// </summary>
        /// <param name="a">Set A</param>
        /// <param name="b">Set B</param>
        /// <returns></returns>
        public static double CalculateCovariance(IEnumerable<long> a, IEnumerable<long> b)
        {
            int count = a.Count();
            if (count == 0
                || count != b.Count())
            {
                return 0;
            }

            var enumerator_a = a.GetEnumerator();
            var enumerator_b = b.GetEnumerator();
            double sum = 0;

            while (enumerator_a.MoveNext())
            {
                enumerator_b.MoveNext();
                sum += enumerator_a.Current * enumerator_b.Current;
            }

            return (sum / (double)count)
                   - (a.Average() * b.Average());
        }
        /// <summary>
        /// Calculates the covariance of two sets
        /// </summary>
        /// <param name="a">Set A</param>
        /// <param name="b">Set B</param>
        /// <returns></returns>
        public static double CalculateCovariance(IEnumerable<float> a, IEnumerable<float> b)
        {
            int count = a.Count();
            if (count == 0
                || count != b.Count())
            {
                return 0;
            }

            var enumerator_a = a.GetEnumerator();
            var enumerator_b = b.GetEnumerator();
            double sum = 0;

            while (enumerator_a.MoveNext())
            {
                enumerator_b.MoveNext();
                sum += enumerator_a.Current * enumerator_b.Current;
            }

            return (sum / (double)count)
                   - (a.Average() * b.Average());
        }
        #endregion

    }
}
