using System.Collections.Generic;
using System.Linq;

namespace Sock
{
    public class CurrentFinance
    {
        public List<Loan> loans;
        public List<FinanceItem> monthlyBudgetItems;
        public double currentSavings;
        public double savingsGrowthRate;

        readonly string interestDesc = ".interest";
        readonly string principalDesc = ".principal";

        public CurrentFinance()
        {
            this.loans = new List<Loan>();
            this.monthlyBudgetItems = new List<FinanceItem>();
            this.currentSavings = 0;
            this.savingsGrowthRate = 0;
        }

        /// -------------------------------------------------------------
        ///
        public void addLoan(Loan loan)
        {
            loans.Add(loan);

            double interest = loan.getInterestMonth();
            double principal = -loan.monthlyPayment - interest;
            monthlyBudgetItems.Add(new FinanceItem(loan.shortName + interestDesc, interest));
            monthlyBudgetItems.Add(new FinanceItem(loan.shortName + principalDesc, principal));
        }

        /// -------------------------------------------------------------
        ///
        public Loan getLoan(string abr)
        {
            foreach (Loan loan in loans)
            {
                if (string.Equals(abr, loan.shortName, System.StringComparison.OrdinalIgnoreCase))
                {
                    return loan;
                }
            }

            return null;
        }

        /// -------------------------------------------------------------
        ///
        public FinanceItem getItem(string itemTitle)
        {
            foreach (FinanceItem item in monthlyBudgetItems)
            {
                if (string.Equals(item.title, itemTitle, System.StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }

            return null;
        }

        /// -------------------------------------------------------------
        ///
        public void refreshLoanBudgetItems(Loan loan)
        {
            double interest = loan.getInterestMonth();
            double principal = -loan.monthlyPayment - interest;

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
        public void deleteLoanBudgetItems(Loan loan)
        {
            List<FinanceItem> deleteItems = new List<FinanceItem>();
            foreach (FinanceItem item in monthlyBudgetItems)
            {
                if (item.title.Substring(0, 3).Equals(loan.shortName))
                {
                    deleteItems.Add(item);
                }
            }

            foreach (FinanceItem deleteItem in deleteItems)
            {
                monthlyBudgetItems.Remove(deleteItem);
            }
        }

        /// -------------------------------------------------------------
        ///
        public double getMonthlyBudgetSum()
        {
            return monthlyBudgetItems.Sum(item => item.amount);
        }

        /// -------------------------------------------------------------
        ///
        public double calculateSavings(int months)
        {
            double estimatedSavings = currentSavings;

            for (int m = 0; m < months; m++)
            {
                estimatedSavings += (estimatedSavings * savingsGrowthRate / 100) / 12;
                estimatedSavings += getMonthlyBudgetSum();
            }

            return estimatedSavings - currentSavings;
        }
    }
}