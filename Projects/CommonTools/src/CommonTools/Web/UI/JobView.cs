using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web;
using CommonTools.Components.Threading;

namespace CommonTools.Web.UI
{
	/// <summary>
	/// The JobView is a control that displays all the jobs present in web.config in a table.
	/// </summary>
    [ToolboxData("<{0}:JobView runat=server> </{0}:JobView>")]
    public class JobView : CompositeControl
    {
        #region properties
        private string _TableCssClass;
		/// <summary>
		/// The Css style for the table itself.
		/// </summary>
        [Category("Layout")]
        public string TableCssClass
        {
            get { return _TableCssClass; }
            set { _TableCssClass = value; }
        }

        private string _TableHeaderCssClass;
		/// <summary>
		/// The style of the table header.
		/// </summary>
        [Category("Layout")]
        public string TableHeaderCssClass
        {
            get { return _TableHeaderCssClass; }
            set { _TableHeaderCssClass = value; }
        }

        private string _TableCellCssClass;
		/// <summary>
		/// The style of the table cell.
		/// </summary>
        [Category("Layout")]
        public string TableCellCssClass
        {
            get { return _TableCellCssClass; }
            set { _TableCellCssClass = value; }
        }

        private string _TableEnumeratingCellCssClass;
		/// <summary>
		/// Enumerating cells.
		/// </summary>
        [Category("Layout")]
        public string TableEnumeratingCellCssClass
        {
            get { return _TableEnumeratingCellCssClass; }
            set { _TableEnumeratingCellCssClass = value; }
        }

        private JobViewTheme _Theme = JobViewTheme.None;
		/// <summary>
		/// The current Job Theme of the class.
		/// </summary>
        [Category("Layout")]
        public JobViewTheme Theme
        {
            get { return _Theme; }
            set { _Theme = value; }
        }
        #endregion

		/// <summary>
		/// Refreshes all child controls.
		/// </summary>
        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }

