using System;
using System.Collections.Generic;
using System.Linq;

namespace AsciiChart.Sharp
{
    public static class AsciiChart
    {
        public static string Plot(IEnumerable<double> series, Options options = null)
        {
            options = options ?? new Options();

            var seriesList = series.ToList();
            var min = seriesList.Min();
            var max = seriesList.Max();

            var range = Math.Abs(max - min);
            var ratio = (options.Height ?? range) / range;
            var min2 = Math.Round(min * ratio);
            var max2 = Math.Round(max * ratio);
            var rows = Math.Abs(max2 - min2);
            var width = seriesList.Count + options.Offset;

            var result = new string[((int)rows+1)][];
            for (var i = 0; i <= rows; i++)
            {
                result[i] = new string[width];
                for (var j = 0; j < width; j++)
                {
                    result[i][j] = " ";
                }
            }

            for (var y = min2; y <= max2; y++)
            {
                var label = FormatAxisLabel(max - (y - min2) * range / rows, options); // bonus extra arg, y - min2
                result[(int) (y - min2)][Math.Max(options.Offset - label.Length, 0)] = label;
                result[(int) (y - min2)][options.Offset - 1] = (y == 0) ? "┼" : "┤";
            }

            var y0 = Math.Round(seriesList[0] * ratio) - min2;
            result[(int) (rows - y0)][options.Offset - 1] = "┼";


            for (var x = 0; x < seriesList.Count - 1; x++)
            {
                y0 = Math.Round(seriesList[x + 0] * ratio) - min2;
                var y1 = Math.Round(seriesList[x + 1] * ratio) - min2;
                if (y0 == y1)
                {
                    result[(int) (rows - y0)][x + options.Offset] = "─";
                }
                else
                {
                    result[(int) (rows - y1)][x + options.Offset] = (y0 > y1) ? "╰" : "╭";
                    result[(int) (rows - y0)][x + options.Offset] = (y0 > y1) ? "╮" : "╯";
                    var from = Math.Min(y0, y1);
                    var to = Math.Max(y0, y1);
                    for (var y = from + 1; y < to; y++)
                    {
                        result[(int) (rows - y)][x + options.Offset] = "│";
                    }
                }
            }

            var rowStrings = result.Select(row => String.Join("", row));
            return String.Join(Environment.NewLine, rowStrings);
        }

        static string FormatAxisLabel(double d, Options options)
        {
            var axisValue = Math.Round(d, 2).ToString();

            return axisValue.PadLeft(options.Padding);
        }
    }

    public class Options
    {
        public int Offset { get; set; } = 3;
        public int Padding { get; set; } = 11; // todo offer non-whitespace padding
        public int? Height { get; set; }
    }
}