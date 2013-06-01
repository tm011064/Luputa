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
    [ToolboxData("<{0}:SkinnedNavigationBreadcrumbs runat=server> </{0}:SkinnedNavigationBreadcrumbs>")]
    public class SkinnedNavigationBreadcrumbs : CompositeControl
    {
        #region properties

        private ISiteMapMenuItem _ActiveMenuItem;
        /// <summary>
        /// Gets or sets the active item.
        /// </summary>
        /// <value>The active item.</value>
        [Category("Data")]
        public ISiteMapMenuItem ActiveMenuItem
        {
            get { return _ActiveMenuItem; }
            set { _ActiveMenuItem = value; }
        }
        private bool _ActivePageIsLink = false;
        /// <summary>
        /// Gets or sets a value indicating whether the currently active page text is rendered as a link. If false,
        /// the active page is rendered as a span.
        /// </summary>
        /// <value><c>true</c> if [_ active page is link]; otherwise, <c>false</c>.</value>
        [Category("Layout")]
        public bool ActivePageIsLink
        {
            get { return _ActivePageIsLink; }
            set { _ActivePageIsLink = value; }
        }
        private bool _ShowFirstSeparator = false;
        /// <summary>
        /// Gets or sets a value indicating whether [show first separator].
        /// </summary>
        /// <value><c>true</c> if [show first separator]; otherwise, <c>false</c>.</value>
        [Category("Layout")]
        public bool ShowFirstSeparator
        {
            get { return _ShowFirstSeparator; }
            set { _ShowFirstSeparator = value; }
        }

        private string _SeparatorControlToLoad = null;
        /// <summary>
        /// Gets or sets the link CSS class.
        /// </summary>
        /// <value>The link CSS class.</value>
        [Category("Layout")]
        public string SeparatorControlToLoad
        {
            get { return _SeparatorControlToLoad; }
            set { _SeparatorControlToLoad = value; }
        }
        private string _ClickableNodeControlToLoad = null;
        /// <summary>
        /// Gets or sets the link CSS class.
        /// </summary>
        /// <value>The link CSS class.</value>
        [Category("Layout")]
        public string ClickableNodeControlToLoad
        {
            get { return _ClickableNodeControlToLoad; }
            set { _ClickableNodeControlToLoad = value; }
        }
        private string _UnclickableNodeControlToLoad = null;
        /// <summary>
        /// Gets or sets the link CSS class.
        /// </summary>
        /// <value>The link CSS class.</value>
        [Category("Layout")]
        public string UnclickableNodeControlToLoad
        {
            get { return _UnclickableNodeControlToLoad; }
            set { _UnclickableNodeControlToLoad = value; }
        }
        #endregion

        #region life-cycle
        /// <see cref="JobView.RecreateChildControls"/>
        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }
        #endregion

        #region overrides
        private void LoadBreadcrumbs(ISiteMapMenuItem item)
        {
            if (item.ParentNode != null)
            {
                item = item.ParentNode;

                // insert link
                NavigationBreadcrumbsSkin control = this.Page.LoadControl(this.ClickableNodeControlToLoad) as NavigationBreadcrumbsSkin;
                if (control != null)
                {
                    control.MenuItem = item;
                    control.IsLastNode = false;
                    this.Controls.AddAt(0, control);
                }

                if ((item.ParentNode == null && this.ShowFirstSeparator)
                        || (item.ParentNode != null))
                {
                    // insert separator
                    this.Controls.AddAt(0, this.Page.LoadControl(this.SeparatorControlToLoad));
                }

                LoadBreadcrumbs(item);
            }
        }
        #endregion

        #region render
        /// <see cref="JobView.Render"/>
        protected override void OnPreRender(EventArgs e)
        {
            if (ActiveMenuItem == null)
                return;

            // render link
            NavigationBreadcrumbsSkin control;
            if (ActivePageIsLink)
            {
                control = this.Page.LoadControl(this.ClickableNodeControlToLoad) as NavigationBreadcrumbsSkin;
                if (control != null)
                {
                    control.MenuItem = ActiveMenuItem;
                    control.IsLastNode = true;
                    this.Controls.AddAt(0, control);
                }
            }
            else
            {
                control = this.Page.LoadControl(this.UnclickableNodeControlToLoad) as NavigationBreadcrumbsSkin;
                if (control != null)
                {
                    control.MenuItem = ActiveMenuItem;
                    control.IsLastNode = true;
                    this.Controls.AddAt(0, control);
                }
            }

            if ((ActiveMenuItem.ParentNode == null && this.ShowFirstSeparator)
                || (ActiveMenuItem.ParentNode != null))
            {
                // append separator
                this.Controls.AddAt(0, this.Page.LoadControl(this.SeparatorControlToLoad));
            }

            LoadBreadcrumbs(ActiveMenuItem);
        }
        #endregion
    }
}
