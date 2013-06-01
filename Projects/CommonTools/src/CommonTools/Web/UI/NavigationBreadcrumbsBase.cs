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
    public class NavigationBreadcrumbsBase : CompositeControl
    {
        #region members
        /// <summary>
        /// 
        /// </summary>
        protected IUrlRewriteController _Controller;
        #endregion

        #region properties
        private string _LinkCssClass = string.Empty;
        /// <summary>
        /// Gets or sets the link CSS class.
        /// </summary>
        /// <value>The link CSS class.</value>
        [Category("Layout")]
        public string LinkCssClass
        {
            get { return _LinkCssClass; }
            set { _LinkCssClass = value; }
        }
        private string _ActivePageCssClass = string.Empty;
        /// <summary>
        /// Gets or sets the active link CSS class.
        /// </summary>
        /// <value>The active link CSS class.</value>
        [Category("Layout")]
        public string ActivePageCssClass
        {
            get { return _ActivePageCssClass; }
            set { _ActivePageCssClass = value; }
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
        private bool _ShowRootNode = true;
        /// <summary>
        /// Gets or sets a value indicating whether to show the root node.
        /// </summary>
        /// <value><c>true</c> if [show root node]; otherwise, <c>false</c>.</value>
        [Category("Layout")]
        public bool ShowRootNode
        {
            get { return _ShowRootNode; }
            set { _ShowRootNode = value; }
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
        private string _SeparatorCssClass = string.Empty;
        /// <summary>
        /// Gets or sets the separator CSS class.
        /// </summary>
        /// <value>The separator CSS class.</value>
        [Category("Layout")]
        public string SeparatorCssClass
        {
            get { return _SeparatorCssClass; }
            set { _SeparatorCssClass = value; }
        }
        private string _Separator = string.Empty;
        /// <summary>
        /// Gets or sets the seperator between links.
        /// </summary>
        /// <value>The seperator.</value>
        [Category("Layout")]
        public string Separator
        {
            get { return _Separator; }
            set { _Separator = value; }
        }
        private string _VirtualSeparatorImageUrl = string.Empty;
        /// <summary>
        /// Gets or sets the virtual separator image URL between links.
        /// </summary>
        /// <value>The virtual separator image URL.</value>
        [Category("Layout")]
        public string VirtualSeparatorImageUrl
        {
            get { return _VirtualSeparatorImageUrl; }
            set { _VirtualSeparatorImageUrl = value; }
        }
        private string _Prefix;
        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>The controller.</value>
        [Category("Layout")]
        public string Prefix
        {
            get { return _Prefix; }
            set { _Prefix = value; }
        }
        #endregion

        #region life-cycle
        /// <see cref="JobView.RecreateChildControls"/>
        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }
        #endregion

        #region protected methods
        /// <summary>
        /// Gets the breadcrumbs.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected virtual StringBuilder GetBreadcrumbs(StringBuilder output, IUrlRewriteItem item)
        {
            if (!string.IsNullOrEmpty(item.ParentName))
            {
                item = _Controller.GetUrlRewriteItem(item.ParentName);

                if (item == null)
                {// item not found, return
                    return output;
                }

                if (!this.ShowRootNode && string.IsNullOrEmpty(item.ParentName))
                {// don't render the root node
                    return output;
                }

                // insert link
                output.Insert(0, @"<a" + (string.IsNullOrEmpty(LinkCssClass) ? ("") : (@" class=""" + LinkCssClass + @""""))
                    + @" href=""" + ResolveUrl((item.Url + item.Path).Replace("//", "/")) + @""">" + (string.IsNullOrEmpty(item.BreadcrumbTitle) ? ResolveUrl(item.FullVirtualPath) : item.BreadcrumbTitle) + @"</a>");

                if ((string.IsNullOrEmpty(item.ParentName) && this.ShowFirstSeparator)
                        || (!string.IsNullOrEmpty(item.ParentName)))
                {
                    // insert separator
                    if (!string.IsNullOrEmpty(this.VirtualSeparatorImageUrl))
                        output.Insert(0, "<img alt=\"\" border=\"0\" src=\"" + ResolveUrl(this.VirtualSeparatorImageUrl) + "\"/>");
                    else
                        output.Insert(0, "<span" + (string.IsNullOrEmpty(SeparatorCssClass) ? ("") : (@" class=""" + SeparatorCssClass + @"""")) + ">" + Separator + "</span>");
                }

                return GetBreadcrumbs(output, item);
            }

            return output;
        }

        /// <summary>
        /// Gets the breadcrumbs.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected virtual StringBuilder GetBreadcrumbs(StringBuilder output, ISiteMapMenuItem item)
        {
            if (item.ParentNode != null)
            {
                item = item.ParentNode;

                if (!this.ShowRootNode && item.ParentNode == null)
                {// don't render the root node
                    return output;
                }

                // insert link
                output.Insert(0, @"<a" + (string.IsNullOrEmpty(LinkCssClass) ? ("") : (@" class=""" + LinkCssClass + @""""))
                    + @" href=""" + ResolveUrl((item.UrlRewriteItem.Url + item.UrlRewriteItem.Path).Replace("//", "/")) + @""">"
                    + (string.IsNullOrEmpty(item.BreadcrumbTitle) ? ResolveUrl(item.UrlRewriteItem.FullVirtualPath) : item.BreadcrumbTitle) + @"</a>");

                if ((item.ParentNode == null && this.ShowFirstSeparator)
                        || (item.ParentNode != null))
                {
                    // insert separator
                    if (!string.IsNullOrEmpty(this.VirtualSeparatorImageUrl))
                        output.Insert(0, "<img alt=\"\" border=\"0\" src=\"" + ResolveUrl(this.VirtualSeparatorImageUrl) + "\"/>");
                    else
                        output.Insert(0, "<span" + (string.IsNullOrEmpty(SeparatorCssClass) ? ("") : (@" class=""" + SeparatorCssClass + @"""")) + ">" + Separator + "</span>");
                }

                return GetBreadcrumbs(output, item);
            }

            return output;
        }
        #endregion
    }
}
