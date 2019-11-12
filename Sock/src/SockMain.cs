using System;
using System.Collections.Generic;

namespace Sock
{
    class SockMain
    {
        static readonly string filePath = "datafiles/data.csv";
        
        static void Main(string[] args)
        {
            string data = DataLoader.readFileContent(filePath);
            CurrentFinance finance = DataLoader.csvToCurrentFinance(data);

            NavigationControl navigation = new NavigationControl(finance);

            string inputCommand = "";

            while (!inputCommand.ToLower().Equals("exit"))
            {
                Render.clearScreen();
                Render.renderFrame();

                Render.renderPageInfo(navigation.currentPage.pageInfo);
                navigation.currentPage.renderContent();

                inputCommand = InputHandler.processInput("Command");
                if (!navigation.navigate(inputCommand))
                {
                    navigation.currentPage.handleCommand(inputCommand);
                }
            }

            string storeData = DataLoader.currentFinanceToCsv(finance);
            DataLoader.writeFileContent(storeData, filePath);

            Console.SetCursorPosition(0, Console.WindowHeight);
        }
    }
}
