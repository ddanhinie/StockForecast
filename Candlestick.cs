using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Project_1
{
    public class Candlestick
    {
        /// <summary>
        /// Represents a candlestick for financial data.
        /// </summary>
        public DateTime Date { get; set; }  // Date of the candlestick
        public decimal Open { get; set; }    // Opening price
        public decimal Close { get; set; }   // Closing price
        public decimal Low { get; set; }     // Lowest price
        public decimal High { get; set; }    // Highest price
        public ulong Volume { get; set; }    // Trading volume

        /// <summary>
        /// Initializes a new instance using CSV data.
        /// </summary>
        /// <param name="rowOfData">CSV line for stock data.</param>
        public Candlestick(string rowOfData)
        {
            string[] subs = SplitCsvRow(rowOfData);  // Split CSV row

            Date = DateTime.Parse(subs[0]);          // Parse date
            Open = ParseDecimal(subs[1]);            // Parse open price
            High = ParseDecimal(subs[2]);            // Parse high price
            Low = ParseDecimal(subs[3]);             // Parse low price
            Close = ParseDecimal(subs[4]);           // Parse close price
            Volume = ParseUlong(subs[5]);            // Parse volume
        }

        public Candlestick() { }

        /// <summary>
        /// Splits a CSV row into an array of strings.
        /// </summary>
        /// <param name="rowOfData">The CSV row to split.</param>
        /// <returns>An array of strings.</returns>
        private string[] SplitCsvRow(string rowOfData)
        {
            char[] separators = new char[] { ',', ' ', '"' };  // Define separators
            return rowOfData.Split(separators, StringSplitOptions.RemoveEmptyEntries);  // Split and remove empty entries
        }

        /// <summary>
        /// Parses a string into a decimal value.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <returns>Parsed decimal value or 0 if failed.</returns>
        private decimal ParseDecimal(string value)
        {
            return decimal.TryParse(value, out decimal result) ? decimal.Round(result, 2) : 0;  // Return rounded value or 0
        }

        /// <summary>
        /// Parses a string into an unsigned long value.
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <returns>Parsed ulong value or 0 if failed.</returns>
        private ulong ParseUlong(string value)
        {
            return ulong.TryParse(value, out ulong result) ? result : 0;  // Return parsed value or 0
        }

        /// <summary>
        /// Represents an enhanced candlestick with additional properties and pattern recognition.
        /// </summary>
        internal class SmartCandlestick : Candlestick
        {
            // Calculated properties for the candlestick's physical attributes
            public decimal Range => High - Low;                    // Range of the whole candlestick
            public decimal BodyRange => Math.Abs(Close - Open);    // Range from open to close
            public decimal TopPrice => Math.Max(Open, Close);      // Higher of open or close
            public decimal BottomPrice => Math.Min(Open, Close);   // Lower of open or close
            public decimal UpperTail => High - TopPrice;           // Height of the upper tail
            public decimal LowerTail => BottomPrice - Low;         // Height of the lower tail

            // Boolean properties for each candlestick pattern
            public bool IsBullish { get; private set; }
            public bool IsBearish { get; private set; }
            public bool IsNeutral { get; private set; }
            public bool IsDoji { get; private set; }
            public bool IsMarubozu { get; private set; }
            public bool IsHammer { get; private set; }
            public bool IsDragonflyDoji { get; private set; }
            public bool IsGravestoneDoji { get; private set; }

            public SmartCandlestick(string rowOfData) : base(rowOfData)
            {
                DeterminePatterns();
            }

            public SmartCandlestick(Candlestick candlestick)
            {
                // the date is assigned to the candlestick date
                Date = candlestick.Date;
                // the open is assigned to the candlestick open
                Open = candlestick.Open;
                // the high is assigned to the candlestick high
                High = candlestick.High;
                // the low is assigned to the candlestick low
                Low = candlestick.Low;
                // the volume is assigned to the candlestick volume
                Volume = candlestick.Volume;
                // the close is assigned to the candlestick close
                Close = candlestick.Close;
                DeterminePatterns();
            }

            /// <summary>
            /// Determines the candlestick patterns and sets the corresponding boolean properties.
            /// </summary>
            private void DeterminePatterns()
            {
                // Set the bullish, bearish, and neutral properties
                IsBullish = Open < Close;
                IsBearish = Open > Close;
                IsNeutral = Math.Abs(Close - Open) < 0.5m;

                // Set other patterns based on specific conditions
                IsDoji = BodyRange < Open * 0.05m;
                IsMarubozu = BodyRange > (Range * 0.9m);
                IsHammer = LowerTail > (BodyRange * 2) && BodyRange < (Range * 0.3m);               
                IsDragonflyDoji = BodyRange < (0.05m * Range) && UpperTail < (0.05m * Range) && LowerTail > (2 * BodyRange);
                IsGravestoneDoji = BodyRange < (0.05m * Range) && LowerTail < (0.05m * Range) && UpperTail > (2 * BodyRange);
            }

            /// <summary>
            /// Checks if the candlestick is a peak.
            /// </summary>
            public bool IsPeak(decimal previousHigh, decimal nextHigh)
            {
                return High > previousHigh && High > nextHigh;
            }

            /// <summary>
            /// Checks if the candlestick is a valley.
            /// </summary>
            public bool IsValley(decimal previousLow, decimal nextLow)
            {
                return Low < previousLow && Low < nextLow;
            }
        }
    }
}