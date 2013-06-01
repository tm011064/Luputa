using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.Components.Testing
{
    /// <summary>
    /// 
    /// </summary>
    public struct ElapsedTimeInfo
    {
        private double _AverageMilliseconds;
        /// <summary>
        /// Gets or sets the average milliseconds.
        /// </summary>
        /// <value>The average milliseconds.</value>
        public double AverageMilliseconds
        {
            get { return _AverageMilliseconds; }
            set { _AverageMilliseconds = value; }
        }
        private long _MedianMilliseconds;
        /// <summary>
        /// Gets or sets the median milliseconds.
        /// </summary>
        /// <value>The median milliseconds.</value>
        public long MedianMilliseconds
        {
            get { return _MedianMilliseconds; }
            set { _MedianMilliseconds = value; }
        }
        private int _Repetitions;
        /// <summary>
        /// Gets or sets the repetitions.
        /// </summary>
        /// <value>The repetitions.</value>
        public int Repetitions
        {
            get { return _Repetitions; }
            set { _Repetitions = value; }
        }

        private long _TotalMilliseconds;
        /// <summary>
        /// Gets or sets the total milliseconds.
        /// </summary>
        /// <value>The total milliseconds.</value>
        public long TotalMilliseconds
        {
            get { return _TotalMilliseconds; }
            set { _TotalMilliseconds = value; }
        }

        /// <summary>
        /// Converts this object to a String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Repetitions: " + this.Repetitions + ", Average: " + this.AverageMilliseconds.ToString("N2") + " ms, Median: " + this.MedianMilliseconds + " ms, Total: " + this.TotalMilliseconds + " ms";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElapsedTimeInfo"/> struct.
        /// </summary>
        /// <param name="averageMilliseconds">The average milliseconds.</param>
        /// <param name="medianMilliseconds">The median milliseconds.</param>
        /// <param name="repetitions">The repetitions.</param>
        /// <param name="totalMilliseconds">The total milliseconds.</param>
        public ElapsedTimeInfo(double averageMilliseconds, long medianMilliseconds, int repetitions, long totalMilliseconds)
        {
            this._AverageMilliseconds = averageMilliseconds;
            this._MedianMilliseconds = medianMilliseconds;
            this._Repetitions = repetitions;
            this._TotalMilliseconds = totalMilliseconds;
        }
    }
}
