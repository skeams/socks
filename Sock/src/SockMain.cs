using System;
using System.Collections.Generic;

namespace Sock
{
    class SockMain
    {
        static void Main(string[] args)
        {
            List<string> bullshit = new List<string>
            {
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor",
                "in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident,",
                "sunt in culpa qui officia deserunt mollit anim id est laborum.",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor",
                "in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident,",
                "sunt in culpa qui officia deserunt mollit anim id est laborum.",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor",
                "in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident,",
                "sunt in culpa qui officia deserunt mollit anim id est laborum.",
            };

            CurrentFinance myFinance = new CurrentFinance();
            myFinance.monthlyBudgetItems.Add(new FinanceItem("Salary", 15000));
            myFinance.monthlyBudgetItems.Add(new FinanceItem("Buss", -800));
            myFinance.monthlyBudgetItems.Add(new FinanceItem("WiFi", -300));
            myFinance.monthlyBudgetItems.Add(new FinanceItem("Power", -1000));
            myFinance.monthlyBudgetItems.Add(new FinanceItem("Food", -2000));

            myFinance.addLoan(new Loan("Mortgage", -1500000, 2.65, 8000, "Mtg"));
            myFinance.addLoan(new Loan("Student loan", -160000, 2.5, 1000, "Std"));

            CurrentFinancePage currentFinancePage = new CurrentFinancePage(myFinance);
            DashboardPage dashboardPage = new DashboardPage(myFinance);

            string inputCommand = "";

            while (!inputCommand.ToLower().Equals("exit"))
            {
                Render.clearScreen();
                Render.renderFrame();
                // Render.renderColumnContent(bullshit);

                // Render.renderPageInfo(dashboardPage.pageInfo);
                // dashboardPage.renderContent();

                Render.renderPageInfo(currentFinancePage.pageInfo);
                currentFinancePage.renderContent();

                inputCommand = InputHandler.processInput("Command");
                currentFinancePage.handleCommand(inputCommand);
                // dashboardPage.handleCommand(inputCommand);
                // Do stuff
            }


            Console.SetCursorPosition(0, Console.WindowHeight);
        }
    }
}
