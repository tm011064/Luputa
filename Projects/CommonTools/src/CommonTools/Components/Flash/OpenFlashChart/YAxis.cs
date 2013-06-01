using System;
using System.Collections.Generic;
using System.Text;
using CommonTools.Web.JavaScript;

namespace CommonTools.Components.Flash.OpenFlashChart
{
    /// <summary>
    /// 
    /// </summary>
    public class YAxis : Axis
    {
        #region properties
        /// <summary>
        /// Gets or sets the length of the tick.
        /// </summary>
        /// <value>The length of the tick.</value>
        [JsonProperty("tick-length")]
        public int TickLength { get; set; }
        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>The offset.</value>
        [JsonProperty("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the axis label values.
        /// </summary>
        /// <value>The axis label values.</value>
        [JsonProperty("labels")]
        public List<string> AxisLabelValues { get; set; }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="YAxis"/> class.
        /// </summary>
        public YAxis()
        {

        }
        #endregion

    }
}
