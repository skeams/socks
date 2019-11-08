using System.Collections.Generic;

namespace Sock
{
    public class CurrentFinance
    {
        public List<Loan> loans;
        public List<FinanceItem> monthlyBudgetItems;

        public CurrentFinance()
        {
            this.loans = new List<Loan>();
            this.monthlyBudgetItems = new List<FinanceItem>();
        }

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

            double interest = loan.getInterestMonth();
            double downPayment = - loan.principal - interest;
            addBudgetItem(new FinanceItem(loan.shortName + "." + "interest", interest));
            addBudgetItem(new FinanceItem(loan.shortName + "." + "downpayment", downPayment));
        }
    }
}