using System.Linq;
using System.Collections.Generic;

namespace Sock
{
    public class BudgetPage : Page
    {
        public override List<string> pageInfo { get; set; }
        public override Budget currentBudget { get; set; }
        public override string defaultStatus { get; set; }
    
        public BudgetPage(Budget budget)
        {
            this.defaultStatus = "new/edit/delete item/loan/goal | savings";

            this.currentBudget = budget;
            this.pageInfo = new List<string>
            {
                "Budget","",
                "Here you can update your monthly budget, loans and savings. Your monthly budget is automatically updated with loan interest/principal. " +
                "Savings / Net Amount will automatically update your annual savings growth.",
            };
        }

        /// -------------------------------------------------------------
        ///
        public override void handleCommand(string command)
        {
            switch (command)
            {
                case "new goal":
                    newGoalAction();
                    break;

                case "edit goal":
                    Render.setStatus("Only new/delete available for goals", true);
                    break;

                case "delete goal":
                    deleteGoalAction();
                    break;
                    
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

                case "savings":
                    editSavingsAction();
                    break;
            }
        }

        /// -------------------------------------------------------------
        ///
        public void newGoalAction()
        {
            Render.setStatus("savings/debt", false);
            string input = InputHandler.processInput("Goal type");
            bool isSavingsGoal = false;
            if (input.ToLower().Equals("savings"))
            {
                isSavingsGoal = true;
            }

            Render.setStatus("Enter " + (isSavingsGoal ? "savings" : "debt") + " goal details", false);
            string title = InputHandler.processInput("Title");
            double amount =  InputHandler.processNumberInput("Amount", 0);
            FinanceItem goal = new FinanceItem(title, amount);

            if (isSavingsGoal)
            {
                this.currentBudget.savingsGoals.Add(goal);
            }
            else
            {
                this.currentBudget.debtGoals.Add(goal);
            }
        }

        /// -------------------------------------------------------------
        ///
        public void deleteGoalAction()
        {
            Render.setStatus("Delete goal?", false);
            string goalTitle = InputHandler.processInput("Goal title");
            FinanceItem goal = null;

            foreach (FinanceItem savingsGoal in currentBudget.savingsGoals)
            {
                if (savingsGoal.title.ToLower().Equals(goalTitle.ToLower()))
                {
                    goal = savingsGoal;
                    break;
                }
            }

            if (goal != null)
            {
                currentBudget.savingsGoals.Remove(goal);
                return;
            }

            foreach (FinanceItem debtGoal in currentBudget.debtGoals)
            {
                if (debtGoal.title.ToLower().Equals(goalTitle.ToLower()))
                {
                    goal = debtGoal;
                    break;
                }
            }

            if (goal != null)
            {
                currentBudget.debtGoals.Remove(goal);
            }
        }

        /// -------------------------------------------------------------
        ///
        public void editSavingsAction()
        {
            Render.setStatus("Edit savings", false);
            currentBudget.currentSavings = InputHandler.processNumberInput("Savings amount", currentBudget.currentSavings);
            currentBudget.savingsGrowthRate = InputHandler.processNumberInput("Growth rate", currentBudget.savingsGrowthRate);
        }

        /// -------------------------------------------------------------
        ///
        public void newLoanAction()
        {
            Render.setStatus("New loan", false);
            string title = InputHandler.processInput("Title");
            string abr = InputHandler.processInput("Abr (3 letters)");
            double amount = InputHandler.processNumberInput("Amount", 0);
            double interest = InputHandler.processNumberInput("Interest %", 0);
            double monthlyPayment = InputHandler.processNumberInput("Monthly payment", 0);
            currentBudget.addLoan(new Loan(title, abr, amount, interest, monthlyPayment));
        }

        /// -------------------------------------------------------------
        ///
        public void editLoanAction()
        {
            Render.setStatus("Which loan to edit?", false);
            string abr = InputHandler.processInput("Loan abr.");
            Loan editLoan = currentBudget.getLoan(abr);

            if (editLoan != null)
            {
                Render.setStatus("Edit loan (" + editLoan.title + ")", false);
                editLoan.amount = InputHandler.processNumberInput("Amount", editLoan.amount);
                editLoan.interestPercentage = InputHandler.processNumberInput("Interest %", editLoan.interestPercentage);
                editLoan.monthlyPayment = InputHandler.processNumberInput("Monthly payment", editLoan.monthlyPayment);
                currentBudget.refreshLoanBudgetItems(editLoan);
            }
        }

        /// -------------------------------------------------------------
        ///
        public void deleteLoanAction()
        {
            Render.setStatus("Which loan do you want to delete?", false);
            string abr = InputHandler.processInput("Loan abr.");
            Loan deleteLoan = currentBudget.getLoan(abr);

            if (deleteLoan != null)
            {
                currentBudget.deleteLoanBudgetItems(deleteLoan);
                currentBudget.loans.Remove(deleteLoan);
            }
        }

