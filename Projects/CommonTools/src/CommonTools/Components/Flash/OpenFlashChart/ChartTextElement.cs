using System;
using System.Collections.Generic;
using System.Text;
using CommonTools.Web.JavaScript;

namespace CommonTools.Components.Flash.OpenFlashChart
{
    /// <summary>
    /// 
    /// </summary>
    public class ChartTextElement 
    {
        #region properties
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        [JsonProperty("text")]
        public string Text { get; set; }
        #endregion

        #region
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartTextElement"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public ChartTextElement(string text)
        {
            this.Text = text;
        }
        #endregion
    }
}
