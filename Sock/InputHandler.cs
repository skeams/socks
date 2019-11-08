using System;

namespace Sock
{
    public class InputHandler
    {
        public int processInput() {
            
            Console.Write("Command: ");
            string input = Console.ReadLine();
            

            if (input.ToLower().Equals("exit"))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}