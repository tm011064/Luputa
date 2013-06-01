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
using CommonTools.Components.Caching;

namespace CommonTools.Web.UI
{
	/// <summary>
	/// This user control provides a user interface editor to view the transact cache objects
	/// in the system.
	/// </summary>
    [ToolboxData("<{0}:CacheView runat=server> </{0}:CacheView>")]
    public class CacheView : CompositeControl
    {
        #region properties
        private string _TableCssClass;
		/// <see cref="JobView.TableCssClass"/>
        [Category("Layout")]
        public string TableCssClass
        {
            get { return _TableCssClass; }
            set { _TableCssClass = value; }
        }

        private string _TableHeaderCssClass;
		/// <see cref="JobView.TableHeaderCssClass"/>
		[Category("Layout")]
        public string TableHeaderCssClass
        {
            get { return _TableHeaderCssClass; }
            set { _TableHeaderCssClass = value; }
        }

        private string _TableCellCssClass;
		/// <see cref="JobView.TableCellCssClass"/>
		[Category("Layout")]
        public string TableCellCssClass
        {
            get { return _TableCellCssClass; }
            set { _TableCellCssClass = value; }
        }

        private string _TableEnumeratingCellCssClass;
		/// <see cref="JobView.TableEnumeratingCellCssClass"/>
		[Category("Layout")]
        public string TableEnumeratingCellCssClass
        {
            get { return _TableEnumeratingCellCssClass; }
            set { _TableEnumeratingCellCssClass = value; }
        }

        private CacheViewTheme _Theme = CacheViewTheme.None;
		/// <see cref="JobView.Theme"/>
		[Category("Layout")]
        public CacheViewTheme Theme
        {
            get { return _Theme; }
            set { _Theme = value; }
        }
        #endregion

		/// <see cref="JobView.RecreateChildControls"/>
        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }

		/// <see cref="JobView.CreateChildControls"/>
		protected override void CreateChildControls()
        {
            Controls.Clear();
        }

		/// <see cref="JobView.Render"/>
		protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("<p style=\"font-family: " + this.Font.Name + "; font-size: " + this.Font.Size.ToString() + "; color: #FFFF99; border: outset 1px #000000; padding: 0; background-color: #5a7ab8\">");
                {
                    sb.Append("<b>CacheView</b>");
                }
                sb.Append("</p>");

