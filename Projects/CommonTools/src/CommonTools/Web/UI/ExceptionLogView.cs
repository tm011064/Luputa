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
using CommonTools.Extensions;

namespace CommonTools.Web.UI
{
    /// <summary>
    /// 
    /// </summary>
    [ToolboxData("<{0}:ExceptionLogView runat=server> </{0}:ExceptionLogView>")]
    public class ExceptionLogView : CompositeControl, IPostBackEventHandler
    {
        #region members
        private const string DELETE_RECORDS_ARGUMENT = "DR";
        private const string UPDATE_RECORDS_ARGUMENT = "UR";
        private const string GETPAGE_ARGUMENT = "GP";
        private const char POSTBACK_SEPARATOR = '$';

        private DropDownList _DDLOrderBy = new DropDownList();
        private DropDownList _DDLApplicationLocations = new DropDownList();
        private DropDownList _DDLPageSize = new DropDownList();
        private DropDownList _DDLHandlingStatus = new DropDownList();
        private DropDownList _DDLRemoveOlder = new DropDownList();
        private Button _BtnRefresh = new Button();
        private Button _BtnRemove = new Button();

        private Dictionary<int, string> _ApplicationLocations = new Dictionary<int, string>();
        private string _ApplicationLocationEnumType = null;

        private bool _ShowPager = true;
        private int? _TotalRowCount = 0;
        private LogDatasets.ExceptionLogDataTable _ExceptionLogTable;
        private string _PagerRowHTML;
        private string _PageInfoHMTL;
        #endregion

        #region properties
        private string _ExceptionCellCssClass = string.Empty;
        /// <see cref="JobView.TableCssClass"/>
        [Category("Layout")]
        public string ExceptionCellCssClass
        {
            get { return _ExceptionCellCssClass; }
            set { _ExceptionCellCssClass = value; }
        }

        private string _FilterCssClass = string.Empty;
        /// <see cref="JobView.TableCssClass"/>
        [Category("Layout")]
        public string FilterCssClass
        {
            get { return _FilterCssClass; }
            set { _FilterCssClass = value; }
        }

        private string _DeleteCellCssClass = string.Empty;
        /// <see cref="JobView.TableCssClass"/>
        [Category("Layout")]
        public string DeleteCellCssClass
        {
            get { return _DeleteCellCssClass; }
            set { _DeleteCellCssClass = value; }
        }

        private bool _UseDefaultStyling = true;
        /// <see cref="JobView.TableCssClass"/>
        [Category("Layout")]
        public bool UseDefaultStyling
        {
            get { return _UseDefaultStyling; }
            set { _UseDefaultStyling = value; }
        }

        private string _PagerCssClass = string.Empty;
        /// <see cref="JobView.TableCellCssClass"/>
        [Category("Layout")]
        public string PagerCssClass
        {
            get { return _PagerCssClass; }
            set { _PagerCssClass = value; }
        }
        private string _PagerCellCssClass = string.Empty;
        /// <see cref="JobView.TableCellCssClass"/>
        [Category("Layout")]
        public string PagerCellCssClass
        {
            get { return _PagerCellCssClass; }
            set { _PagerCellCssClass = value; }
        }
        private string _PagerSelectedCellCssClass = string.Empty;
        /// <see cref="JobView.TableCellCssClass"/>
        [Category("Layout")]
        public string PagerSelectedCellCssClass
        {
            get { return _PagerSelectedCellCssClass; }
            set { _PagerSelectedCellCssClass = value; }
        }

        private int _MaxPagerItemsPerRow = 8;
        /// <see cref="JobView.TableCellCssClass"/>
        [Category("Layout")]
        public int MaxPagerItemsPerRow
        {
            get { return _MaxPagerItemsPerRow; }
            set { _MaxPagerItemsPerRow = value; }
        }

