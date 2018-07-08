using System;
using System.Text;

namespace AsciiChart.Sharp.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var series = new double[100];
            for (var i = 0; i < series.Length; i++)
            {
                series[i] = 15 * Math.Sin(i * ((Math.PI * 4) / series.Length));
            }
            
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(AsciiChart.Plot(series));

            Console.WriteLine();

            Console.WriteLine(AsciiChart.Plot(series, new Options
            {
                AxisLabelRightMargin = 0,
                Height = 4,
                Fill = '░',
                AxisLabelWidth = 10,
                AxisLabelFormat = "0,000.000"
            }));
        }
    }
}
