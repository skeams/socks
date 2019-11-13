using System.Collections.Generic;
using System.Globalization;

namespace Sock
{
    public class ForecastPage : Page
    {
        public override List<string> pageInfo { get; set; }
        public override Budget currentBudget { get; set; }

        int periods;
        bool isMonthBased;
    
        public ForecastPage(Budget budget)
        {
            this.currentBudget = budget;
            this.pageInfo = new List<string>
            {
                "Forecast","",
                "Regardless of period/view, the forecast will calculate a new interest and principal for each loan every month. " +
                "When a loan is paid in full, the leftover payments will go into the savings growth.",
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
                case "years":
                    this.isMonthBased = false;
                    break;

                case "months":
                    this.isMonthBased = true;
                    break;

                case "periods":
                    Render.renderStatus("Number of periods", false);
                    this.periods = (int) InputHandler.processNumberInput("Periods", this.periods);
                    break;
            }
        }

        /// -------------------------------------------------------------
        ///
        public override void renderContent()
        {
            int lineWidth = 30;

            List<string> forecastData = new List<string>();
            forecastData.Add("Forecast for " + periods + (isMonthBased ? " months" : " years"));
            forecastData.Add("");

            double estimatedSavings = currentBudget.currentSavings;
            double monthlyBudgetSum = currentBudget.getMonthlyBudgetItemsSum();

            List<Loan> loans = new List<Loan>();
            foreach (Loan existingLoan in currentBudget.loans)
            {
                loans.Add(existingLoan.clone());
            }

            System.DateTime now = System.DateTime.Now;
            List<string> goalsAchieved = new List<string>();

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

                double monthlyLeftovers = monthlyBudgetSum;
                double debtSum = 0;

                foreach (Loan loan in loans)
                {
                    if (loan.amount != 0)
                    {
                        double interest = (-loan.amount * loan.interestPercentage / 100) / 12;
                        loan.amount += (loan.monthlyPayment - interest);
                        
                        monthlyLeftovers -= loan.monthlyPayment;

                        if (loan.amount > 0)
                        {
                            estimatedSavings += loan.amount;
                            loan.amount = 0;
                        }

                        debtSum += loan.amount;

                        if (shouldPrint)
                        {
                            forecastData.Add(Formatter.formatAmountLine(loan.title, loan.amount, lineWidth));
                        }
                    }
                }

                estimatedSavings = estimatedSavings + (estimatedSavings * (currentBudget.savingsGrowthRate / 100) / 12);
                estimatedSavings += monthlyLeftovers;
                if (shouldPrint)
                {
                    forecastData.Add(Formatter.formatAmountLine("Savings", estimatedSavings, lineWidth));
                }

                if (shouldPrint)
                {
                    foreach (FinanceItem goal in currentBudget.debtGoals)
                    {
                        if (debtSum >= goal.amount && !goalsAchieved.Contains(goal.title))
                        {
                            goalsAchieved.Add(goal.title);
                            forecastData.Add('^' + Formatter.formatAmountLine(goal.title, goal.amount, lineWidth));
                        }
                    }

                    foreach (FinanceItem goal in currentBudget.savingsGoals)
                    {
                        if (estimatedSavings >= goal.amount && !goalsAchieved.Contains(goal.title))
                        {
                            goalsAchieved.Add(goal.title);
                            forecastData.Add('^' + Formatter.formatAmountLine(goal.title, goal.amount, lineWidth));
                        }
                    }
                }
            }
            
            Render.renderStatus("years | months | periods", false);
            Render.renderColumnContent(forecastData);
        }
    }
}