        /// -------------------------------------------------------------
        ///
        public void newItemAction()
        {
            Render.setStatus("New budget item", false);
            string itemTitle = InputHandler.processInput("Title");
            double itemAmount = InputHandler.processNumberInput("Amount", 0);
            currentBudget.monthlyBudgetItems.Add(new FinanceItem(itemTitle, itemAmount));
        }

        /// -------------------------------------------------------------
        ///
        public void editItemAction()
        {
            Render.setStatus("Which budget item to edit?", false);
            string itemName = InputHandler.processInput("Title");
            FinanceItem editItem = currentBudget.getItem(itemName);

            if (editItem != null)
            {
                Render.setStatus("Edit item (" + editItem.title + ")", false);
                editItem.amount =InputHandler.processNumberInput("Amount", editItem.amount);
            }
        }

        /// -------------------------------------------------------------
        ///
        public void deleteItemAction()
        {
            Render.setStatus("Which budget item to delete?", false);
            string itemName = InputHandler.processInput("Title");
            FinanceItem deleteItem = currentBudget.getItem(itemName);

            if (deleteItem != null)
            {
                currentBudget.monthlyBudgetItems.Remove(deleteItem); 
            }
        }

        /// -------------------------------------------------------------
        ///
        public override void renderContent()
        {
            int lineWidth = 30;
            currentBudget.sortBudgetItems();

            List<string> financeData = new List<string>();
            financeData.Add("~~~~~~  Monthly Budget  ~~~~~~");
            financeData.Add("");

            foreach (FinanceItem budgetItem in currentBudget.monthlyBudgetItems)
            {
                financeData.Add(Formatter.formatAmountLine(budgetItem.title, budgetItem.amount, lineWidth));
            }
            financeData.Add("");
            foreach (FinanceItem budgetItem in currentBudget.monthlyIntrPrinc)
            {
                financeData.Add(Formatter.formatAmountLine(budgetItem.title, budgetItem.amount, lineWidth));
            }

            financeData.Add("______________________________");
            financeData.Add("");
            financeData.Add(Formatter.formatAmountLine("Saving/Net Amount", currentBudget.getMonthlyNetSum(), lineWidth));

            financeData.Add("");
            financeData.Add("");
            financeData.Add("");
            financeData.Add("");
            financeData.Add("~~~~~~~~~~~  Debt  ~~~~~~~~~~~");
            financeData.Add("");

            foreach (Loan loan in currentBudget.loans)
            {
                financeData.Add(Formatter.formatAmountLine(loan.title + " (" + loan.shortName + ")", loan.amount, lineWidth));
                financeData.Add(Formatter.formatAmountLine(" - Monthly payment", loan.monthlyPayment, lineWidth));
                financeData.Add(Formatter.formatAmountLine(" - Interest %", loan.interestPercentage, lineWidth, 2));
                financeData.Add(Formatter.formatAmountLine(" - Annual principal", loan.calculatePrincipal(12), lineWidth));
                financeData.Add("");
            }

            financeData.Add("");
            financeData.Add("");
            financeData.Add("");
            financeData.Add("~~~~~~~~~  Savings  ~~~~~~~~~~");
            financeData.Add("");
            financeData.Add(Formatter.formatAmountLine("Current savings", currentBudget.currentSavings, lineWidth));
            financeData.Add(Formatter.formatAmountLine("Savings growth %", currentBudget.savingsGrowthRate, lineWidth, 2));
            financeData.Add(Formatter.formatAmountLine("Annual growth", currentBudget.calculateSavings(12), lineWidth));

            financeData.Add("");
            financeData.Add("");
            financeData.Add("");
            financeData.Add("~~~~~~~~~~  Goals  ~~~~~~~~~~~");
            financeData.Add("");
            if (currentBudget.savingsGoals.Count > 0)
            {
                financeData.Add("Savings Goals");
            }
            foreach (FinanceItem savingsGoal in currentBudget.savingsGoals)
            {
                financeData.Add(Formatter.formatAmountLine(" - " + savingsGoal.title, savingsGoal.amount, lineWidth));
            }
            financeData.Add("");
            if (currentBudget.debtGoals.Count > 0)
            {
                financeData.Add("Debt Goals");
            }
            foreach (FinanceItem debtGoal in currentBudget.debtGoals)
            {
                financeData.Add(Formatter.formatAmountLine(" - " + debtGoal.title, debtGoal.amount, lineWidth));
            }

            List<FinanceItem> chartItems = currentBudget.monthlyBudgetItems.ToList().Concat(currentBudget.monthlyIntrPrinc).ToList();
            chartItems.Add(new FinanceItem(
                "Savings",
                -currentBudget.getMonthlyNetSum()
            ));
            PieChart simpleChart = new PieChart(10, chartItems);
            
            Render.renderChart(simpleChart.chart, chartItems);

            Render.renderColumnContent(financeData);
        }

        
    }
}