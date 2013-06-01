using System;
using System.Collections.Generic;
using System.Text;
using CommonTools.Web.JavaScript;


namespace CommonTools.Components.Flash.OpenFlashChart
{
    /// <summary>
    /// 
    /// </summary>
    public class XAxisLabels
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
        /// <summary>
        /// Gets or sets the rotate.
        /// </summary>
        /// <value>The rotate.</value>
        [JsonProperty("rotate")]
        public string Rotate
        {
            get { return this.Orientation == null ? null : EnumUtility.GetOrientationString(this.Orientation.Value); }
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
        /// Initializes a new instance of the <see cref="XAxisLabels"/> class.
        /// </summary>
        public XAxisLabels()
        {
            this.AxisLabelValues = new List<AxisLabel>();
        }
        #endregion
    }
}
