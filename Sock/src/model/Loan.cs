using System.Collections.Generic;

namespace Sock
{
    public class Loan : FinanceItem
    {
        public double interestPercentage;
        public double principal;
        public string shortName;

        public Loan(string title, double amount, double interestPercentage, double principal, string shortName) : base(title, amount)
        {
            this.interestPercentage = interestPercentage;
            this.principal = principal;
            this.shortName = shortName;
        }

        public double getInterestMonth()
        {
            return (amount * interestPercentage / 100) / 12;
        }
    }
}