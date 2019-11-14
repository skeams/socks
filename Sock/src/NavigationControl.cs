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
            Render.setStatus(this.currentPage.defaultStatus, false);
        }

        /// -------------------------------------------------------------
        ///
        public bool navigate(string command)
        {
            bool didNavigate = false;
            switch (command)
            {
                case "budget":
                    this.currentPage = budgetPage;
                    didNavigate = true;
                    break;

                case "home":
                    this.currentPage = dashboardPage;
                    didNavigate = true;
                    break;

                case "forecast":
                    this.currentPage = forecastPage;
                    didNavigate = true;
                    break;

                case "switch":
                    switchBudgetAction();
                    didNavigate = true;
                    break;
            }

            if (didNavigate)
            {
                Render.setStatus(this.currentPage.defaultStatus, false);
            }

            return didNavigate; // false will send command to page control
        }

        /// -------------------------------------------------------------
        ///
        public void switchBudgetAction()
        {
            string budgetTitles = "";
            budgets.ForEach(budget => budgetTitles += budget.title + " | ");
            budgetTitles += "(choose budget)";
            Render.setStatus(budgetTitles, false);
            
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