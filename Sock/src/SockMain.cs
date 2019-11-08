using System;
using System.Collections.Generic;

namespace Sock
{
    class SockMain
    {
        static void Main(string[] args)
        {
            InputHandler inHandler = new InputHandler();

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
            myFinance.addBudgetItem(new FinanceItem("Salary", 15000));
            myFinance.addBudgetItem(new FinanceItem("Buss", -800));
            myFinance.addBudgetItem(new FinanceItem("WiFi", -300));
            myFinance.addBudgetItem(new FinanceItem("Power", -1000));
            myFinance.addBudgetItem(new FinanceItem("Food", -2000));

            myFinance.addLoan(new Loan("Mortgage", -1000000, 2.65, 5000, "Mtg"));
            myFinance.addLoan(new Loan("Student loan", -500000, 2.5, 3000, "Std"));

            CurrentFinancePage currentFinancePage = new CurrentFinancePage(myFinance);

            int isAlive = 1;

            while (isAlive != 0)
            {
                Render.clearScreen();
                Render.renderFrame();
                // Render.renderColumnContent(bullshit);

                Render.renderPageInfo(currentFinancePage.pageInfo);
                currentFinancePage.renderContent();

                Render.positionInputCursor();
                isAlive = inHandler.processInput();
                // Do stuff
            }


            Console.SetCursorPosition(0, Console.WindowHeight);
        }
    }
}
