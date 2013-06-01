using System;
using System.Collections.Generic;
using System.Text;
using CommonTools.Web.JavaScript;

namespace CommonTools.Components.Flash.OpenFlashChart
{
    /// <summary>
    /// 
    /// </summary>
    public class LineChart : ChartBase
    {
        #region properties
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        [JsonProperty("width")]
        public virtual int Width { get; set; }
        /// <summary>
        /// Gets or sets the size of the dot.
        /// </summary>
        /// <value>The size of the dot.</value>
        [JsonProperty("dot-size")]
        public virtual int DotSize { get; set; }
        /// <summary>
        /// Gets or sets the size of the halo.
        /// </summary>
        /// <value>The size of the halo.</value>
        [JsonProperty("halo-size")]
        public virtual int HaloSize { get; set; }
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LineChart"/> class.
        /// </summary>
        /// <param name="lineChartType">Type of the line chart.</param>
        /// <param name="values">The values.</param>
        public LineChart(LineChartType lineChartType, List<IChartValue> values)
            : base(EnumUtility.GetChartTypeFromLineChartType(lineChartType), values)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LineChart"/> class.
        /// </summary>
        /// <param name="lineChartType">Type of the line chart.</param>
        /// <param name="values">The values.</param>
        /// <param name="width">The width.</param>
        /// <param name="dotSize">Size of the dot.</param>
        /// <param name="haloSize">Size of the halo.</param>
        public LineChart(LineChartType lineChartType, List<IChartValue> values, int width, int dotSize, int haloSize)
            : base(EnumUtility.GetChartTypeFromLineChartType(lineChartType), values)
        {
            this.Width = width;
            this.DotSize = dotSize;
            this.HaloSize = haloSize;
        }
        #endregion
    }
}
