using System.Collections.Generic;

namespace Sock
{
    public class NavigationControl
    {
        public Page currentPage;

        BudgetPage budgetPage;
        DashboardPage dashboardPage;
        ForecastPage forecastPage;

        List<Budget> budgets;

        public NavigationControl(List<Budget> budgets)
        {
            this.budgets = budgets;

            this.budgetPage = new BudgetPage(this.budgets[0]);
            this.dashboardPage = new DashboardPage(this.budgets[0], this.budgets);
            this.forecastPage = new ForecastPage(this.budgets[0]);

            this.currentPage = this.dashboardPage;
        }

        /// -------------------------------------------------------------
        ///
        public bool navigate(string command)
        {
            switch (command)
            {
                case "budget":
                    this.currentPage = budgetPage;
                    return true;

                case "home":
                    this.currentPage = dashboardPage;
                    return true;

                case "forecast":
                    this.currentPage = forecastPage;
                    return true;

                case "switch":
                    switchBudgetAction();
                    return true;
            }
            
            return false; // false will send command to page control
        }

        /// -------------------------------------------------------------
        ///
        public void switchBudgetAction()
        {
            string budgetTitles = "";
            budgets.ForEach(budget => budgetTitles += budget.title + " | ");
            budgetTitles += "(must be unique)";
            Render.renderStatus(budgetTitles, false);
            
            string budgetTitle = InputHandler.processInput("Switch to");
            Budget switchBudget = budgets.Find(budget => budget.title.ToLower().Equals(budgetTitle.ToLower()));

            if (switchBudget != null)
            {
                this.budgetPage.currentBudget = switchBudget;
                this.dashboardPage.currentBudget = switchBudget;
                this.forecastPage.currentBudget = switchBudget;
            }
        }
    }
}