        private int _PageIndex = 0;
        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>The index of the page.</value>
        public int PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; }
        }

        /// <summary>
        /// Gets or sets the event types.
        /// </summary>
        /// <value>The event types.</value>
        public string ApplicationLocationEnumType
        {
            get { return _ApplicationLocationEnumType; }
            set
            {
                _ApplicationLocationEnumType = value;

                _ApplicationLocations = new Dictionary<int, string>();
                foreach (Enum v in Enum.GetValues(Type.GetType(_ApplicationLocationEnumType)))
                    _ApplicationLocations.Add(int.Parse(v.ToString("d")), v.ToString());

                _DDLApplicationLocations.Items.Clear();
                _DDLApplicationLocations.Items.Add(new ListItem("show all", string.Empty));
                foreach (int key in _ApplicationLocations.Keys)
                    _DDLApplicationLocations.Items.Add(new ListItem(_ApplicationLocations[key], key.ToString()));
            }
        }
        #endregion

        #region life-cycle
        /// <see cref="JobView.RecreateChildControls"/>
        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }

        /// <see cref="JobView.CreateChildControls"/>
        protected override void CreateChildControls()
        {
            _DDLPageSize = new DropDownList();

            _DDLPageSize.Items.Add(new ListItem("10", "10"));
            _DDLPageSize.Items.Add(new ListItem("25", "25"));
            _DDLPageSize.Items.Add(new ListItem("50", "50"));
            _DDLPageSize.Items.Add(new ListItem("100", "100"));

            _DDLOrderBy = new DropDownList();
            _DDLOrderBy.Items.Add(new ListItem("DateLastOccurred DESC", LogManager.ExceptionsOrderBy.DateLastOccurredDesc.ToString("d")));
            _DDLOrderBy.Items.Add(new ListItem("DateCreated DESC", LogManager.ExceptionsOrderBy.DateCreatedDesc.ToString("d")));
            _DDLOrderBy.Items.Add(new ListItem("TotalOccurrences DESC", LogManager.ExceptionsOrderBy.TotalOccurrencesDesc.ToString("d")));
            _DDLOrderBy.Items.Add(new ListItem("DateLastOccurred ASC", LogManager.ExceptionsOrderBy.DateCreatedAsc.ToString("d")));
            _DDLOrderBy.Items.Add(new ListItem("DateCreated ASC", LogManager.ExceptionsOrderBy.DateCreatedAsc.ToString("d")));
            _DDLOrderBy.Items.Add(new ListItem("TotalOccurrences ASC", LogManager.ExceptionsOrderBy.TotalOccurrencesAsc.ToString("d")));


            _BtnRefresh.Text = "Filter";
            _BtnRefresh.UseSubmitBehavior = false;
            _BtnRemove.Text = "Delete";
            _BtnRemove.UseSubmitBehavior = false;

            _BtnRemove.OnClientClick = "if (confirm('Do you really want to delete those records?')) { " + Page.ClientScript.GetPostBackEventReference(this, DELETE_RECORDS_ARGUMENT) + "; return true; } return false;";

            _DDLRemoveOlder = new DropDownList();
            _DDLRemoveOlder.Items.Add(new ListItem("a year", "365"));
            _DDLRemoveOlder.Items.Add(new ListItem("a month", "31"));
            _DDLRemoveOlder.Items.Add(new ListItem("a week", "7"));
            _DDLRemoveOlder.Items.Add(new ListItem("a day", "1"));

            _DDLHandlingStatus = new DropDownList();
            _DDLHandlingStatus.Items.Add(new ListItem("show all", string.Empty));
            foreach (ExceptionHandlingStatus status in Enum.GetValues(typeof(ExceptionHandlingStatus)))
                _DDLHandlingStatus.Items.Add(new ListItem(status.ToString(), status.ToString("d")));

            this.Controls.Add(_DDLHandlingStatus);
            this.Controls.Add(_BtnRefresh);
            this.Controls.Add(_DDLApplicationLocations);
            this.Controls.Add(_DDLPageSize);
            this.Controls.Add(_DDLRemoveOlder);
            this.Controls.Add(_DDLOrderBy);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            #region get eventlog table
            int pageSize = int.Parse(_DDLPageSize.SelectedValue);
            _ExceptionLogTable = LogManager.GetExceptionPage(
                pageSize
                , this.PageIndex
                , out _TotalRowCount
                , (string.IsNullOrEmpty(_DDLApplicationLocations.SelectedValue) ? -1 : int.Parse(_DDLApplicationLocations.SelectedValue))
                , (string.IsNullOrEmpty(_DDLHandlingStatus.SelectedValue) ? -1 : int.Parse(_DDLHandlingStatus.SelectedValue))
                , (string.IsNullOrEmpty(_DDLOrderBy.SelectedValue) 
                    ? LogManager.ExceptionsOrderBy.DateCreatedDesc 
                    : (LogManager.ExceptionsOrderBy)Enum.Parse(typeof(LogManager.ExceptionsOrderBy), _DDLOrderBy.SelectedValue)));
            #endregion

            #region styling
            if (this.UseDefaultStyling)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ExceptionLogViewSyles",
@"
<style type=""text/css"">
<!--
.ExceptionLogView {
}
.ExceptionLogView table {
	border: Solid 1px #FCFCFC;
	width: 802px;
}
.ExceptionLogView table th {
	text-align: center;
	font-weight: bold;
	padding: 3px 4px 3px 4px;
	border: Solid 1px #212121;
}
.ExceptionLogView table td {
	padding: 1px 4px 1px 4px;
	border: Solid 1px #212121;
}
.ExceptionLogViewPager {
	width: 800px;
	border: Solid 1px #212121;
	padding-top: 1px;
	padding-bottom: 1px;
	text-align: center;
	height: 120px;
	margin-top: 8px;
	overflow: auto;
	display: block;
	clear: both;
	float: left;
}
.ExceptionLogViewPager table {
	width: 780px;
}
.ExceptionLogViewPager table td {
	cursor: pointer;
}
.ExceptionLogViewPagerCell {
}
.ExceptionLogViewPagerCellSelected {
	background-color: #FEC654;
}
.ExceptionLogViewCell {
	text-align: left;
}
.ExceptionLogViewCell p {
	padding: 1px 0px 1px 0px;
    margin: 0px;
}
.ExceptionLogViewFilter {
	display: inline;
	float: left;
	clear: none;
	border: Solid 1px #212121;
	padding: 2px 6px 2px 6px;
	background-color: #33FF99;
}
.ExceptionLogViewDelete {
	display: block;
	float: left;
	clear: both;
	border: Solid 1px #212121;
	padding: 2px 6px 2px 6px;
	margin-top: 4px;
	background-color: #CC66FF;
}
-->
</style>
", false);
                this.CssClass = "ExceptionLogView";
                this.ExceptionCellCssClass = "ExceptionLogViewCell";
                this.DeleteCellCssClass = "ExceptionLogViewDelete";
                this.FilterCssClass = "ExceptionLogViewFilter";
                this.PagerCssClass = "ExceptionLogViewPager";
                this.PagerCellCssClass = "ExceptionLogViewPagerCell";
                this.PagerSelectedCellCssClass = "ExceptionLogViewPagerCellSelected";
            }
            #endregion

            #region pager
            int pageCount = (int)_TotalRowCount / pageSize;

            if (pageCount == 0)
                pageCount = 1;
            else if (_TotalRowCount % pageSize != 0)
            {
                pageCount++;
            }

            string cellClass;
            if (pageCount > 1)
            {
                _ShowPager = true;
                int maxItemCounter = 0;
                _PagerRowHTML = string.Empty;
                _PagerRowHTML += "<table cellpadding='2' cellspacing='1' border='0'><tr>";

                for (int i = 0; i < pageCount - 1; i++)
                {
                    if (maxItemCounter >= MaxPagerItemsPerRow)
                    {
                        _PagerRowHTML += "</tr><tr>";
                        maxItemCounter = 0;
                    }

                    if (i == this.PageIndex)
                    {
                        _PageInfoHMTL = "<div style='display: inline; float: left; clear: none; width: 300px; text-align: right;'>showing results " + ((i * pageSize) + 1).ToString() + "&nbsp;-&nbsp;" + (((i + 1) * pageSize)).ToString() + " from " + _TotalRowCount.ToString() + " records.</div>"
                            + ((i == 0) ? ("") : ("<a onclick=\"" + Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this, GETPAGE_ARGUMENT + POSTBACK_SEPARATOR.ToString() + (i - 1).ToString())) + "\">&lt; previous</a>&nbsp;|&nbsp;"))
                            + "<a onclick=\"" + Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this, GETPAGE_ARGUMENT + POSTBACK_SEPARATOR.ToString() + (i + 1).ToString())) + "\">next &gt;</a>";
                        cellClass = (string.IsNullOrEmpty(PagerSelectedCellCssClass) ? ("") : (@" class=""" + PagerSelectedCellCssClass + @""""));
                    }
                    else
                    {
                        cellClass = (string.IsNullOrEmpty(PagerCellCssClass) ? ("") : (@" class=""" + PagerCellCssClass + @""""));
                    }

                    _PagerRowHTML += @"<td" + cellClass
                        + @" onclick=""" + Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this, GETPAGE_ARGUMENT + POSTBACK_SEPARATOR.ToString() + i.ToString())) + @""">"
                        + ((i * pageSize) + 1).ToString() + "&nbsp;-&nbsp;" + (((i + 1) * pageSize)).ToString() + @"</td>";
                    maxItemCounter++;
                }

                // add last pager
                if ((pageCount - 1) == this.PageIndex)
                {
                    _PageInfoHMTL = "Showing results " + (((pageCount - 1) * pageSize) + 1).ToString() + "&nbsp;-&nbsp;" + _TotalRowCount.ToString() + " from " + _TotalRowCount.ToString() + " records."
                        + ((pageCount < 2) ? ("") : ("<a onclick=\"" + Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this, GETPAGE_ARGUMENT + POSTBACK_SEPARATOR.ToString() + (pageCount - 2).ToString())) + "\">&lt; previous</a>&nbsp;&nbsp;"));
                    cellClass = (string.IsNullOrEmpty(PagerSelectedCellCssClass) ? ("") : (@" class=""" + PagerSelectedCellCssClass + @""""));
                }
                else
                {
                    cellClass = (string.IsNullOrEmpty(PagerCellCssClass) ? ("") : (@" class=""" + PagerCellCssClass + @""""));
                }
                _PagerRowHTML += (maxItemCounter >= MaxPagerItemsPerRow ? "</tr><tr>" : "") + @"<td" + cellClass
                    + @" onclick=""" + Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this, GETPAGE_ARGUMENT + POSTBACK_SEPARATOR.ToString() + (pageCount - 1).ToString())) + @""">" + (((pageCount - 1) * pageSize) + 1).ToString()
                    + "&nbsp;-&nbsp;" + _TotalRowCount.ToString() + @"</td></tr></table>";
            }
            else
            {
                _ShowPager = false;
            }
            #endregion

            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("EV_MarkAs"))
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "EV_MarkAs",
@"
function __UpdateExceptionStatus(dropDownId, exceptionId) {
    var o = document.getElementById(dropDownId);
    if (o != null) {
        __doPostBack('" + this.UniqueID + @"', '" + UPDATE_RECORDS_ARGUMENT + POSTBACK_SEPARATOR.ToString() + @"' + exceptionId + '" + POSTBACK_SEPARATOR.ToString() + @"' + o.options[o.selectedIndex].value);
        return true;
    }    
    alert('DropDown not found.');
    return false;
}
", true);
            base.OnPreRender(e);
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
                    sb.Append("<b>ExceptionLogView</b>");
                }
                sb.Append("</p>");

                writer.Write(sb.ToString());
                base.Render(writer);
            }
            else
            {
                AddAttributesToRender(writer);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                {
                    writer.Write("<div" + (string.IsNullOrEmpty(this.FilterCssClass) ? ("") : (" class=\"" + this.FilterCssClass + "\"")) + ">Filter by ");
                    _DDLApplicationLocations.RenderControl(writer);
                    writer.Write(" and ");
                    _DDLHandlingStatus.RenderControl(writer);
                    writer.Write(", order by ");
                    _DDLOrderBy.RenderControl(writer);
                    writer.Write(", show ");
                    _DDLPageSize.RenderControl(writer);
                    writer.Write(" results");
                    _BtnRefresh.RenderControl(writer);
                    writer.Write("</div><div" + (string.IsNullOrEmpty(this.DeleteCellCssClass) ? ("") : (" class=\"" + this.DeleteCellCssClass + "\"")) + ">Remove older than ");
                    _DDLRemoveOlder.RenderControl(writer);
                    writer.Write("&nbsp;&nbsp;");
                    _BtnRemove.RenderControl(writer);
                    writer.Write("</div>");

                    writer.Write("<div style='padding: 4px 0px 1px 0px; display: block; float: left; clear: both;'>" + _PageInfoHMTL + "</div>");
                    StringBuilder sb = new StringBuilder();

                    sb = new StringBuilder();
                    sb.Append("<div style='display: block; float: left; clear: both;'><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><th>Exception Type</th><th>AppLocation</th><th>Details</th><th>Last Occurred</th><th></th></tr>");
                    foreach (LogDatasets.ExceptionLogRow dr in _ExceptionLogTable)
                    {
                        sb.Append(
@"
<tr>
    <td>" + (dr.Exception.LastIndexOf(".") >= 0 ? (dr.Exception.Remove(0, dr.Exception.LastIndexOf(".") + 1)) : (dr.Exception)) + @"</td>
    <td>" + (_ApplicationLocations.ContainsKey(dr.AppLocation) ? _ApplicationLocations[dr.AppLocation] : dr.AppLocation.ToString()) + @"</td>
    <td>IP: " + (dr.IsIPAddressNull() ? "" : dr.IPAddress) + @"<br/>Machine: " + dr.MachineName + @"<br/>User: " + dr.AuthenticatedUserId + @"</td>
    <td>" + dr.DateLastOccurred.ToString("dd/MM/yyyy a\\t HH:mm:ss.fff") + @"</td>
    <td>Total: <strong>" + dr.TotalOccurrences.ToString() + @"</strong><br/>
        Set: <select id='" + this.ClientID + "_EID_" + dr.ExceptionID.ToString() + @"'>
                    <option value='0'" + ((dr.HandlingStatus == (byte)ExceptionHandlingStatus.Expected) ? (" selected='selected'") : ("")) + @">Expected</option>
                    <option value='1'" + ((dr.HandlingStatus == (byte)ExceptionHandlingStatus.HandledInCode) ? (" selected='selected'") : ("")) + @">Handled</option>
                    <option value='2'" + ((dr.HandlingStatus == (byte)ExceptionHandlingStatus.Unhandled) ? (" selected='selected'") : ("")) + @">Unhandled</option>
                    <option value='3'" + ((dr.HandlingStatus == (byte)ExceptionHandlingStatus.Resolved) ? (" selected='selected'") : ("")) + @">Resolved</option>
                 </select>&nbsp;<a onclick=""__UpdateExceptionStatus('" + this.ClientID + "_EID_" + dr.ExceptionID + @"', " + dr.ExceptionID.ToString() + ")" + @""">set</a>
    </td>
