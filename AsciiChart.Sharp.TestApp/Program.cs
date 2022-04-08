using System;
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
        }
    }
}