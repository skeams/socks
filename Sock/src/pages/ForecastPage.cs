using System.Collections.Generic;
using System.Globalization;

namespace Sock
{
    public class ForecastPage : Page
    {
        public override List<string> pageInfo { get; set; }

        CurrentFinance finance;

        int periods;
        bool isMonthBased;
    
        public ForecastPage(CurrentFinance finance)
        {
            this.finance = finance;
            this.pageInfo = new List<string>
            {
                "Debt and Savings Forecast","",
                "More info will follow.",
            };

            this.periods = 24;
            this.isMonthBased = true;
        }

        /// -------------------------------------------------------------
        ///
        public override void handleCommand(string command)
        {
            switch (command)
            {
                case "show years":
                    this.isMonthBased = false;
                    break;

                case "show months":
                    this.isMonthBased = true;
                    break;

                case "periods":
                    this.periods = (int) InputHandler.processNumberInput("Enter periods", 24);
                    break;
            }
        }

        // string getPeriodText(int p)
        // {

        // }

        /// -------------------------------------------------------------
        ///
        public override void renderContent()
        {
            int lineWidth = 30;

            List<string> forecastData = new List<string>();
            forecastData.Add("Forecast for " + periods + (isMonthBased ? " months" : " years"));
            forecastData.Add("");

            double estimatedSavings = finance.currentSavings;
            double monthlyBudgetSum = finance.getMonthlyBudgetItemsSum();

            List<Loan> loans = new List<Loan>();
            foreach (Loan existingLoan in finance.loans)
            {
                loans.Add(existingLoan.clone());
            }

            System.DateTime now = System.DateTime.Now;

            for (int p = 0; p < periods * (isMonthBased ? 1 : 12); p++)
            {
                bool shouldPrint = this.isMonthBased | (p % 12 == 0);
                now = now.AddMonths(1);
                if (shouldPrint)
                {
                    forecastData.Add("");
                    string monthName = now.ToString("MMM", CultureInfo.InvariantCulture);
                    forecastData.Add((p / (isMonthBased ? 1 : 12) + 1) + ": " + monthName + ", " + now.Year);
                }

                foreach (Loan loan in loans)
                {
                    double interest = (-loan.amount * loan.interestPercentage / 100) / 12;
                    loan.amount += (loan.monthlyPayment - interest);

                    if (shouldPrint)
                    {
                        forecastData.Add(Formatter.formatAmountLine(loan.title, loan.amount, lineWidth));
                    }
                }


            }
            
            Render.renderColumnContent(forecastData);
        }
    }
}