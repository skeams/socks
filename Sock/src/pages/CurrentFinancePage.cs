using System.Collections.Generic;

namespace Sock
{
    public class CurrentFinancePage : IPage
    {
        public string pageTitle { get; set; }
        public List<string> commands { get; set; }

        public CurrentFinance finance;
        
        public CurrentFinancePage(CurrentFinance finance)
        {
            this.finance = finance;

            this.pageTitle = "Welcome to Super Bowl!";
            this.commands = new List<string>
            {
                "home",
                "add",
            };
        }

        public void renderContent()
        {
            List<string> financeData = new List<string>();
            financeData.Add("Monthly Budget items");
            financeData.Add("");

            foreach (FinanceItem budgetItem in finance.monthlyBudgetItems)
            {
                string itemLine = " * " + budgetItem.title + ": " + budgetItem.amount;
                financeData.Add(itemLine);
            }

            financeData.Add("");
            financeData.Add("Loans");
            financeData.Add("");

            foreach (Loan loan in finance.loans)
            {

            }

            Render.renderColumnContent(financeData);
        }
    }
}