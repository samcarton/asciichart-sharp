using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return Plot(new[] { series }, options);
        }

        public static string Plot(IEnumerable<IEnumerable<double>> data, Options options = null)
        {
            options = options ?? new Options();

            var dataList = data.ToList();
            var min = dataList.SelectMany(s => s).Where(v => !double.IsNaN(v)).Min();
            var max = dataList.SelectMany(s => s).Max();

            var range = Math.Abs(max - min);
            var ratio = range == 0 ? 0 : (options.Height ?? range) / range;
            var min2 = Math.Round(min * ratio, MidpointRounding.AwayFromZero);
            var max2 = Math.Round(max * ratio, MidpointRounding.AwayFromZero);
            var rows = Math.Abs(max2 - min2);

            var columnIndexOfFirstDataPoint = options.AxisLabelRightMargin + NumberOfNonDataColumns;
            var width = dataList.Max(s => s.Count()) + columnIndexOfFirstDataPoint;

            var resultArray = CreateAndFill2dArray(rows, width, options.Fill.ToString());
            var colorArray = Enumerable.Repeat(Enumerable.Repeat(AnsiColor.Default, width), (int)rows + 1).Select(row => row.ToArray()).ToArray();

            var yAxisLabels = GetYAxisLabels(max, range, rows, options);
            ApplyYAxisLabels(resultArray, colorArray, yAxisLabels, columnIndexOfFirstDataPoint, options);

            var i = 0;
            foreach (var series in dataList)
            {
                var color = options.SeriesColors?.Length > i ? options.SeriesColors[i++] : AnsiColor.Default;

                var seriesList = series.ToList();
                var rowIndex0 = Math.Round(seriesList[0] * ratio, MidpointRounding.AwayFromZero) - min2;
                if (!double.IsNaN(rowIndex0))
                {
                    resultArray[(int)(rows - rowIndex0)][columnIndexOfFirstDataPoint - 1] = "┼";
                    colorArray[(int)(rows - rowIndex0)][columnIndexOfFirstDataPoint - 1] = color;
                }

                for (var x = 0; x < seriesList.Count - 1; x++)
                {
                    var rowIndex1 = Math.Round(seriesList[x + 1] * ratio, MidpointRounding.AwayFromZero) - min2;
                    if (double.IsNaN(rowIndex0) && double.IsNaN(rowIndex1))
                    {
                        continue;
                    }

                    if (double.IsNaN(rowIndex0))
                    {
                        resultArray[(int)(rows - rowIndex1)][x + columnIndexOfFirstDataPoint] = "╶";
                    }
                    else if (double.IsNaN(rowIndex1))
                    {
                        resultArray[(int)(rows - rowIndex0)][x + columnIndexOfFirstDataPoint] = "╴";
                    }
                    else if (rowIndex0 == rowIndex1)
                    {
                        resultArray[(int)(rows - rowIndex0)][x + columnIndexOfFirstDataPoint] = "─";
                    }
                    else
                    {
                        resultArray[(int)(rows - rowIndex1)][x + columnIndexOfFirstDataPoint] = rowIndex0 > rowIndex1 ? "╰" : "╭";
                        resultArray[(int)(rows - rowIndex0)][x + columnIndexOfFirstDataPoint] = rowIndex0 > rowIndex1 ? "╮" : "╯";
                        var from = Math.Min(rowIndex0, rowIndex1);
                        var to = Math.Max(rowIndex0, rowIndex1);
                        for (var y = from + 1; y < to; y++)
                        {
                            resultArray[(int)(rows - y)][x + columnIndexOfFirstDataPoint] = "│";
                        }
                    }

                    var lower = double.IsNaN(rowIndex0) ? rowIndex1 : rowIndex0;
                    var upper = double.IsNaN(rowIndex1) ? rowIndex0 : rowIndex1;
                    if (lower > upper)
                    {
                        var tmp = lower;
                        lower = upper;
                        upper = tmp;
                    }

                    for (var y = lower; y <= upper; y++)
                    {
                        colorArray[(int)(rows - y)][x + columnIndexOfFirstDataPoint] = color;
                    }

                    rowIndex0 = rowIndex1;
                }
            }

            return ToString(resultArray, colorArray);
        }

        static string[][] CreateAndFill2dArray(double rows, int width, string fill)
        {
            var array = new string[(int)rows + 1][];
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

        static IEnumerable<double> GetYAxisTicks(double max, double range, double rows)
        {
            var numberOfTicks = rows + 1;
            var yTicks = new List<double>();
            for (var i = 0; i < numberOfTicks; i++)
            {
                yTicks.Add(max - i * (range == rows ? 1 : range/rows));
            }

            return yTicks;
        }

        static void ApplyYAxisLabels(IReadOnlyList<string[]> resultArray, IReadOnlyList<AnsiColor[]> colorArray, IReadOnlyList<AxisLabel> yAxisLabels, int columnIndexOfFirstDataPoint, Options options)
        {
            for (var i = 0; i < yAxisLabels.Count; i++)
            {
                resultArray[i][0] = yAxisLabels[i].Label;
                colorArray[i][0] = options.LabelColor;
                resultArray[i][columnIndexOfFirstDataPoint - 1] = "┤";
                colorArray[i][columnIndexOfFirstDataPoint - 1] = options.AxisColor;
            }
        }

        static string ToString(IReadOnlyList<string[]> resultArray, IReadOnlyList<AnsiColor[]> colorArray)
        {
            var builder = new StringBuilder();
            for (var y = 0; y < resultArray.Count; y++)
            {
                var prev = AnsiColor.Default;
                for (var x = 0; x < resultArray[y].Length; x++)
                {
                    var color = colorArray[y][x];
                    if (color != prev)
                    {
                        builder.Append(ColorString(color));
                        prev = color;
                    }

                    builder.Append(resultArray[y][x]);
                }

                if (y < resultArray.Count - 1)
                {
                    builder.Append(Environment.NewLine);
                }
            }

            return builder.ToString();
        }

        static string ColorString(AnsiColor color)
        {
            if (color == AnsiColor.Default)
            {
                return "\x1b[0m";
            }

            if (color == AnsiColor.Black)
            {
                color = AnsiColor.Default;
            }

            if (color <= AnsiColor.Silver)
            {
                // 3-bit color
                return $"\x1b[{30 + (byte)color}m";
            }

            if (color <= AnsiColor.White)
            {
                // 4-bit color
                return ($"\x1b[{82 + (byte)color}m");
            }

            // 8-bit color
            return ($"\x1b[38;5;{(byte)color}m");
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
            public int LeftPad { get; set; }
            public string Label => Value.ToString(_format).PadLeft(LeftPad);
        }
    }
}