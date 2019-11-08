using System;

namespace Sock
{
    public static class InputHandler
    {
        public static string processInput(string inputText)
        {
            // Read input
            Render.positionInputCursor();
            Console.Write(inputText + ": ");
            string result = Console.ReadLine();

            // Clear input
            Render.positionInputCursor();
            Console.Write(new string(' ', inputText.Length + 2 + result.Length));
            return  result;
        }
    }
}