</tr>
<tr>
    <td colspan='5'" + (string.IsNullOrEmpty(this.ExceptionCellCssClass) ? ("") : (@" class=""" + this.ExceptionCellCssClass + @"""")) + @">
        " + (string.IsNullOrEmpty(dr.Method) ? ("") : ("<p><strong>Method:</strong>&nbsp;" + dr.Method + @"</p>")) + @"
        " + (dr.IsUserAgentNull() ? ("") : ("<p><strong>Agent:</strong>&nbsp;" + dr.UserAgent + @"</p>")) + @"
        " + (dr.IsUrlNull() ? ("") : ("<p><strong>Url:</strong>&nbsp;" + dr.Url + " as HTTP " + dr.HttpVerb + @"</p>")) + @"
        " + (dr.IsHttpReferrerNull() ? ("") : ("<p><strong>Referrer:</strong>&nbsp;" + dr.HttpReferrer + @"</p>")) + @"
        <p>" + dr.ExceptionMessage + @"</p>
    </td>
</tr>
");
                    }

                    sb.Append("</table></div>");
                    writer.Write(sb.ToString());

                    if (_ShowPager)
                        writer.Write(@"<div " + (string.IsNullOrEmpty(PagerCssClass) ? ("") : (@"class=""" + PagerCssClass + @"""")) + ">" + _PagerRowHTML + @"</div>");

                }
                writer.RenderEndTag();
            }
        }
        #endregion

        #region IPostBackEventHandler Members

        /// <summary>
        /// When implemented by a class, enables a server control to process an event raised when a form is posted to the server.
        /// </summary>
        /// <param name="eventArgument">A <see cref="T:System.String"></see> that represents an optional event argument to be passed to the event handler.</param>
        public void RaisePostBackEvent(string eventArgument)
        {
            if (!string.IsNullOrEmpty(eventArgument))
            {
                string[] splitted = eventArgument.Split(POSTBACK_SEPARATOR);
                if (splitted.Length > 0)
                {
                    try
                    {
                        switch (splitted[0])
                        {
                            case DELETE_RECORDS_ARGUMENT:
                                LogManager.DeleteExceptionByDate(int.Parse(_DDLRemoveOlder.SelectedValue));
                                break;
                            case UPDATE_RECORDS_ARGUMENT:
                                if (splitted.Length >= 3)
                                {
                                    LogManager.UpdateExceptionHandlingStatus(long.Parse(splitted[1]), ((ExceptionHandlingStatus)Enum.Parse(typeof(ExceptionHandlingStatus), splitted[2])));
                                }
                                break;
                            case GETPAGE_ARGUMENT:
                                if (splitted.Length >= 2)
                                    this.PageIndex = int.Parse(splitted[1]);
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Ex_EV_PB", "alert('An exception occurred during parsing the page postback argument:\n" + e.Message + "');", true);
                    }
                }
            }
        }

        #endregion
    }
}
