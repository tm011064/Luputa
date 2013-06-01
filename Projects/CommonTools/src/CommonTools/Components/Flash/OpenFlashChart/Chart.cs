using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CommonTools.Web.JavaScript;
using System.IO;
using System.Globalization;

namespace CommonTools.Components.Flash.OpenFlashChart
{
    /// <summary>
    /// 
    /// </summary>
    public class Chart
    {
        #region members
        private decimal _MinValueY = 0
                        , _MaxValueY = 0;
        private int _ColumnsX = 1
                    , _ColumnsY = 1
                    , _MaxValueCountX = 1
                    , _MinValueYFloored = 0
                    , _MaxValueYCeiled = 0;
        #endregion

        #region properties
        /// <summary>
        /// Gets or sets the chart base.
        /// </summary>
        /// <value>The chart base.</value>
        [JsonProperty("elements")]
        public List<ChartBase> ChartBase { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [JsonProperty("title")]
        public ChartStylableTextElement Title { get; set; }
        /// <summary>
        /// Gets or sets the x_ axis.
        /// </summary>
        /// <value>The x_ axis.</value>
        [JsonProperty("x_axis")]
        public XAxis X_Axis { get; set; }
        /// <summary>
        /// Gets or sets the y_ axis.
        /// </summary>
        /// <value>The y_ axis.</value>
        [JsonProperty("y_axis")]
        public YAxis Y_Axis { get; set; }
        /// <summary>
        /// Gets or sets the y_ axis_ right.
        /// </summary>
        /// <value>The y_ axis_ right.</value>
        [JsonProperty("y_axis_right")]
        public YAxis Y_Axis_Right { get; set; }
        /// <summary>
        /// Gets or sets the legend X.
        /// </summary>
        /// <value>The legend X.</value>
        [JsonProperty("x_legend")]
        public ChartStylableTextElement LegendX { get; set; }
        /// <summary>
        /// Gets or sets the legend Y.
        /// </summary>
        /// <value>The legend Y.</value>
        [JsonProperty("y_legend")]
        public ChartStylableTextElement LegendY { get; set; }
        /// <summary>
        /// Gets or sets the bgcolor.
        /// </summary>
        /// <value>The bgcolor.</value>
        [JsonProperty("bg_colour")]
        public string Bgcolor { get; set; }
        /// <summary>
        /// Gets or sets the tool tip.
        /// </summary>
        /// <value>The tool tip.</value>
        [JsonProperty("tooltip")]
        public ChartTextElement ToolTip { get; set; }
        #endregion

        #region public methods
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            StringWriter sw = new StringWriter(CultureInfo.InvariantCulture);
            using (JsonWriter writer = new JsonWriter(sw))
            {
                writer.SkipNullValue = true;
                writer.Write(this);
            }
            return sw.ToString();
        }
        /// <summary>
        /// Toes the debug string.
        /// </summary>
        /// <returns></returns>
        public string ToDebugString()
        {
            StringWriter sw = new StringWriter(CultureInfo.InvariantCulture);
            using (JsonWriter writer = new JsonWriter(sw))
            {
                writer.SkipNullValue = true;
                writer.PrettyPrint = true;
                writer.Write(this);
            }
            return sw.ToString();
        }