		/// <summary>
		/// Removes all child controls from the JobView.
		/// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();
        }

		/// <summary>
		/// Overridden. Renders the JobView as a table.
		/// </summary>
		/// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
			/* TODO: rewrite this to use child WebControls instead of a bunch of writer.writes. */
            if (HttpContext.Current == null)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("<p style=\"font-family: " + this.Font.Name + "; font-size: " + this.Font.Size.ToString() + "; color: #FFFF99; border: outset 1px #000000; padding: 0; background-color: #5a7ab8\">");
                {
                    sb.Append("<b>JobView</b>");
                }
                sb.Append("</p>");

                writer.Write(sb.ToString());
                base.Render(writer);
            }
            else
            {
                AddAttributesToRender(writer);

                StringBuilder sb = new StringBuilder();
                sb.Append("No jobs found at web.config");

                Jobs jobInstance = Jobs.Instance();
                if (jobInstance != null)
                {

                    string tableClass = string.Empty,
                            tableHeaderClass = string.Empty,
                            tableCellClass = string.Empty,
                            tableEnumeratingCellClass = string.Empty,
                            cellValueTrue = string.Empty,
                            cellValueFalse = string.Empty;

                    #region get styles
                    switch (Theme)
                    {
                        case JobViewTheme.None:
                            tableClass = string.IsNullOrEmpty(TableCssClass) ? "" : (" class=\"" + TableCssClass + "\"");
                            tableHeaderClass = string.IsNullOrEmpty(TableHeaderCssClass) ? "" : (" class=\"" + TableHeaderCssClass + "\"");
                            tableCellClass = string.IsNullOrEmpty(TableCellCssClass) ? "" : (" class=\"" + TableCellCssClass + "\"");
                            tableEnumeratingCellClass = string.IsNullOrEmpty(TableEnumeratingCellCssClass) ? "" : (" class=\"" + TableEnumeratingCellCssClass + "\"");
                            break;

                        case JobViewTheme.Default:
                            tableClass = @" style=""width: 100%; border: Solid 2px #5a7ab8; font-family: " + this.Font.Name + @"; font-size: " + this.Font.Size.ToString() + @"""";
                            tableHeaderClass = @" style=""background-color: #5A7AB8; color: #FFFFFF; padding: 2px 4px 2px 4px; border: Solid 1px #5A7AB8; border-collapse: collapse;""";
                            tableCellClass = @" style=""background-color: {0}; color: #000000; padding: 2px 2px 2px 2px; border-left: Solid 1px #000000; border-right: Solid 1px #dddddd; border-bottom: Solid 1px #dddddd; border-top: Solid 1px #000000; border-collapse: collapse;""";
                            break;
                    }
                    #endregion

                    sb = new StringBuilder();
                    sb.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"margin-bottom: 20px;\">");
                    sb.Append(
        @"
<tr>
    <td align=""left""><strong>Created:</strong></td>
    <td align=""left"" style=""padding-left: 12px;"">" + jobInstance.Created.ToString("HH:mm:ss") + @"</td>
</tr>
<tr>
    <td align=""left""><strong>IsRunning:</strong></td>
    <td align=""left"" style=""padding-left: 12px;"">" + jobInstance.IsRunning + @"</td>
</tr>
<tr>
    <td align=""left""><strong>Last start:</strong></td>
    <td align=""left"" style=""padding-left: 12px;"">" + jobInstance.LastStart.ToString("HH:mm:ss") + @"</td>
</tr>
<tr>
    <td align=""left""><strong>Last stop:</strong></td>
    <td align=""left"" style=""padding-left: 12px;"">" + jobInstance.LastStop.ToString("HH:mm:ss") + @"</td>
</tr>
<tr>
    <td align=""left""><strong>Default Interval:</strong></td>
    <td align=""left"" style=""padding-left: 12px;"">" + jobInstance.Minutes.ToString() + @" minutes</td>
</tr>
");
                    sb.Append("</table>");

                    sb.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\"" + tableClass + ">");
                    sb.Append(
        @"
<tr>
    <th" + tableHeaderClass + @">Name</th>
    <th" + tableHeaderClass + @">Type</th>
    <th" + tableHeaderClass + @">IsRunning</th>
    <th" + tableHeaderClass + @">LastStart</th>
    <th" + tableHeaderClass + @">LastEnd</th>
    <th" + tableHeaderClass + @">LastSuccess</th>
    <th" + tableHeaderClass + @">Single Thread</th>
    <th" + tableHeaderClass + @">Interval</th>
</tr>
");
                    DateTime nullTime = new DateTime();
                    foreach (Job job in Jobs.Instance().CurrentJobs)
                    {
                        string lastEnd = "&nbsp;";
                        if (job.LastEnd.CompareTo(nullTime) != 0)
                            lastEnd = job.LastEnd.ToString("HH:mm:ss");
                        string lastStart = "&nbsp;";
                        if (job.LastStarted.CompareTo(nullTime) != 0)
                            lastStart = job.LastStarted.ToString("HH:mm:ss");
                        string lastSuccess = "&nbsp;";
                        if (job.LastSuccess.CompareTo(nullTime) != 0)
                            lastSuccess = job.LastSuccess.ToString("HH:mm:ss");

                        sb.AppendFormat(
        @"
<tr>
    <td" + (Theme != JobViewTheme.None ? String.Format(tableCellClass, ThemeColors.GetDefaultCellBgColor(Theme)) : tableCellClass) + @" align=""left"">" + job.Name + @"</td>
    <td" + (Theme != JobViewTheme.None ? String.Format(tableCellClass, ThemeColors.GetDefaultCellBgColor(Theme)) : tableCellClass) + @" align=""left"">" + job.JobType.ToString() + @"</td>
    <td" + (Theme != JobViewTheme.None ? String.Format(tableCellClass, ThemeColors.GetBooleanValueColor(Theme, job.IsRunning)) : tableCellClass) + @" align=""center"">" + job.IsRunning.ToString() + @"</td>
    <td" + (Theme != JobViewTheme.None ? String.Format(tableCellClass, ThemeColors.GetDefaultCellBgColor(Theme)) : tableCellClass) + @" align=""left"">" + lastStart + @"</td>
    <td" + (Theme != JobViewTheme.None ? String.Format(tableCellClass, ThemeColors.GetDefaultCellBgColor(Theme)) : tableCellClass) + @" align=""left"">" + lastEnd + @"</td>
    <td" + (Theme != JobViewTheme.None ? String.Format(tableCellClass, ThemeColors.GetDefaultCellBgColor(Theme)) : tableCellClass) + @" align=""left"">" + lastSuccess + @"</td>
    <td" + (Theme != JobViewTheme.None ? String.Format(tableCellClass, ThemeColors.GetBooleanValueColor(Theme, job.ExecuteOnOwnThread)) : tableCellClass) + @" align=""center"">" + job.ExecuteOnOwnThread.ToString() + @"</td>
    <td" + (Theme != JobViewTheme.None ? String.Format(tableCellClass, ThemeColors.GetDefaultCellBgColor(Theme)) : tableCellClass) + @" align=""left"">" + (job.Interval.ToString()) + @"</td>
</tr>
");
                    }
                    sb.Append("</table>");
                }

                writer.Write(sb.ToString());
            }
        }
    }
}
