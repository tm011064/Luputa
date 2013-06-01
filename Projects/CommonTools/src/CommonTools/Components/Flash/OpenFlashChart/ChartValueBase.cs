using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTools.Web.JavaScript;

namespace CommonTools.Components.Flash.OpenFlashChart
{
    /// <summary>
    /// 
    /// </summary>
    public class ChartValueBase : IChartValue
    {
        #region properties
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [JsonProperty("value")]
        public decimal Value { get; set; }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartValueBase"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ChartValueBase(decimal value)
        {
            this.Value = value;
        }
        #endregion
    }
}
