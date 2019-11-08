namespace Sock
{
    public class Loan : FinanceItem
    {
        public double interest;
        public double principal;

        public Loan(string title, double amount, double interest, double principal) : base(title, amount)
        {
            this.interest = interest;
            this.principal = principal;
        }
    }
}