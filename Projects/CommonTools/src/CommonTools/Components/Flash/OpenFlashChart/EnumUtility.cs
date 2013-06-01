using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.Components.Flash.OpenFlashChart
{
    internal static class EnumUtility
    {
        /// <summary>
        /// Gets the chart type string.
        /// </summary>
        /// <param name="chartType">Type of the chart.</param>
        /// <returns></returns>
        internal static string GetChartTypeString(ChartType chartType)
        {
            switch (chartType)
            {
                case ChartType.Pie:
                    return "pie";
                case ChartType.Line:
                    return "line";
                case ChartType.LineDot:
                    return "line_dot";
                case ChartType.LineHollow:
                    return "line_hollow";
            }

            throw new NotImplementedException("ChartType" + chartType.ToString() + " not recognized");
        }
        /// <summary>
        /// Gets the orientation string.
        /// </summary>
        /// <param name="orientation">The orientation.</param>
        /// <returns></returns>
        internal static string GetOrientationString(Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.Horizontal:
                    return "horizontal";
                case Orientation.Vertical:
                    return "vertical";
                case Orientation.Diagonal:
                    return "diagonal";
            }

            throw new NotImplementedException("Orientation" + orientation.ToString() + " not recognized");
        }

        /// <summary>
        /// Gets the type of the chart type from line chart.
        /// </summary>
        /// <param name="lineChartType">Type of the line chart.</param>
        /// <returns></returns>
        internal static ChartType GetChartTypeFromLineChartType(LineChartType lineChartType)
        {
            return (ChartType)Enum.Parse(typeof(ChartType), lineChartType.ToString(), true);
        }


    }
}
