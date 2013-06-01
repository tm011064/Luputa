using System;
using System.Collections.Generic;
using System.Text;
using CommonTools.Web.JavaScript;


namespace CommonTools.Components.Flash.OpenFlashChart
{
    /// <summary>
    /// 
    /// </summary>
    public class Axis
    {
        #region properties
        /// <summary>
        /// Gets or sets the stroke.
        /// </summary>
        /// <value>The stroke.</value>
        [JsonProperty("stroke")]
        public int Stroke { get; set; }
        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        /// <value>The colour.</value>
        [JsonProperty("colour")]
        public string Colour { get; set; }
        /// <summary>
        /// Gets or sets the grid colour.
        /// </summary>
        /// <value>The grid colour.</value>
        [JsonProperty("grid-colour")]
        public string GridColour { get; set; }
        /// <summary>
        /// Gets or sets the steps.
        /// </summary>
        /// <value>The steps.</value>
        [JsonProperty("steps")]
        public int Steps { get; set; }
        /// <summary>
        /// Gets or sets the min.
        /// </summary>
        /// <value>The min.</value>
        [JsonProperty("min")]
        public decimal? Min { get; set; }
        /// <summary>
        /// Gets or sets the max.
        /// </summary>
        /// <value>The max.</value>
        [JsonProperty("max")]
        public decimal? Max { get; set; }
        /// <summary>
        /// Gets or sets the width of the axis3 D.
        /// </summary>
        /// <value>The width of the axis3 D.</value>
        [JsonProperty("3d")]
        public int Axis3DWidth { get; set; }
        #endregion

        #region public methods
        /// <summary>
        /// Sets the range.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        public void SetRange(decimal min, decimal max)
        {
            this.Max = max;
            this.Min = min;
        }
        /// <summary>
        /// Sets the range.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <param name="step">The step.</param>
        public void SetRange(decimal min, decimal max, int step)
        {
            this.Max = max;
            this.Min = min;
            this.Steps = step;
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Axis"/> class.
        /// </summary>
        public Axis() : this(1) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Axis"/> class.
        /// </summary>
        /// <param name="steps">The steps.</param>
        public Axis(int steps) { this.Steps = steps; }
        #endregion
    }
}
