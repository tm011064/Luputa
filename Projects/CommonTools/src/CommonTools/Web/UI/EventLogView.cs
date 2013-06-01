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

namespace CommonTools.Web.UI
{
    /// <summary>
    /// 
    /// </summary>
    [ToolboxData("<{0}:EventLogView runat=server> </{0}:EventLogView>")]
    public class EventLogView : CompositeControl, IPostBackEventHandler
    {
        #region members
        private const string DELETE_RECORDS_ARGUMENT = "DELETE_RECORDS";

        private DropDownList _DDLApplicationLocations = new DropDownList();
        private DropDownList _DDLEventTypes = new DropDownList();
        private DropDownList _DDLPageSize = new DropDownList();
        private DropDownList _DDLRemoveOlder = new DropDownList();
        private Button _BtnRefresh = new Button();
        private Button _BtnRemove = new Button();

        private Dictionary<int, string> _EventTypes = new Dictionary<int, string>();
        private Dictionary<int, string> _ApplicationLocations = new Dictionary<int, string>();
        private string _ApplicationLocationEnumType = null;
        private string _EventTypesEnumType = null;

        private bool _ShowPager = true;
        private int? _TotalRowCount = 0;
        private LogDatasets.EventLogDataTable _EventLogTable;
        private string _PagerRowHTML;
        private string _PageInfoHMTL;
        #endregion

