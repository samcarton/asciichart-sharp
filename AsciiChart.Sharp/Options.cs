using System;

namespace AsciiChart.Sharp
{
    public class Options
    {
        int _axisLabelLeftMargin = 1;
        int _axisLabelRightMargin = 1;

        /// <summary>
        /// The margin between the axis label and the left of the output.
        /// </summary>
        public int AxisLabelLeftMargin
        {
            get => _axisLabelLeftMargin;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Margin must be >= 0");
                }
                _axisLabelLeftMargin = value;
            }
        }

        /// <summary>
        /// The margin between the axis label and the axis.
        /// </summary>
        public int AxisLabelRightMargin
        {
            get => _axisLabelRightMargin;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Margin must be >= 0");
                }
                _axisLabelRightMargin = value;
            }
        }

        /// <summary>
        /// Roughly the number of lines to scale the output to.
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// The background fill.
        /// </summary>
        public char Fill { get; set; } = ' ';

        /// <summary>
        /// The axis label format.
        /// </summary>
        public string AxisLabelFormat { get; set; } = "0.00";

        /// <summary>
        /// The axis color.
        /// </summary>
        public AnsiColor AxisColor { get; set; }

        /// <summary>
        /// The axis label color.
        /// </summary>
        public AnsiColor LabelColor { get; set; }

        /// <summary>
        /// The color of each series.
        /// </summary>
        public AnsiColor[] SeriesColors { get; set; }
    }
}