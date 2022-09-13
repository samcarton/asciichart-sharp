using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AsciiChart.Sharp.Tests
{
    public class Tests
    {
        static readonly Regex _normalize = new Regex(@" *(\r\n|\n\r|\n|\r)", RegexOptions.Compiled);

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void TestPlot(double[] series, Options opts, string expected)
        {
            var actual = AsciiChart.Plot(series, opts);
            Assert.AreEqual(Normalize(expected), Normalize(actual));
        }

        static object[][] Cases =
        {
            new object[] { new double[] { 1 }, null, " 1.00 ┼ " },
            new object[] { new double[] { 1, 1, 1, 1, 1 }, null, " 1.00 ┼──── " },
            new object[] { new double[] { 0, 0, 0, 0, 0 }, null, " 0.00 ┼──── " },
            new object[]
            {
                new double[] { 2, 1, 1, 2, -2, 5, 7, 11, 3, 7, 1 }, null, @"
 11.00 ┤      ╭╮
 10.00 ┤      ││
  9.00 ┤      ││
  8.00 ┤      ││
  7.00 ┤     ╭╯│╭╮
  6.00 ┤     │ │││
  5.00 ┤    ╭╯ │││
  4.00 ┤    │  │││
  3.00 ┤    │  ╰╯│
  2.00 ┼╮ ╭╮│    │
  1.00 ┤╰─╯││    ╰
  0.00 ┤   ││
 -1.00 ┤   ││
 -2.00 ┤   ╰╯"
            },
            new object[]
            {
                new double[] { 2, 1, 1, 2, -2, 5, 7, 11, 3, 7, 4, 5, 6, 9, 4, 0, 6, 1, 5, 3, 6, 2 }, null, @"
 11.00 ┤      ╭╮
 10.00 ┤      ││
  9.00 ┤      ││    ╭╮
  8.00 ┤      ││    ││
  7.00 ┤     ╭╯│╭╮  ││
  6.00 ┤     │ │││ ╭╯│ ╭╮  ╭╮
  5.00 ┤    ╭╯ │││╭╯ │ ││╭╮││
  4.00 ┤    │  ││╰╯  ╰╮││││││
  3.00 ┤    │  ╰╯     ││││╰╯│
  2.00 ┼╮ ╭╮│         ││││  ╰
  1.00 ┤╰─╯││         ││╰╯
  0.00 ┤   ││         ╰╯
 -1.00 ┤   ││
 -2.00 ┤   ╰╯"
            },
            new object[]
            {
                new[] { 0.2, 0.1, 0.2, 2, -0.9, 0.7, 0.91, 0.3, 0.7, 0.4, 0.5 }, null, @"
  2.00 ┤  ╭╮
  1.03 ┤  ││╭─╮╭╮╭
  0.07 ┼──╯││ ╰╯╰╯
 -0.90 ┤   ╰╯"
            },
            new object[]
            {
                new double[] { 2, 1, 1, 2, -2, 5, 7, 11, 3, 7, 1 }, new Options { Height = 4 }, @"
 11.00 ┤      ╭╮
  7.75 ┤    ╭─╯│╭╮
  4.50 ┼╮ ╭╮│  ╰╯│
  1.25 ┤╰─╯││    ╰
 -2.00 ┤   ╰╯"
            },
            new object[]
            {
                new[] { 0.453, 0.141, 0.951, 0.251, 0.223, 0.581, 0.771, 0.191, 0.393, 0.617, 0.478 }, new Options { Height = 8 }, @"
 0.95 ┤ ╭╮
 0.85 ┤ ││  ╭╮
 0.75 ┤ ││  ││
 0.65 ┤ ││ ╭╯│ ╭╮
 0.55 ┤ ││ │ │ │╰
 0.44 ┼╮││ │ │╭╯
 0.34 ┤│││ │ ││
 0.24 ┤││╰─╯ ╰╯
 0.14 ┤╰╯"
            },
            new object[]
            {
                new[] { 0.01, 0.004, 0.003, 0.0042, 0.0083, 0.0033, 0.0079 }, new Options { AxisLabelFormat = "0.000", Height = 7 }, @"
 0.010 ┼╮
 0.009 ┤│
 0.008 ┤│  ╭╮╭
 0.007 ┤│  │││
 0.006 ┤│  │││
 0.005 ┤│  │││
 0.004 ┤╰╮╭╯││
 0.003 ┤ ╰╯ ╰╯"
            },
            new object[]
            {
                new double[] { 192, 431, 112, 449, -122, 375, 782, 123, 911, 1711, 172 }, new Options { AxisLabelFormat = "0", Height = 10 }, @"
 1711 ┤        ╭╮
 1528 ┤        ││
 1344 ┤        ││
 1161 ┤        ││
  978 ┤       ╭╯│
  795 ┤     ╭╮│ │
  611 ┤     │││ │
  428 ┤╭╮╭╮╭╯││ │
  245 ┼╯╰╯││ ╰╯ ╰
   61 ┤   ││
 -122 ┤   ╰╯"
            },
            new object[]
            {
                new[]
                {
                    0, 0, 0, 0, 1.5, 0, 0, -0.5, 9, -3, 0, 0, 1, 2, 1, 0, 0, 0, 0,
                    0, 0, 0, 0, 1.5, 0, 0, -0.5, 8, -3, 0, 0, 1, 2, 1, 0, 0, 0, 0,
                    0, 0, 0, 0, 1.5, 0, 0, -0.5, 10, -3, 0, 0, 1, 2, 1, 0, 0, 0, 0
                },
                new Options { AxisLabelRightMargin = 4, Height = 10 },
                @"
 10.00    ┤                                             ╭╮
  8.70    ┤       ╭╮                                    ││
  7.40    ┤       ││                 ╭╮                 ││
  6.10    ┤       ││                 ││                 ││
  4.80    ┤       ││                 ││                 ││
  3.50    ┤       ││                 ││                 ││
  2.20    ┤       ││   ╭╮            ││   ╭╮            ││   ╭╮
  0.90    ┤   ╭╮  ││  ╭╯╰╮       ╭╮  ││  ╭╯╰╮       ╭╮  ││  ╭╯╰╮
 -0.40    ┼───╯╰──╯│╭─╯  ╰───────╯╰──╯│╭─╯  ╰───────╯╰──╯│╭─╯  ╰───
 -1.70    ┤        ││                 ││                 ││
 -3.00    ┤        ╰╯                 ╰╯                 ╰╯"
            },
            new object[]
            {
                new double[] { -5, -2, -3, -4, 0, -5, -6, -7, -8, 0, -9, -3, -5, -2, -9, -3, -1 }, null, @"
  0.00 ┤   ╭╮   ╭╮
 -1.00 ┤   ││   ││     ╭
 -2.00 ┤╭╮ ││   ││  ╭╮ │
 -3.00 ┤│╰╮││   ││╭╮││╭╯
 -4.00 ┤│ ╰╯│   │││││││
 -5.00 ┼╯   ╰╮  │││╰╯││
 -6.00 ┤     ╰╮ │││  ││
 -7.00 ┤      ╰╮│││  ││
 -8.00 ┤       ╰╯││  ││
 -9.00 ┤         ╰╯  ╰╯"
            },
            new object[]
            {
                new[]
                {
                    57.76, 54.04, 56.31, 57.02, 59.5, 52.63, 52.97, 56.44, 56.75, 52.96, 55.54, 55.09, 58.22, 56.85, 60.61, 59.62, 59.73, 59.93, 56.3, 54.69, 55.32, 54.03, 50.98, 50.48, 54.55, 47.49,
                    55.3, 46.74, 46, 45.8, 49.6, 48.83, 47.64, 46.61, 54.72, 42.77, 50.3, 42.79, 41.84, 44.19, 43.36, 45.62, 45.09, 44.95, 50.36, 47.21, 47.77, 52.04, 47.46, 44.19, 47.22, 45.55,
                    40.65, 39.64, 37.26, 40.71, 42.15, 36.45, 39.14, 36.62
                },
                new Options { Height = 24 },
                @"
 60.61 ┤             ╭╮ ╭╮
 59.60 ┤   ╭╮        │╰─╯│
 58.60 ┤   ││      ╭╮│   │
 57.59 ┼╮ ╭╯│      │││   │
 56.58 ┤│╭╯ │ ╭─╮  │╰╯   ╰╮
 55.58 ┤││  │ │ │╭─╯      │╭╮    ╭╮
 54.57 ┤╰╯  │ │ ││        ╰╯╰╮ ╭╮││      ╭╮
 53.56 ┤    │╭╯ ╰╯           │ ││││      ││
 52.56 ┤    ╰╯               │ ││││      ││           ╭╮
 51.55 ┤                     ╰╮││││      ││           ││
 50.54 ┤                      ╰╯│││      ││╭╮      ╭╮ ││
 49.54 ┤                        │││  ╭─╮ ││││      ││ ││
 48.53 ┤                        │││  │ │ ││││      ││ ││
 47.52 ┤                        ╰╯│  │ ╰╮││││      │╰─╯╰╮╭╮
 46.52 ┤                          ╰─╮│  ╰╯│││      │    │││
 45.51 ┤                            ╰╯    │││   ╭──╯    ││╰╮
 44.50 ┤                                  │││ ╭╮│       ╰╯ │
 43.50 ┤                                  ││╰╮│╰╯          │
 42.49 ┤                                  ╰╯ ╰╯            │   ╭╮
 41.48 ┤                                                   │   ││
 40.48 ┤                                                   ╰╮ ╭╯│
 39.47 ┤                                                    ╰╮│ │╭╮
 38.46 ┤                                                     ││ │││
 37.46 ┤                                                     ╰╯ │││
 36.45 ┤                                                        ╰╯╰"
            },
            new object[] { new[] { 1, 1, double.NaN, 1, 1 }, null, @" 1.00 ┼─╴╶─ " },
            new object[] { new[] { double.NaN, 1 }, null, @" 1.00 ┤╶ " },
            new object[]
            {
                new[] { 0, 0, 1, 1, double.NaN, double.NaN, 3, 3, 4 }, null, @"
 4.00 ┤       ╭
 3.00 ┤     ╶─╯
 2.00 ┤
 1.00 ┤ ╭─╴
 0.00 ┼─╯"
            },
            new object[]
            {
                new[] { 0.1, 0.2, 0.3, double.NaN, 0.5, 0.6, 0.7, double.NaN, double.NaN, 0.9, 1 }, new Options { Height = 9 }, @"
 1.00 ┤         ╭
 0.90 ┤        ╶╯
 0.80 ┤
 0.70 ┤     ╭╴
 0.60 ┤    ╭╯
 0.50 ┤   ╶╯
 0.40 ┤
 0.30 ┤ ╭╴
 0.20 ┤╭╯
 0.10 ┼╯"
            }
        };

        [Test]
        [TestCaseSource(nameof(MultiCases))]
        public void TestPlotMulti(double[][] series, Options opts, string expected)
        {
            var actual = AsciiChart.Plot(series, opts);
            Assert.AreEqual(Normalize(expected), Normalize(actual));
        }

        static object[][] MultiCases =
        {
            new object[]
            {
                new[] { new double[] { 0 }, new double[] { 1 }, new double[] { 2 } }, null, @"
 2.00 ┼
 1.00 ┼
 0.00 ┼"
            },
            new object[]
            {
                new[] { new[] { 0, 0, 2, 2, double.NaN }, new double[] { 1, 1, 1, 1, 1, 1, 1 }, new[] { double.NaN, double.NaN, double.NaN, 0, 0, 2, 2 } }, null, @"
 2.00 ┤ ╭─╴╭─
 1.00 ┼────│─
 0.00 ┼─╯╶─╯"
            },
            new object[]
            {
                new[] { new double[] { 0, 0, 0 }, new[] { double.NaN, 0, 0 }, new[] { double.NaN, double.NaN, 0 } }, null, " 0.00 ┼╶╶ "
            }
        };

        static string Normalize(string text) => _normalize.Replace(text.Trim('\r', '\n') + Environment.NewLine, Environment.NewLine);
    }
}