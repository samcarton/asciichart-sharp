using System;
using System.Collections.Generic;
using System.Linq;

namespace AsciiChart.Sharp
{
    public static class AsciiChart
    {
        const int NumberOfNonDataColumns = 2;

        /// <summary>
        /// Plot the series.
        /// </summary>
        /// <param name="series">The series to plot.</param>
        /// <param name="options">The plot options.</param>
        /// <returns>The ASCII Chart.</returns>
        public static string Plot(IEnumerable<double> series, Options options = null)
        {
            options = options ?? new Options();

            var seriesList = series.ToList();
            var min = seriesList.Min();
            var max = seriesList.Max();

            var range = Math.Abs(max - min);
            var ratio = ((options.Height) ?? range) / range;
            var min2 = Math.Round(min * ratio, MidpointRounding.AwayFromZero);
            var max2 = Math.Round(max * ratio, MidpointRounding.AwayFromZero);
            var rows = Math.Abs(max2 - min2);
            
            var columnIndexOfFirstDataPoint = options.AxisLabelRightMargin + NumberOfNonDataColumns;
            var width = seriesList.Count + columnIndexOfFirstDataPoint;

            var resultArray = CreateAndFill2dArray(rows, width, options.Fill.ToString());

            var yAxisLabels = GetYAxisLabels(max, range, rows, options);
            ApplyYAxisLabels(resultArray, yAxisLabels, columnIndexOfFirstDataPoint);
            
            for (var x = 0; x < seriesList.Count - 1; x++)
            {
                var rowIndex0 = Math.Round(seriesList[x] * ratio, MidpointRounding.AwayFromZero) - min2;
                var rowIndex1 = Math.Round(seriesList[x + 1] * ratio, MidpointRounding.AwayFromZero) - min2;

                if (x == 0)
                {
                    resultArray[(int) (rows - rowIndex0)][columnIndexOfFirstDataPoint - 1] = "┼";
                }

                if (rowIndex0 == rowIndex1)
                {
                    resultArray[(int) (rows - rowIndex0)][x + columnIndexOfFirstDataPoint] = "─";
                }
                else
                {
                    resultArray[(int) (rows - rowIndex1)][x + columnIndexOfFirstDataPoint] = (rowIndex0 > rowIndex1) ? "╰" : "╭";
                    resultArray[(int) (rows - rowIndex0)][x + columnIndexOfFirstDataPoint] = (rowIndex0 > rowIndex1) ? "╮" : "╯";
                    var from = Math.Min(rowIndex0, rowIndex1);
                    var to = Math.Max(rowIndex0, rowIndex1);
                    for (var y = from + 1; y < to; y++)
                    {
                        resultArray[(int) (rows - y)][x + columnIndexOfFirstDataPoint] = "│";
                    }
                }
            }

            return ToString(resultArray);
        }

        static string[][] CreateAndFill2dArray(double rows, int width, string fill)
        {
            var array = new string[((int)rows+1)][];
            for (var i = 0; i <= rows; i++)
            {
                array[i] = new string[width];
                for (var j = 0; j < width; j++)
                {
                    array[i][j] = fill;
                }
            }

            return array;
        }

        static IReadOnlyList<AxisLabel> GetYAxisLabels(double max, double range, double rows, Options options)
        {
            var yAxisTicks = GetYAxisTicks(max, range, rows);
            var labels = yAxisTicks.Select(tick => new AxisLabel(tick, options.AxisLabelFormat)).ToList();

            var maxLabelLength = labels.Max(label => label.Label.Length) + options.AxisLabelLeftMargin;
            foreach (var label in labels)
            {
                label.LeftPad = maxLabelLength;
            }

            return labels;
        }

        static IReadOnlyList<double> GetYAxisTicks(double max, double range, double rows)
        {
            var numberOfTicks = rows + 1;
            var yTicks = new List<double>();
            for (var i = 0; i < numberOfTicks; i++)
            {
                yTicks.Add(max - i * range/rows);
            }

            return yTicks;
        }

        static void ApplyYAxisLabels(string[][] resultArray, IReadOnlyList<AxisLabel> yAxisLabels, int columnIndexOfFirstDataPoint)
        {
            for (var i = 0; i < yAxisLabels.Count; i++)
            {
                resultArray[i][0] = yAxisLabels[i].Label;
                resultArray[i][columnIndexOfFirstDataPoint - 1] = (Math.Abs(yAxisLabels[i].Value) < 0.001) ? "┼" : "┤";
            }
        }

        static string ToString(string[][] resultArray)
        {
            var rowStrings = resultArray.Select(row => String.Join("", row));
            return String.Join(Environment.NewLine, rowStrings);
        }

        class AxisLabel
        {
            readonly string _format;

            public AxisLabel(double value, string format)
            {
                _format = format;
                Value = value;
            }

            public double Value { get; }
            public int LeftPad { get; set; } = 0;
            public string Label => Value.ToString(_format).PadLeft(LeftPad);
        }
    }
}