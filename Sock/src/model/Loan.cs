using System.Collections.Generic;

namespace Sock
{
    public class Loan : FinanceItem
    {
        public double interestPercentage;
        public double monthlyPayment;
        public string shortName; // MUST BE UNIQUE AND IS USED AS IDENTIFIER

        public Loan(string title, double amount, double interestPercentage, double monthlyPayment, string shortName) : base(title, amount)
        {
            this.interestPercentage = interestPercentage;
            this.monthlyPayment = monthlyPayment;
            this.shortName = shortName;
        }

        /// -------------------------------------------------------------
        ///
        public double getInterestMonth()
        {
            return (amount * interestPercentage / 100) / 12;
        }

        /// -------------------------------------------------------------
        ///
        public double calculatePrincipal(int months)
        {
            double remainingAmount = amount;
            double interest = 0;

            for (int m = 0; m < months; m++)
            {
                interest = (-remainingAmount * interestPercentage / 100) / 12;
                remainingAmount = remainingAmount + (monthlyPayment - interest);
            }

            return System.Math.Abs(amount - remainingAmount);
        }

        /// -------------------------------------------------------------
        ///
        public Loan clone()
        {
            return new Loan(this.title, this.amount, this.interestPercentage, this.monthlyPayment, this.shortName);
        }
    }
}