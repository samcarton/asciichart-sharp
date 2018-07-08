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

            for (var y = min2; y <= max2; y++)
            {
                var rowIndex = (int)(y - min2);
                var rowLabel = max - rowIndex * range / rows;
                var label = FormatAxisLabel(rowLabel, options); // bonus extra arg, y - min2
                resultArray[rowIndex][Math.Max(columnIndexOfFirstDataPoint - label.Length, 0)] = label;
                resultArray[rowIndex][columnIndexOfFirstDataPoint - 1] = (Math.Abs(rowLabel) < 0.001) ? "┼" : "┤";
            }
            
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

        static string ToString(string[][] resultArray)
        {
            var rowStrings = resultArray.Select(row => String.Join("", row));
            return String.Join(Environment.NewLine, rowStrings);
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

        static string FormatAxisLabel(double d, Options options)
        {
            var axisValue = d.ToString(options.AxisLabelFormat);

            return axisValue.PadLeft(options.AxisLabelWidth);
        }
    }
}