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
                "Current Finance","",
                "Here you can add, edit, or delete monthly budget items and loans.",
                "Your monthly budget items are automatically updated with the loan principal values."
            };
        }

        /// -------------------------------------------------------------
        ///
        public void handleCommand(string command)
        {
            switch (command)
            {
                case "new item":
                    newItemAction();
                    break;

                case "edit item":
                    editItemAction();
                    break;

                case "delete item":
                    deleteItemAction();
                    break;

                case "new loan":
                    newLoanAction();
                    break;

                case "edit loan":
                    editLoanAction();
                    break;

                case "delete loan":
                    // newLoanAction();
                    break;
            }
        }

        /// -------------------------------------------------------------
        ///
        public void newLoanAction()
        {
            string title = InputHandler.processInput("Enter loan title");
            string abr = InputHandler.processInput("Enter loan abr (3 letters)");
            double amount = double.Parse(
                InputHandler.processInput("Enter loan amount")
            );
            double interest = double.Parse(
                InputHandler.processInput("Enter loan interest")
            );
            double monthlyPayment = double.Parse(
                InputHandler.processInput("Enter monthly payment")
            );
            finance.addLoan(new Loan(title, amount, interest, monthlyPayment, abr));
        }

        /// -------------------------------------------------------------
        ///
        public void editLoanAction()
        {
            string abr = InputHandler.processInput("Enter loan abr.");
            
            Loan editLoan = null;
            foreach (Loan loan in finance.loans)
            {
                if (string.Equals(abr, loan.shortName, System.StringComparison.OrdinalIgnoreCase))
                {
                    editLoan = loan;
                    break;
                }
            }

            if (editLoan != null)
            {
                editLoan.amount = InputHandler.processNumberInput("Enter loan amount", editLoan.amount);
                editLoan.interestPercentage = InputHandler.processNumberInput("Enter loan interest", editLoan.interestPercentage);
                editLoan.monthlyPayment = InputHandler.processNumberInput("Enter monthly payment", editLoan.monthlyPayment);
                finance.refreshLoanBudgetItems(editLoan);
            }
        }

        /// -------------------------------------------------------------
        ///
        public void newItemAction()
        {
            string itemTitle = InputHandler.processInput("Enter item title");
            double itemAmount = double.Parse(
                InputHandler.processInput("Enter item amount")
            );
            finance.addBudgetItem(new FinanceItem(itemTitle, itemAmount));
        }

        /// -------------------------------------------------------------
        ///
        public void editItemAction()
        {
            string oldItemName = InputHandler.processInput("Enter name of item you want to edit");

            FinanceItem editItem = null;
            foreach (FinanceItem item in finance.monthlyBudgetItems)
            {
                if (string.Equals(oldItemName, item.title, System.StringComparison.OrdinalIgnoreCase))
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
        }

        /// -------------------------------------------------------------
        ///
        public void deleteItemAction()
        {
            string itemName = InputHandler.processInput("Enter name of item you want to delete");

            FinanceItem deleteItem = null;
            foreach (FinanceItem item in finance.monthlyBudgetItems)
            {
                if (string.Equals(itemName, item.title, System.StringComparison.OrdinalIgnoreCase))
                {
                    deleteItem = item;
                    break;
                }
            }

            if (deleteItem != null)
            {
                finance.monthlyBudgetItems.Remove(deleteItem); 
            }
        }

        /// -------------------------------------------------------------
        ///
        public void renderContent()
        {
            int lineWidth = 30;

            List<string> financeData = new List<string>();
            financeData.Add("~~~~~~  Monthly Budget  ~~~~~~");
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
            financeData.Add("");
            financeData.Add("");
            financeData.Add("~~~~~~~~~~  Loans  ~~~~~~~~~~~");
            financeData.Add("");

            foreach (Loan loan in finance.loans)
            {
                financeData.Add(formatAmountLine(loan.title, loan.amount, lineWidth));
                financeData.Add(formatAmountLine(" - Montly payment", loan.monthlyPayment, lineWidth));
                financeData.Add(formatAmountLine(" - Interest %", loan.interestPercentage, lineWidth, 2));
                financeData.Add(formatAmountLine(" - Yearly principal", loan.calculatePrincipal(12), lineWidth));
                financeData.Add("");
            }

            Render.renderColumnContent(financeData);
        }

        /// -------------------------------------------------------------
        ///
        string formatAmountLine(string title, double amount, int width)
        {
            string formattedLine = title;
            string numberFormatted = amount.ToString("N0", CultureInfo.CreateSpecificCulture("nb-NO"));

            return formattedLine + new string(' ', width - (formattedLine.Length + numberFormatted.Length)) + numberFormatted;
        }
        string formatAmountLine(string title, double amount, int width, int decimals)
        {
            string formattedLine = title;
            string numberFormatted = amount.ToString("N" + decimals.ToString(), CultureInfo.CreateSpecificCulture("nb-NO"));

            return formattedLine + new string(' ', width - (formattedLine.Length + numberFormatted.Length)) + numberFormatted;
        }
    }
}