                writer.Write(sb.ToString());
                base.Render(writer);
            }
            else
            {
                AddAttributesToRender(writer);

                StringBuilder sb = new StringBuilder();

                List<CachedElement> elements = new List<CachedElement>();
                ICacheController controller = CacheControllerFactory.CreateCacheController();
                foreach (ICacheItem item in controller.CacheItems.Values)
                    elements.Add(new CachedElement(item, controller));

                sb.Append("No cache item definitions found at the web configuration cache section.");
                if (elements.Count > 0)
                {

                    string tableClass = string.Empty,
                            tableHeaderClass = string.Empty,
                            tableCellClass = string.Empty,
                            tableEnumeratingCellClass = string.Empty,
                            cellValueTrue = string.Empty,
                            cellValueFalse = string.Empty,
                            cellValueCIP = string.Empty,
                            cellValueCIPNotRemovable = string.Empty,
                            cellValueCIPHigh = string.Empty,
                            cellValueCIPAboveNormal = string.Empty,
                            cellValueCIPNormal = string.Empty,
                            cellValueCIPBelowNormal = string.Empty,
                            cellValueCIPLow = string.Empty;

                    #region get styles
                    switch (Theme)
                    {
                        case CacheViewTheme.None:
                            tableClass = string.IsNullOrEmpty(TableCssClass) ? "" : (" class=\"" + TableCssClass + "\"");
                            tableHeaderClass = string.IsNullOrEmpty(TableHeaderCssClass) ? "" : (" class=\"" + TableHeaderCssClass + "\"");
                            tableCellClass = string.IsNullOrEmpty(TableCellCssClass) ? "" : (" class=\"" + TableCellCssClass + "\"");
                            tableEnumeratingCellClass = string.IsNullOrEmpty(TableEnumeratingCellCssClass) ? "" : (" class=\"" + TableEnumeratingCellCssClass + "\"");
                            break;

                        case CacheViewTheme.Default:
                            tableClass = @" style=""width: 100%; border: Solid 2px #5a7ab8; font-family: " + this.Font.Name + @"; font-size: " + this.Font.Size.ToString() + @"""";
                            tableHeaderClass = @" style=""background-color: #5A7AB8; color: #FFFFFF; padding: 2px 4px 2px 4px; border: Solid 1px #5A7AB8; border-collapse: collapse;""";
                            tableCellClass = @" style=""background-color: {0}; color: #000000; padding: 2px 2px 2px 2px; border-left: Solid 1px #000000; border-right: Solid 1px #dddddd; border-bottom: Solid 1px #dddddd; border-top: Solid 1px #000000; border-collapse: collapse;""";
                            tableEnumeratingCellClass =
    @" style=""width: 100%; background-color: {0}; color: #000000; padding: 2px 2px 2px 16px; border-left: Solid 1px #000000; border-right: Solid 1px #dddddd; border-bottom: Solid 1px #dddddd; border-top: None; border-collapse: collapse;""";
                            break;
                    }
                    #endregion

                    sb = new StringBuilder();
                    sb.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\"" + tableClass + ">");
                    sb.Append(
        @"
<tr>
    <th" + tableHeaderClass + @">Name</th>
    <th" + tableHeaderClass + @">Type</th>
    <th" + tableHeaderClass + @">Enabled</th>
    <th" + tableHeaderClass + @">IsUsed</th>
    <th" + tableHeaderClass + @">Interval (minutes)</th>
    <th" + tableHeaderClass + @">Objects</th>
    <th" + tableHeaderClass + @">Priority</th>
</tr>
");
                    foreach (CachedElement element in elements)
                    {

                        sb.Append(
        @"
<tr>
    <td" + (Theme != CacheViewTheme.None ? String.Format(tableCellClass, ThemeColors.GetDefaultCellBgColor(Theme)) : tableCellClass) + @" align=""left"">" + element.Name + @"</td>
    <td" + (Theme != CacheViewTheme.None ? String.Format(tableCellClass, ThemeColors.GetDefaultCellBgColor(Theme)) : tableCellClass) + @" align=""left"">" + element.Type.ToString() + @"</td>
    <td" + (Theme != CacheViewTheme.None ? String.Format(tableCellClass, ThemeColors.GetBooleanValueColor(Theme, element.Enabled)) : tableCellClass) + @" align=""center"">" + element.Enabled.ToString() + @"</td>
    <td" + (Theme != CacheViewTheme.None ? String.Format(tableCellClass, ThemeColors.GetBooleanValueColor(Theme, element.IsUsed)) : tableCellClass) + @" align=""center"">" + element.IsUsed.ToString() + @"</td>
    <td" + (Theme != CacheViewTheme.None ? String.Format(tableCellClass, ThemeColors.GetDefaultCellBgColor(Theme)) : tableCellClass) + @" align=""center"">" + element.Minutes.ToString() + @"</td>
    <td" + (Theme != CacheViewTheme.None ? String.Format(tableCellClass, ThemeColors.GetDefaultCellBgColor(Theme)) : tableCellClass) + @" align=""center"">" + element.NumberOfObjects.ToString() + @"</td>
    <td" + (Theme != CacheViewTheme.None ? String.Format(tableCellClass, ThemeColors.GetCacheItemPriorityColor(Theme, element.CacheItemPriority)) : tableCellClass) + @" align=""center"">" + element.CacheItemPriority.ToString() + @"</td>
</tr>
");

                        if (element.IsIterating)
                        {
                            foreach (string name in element.EnumeratedElements)
                            {
                                sb.Append(
        @"
<tr>
    <td" + (Theme != CacheViewTheme.None ? String.Format(tableEnumeratingCellClass, ThemeColors.GetDefaultCellBgColor(Theme)) : tableEnumeratingCellClass) + @" colspan=""7"" align=""left"">-&gt; " + name + @"</td>
</tr>
");
                            }
                        }
                    }
                    sb.Append("</table>");
                }

                writer.Write(sb.ToString());
            }
        }
    }
}
