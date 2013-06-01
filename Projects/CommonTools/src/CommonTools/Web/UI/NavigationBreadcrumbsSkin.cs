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
using CommonTools.Web.Navigation;

namespace CommonTools.Web.UI
{
    /// <summary>
    /// 
    /// </summary>
    [ToolboxData("<{0}:NavigationBreadcrumbsSkin runat=server> </{0}:NavigationBreadcrumbsSkin>")]
    public class NavigationBreadcrumbsSkin : UserControl
    {
        #region properties
        private ISiteMapMenuItem _MenuItem;
        /// <summary>
        /// Gets or sets the menu item.
        /// </summary>
        /// <value>The menu item.</value>
        public ISiteMapMenuItem MenuItem
        {
            get { return _MenuItem; }
            set { _MenuItem = value; }
        }
        private bool _IsLastNode;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is last node.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is last node; otherwise, <c>false</c>.
        /// </value>
        public bool IsLastNode
        {
            get { return _IsLastNode; }
            set { _IsLastNode = value; }
        }
        #endregion

        #region render
        /// <see cref="JobView.Render"/>
        protected override void OnPreRender(EventArgs e)
        {
            this.DataBind();
        }
        #endregion
    }
}
