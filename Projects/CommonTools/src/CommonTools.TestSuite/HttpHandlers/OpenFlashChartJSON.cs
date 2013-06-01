using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Drawing;
using CommonTools.Components.Flash.OpenFlashChart.Charts;
using CommonTools.Components.Flash.OpenFlashChart;

namespace CommonTools.TestSuite.HttpHandlers
{
    public class OpenFlashChartJSON : IHttpHandler
    {
        private string GetRandomColour(Random random)
        {
            return ColorTranslator.ToHtml(Color.FromArgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256)));
        }

        private Chart GetPieChart()
        {
            PieChart pieChart = new PieChart(
                new List<IChartValue>()
                {
                    new PieChartValue(2m, "two")
                    , new PieChartValue(3m, "three")
                    , new PieChartValue(4m, "MIDDLE")
                    , new PieChartValue(7m, "somethingelse")            
                });
            pieChart.Tip = TipConstants.VALUE + " of " + TipConstants.TOTAL + "\n" + TipConstants.PERCENT + " of 100%";
            pieChart.Text = "this is a test";

            return new Chart(
                new ChartStylableTextElement("my title")
                , new ChartStylableTextElement("my x legend")
                , new ChartStylableTextElement("my y legend")
                , "hover over me"
                , pieChart);
        }

        private Chart GetLineChart()
        {
            List<ChartBase> manyCharts = new List<ChartBase>();

            List<IChartValue> values;
            Random random = new Random();
            for (int i = 0; i < 3; i++)
            {
                values = new List<IChartValue>();

                for (int j = 0; j < 20; j++)
                    values.Add(new ChartValueBase((decimal)random.Next(80, 110) + (decimal)random.NextDouble()));

                manyCharts.Add(new LineChart(LineChartType.LineDot, values, 0, 0, 0)
                {
                    Width = random.Next(1, 3),
                    Colour = GetRandomColour(random),
                    Tip = @"Value: " + TipConstants.VALUE// #percent# of 100%"
                });

            }

            Chart chart = new Chart(
                null
                , "hover over me"
                , manyCharts);

            chart.SetColumnsX(3);
            chart.SetColumnsY(1);
            chart.SetXAxisLabels(new List<AxisLabel>()
            {          
                new AxisLabel("16-Dec", GetRandomColour(random), 12, true),
                new AxisLabel("21-Mar", GetRandomColour(random), 12, true),
                new AxisLabel("25-Jun", GetRandomColour(random), 12, true),
                new AxisLabel("", null, 0, true)
            });
            chart.SetYAxisLabels(new List<string>() { "80", "110" });

            chart.Bgcolor = "#FFFFFF";
            chart.Y_Axis.GridColour = "#C1C1C1";
            chart.X_Axis.GridColour = "#C1C1C1";
            chart.X_Axis.Colour = "#000000";
            chart.Y_Axis.Colour = "#000000";
            chart.Y_Axis.Stroke = 1;
            chart.X_Axis.Stroke = 1;
            chart.X_Axis.TickHeight = 4;
            chart.X_Axis.Offset = true;
            chart.Y_Axis.Offset = 2;

            return chart;
        }

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write(GetPieChart().ToDebugString());
            context.Response.Write(GetLineChart().ToDebugString());
        }

        #endregion
    }
}
