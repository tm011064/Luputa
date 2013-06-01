using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CommonTools.Web.JavaScript;
using System.IO;
using System.Globalization;

namespace CommonTools.Components.Flash.OpenFlashChart
{
    /// <summary>
    /// 
    /// </summary>
    public class ChartBase
    {
        #region properties
        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        /// <value>The colour.</value>
        [JsonProperty("colour")]
        public virtual string Colour { get; set; }
        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
        [JsonProperty("values")]
        public virtual List<IChartValue> Values { get; set; }
        /// <summary>
        /// Gets or sets the fontsize.
        /// </summary>
        /// <value>The fontsize.</value>
        [JsonProperty("font-size")]
        public double Fontsize { get; set; }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        [JsonProperty("text")]
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets the tip.
        /// </summary>
        /// <value>The tip.</value>
        [JsonProperty("tip")]
        public string Tip { get; set; }
        /// <summary>
        /// Gets or sets the fillalpha.
        /// </summary>
        /// <value>The fillalpha.</value>
        [JsonProperty("fillalpha")]
        public double Fillalpha { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [JsonProperty("type")]
        public string Type
        {
            get { return EnumUtility.GetChartTypeString(this.ChartType); }
            internal set { }
        }

        /// <summary>
        /// Gets or sets the type of the chart.
        /// </summary>
        /// <value>The type of the chart.</value>
        [JsonIgnore]
        public ChartType ChartType { get; private set; }
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartBase"/> class.
        /// </summary>
        /// <param name="chartType">Type of the chart.</param>
        /// <param name="values">The values.</param>
        public ChartBase(ChartType chartType, List<IChartValue> values)
        {
            this.ChartType = chartType;
            this.Fillalpha = 0.35;
            this.Fontsize = 12.0;
            this.Values = values;
        }
        #endregion
    }
}
