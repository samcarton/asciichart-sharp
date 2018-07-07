using System;
using System.Text;

namespace AsciiChart.Sharp.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var series = new [] {1,5,3,4,5,4,3,2.5,2,1,0,-1 };
            var output = AsciiChart.Plot(series);
            
            Console.OutputEncoding = Encoding.UTF8;
            Console.Write(output);
        }
    }
}
