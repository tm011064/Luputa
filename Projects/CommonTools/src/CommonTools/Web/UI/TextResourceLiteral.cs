using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.Caching;
using System.Web;
using System.ComponentModel;
using CommonTools.Components.Threading;
using CommonTools.Components.Logging;
using CommonTools.Components.TextResources;

namespace CommonTools.Web.UI
{
    /// <summary>
    /// 
    /// </summary>
    [ToolboxData("<{0}:TextResourceLiteral runat=server> </{0}:TextResourceLiteral>")]
    public abstract class TextResourceLiteral : Literal
    {
        #region members
        #endregion

        #region properties
        /// <summary>
        /// Gets or sets the text resource manager.
        /// </summary>
        /// <value>The text resource manager.</value>
        protected abstract TextResourceManager TextResourceManager { get; }

        private string _Culture = string.Empty;
        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        [Category("Data")]
        public string Culture
        {
            get { return _Culture; }
            set { _Culture = value; }
        }
        private string _ResourceKey = string.Empty;
        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        [Category("Data")]
        public string ResourceKey
        {
            get { return _ResourceKey; }
            set { _ResourceKey = value; }
        }
        #endregion

        #region life-cycle
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {

            if (TextResourceManager == null)
                throw new TextResourceManagerException("TextResourceManager must not be null.");

            this.Text = TextResourceManager.GetResourceText(this.ResourceKey, this.Culture);

            base.OnPreRender(e);
        }
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TextResourceLiteral"/> class.
        /// </summary>
        public TextResourceLiteral()
            : base()
        {
        }
        #endregion
    }
}
