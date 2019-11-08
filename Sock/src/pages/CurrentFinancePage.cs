using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace Sock
{
    public class CurrentFinancePage : IPage
    {
        public List<string> pageInfo { get; set; }

        CurrentFinance finance;
    
        public CurrentFinancePage(CurrentFinance finance)
        {
            this.finance = finance;
            this.pageInfo = new List<string>
            {
                "Current Finances (Budget and loans)","",
                "Here you can add, edit, or delete monthly budget items and loans.",
                "Your monthly budget items are automatically updated with the loan principal values."
            };
        }

        public void handleCommand(string command)
        {
            // TODO: Implement a more generic way to handle step operations
            switch (command)
            {
                case "new":
                    string itemTitle = InputHandler.processInput("Enter item title");
                    double itemAmount = double.Parse(
                        InputHandler.processInput("Enter item amount")
                    );
                    finance.addBudgetItem(new FinanceItem(itemTitle, itemAmount));
                    break;

                case "edit":
                    string oldItemName = InputHandler.processInput("Enter name of item you want to edit");

                    FinanceItem editItem = null;
                    foreach (FinanceItem item in finance.monthlyBudgetItems)
                    {
                        if (oldItemName.Equals(item.title))
                        {
                            editItem = item;
                            break;
                        }
                    }

                    if (editItem != null)
                    {
                        editItem.amount = double.Parse(
                            InputHandler.processInput("Enter new item amount")
                        );  
                    }
                    else
                    {
                        // TODO: Implement status window and set status
                    }
                    break;

                case "delete":
                    string itemName = InputHandler.processInput("Enter name of item you want to delete");

                    FinanceItem deleteItem = null;
                    foreach (FinanceItem item in finance.monthlyBudgetItems)
                    {
                        if (itemName.Equals(item.title))
                        {
                            deleteItem = item;
                            break;
                        }
                    }

                    if (deleteItem != null)
                    {
                       finance.monthlyBudgetItems.Remove(deleteItem); 
                    }
                    else
                    {
                        // TODO: Implement status window and set status
                    }
                    break;
            }
        }

        /// -------------------------------------------------------------
        ///
        /// Render budget data
        ///
        public void renderContent()
        {
            int lineWidth = 30;

            List<string> financeData = new List<string>();
            financeData.Add("");
            financeData.Add("Monthly Budget:");
            financeData.Add("");

            foreach (FinanceItem budgetItem in finance.monthlyBudgetItems)
            {
                financeData.Add(formatAmountLine(budgetItem.title, budgetItem.amount, lineWidth));
            }

            financeData.Add("______________________________");
            financeData.Add("");
            financeData.Add(formatAmountLine("Net Amount", finance.getMonthlyBudgetSum(), lineWidth));

            financeData.Add("");
            financeData.Add("");
            financeData.Add("Loans:");
            financeData.Add("");

            foreach (Loan loan in finance.loans)
            {
                financeData.Add(formatAmountLine(loan.title, loan.amount, lineWidth));
            }

            Render.renderColumnContent(financeData);
        }

        /// -------------------------------------------------------------
        ///
        /// Formats and right aligns numbers. Ensures that the string is
        /// of the required length.
        ///
        string formatAmountLine(string title, double amount, int width)
        {
            string formattedLine = " * " + title + ":";
            string numberFormatted = amount.ToString("N0", CultureInfo.CreateSpecificCulture("nb-NO"));

            return formattedLine + new string(' ', width - (formattedLine.Length + numberFormatted.Length)) + numberFormatted;
        }
    }
}