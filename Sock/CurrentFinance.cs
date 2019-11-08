using System.Collections.Generic;

namespace Sock
{
    public class CurrentFinance
    {
        public List<Loan> loans;
        public List<FinanceItem> monthlyBudgetItems;

        public void addBudgetItem(FinanceItem item)
        {
            monthlyBudgetItems.Add(item);
        }

        public void updateBudgetItem(FinanceItem item)
        {

        }

        public void deleteBudgetItem(FinanceItem item)
        {

        }

        public void addLoan(Loan loan)
        {
            loans.Add(loan);
        }
    }
}