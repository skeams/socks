using System;
using System.Collections.Generic;

namespace Sock
{
    class SockMain
    {
        static void Main(string[] args)
        {
            CurrentFinance myFinance = initMyFinance();

            NavigationControl navigation = new NavigationControl(myFinance);

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

            Console.SetCursorPosition(0, Console.WindowHeight);
        }

        // TODO: Move and add CSV loading
        static CurrentFinance initMyFinance()
        {
            CurrentFinance myFinance = new CurrentFinance();
            myFinance.monthlyBudgetItems.Add(new FinanceItem("Salary", 15000));
            myFinance.monthlyBudgetItems.Add(new FinanceItem("Buss", -800));
            myFinance.monthlyBudgetItems.Add(new FinanceItem("WiFi", -300));
            myFinance.monthlyBudgetItems.Add(new FinanceItem("Power", -1000));
            myFinance.monthlyBudgetItems.Add(new FinanceItem("Food", -2000));

            myFinance.addLoan(new Loan("Mortgage", -1500000, 2.65, 8000, "Mtg"));
            myFinance.addLoan(new Loan("Student loan", -160000, 2.5, 1000, "Std"));

            return myFinance;
        }
    }
}
