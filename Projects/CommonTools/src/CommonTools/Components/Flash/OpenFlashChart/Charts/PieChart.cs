using CommonTools.Web.JavaScript;
using System.Collections.Generic;

namespace CommonTools.Components.Flash.OpenFlashChart.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class PieChart : ChartBase
    {
        #region properties
        /// <summary>
        /// Gets or sets the colours.
        /// </summary>
        /// <value>The colours.</value>
        [JsonProperty("colours")]
        public List<string> Colours { get; set; }
        /// <summary>
        /// Gets or sets the border.
        /// </summary>
        /// <value>The border.</value>
        [JsonProperty("border")]
        public int Border { get; set; }
        /// <summary>
        /// Gets or sets the alpha.
        /// </summary>
        /// <value>The alpha.</value>
        [JsonProperty("alpha")]
        public double Alpha { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [do animate].
        /// </summary>
        /// <value><c>true</c> if [do animate]; otherwise, <c>false</c>.</value>
        [JsonProperty("animate")]
        public bool DoAnimate { get; set; }
        /// <summary>
        /// Gets or sets the start angle.
        /// </summary>
        /// <value>The start angle.</value>
        [JsonProperty("start-angle")]
        public double StartAngle { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [do animate].
        /// </summary>
        /// <value><c>true</c> if [do animate]; otherwise, <c>false</c>.</value>
        [JsonProperty("gradient-fill")]
        public bool GradientFill { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [do animate].
        /// </summary>
        /// <value><c>true</c> if [do animate]; otherwise, <c>false</c>.</value>
        [JsonProperty("no-labels")]
        public bool HideLabels { get; set; }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PieChart"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        public PieChart(List<IChartValue> values)
            : base(ChartType.Pie, values)
        {
            this.Border = 2;
            this.Colours = new List<string>() { "#d01f3c", "#356aa0", "#C79810" };
            this.Alpha = 0.6;
            this.DoAnimate = true;
            this.GradientFill = true;
            this.HideLabels = false;
        }
        #endregion
    }
}
