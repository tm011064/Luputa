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
    [ToolboxData("<{0}:NavigationBreadcrumbs runat=server> </{0}:NavigationBreadcrumbs>")]
    public class NavigationBreadcrumbs : NavigationBreadcrumbsBase
    {
        #region members

        #endregion

        #region properties

        private IUrlRewriteItem _ActiveItem;
        /// <summary>
        /// Gets or sets the active item.
        /// </summary>
        /// <value>The active item.</value>
        [Category("Data")]
        public IUrlRewriteItem ActiveItem
        {
            get { return _ActiveItem; }
            set { _ActiveItem = value; }
        }

        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>The controller.</value>
        [Category("Data")]
        public IUrlRewriteController Controller
        {
            get { return _Controller; }
            set { _Controller = value; }
        }
        #endregion

        #region life-cycle
        /// <see cref="JobView.RecreateChildControls"/>
        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }
        #endregion

        #region render
        /// <see cref="JobView.Render"/>
        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("<p style=\"font-family: " + this.Font.Name + "; font-size: " + this.Font.Size.ToString() + "; color: #FFFF99; border: outset 1px #000000; padding: 0; background-color: #5a7ab8\">");
                {
                    sb.Append("<b>NavigationBreadcrumbs</b>");
                }
                sb.Append("</p>");

                writer.Write(sb.ToString());
                base.Render(writer);
            }
            else
            {
                if (ActiveItem == null)
                    return;

                StringBuilder output = new StringBuilder();
                AddAttributesToRender(writer);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                {
                    if ((string.IsNullOrEmpty(ActiveItem.ParentName) && this.ShowFirstSeparator)
                        || (!string.IsNullOrEmpty(ActiveItem.ParentName)))
                    {
                        // append separator
                        if (!string.IsNullOrEmpty(this.VirtualSeparatorImageUrl))
                            output.Append("<img alt=\"\" border=\"0\" src=\"" + ResolveUrl(this.VirtualSeparatorImageUrl) + "\"/>");
                        else
                            output.Append("<span" + (string.IsNullOrEmpty(SeparatorCssClass) ? ("") : (@" class=""" + SeparatorCssClass + @"""")) + ">" + Separator + "</span>");
                    }

                    // render link
                    if (ActivePageIsLink)
                    {
                        output.Append(@"<a" + (string.IsNullOrEmpty(ActivePageCssClass) ? ("") : (@" class=""" + ActivePageCssClass + @""""))
                            + @" href=""" + ResolveUrl((ActiveItem.Url + ActiveItem.Path).Replace("//", "/")) + @""">"
                            + (string.IsNullOrEmpty(ActiveItem.BreadcrumbTitle) ? ResolveUrl(ActiveItem.FullVirtualPath) : ActiveItem.BreadcrumbTitle) + @"</a>");
                    }
                    else
                    {
                        output.Append(@"<span" + (string.IsNullOrEmpty(ActivePageCssClass) ? ("") : (@" class=""" + ActivePageCssClass + @""""))
                            + @">" + ActiveItem.BreadcrumbTitle + @"</span>");
                    }

                    output = GetBreadcrumbs(output, ActiveItem);
                    if (!string.IsNullOrEmpty(this.Prefix))
                        output.Insert(0, this.Prefix);

                    writer.Write(output.ToString());
                }
                writer.RenderEndTag();
            }
        }
        #endregion
    }
}
