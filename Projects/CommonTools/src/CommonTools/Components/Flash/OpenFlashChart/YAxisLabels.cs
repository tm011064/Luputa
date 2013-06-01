using System;
using System.Collections.Generic;
using System.Text;
using CommonTools.Web.JavaScript;


namespace CommonTools.Components.Flash.OpenFlashChart
{
    /// <summary>
    /// 
    /// </summary>
    public class YAxisLabels
    {
        #region properties
        /// <summary>
        /// Gets or sets the axis label values.
        /// </summary>
        /// <value>The axis label values.</value>
        [JsonProperty("labels")]
        public List<AxisLabel> AxisLabelValues { get; set; }
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        [JsonProperty("colour")]
        public string Color { get; set; }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="XAxisLabels"/> class.
        /// </summary>
        public YAxisLabels()
        {
            this.AxisLabelValues = new List<AxisLabel>();
        }
        #endregion
    }
}
