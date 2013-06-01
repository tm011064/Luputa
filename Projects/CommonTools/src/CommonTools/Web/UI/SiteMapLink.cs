using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using CommonTools.Web.Navigation;
using System.Web;

namespace CommonTools.Web.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class SiteMapLink : WebControl, ITextControl
    {
        #region properties
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>The host.</value>
        public virtual string Host { get; set; }
        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        public virtual IUrlRewriteItem Item { get; set; }
        /// <summary>
        /// Gets or sets the current context.
        /// </summary>
        /// <value>The current context.</value>
        public virtual HttpContext CurrentContext { get; set; }

        private List<string> _PlaceholderValues = new List<string>();
        /// <summary>
        /// Gets or sets the placeholder values.
        /// </summary>
        /// <value>The placeholder values.</value>
        public virtual List<string> PlaceholderValues
        {
            get { return _PlaceholderValues; }
            set { _PlaceholderValues = value; }
        }
        #region ITextControl Members

        private string _Text = string.Empty;
        /// <summary>
        /// Gets or sets the text content of a control.
        /// </summary>
        /// <value></value>
        /// <returns>The text content of a control.</returns>
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        #endregion
        #endregion

        #region life-cycle
        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(string.Format(@"<a href=""{0}""{1}>{2}</a>"
                , SiteMapManager.ResolveRewriteItem(this.CurrentContext ?? HttpContext.Current, this, Host, Item, PlaceholderValues.Count > 0 ? PlaceholderValues.ToArray() : null)
                , string.IsNullOrEmpty(this.CssClass) ? "" : @" class="""+this.CssClass+@""""
                , this.Text));
        }
        #endregion
    }
}
