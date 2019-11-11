using System.Collections.Generic;
using System.Linq;

namespace Sock
{
    public class CurrentFinance
    {
        public List<Loan> loans;
        public List<FinanceItem> monthlyBudgetItems;

        readonly string interestDesc = ".interest";
        readonly string principalDesc = ".principal";

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
            double principal = - loan.monthlyPayment - interest;
            addBudgetItem(new FinanceItem(loan.shortName + interestDesc, interest));
            addBudgetItem(new FinanceItem(loan.shortName + principalDesc, principal));
        }

        /// -------------------------------------------------------------
        ///
        public void refreshLoanBudgetItems(Loan loan)
        {
            double interest = loan.getInterestMonth();
            double principal = - loan.monthlyPayment - interest;

            foreach (FinanceItem item in monthlyBudgetItems)
            {
                if (item.title.Substring(0, 3).Equals(loan.shortName))
                {
                    if (item.title.Substring(3).Equals(interestDesc))
                    {
                        item.amount = interest;
                    }
                    if (item.title.Substring(3).Equals(principalDesc))
                    {
                        item.amount = principal;
                    }
                }
            }
        }

        /// -------------------------------------------------------------
        ///
        public double getMonthlyBudgetSum()
        {
            return monthlyBudgetItems.Sum(item => item.amount);
        }
    }
}