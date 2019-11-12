using Xunit;

namespace Sock
{
    public class DataLoaderTest
    {
        [Fact]
        public void testCsvToCurrentFinance()
        {
            CurrentFinance blueprint = new CurrentFinance();
            blueprint.monthlyBudgetItems.Add(new FinanceItem("Buss", -780));
            blueprint.addLoan(new Loan("Mortgage", "Mtg", -1500000, 2.65, 8000));
            blueprint.savingsGrowthRate = 3;
            blueprint.currentSavings = 50000;
            blueprint.title = "Martin";

            CurrentFinance result = DataLoader.csvToCurrentFinance("name,Martin;item,Buss,-780;loan,Mortgage,Mtg,-1500000,2.65,8000;savings,50000,3");

            Assert.Equal(blueprint.title, result.title);
            Assert.Equal(blueprint.currentSavings, result.currentSavings);
            Assert.Equal(blueprint.savingsGrowthRate, result.savingsGrowthRate);
            Assert.Equal(blueprint.loans.Count, result.loans.Count);
            Assert.Equal(blueprint.monthlyIntrPrinc.Count, result.monthlyIntrPrinc.Count);
            Assert.Equal(blueprint.monthlyBudgetItems.Count, result.monthlyBudgetItems.Count);
            Assert.Equal(blueprint.loans[0].interestPercentage, result.loans[0].interestPercentage);
        }

         [Fact]
        public void testCurrentFinanceToCsv()
        {
            CurrentFinance finance = new CurrentFinance();
            finance.monthlyBudgetItems.Add(new FinanceItem("Buss", -780));
            finance.addLoan(new Loan("Car loan", "Car", -80000, 6.5, 5000));
            finance.savingsGrowthRate = 5;
            finance.currentSavings = 20000;
            finance.title = "Ole";

            string blueprint = "name,Ole;item,Buss,-780;loan,Car loan,Car,-80000,6.5,5000;savings,20000,5";

            Assert.Equal(blueprint, DataLoader.currentFinanceToCsv(finance));
        }
    }
}