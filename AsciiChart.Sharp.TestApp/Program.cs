using System;
using System.Linq;
using System.Text;

namespace AsciiChart.Sharp.TestApp
{
    static class Program
    {
        static void Main()
        {
            var series = new double[100];
            for (var i = 0; i < series.Length; i++)
            {
                series[i] = 15 * Math.Sin(i * (Math.PI * 4 / series.Length));
            }

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(AsciiChart.Plot(series));

            Console.WriteLine();

            Console.WriteLine(AsciiChart.Plot(series, new Options
            {
                AxisLabelLeftMargin = 3,
                AxisLabelRightMargin = 0,
                Height = 4,
                Fill = '·',
                AxisLabelFormat = "0,000.000"
            }));

            var series2 = new double[100];
            for (var i = 0; i < series.Length; i++)
            {
                series2[i] = 200000 * Math.Cos(i * (Math.PI * 8 / series.Length)) + 1000000;
            }

            Console.WriteLine();
            Console.WriteLine(AsciiChart.Plot(series2, new Options{Height = 10}));

            Console.WriteLine();
            Console.WriteLine(AsciiChart.Plot(
                Enumerable.Range(0, 6).Select(i => Enumerable.Range(-40, 81).Select(x => Math.Abs(x) > 40 - i ? double.NaN : Math.Sqrt((40 - i) * (40 - i) - x * x) / 2)),
                new Options
                {
                    AxisLabelFormat = "0",
                    SeriesColors = new[]
                    {
                        AnsiColor.Red,
                        AnsiColor.Orange,
                        AnsiColor.Yellow,
                        AnsiColor.Green,
                        AnsiColor.Blue,
                        AnsiColor.Purple,
                    }
                }));
        }
    }
}