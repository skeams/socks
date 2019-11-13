using System.Collections.Generic;
using System.Linq;

namespace Sock
{
    public class Budget
    {
        public List<Loan> loans;
        public List<FinanceItem> monthlyBudgetItems;
        public List<FinanceItem> monthlyIntrPrinc;
        public double currentSavings;
        public double savingsGrowthRate;

        public string title;

        readonly string interestDesc = ".interest";
        readonly string principalDesc = ".principal";

        public Budget()
        {
            this.loans = new List<Loan>();
            this.monthlyBudgetItems = new List<FinanceItem>();
            this.monthlyIntrPrinc = new List<FinanceItem>();
            this.currentSavings = 0;
            this.savingsGrowthRate = 0;
            this.title = "<No title given>";
        }

        /// -------------------------------------------------------------
        ///
        public void addLoan(Loan loan)
        {
            loans.Add(loan);

            double interest = loan.getInterestMonth();
            double principal = -loan.monthlyPayment - interest;
            monthlyIntrPrinc.Add(new FinanceItem(loan.shortName + interestDesc, interest));
            monthlyIntrPrinc.Add(new FinanceItem(loan.shortName + principalDesc, principal));
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
        public void sortBudgetItems()
        {
            this.monthlyBudgetItems = this.monthlyBudgetItems.OrderByDescending(item => System.Math.Abs(item.amount)).ToList();
        }

        /// -------------------------------------------------------------
        ///
        public void refreshLoanBudgetItems(Loan loan)
        {
            double interest = loan.getInterestMonth();
            double principal = -loan.monthlyPayment - interest;

            foreach (FinanceItem item in monthlyIntrPrinc)
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
            foreach (FinanceItem item in monthlyIntrPrinc)
            {
                if (item.title.Substring(0, 3).Equals(loan.shortName))
                {
                    deleteItems.Add(item);
                }
            }

            foreach (FinanceItem deleteItem in deleteItems)
            {
                monthlyIntrPrinc.Remove(deleteItem);
            }
        }

        /// -------------------------------------------------------------
        ///
        public double getMonthlyNetSum()
        {
            return monthlyBudgetItems.Concat(monthlyIntrPrinc).Sum(item => item.amount);
        }

        /// -------------------------------------------------------------
        ///
        public double getMonthlyBudgetItemsSum()
        {
            return monthlyBudgetItems.Sum(item => item.amount);
        }

        /// -------------------------------------------------------------
        ///
        public double calculateSavings(int months)
        {
            double estimatedSavings = currentSavings;
            double monthlySavings = getMonthlyNetSum();

            for (int m = 0; m < months; m++)
            {
                estimatedSavings += (estimatedSavings * savingsGrowthRate / 100) / 12;
                estimatedSavings += monthlySavings;
            }

            return estimatedSavings - currentSavings;
        }
    }
}