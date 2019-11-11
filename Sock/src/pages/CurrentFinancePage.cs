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
                "Here you can update your monthly budget, loans and savings. Your monthly budget items are automatically updated with the loan interest and principal. " +
                "Net Amount is added to your savings.",
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
                    deleteLoanAction();
                    break;

                case "edit savings":
                    editSavingsAction();
                    break;
            }
        }

        /// -------------------------------------------------------------
        ///
        public void editSavingsAction()
        {
            finance.currentSavings = InputHandler.processNumberInput("Enter current savings amount", finance.currentSavings);
            finance.savingsGrowthRate = InputHandler.processNumberInput("Enter savings growth rate", finance.savingsGrowthRate);
        }

        /// -------------------------------------------------------------
        ///
        public void newLoanAction()
        {
            string title = InputHandler.processInput("Enter loan title");
            string abr = InputHandler.processInput("Enter loan abr (3 letters)");
            double amount = InputHandler.processNumberInput("Enter loan amount", 0);
            double interest = InputHandler.processNumberInput("Enter loan interest %", 0);
            double monthlyPayment = InputHandler.processNumberInput("Enter monthly payment", 0);
            finance.addLoan(new Loan(title, amount, interest, monthlyPayment, abr));
        }

        /// -------------------------------------------------------------
        ///
        public void editLoanAction()
        {
            string abr = InputHandler.processInput("Enter loan abr.");
            Loan editLoan = finance.getLoan(abr);

            if (editLoan != null)
            {
                editLoan.amount = InputHandler.processNumberInput("Enter loan amount", editLoan.amount);
                editLoan.interestPercentage = InputHandler.processNumberInput("Enter loan interest %", editLoan.interestPercentage);
                editLoan.monthlyPayment = InputHandler.processNumberInput("Enter monthly payment", editLoan.monthlyPayment);
                finance.refreshLoanBudgetItems(editLoan);
            }
        }

        /// -------------------------------------------------------------
        ///
        public void deleteLoanAction()
        {
            string abr = InputHandler.processInput("Enter loan abr.");
            Loan deleteLoan = finance.getLoan(abr);

            if (deleteLoan != null)
            {
                finance.deleteLoanBudgetItems(deleteLoan);
                finance.loans.Remove(deleteLoan);
            }
        }

        /// -------------------------------------------------------------
        ///
        public void newItemAction()
        {
            string itemTitle = InputHandler.processInput("Enter item title");
            double itemAmount = InputHandler.processNumberInput("Enter item amount", 0);
            finance.monthlyBudgetItems.Add(new FinanceItem(itemTitle, itemAmount));
        }

        /// -------------------------------------------------------------
        ///
        public void editItemAction()
        {
            string itemName = InputHandler.processInput("Enter name of item you want to edit");
            FinanceItem editItem = finance.getItem(itemName);

            if (editItem != null)
            {
                editItem.amount =InputHandler.processNumberInput("Enter new item amount", editItem.amount);
            }
        }

        /// -------------------------------------------------------------
        ///
        public void deleteItemAction()
        {
            string itemName = InputHandler.processInput("Enter name of item you want to delete");
            FinanceItem deleteItem = finance.getItem(itemName);

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
            financeData.Add(formatAmountLine("Saving/Net Amount", finance.getMonthlyBudgetSum(), lineWidth));

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

            financeData.Add("");
            financeData.Add("");
            financeData.Add("");
            financeData.Add("~~~~~~~~~  Savings  ~~~~~~~~~~");
            financeData.Add("");
            financeData.Add(formatAmountLine("Current savings", finance.currentSavings, lineWidth));
            financeData.Add(formatAmountLine("Savings growth %", finance.savingsGrowthRate, lineWidth));
            financeData.Add(formatAmountLine("Yearly growth", finance.calculateSavings(12), lineWidth));

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