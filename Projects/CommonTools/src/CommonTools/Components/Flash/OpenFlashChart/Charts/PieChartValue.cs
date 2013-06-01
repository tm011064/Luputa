using System;
using System.Collections.Generic;
using System.Text;
using CommonTools.Web.JavaScript;


namespace CommonTools.Components.Flash.OpenFlashChart.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class PieChartValue : ChartValueBase
    {
        #region properties
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        [JsonProperty("label")]
        public string Label { get; set; }
        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        /// <value>The colour.</value>
        [JsonProperty("colour")]
        public string Colour { get; set; }
        /// <summary>
        /// Gets or sets the label colour.
        /// </summary>
        /// <value>The label colour.</value>
        [JsonProperty("label-colour")]
        public string LabelColour { get; set; }
        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>The size of the font.</value>
        [JsonProperty("font-size")]
        public double FontSize { get; set; }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PieChartValue"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public PieChartValue(decimal value) : this(value, string.Empty) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PieChartValue"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="label">The label.</param>
        public PieChartValue(decimal value, string label) : this(value, label, null, null, 12) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PieChartValue"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="label">The label.</param>
        /// <param name="colour">The colour.</param>
        /// <param name="labelColour">The label colour.</param>
        /// <param name="fontSize">Size of the font.</param>
        public PieChartValue(decimal value, string label, string colour, string labelColour, double fontSize)
            : base(value)
        {
            this.Label = label;
            this.Colour = colour;
            this.FontSize = fontSize;
            this.LabelColour = labelColour;
        }
        #endregion
    }
}