        /// <summary>
        /// Sets the X axis labels.
        /// </summary>
        /// <param name="labels">The labels.</param>
        public void SetXAxisLabels(List<AxisLabel> labels)
        {
            int totalLabels = labels.Count;
            int columns = ((int)Math.Round((decimal)_MaxValueCountX / (decimal)(totalLabels - 1)));
            int labelIndex = 0;

            X_Axis.Labels.AxisLabelValues = new List<AxisLabel>();
            for (int i = 0; i < _MaxValueCountX; i++)
            {
                if (i == _MaxValueCountX - 1)
                {
                    X_Axis.Labels.AxisLabelValues.Add(labels[labels.Count - 1]);
                }
                else
                {
                    if (i % columns == 0 && labelIndex < totalLabels)
                    {
                        X_Axis.Labels.AxisLabelValues.Add(labels[labelIndex]);
                        labelIndex++;
                    }
                    else
                    {
                        X_Axis.Labels.AxisLabelValues.Add(new AxisLabel(false));
                    }
                }
            }
        }
        /// <summary>
        /// Sets the Y axis labels.
        /// </summary>
        /// <param name="labels">The labels.</param>
        public void SetYAxisLabels(List<string> labels)
        {
            int totalLabels = labels.Count;

            int columns = ((int)Math.Round((_MaxValueYCeiled - _MinValueYFloored) / (decimal)(totalLabels - 1)));
            int labelIndex = 0;
            int steps = _MaxValueYCeiled - _MinValueYFloored;
            Y_Axis.AxisLabelValues = new List<string>();
            for (int i = 0; i < steps; i++)
            {
                if (i == steps - 1)
                {
                    Y_Axis.AxisLabelValues.Add(labels[labels.Count - 1]);
                }
                else
                {
                    if (i % columns == 0 && labelIndex < totalLabels)
                    {
                        Y_Axis.AxisLabelValues.Add(labels[labelIndex]);
                        labelIndex++;
                    }
                    else
                    {
                        Y_Axis.AxisLabelValues.Add(string.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the columns X.
        /// </summary>
        /// <param name="totalColumns">The total columns.</param>
        public void SetColumnsX(int totalColumns)
        {
            _ColumnsX = totalColumns;
            X_Axis.Steps = ((int)Math.Round((decimal)_MaxValueCountX / (decimal)_ColumnsX));
        }
        /// <summary>
        /// Sets the columns Y.
        /// </summary>
        /// <param name="totalColumns">The total columns.</param>
        public void SetColumnsY(int totalColumns)
        {
            _ColumnsY = totalColumns;
            Y_Axis.Steps = ((int)Math.Ceiling(((decimal)_MaxValueYCeiled - (decimal)_MinValueYFloored) / (decimal)_ColumnsY));
        }
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Chart"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="toolTip">The tool tip.</param>
        /// <param name="chart">The chart.</param>
        public Chart(ChartStylableTextElement title, string toolTip, ChartBase chart)
            : this(title, null, null, toolTip, new List<ChartBase>() { chart }, 10, 10) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Chart"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="toolTip">The tool tip.</param>
        /// <param name="charts">The charts.</param>
        public Chart(ChartStylableTextElement title, string toolTip, List<ChartBase> charts)
            : this(title, null, null, toolTip, charts, 10, 10) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Chart"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="legendX">The legend X.</param>
        /// <param name="legendY">The legend Y.</param>
        /// <param name="toolTip">The tool tip.</param>
        /// <param name="chart">The chart.</param>
        public Chart(ChartStylableTextElement title, ChartStylableTextElement legendX, ChartStylableTextElement legendY, string toolTip, ChartBase chart)
            : this(title, legendX, legendY, toolTip, new List<ChartBase>() { chart }, 10, 10) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Chart"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="legendX">The legend X.</param>
        /// <param name="legendY">The legend Y.</param>
        /// <param name="toolTip">The tool tip.</param>
        /// <param name="charts">The charts.</param>
        /// <param name="xAxisColumns">The x axis columns.</param>
        /// <param name="yAxisColumns">The y axis columns.</param>
        public Chart(ChartStylableTextElement title, ChartStylableTextElement legendX, ChartStylableTextElement legendY
            , string toolTip, List<ChartBase> charts, int xAxisColumns, int yAxisColumns)
        {
            this.Title = title;
            this.LegendX = legendX;
            this.LegendY = legendY;
            this.ToolTip = new ChartTextElement(toolTip);

            this.ChartBase = charts;
            this.Y_Axis = new YAxis();
            this.X_Axis = new XAxis();

            _MinValueY = charts.SelectMany(c => c.Values).Min(c => c.Value);
            _MaxValueY = charts.SelectMany(c => c.Values).Max(c => c.Value);
            _ColumnsX = xAxisColumns;
            _ColumnsY = yAxisColumns;
            _MaxValueCountX = (from c in charts
                               select c.Values.Count).Max();

            X_Axis.Steps = ((int)Math.Round((decimal)_MaxValueCountX / (decimal)_ColumnsX));


            _MinValueYFloored = (int)Math.Floor(_MinValueY);
            _MaxValueYCeiled = (int)Math.Ceiling(_MaxValueY);
            Y_Axis.SetRange(_MinValueYFloored, _MaxValueYCeiled);
            Y_Axis.Steps = ((int)Math.Ceiling(((decimal)_MaxValueYCeiled - (decimal)_MinValueYFloored) / (decimal)_ColumnsY));

        }
        #endregion
    }
}