        #region properties
        private string _EventIDCellCssClass = string.Empty;
        /// <see cref="JobView.TableCssClass"/>
        [Category("Layout")]
        public string EventIDCellCssClass
        {
            get { return _EventIDCellCssClass; }
            set { _EventIDCellCssClass = value; }
        }
        private string _AppLocationCellCssClass = string.Empty;
        /// <see cref="JobView.TableCssClass"/>
        [Category("Layout")]
        public string AppLocationCellCssClass
        {
            get { return _AppLocationCellCssClass; }
            set { _AppLocationCellCssClass = value; }
        }
        private string _MessageCellCssClass = string.Empty;
        /// <see cref="JobView.TableCssClass"/>
        [Category("Layout")]
        public string MessageCellCssClass
        {
            get { return _MessageCellCssClass; }
            set { _MessageCellCssClass = value; }
        }
        private string _EventTypeCellCssClass = string.Empty;
        /// <see cref="JobView.TableCssClass"/>
        [Category("Layout")]
        public string EventTypeCellCssClass
        {
            get { return _EventTypeCellCssClass; }
            set { _EventTypeCellCssClass = value; }
        }
        private string _EventDateCellCssClass = string.Empty;
        /// <see cref="JobView.TableCssClass"/>
        [Category("Layout")]
        public string EventDateCellCssClass
        {
            get { return _EventDateCellCssClass; }
            set { _EventDateCellCssClass = value; }
        }
        private string _MachineNameCellCssClass = string.Empty;
        /// <see cref="JobView.TableCssClass"/>
        [Category("Layout")]
        public string MachineNameCellCssClass
        {
            get { return _MachineNameCellCssClass; }
            set { _MachineNameCellCssClass = value; }
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

        private bool _ShowEventID = true;
        /// <see cref="JobView.TableCssClass"/>
        [Category("Layout")]
        public bool ShowEventID
        {
            get { return _ShowEventID; }
            set { _ShowEventID = value; }
        }
        private bool _ShowAppLocation = true;
        /// <see cref="JobView.TableCssClass"/>
        [Category("Layout")]
        public bool ShowAppLocation
        {
            get { return _ShowAppLocation; }
            set { _ShowAppLocation = value; }
        }
        private bool _ShowEventType = true;
        /// <see cref="JobView.TableCssClass"/>
        [Category("Layout")]
        public bool ShowEventType
        {
            get { return _ShowEventType; }
            set { _ShowEventType = value; }
        }
        private bool _ShowMachineName = true;
        /// <see cref="JobView.TableCssClass"/>
        [Category("Layout")]
        public bool ShowMachineName
        {
            get { return _ShowMachineName; }
            set { _ShowMachineName = value; }
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
        public string EventTypesEnumType
        {
            get { return _EventTypesEnumType; }
            set
            {
                _EventTypesEnumType = value;

                _EventTypes = new Dictionary<int, string>();
                foreach (Enum v in Enum.GetValues(Type.GetType(_EventTypesEnumType)))
                    _EventTypes.Add(int.Parse(v.ToString("d")), v.ToString());

                _DDLEventTypes.Items.Clear();
                _DDLEventTypes.Items.Add(new ListItem("show all", string.Empty));
                foreach (int key in _EventTypes.Keys)
                    _DDLEventTypes.Items.Add(new ListItem(_EventTypes[key], key.ToString()));
            }
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

            _DDLPageSize.Items.Add(new ListItem("25", "25"));
            _DDLPageSize.Items.Add(new ListItem("50", "50"));
            _DDLPageSize.Items.Add(new ListItem("100", "100"));
            _DDLPageSize.Items.Add(new ListItem("250", "250"));
            _DDLPageSize.Items.Add(new ListItem("500", "500"));

            _BtnRefresh.Text = "Filter";
            _BtnRemove.Text = "Delete";

            _BtnRemove.OnClientClick = "if (confirm('Do you really want to delete those records?')) { " + Page.ClientScript.GetPostBackEventReference(this, DELETE_RECORDS_ARGUMENT) + "; return true; } return false;";

            _DDLRemoveOlder = new DropDownList();
            _DDLRemoveOlder.Items.Add(new ListItem("a year", "365"));
            _DDLRemoveOlder.Items.Add(new ListItem("a month", "31"));
            _DDLRemoveOlder.Items.Add(new ListItem("a week", "7"));
            _DDLRemoveOlder.Items.Add(new ListItem("a day", "1"));

            this.Controls.Add(_BtnRefresh);
            this.Controls.Add(_DDLApplicationLocations);
            this.Controls.Add(_DDLPageSize);
            this.Controls.Add(_DDLEventTypes);
            this.Controls.Add(_DDLRemoveOlder);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            #region get eventlog table
            int pageSize = int.Parse(_DDLPageSize.SelectedValue);
            _EventLogTable = LogManager.GetEventPage(pageSize, this.PageIndex, out _TotalRowCount
                , (string.IsNullOrEmpty(_DDLApplicationLocations.SelectedValue) ? -1 : int.Parse(_DDLApplicationLocations.SelectedValue))
                , (string.IsNullOrEmpty(_DDLEventTypes.SelectedValue) ? -1 : int.Parse(_DDLEventTypes.SelectedValue)));
            #endregion

            #region styling
            if (this.UseDefaultStyling)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "EventLogViewSyles",
@"
<style type=""text/css"">
<!--
.EventLogView {
}
.EventLogView table {
	border: Solid 1px #FCFCFC;
	width: 802px;
}
.EventLogView table th {
	text-align: center;
	font-weight: bold;
	padding: 3px 4px 3px 4px;
	border: Solid 1px #212121;
}
.EventLogView table td {
	padding: 1px 4px 1px 4px;
	border: Solid 1px #212121;
}
.EventLogViewPager {
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
.EventLogViewPager table {
	width: 780px;
}
.EventLogViewPager table td {
	cursor: pointer;
}
.EventLogViewPagerCell {
}
.EventLogViewPagerCellSelected {
	background-color: #FEC654;
}
.EventLogViewEventIDCell {
	width: 60px;
	text-align: center;
}
.EventLogViewAppLocationCell {
	width: 90px;
	text-align: center;
}
.EventLogViewMessageCell {
	width: 320px;
	text-align: left;
}
.EventLogViewEventDateCell {
	width: 160px;
	text-align: center;
}
.EventLogViewEventTypeCell {
	width: 90px;
	text-align: center;
}
.EventLogViewMachineNameCell {
	width: 80px;
	text-align: center;
}
.EventLogViewFilter {
	display: inline;
	float: left;
	clear: none;
	border: Solid 1px #212121;
	padding: 2px 6px 2px 6px;
	background-color: #33FF99;
}
.EventLogViewDelete {
	display: inline;
	float: left;
	clear: right;
	border: Solid 1px #212121;
	padding: 2px 6px 2px 6px;
	margin-left: 4px;
	background-color: #CC66FF;
}
-->
</style>
"
, false);
                this.CssClass = "EventLogView";
                this.EventIDCellCssClass = "EventLogViewEventIDCell";
                this.DeleteCellCssClass = "EventLogViewDelete";
                this.FilterCssClass = "EventLogViewFilter";
                this.AppLocationCellCssClass = "EventLogViewAppLocationCell";
                this.MessageCellCssClass = "EventLogViewMessageCell";
                this.EventDateCellCssClass = "EventLogViewEventDateCell";
                this.EventTypeCellCssClass = "EventLogViewEventTypeCell";
                this.MachineNameCellCssClass = "EventLogViewMachineNameCell";
                this.PagerCssClass = "EventLogViewPager";
                this.PagerCellCssClass = "EventLogViewPagerCell";
                this.PagerSelectedCellCssClass = "EventLogViewPagerCellSelected";
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
                            + ((i == 0) ? ("") : ("<a onclick=\"" + Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this, (i - 1).ToString())) + "\">&lt; previous</a>&nbsp;|&nbsp;"))
                            + "<a onclick=\"" + Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this, (i + 1).ToString())) + "\">next &gt;</a>";
                        cellClass = (string.IsNullOrEmpty(PagerSelectedCellCssClass) ? ("") : (@" class=""" + PagerSelectedCellCssClass + @""""));
                    }
                    else
                    {
                        cellClass = (string.IsNullOrEmpty(PagerCellCssClass) ? ("") : (@" class=""" + PagerCellCssClass + @""""));
                    }

                    _PagerRowHTML += @"<td" + cellClass
                        + @" onclick=""" + Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this, i.ToString())) + @""">"
                        + ((i * pageSize) + 1).ToString() + "&nbsp;-&nbsp;" + (((i + 1) * pageSize)).ToString() + @"</td>";
                    maxItemCounter++;
                }

                // add last pager
                if ((pageCount - 1) == this.PageIndex)
                {
                    _PageInfoHMTL = "Showing results " + (((pageCount - 1) * pageSize) + 1).ToString() + "&nbsp;-&nbsp;" + _TotalRowCount.ToString() + " from " + _TotalRowCount.ToString() + " records."
                        + ((pageCount < 2) ? ("") : ("<a onclick=\"" + Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this, (pageCount - 2).ToString())) + "\">&lt; previous</a>&nbsp;&nbsp;"));
                    cellClass = (string.IsNullOrEmpty(PagerSelectedCellCssClass) ? ("") : (@" class=""" + PagerSelectedCellCssClass + @""""));
                }
                else
                {
                    cellClass = (string.IsNullOrEmpty(PagerCellCssClass) ? ("") : (@" class=""" + PagerCellCssClass + @""""));
                }
                _PagerRowHTML += (maxItemCounter >= MaxPagerItemsPerRow ? "</tr><tr>" : "") + @"<td" + cellClass
                    + @" onclick=""" + Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this, (pageCount - 1).ToString())) + @""">" + (((pageCount - 1) * pageSize) + 1).ToString()
                    + "&nbsp;-&nbsp;" + _TotalRowCount.ToString() + @"</td></tr></table>";
            }
            else
            {
                _ShowPager = false;
            }
            #endregion

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
                    sb.Append("<b>EventLogView</b>");
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
                    _DDLEventTypes.RenderControl(writer);
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
                    sb.Append("<div style='display: block; float: left; clear: both;'><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><th>EventID</th><th>AppLocation</th><th>Message</th><th>EventType</th><th>EventDate</th><th>MachineName</th></tr>");
                    foreach (LogDatasets.EventLogRow dr in _EventLogTable)
                    {
                        sb.Append("<tr>");
                        if (this.ShowEventID)
                            sb.Append("<td" + (string.IsNullOrEmpty(this.EventIDCellCssClass) ? ("") : (" class=\"" + this.EventIDCellCssClass + "\"")) + ">" + dr.EventID.ToString() + @"</td>");
                        if (this.ShowAppLocation)
                            sb.Append("<td" + (string.IsNullOrEmpty(this.AppLocationCellCssClass) ? ("") : (" class=\"" + this.AppLocationCellCssClass + "\"")) + ">" + (_ApplicationLocations.ContainsKey(dr.AppLocation) ? _ApplicationLocations[dr.AppLocation] : dr.AppLocation.ToString()) + @"</td>");
                        sb.Append(@"<td" + (string.IsNullOrEmpty(this.MessageCellCssClass) ? ("") : (" class=\"" + this.MessageCellCssClass + "\"")) + ">" + dr.Message + @"</td>");
                        if (this.ShowEventType)
                            sb.Append("<td" + (string.IsNullOrEmpty(this.EventTypeCellCssClass) ? ("") : (" class=\"" + this.EventTypeCellCssClass + "\"")) + ">" + (_EventTypes.ContainsKey(dr.EventType) ? _EventTypes[dr.EventType] : dr.EventType.ToString()) + @"</td>");
                        sb.Append(@"<td" + (string.IsNullOrEmpty(this.EventDateCellCssClass) ? ("") : (" class=\"" + this.EventDateCellCssClass + "\"")) + ">" + dr.EventDate.ToString("dd.MM.yy HH:mm:ss.fff") + @"</td>");
                        if (this.ShowMachineName)
                            sb.Append("<td" + (string.IsNullOrEmpty(this.MachineNameCellCssClass) ? ("") : (" class=\"" + this.MachineNameCellCssClass + "\"")) + ">" + dr.MachineName + @"</td>");
                        sb.Append("</tr>");
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
                if (eventArgument == DELETE_RECORDS_ARGUMENT)
                {// remove clicked...                    
                    LogManager.DeleteEventByDate(int.Parse(_DDLRemoveOlder.SelectedValue));
                }
                else
                {
                    this.PageIndex = int.Parse(eventArgument);
                }
            }
        }

        #endregion
    }
}
