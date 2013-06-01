using System;
using System.Collections.Generic;
using System.Text;
using CommonTools.Web.JavaScript;

namespace CommonTools.Components.Flash.OpenFlashChart
{
    /// <summary>
    /// 
    /// </summary>
    public class ChartStylableTextElement : ChartTextElement
    {
        #region properties
        /// <summary>
        /// Gets or sets the style. E.g.: {font-size: 20px; color:#0000ff; font-family: Verdana; text-align: center;}
        /// </summary>
        /// <value>The style.</value>
        [JsonProperty("style")]
        public string Style { get; set; }
        #endregion

        #region
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartStylableTextElement"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public ChartStylableTextElement(string text) : this(text, null) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartStylableTextElement"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="style">The style.</param>
        public ChartStylableTextElement(string text, string style)
            : base (text)
        {
            this.Style = style;
        }
        #endregion
    }
}
