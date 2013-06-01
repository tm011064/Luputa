using System;
using System.Collections.Generic;
using System.Text;
using CommonTools.Web.JavaScript;


namespace CommonTools.Components.Flash.OpenFlashChart
{
    /// <summary>
    /// 
    /// </summary>
    public class XAxis : Axis
    {
        #region properties
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="XAxis"/> is offset.
        /// </summary>
        /// <value><c>true</c> if offset; otherwise, <c>false</c>.</value>
        [JsonProperty("offset")]
        public bool Offset { get; set; }
        /// <summary>
        /// Gets or sets the labels.
        /// </summary>
        /// <value>The labels.</value>
        [JsonProperty("labels")]
        public XAxisLabels Labels { get; set; }
        /// <summary>
        /// Gets or sets the height of the tick.
        /// </summary>
        /// <value>The height of the tick.</value>
        [JsonProperty("tick-height")]
        public int TickHeight { get; set; }

        /// <summary>
        /// Gets or sets the rotate.
        /// </summary>
        /// <value>The rotate.</value>
        [JsonProperty("rotate")]
        public string Rotate
        {
            get { return this.Orientation == null || this.Orientation.Value == OpenFlashChart.Orientation.Horizontal ? null : EnumUtility.GetOrientationString(this.Orientation.Value); }
            internal set { }
        }

        private Orientation? _Orientation = null;
        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        [JsonIgnore]
        public Orientation? Orientation
        {
            get { return _Orientation; }
            set { _Orientation = value; }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="XAxis"/> class.
        /// </summary>
        public XAxis()
        {
            this.Labels = new XAxisLabels();
        }
        #endregion
    }
}
