using System;
using System.Collections.Generic;
using System.Text;
using CommonTools.Web.JavaScript;

namespace CommonTools.Components.Flash.OpenFlashChart
{
    /// <summary>
    /// 
    /// </summary>
    public class AxisLabel : ChartTextElement
    {
        #region properties
        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        /// <value>The colour.</value>
        [JsonProperty("colour")]
        public string Colour { get; set; }
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        [JsonProperty("size")]
        public int Size { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AxisLabel"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        [JsonProperty("visible")]
        public bool Visible { get; set; }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisLabel"/> class.
        /// </summary>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        public AxisLabel(bool visible)
            : base(null)
        {
            this.Visible = visible;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisLabel"/> class.
        /// </summary>
        public AxisLabel()
            : base(null)
        {
            this.Visible = true;
            this.Size = 12;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisLabel"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public AxisLabel(string text)
            : base(text)
        {
            this.Visible = true;
            this.Size = 12;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisLabel"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="colour">The colour.</param>
        /// <param name="size">The size.</param>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        public AxisLabel(string text, string colour, int size, bool visible)
            : base(text)
        {
            this.Colour = colour;
            this.Size = size;
            this.Visible = visible;
        }
        #endregion
    }
}
