using System;
using System.Collections.Generic;

namespace Sock
{
    class SockMain
    {
        static readonly string filePath = "data.csv";
        
        static void Main(string[] args)
        {
            string data = DataLoader.readFileContent(filePath);
            List<Budget> budgets = DataLoader.csvToBudgets(data);

            NavigationControl navigation = new NavigationControl(budgets);

            string inputCommand = "";

            while (!inputCommand.ToLower().Equals("exit"))
            {
                Render.clearScreen();
                Render.renderFrame();
                Render.renderStatus();
                Render.renderPageInfo(navigation.currentPage.pageInfo, navigation.currentPage.currentBudget.title);
                navigation.currentPage.renderContent();

                inputCommand = InputHandler.processInput("Command");
                if (!navigation.navigate(inputCommand))
                {
                    navigation.currentPage.handleCommand(inputCommand);
                }
            }

            string storeData = DataLoader.budgetsToCsv(budgets);
            DataLoader.writeFileContent(storeData, filePath);

            Render.positionInputCursor();
            Console.WriteLine("Bye!");
            Console.SetCursorPosition(0, Console.WindowHeight);
        }
    }
}
