using System;

namespace Sock
{
    public static class InputHandler
    {
        /// -------------------------------------------------------------
        ///
        public static string processInput(string inputText)
        {
            // Read input
            Render.positionInputCursor();
            Console.Write(inputText + ": ");
            string result = Console.ReadLine();
            // ,;# characters are used in the dataFormat and must be removed
            string cleanedResult = System.Text.RegularExpressions.Regex.Replace(result, @"(,|;|#)", "");

            // Clear input
            Render.positionInputCursor();
            Console.Write(new string(' ', inputText.Length + 2 + result.Length));
            return cleanedResult;
        }

        /// -------------------------------------------------------------
        ///
        public static double processNumberInput(string inputText, double existingValue)
        {
            string numberInputString = processInput(inputText + " [" + existingValue + "]");
            double newNumber;

            if (Double.TryParse(numberInputString, out newNumber))
            {
                return newNumber;
            }
            else
            {
                return existingValue;
            }
        }
    }
}