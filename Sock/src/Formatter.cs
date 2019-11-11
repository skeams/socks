using System.Globalization;

namespace Sock
{
    public static class Formatter
    {
        /// -------------------------------------------------------------
        ///
        public static string formatAmountLine(string title, double amount, int width)
        {
            string formattedLine = title;
            string numberFormatted = amount.ToString("N0", CultureInfo.CreateSpecificCulture("nb-NO"));

            return formattedLine + new string(' ', width - (formattedLine.Length + numberFormatted.Length)) + numberFormatted;
        }
        public static string formatAmountLine(string title, double amount, int width, int decimals)
        {
            string formattedLine = title;
            string numberFormatted = amount.ToString("N" + decimals.ToString(), CultureInfo.CreateSpecificCulture("nb-NO"));

            return formattedLine + new string(' ', width - (formattedLine.Length + numberFormatted.Length)) + numberFormatted;
        }
    }
}