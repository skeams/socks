using System.Collections.Generic;
using System.IO;
using System;

namespace Sock
{
    public static class DataLoader
    {
        /// -------------------------------------------------------------
        ///
        public static string readFileContent(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(@filePath);
            }
            else
            {
                return null;
                // TODO: Add status that path was not found
                // -----> Make a file loader page?
            }
        }

        /// -------------------------------------------------------------
        ///
        public static void writeFileContent(string fileData, string filePath)
        {
            if (File.Exists(filePath))
            {
                File.WriteAllText(@filePath, fileData);
            }
            else
            {
                // TODO: Add status that path was not found
                // -----> Make a file loader page?
            }
        }

        /// -------------------------------------------------------------
        ///
        public static CurrentFinance csvToCurrentFinance(string fileData)
        {
            CurrentFinance finance = new CurrentFinance();

            foreach (string line in fileData.Split(';'))
            {
                string[] lineParts = line.Split(',');

                if (lineParts.Length > 0)
                {
                    switch(lineParts[0])
                    {
                        case "name":
                            if (lineParts.Length == 2)
                            {
                                finance.title = lineParts[1];
                            }
                            break;

                        case "item":
                            if (lineParts.Length == 3)
                            {
                                FinanceItem item = new FinanceItem(lineParts[1], double.Parse(lineParts[2]));
                                finance.monthlyBudgetItems.Add(item);
                            }
                            break;

                        case "loan":
                            if (lineParts.Length == 6)
                            {
                                finance.addLoan(new Loan(
                                    lineParts[1],
                                    lineParts[2],
                                    double.Parse(lineParts[3]),
                                    double.Parse(lineParts[4]),
                                    double.Parse(lineParts[5])
                                ));
                            }
                            break;

                        case "savings":
                            if (lineParts.Length == 3)
                            {
                                finance.currentSavings = double.Parse(lineParts[1]);
                                finance.savingsGrowthRate = double.Parse(lineParts[2]);
                            }
                            break;
                    }
                }
            }

            return finance;
        }

        /// -------------------------------------------------------------
        ///
        public static string currentFinanceToCsv(CurrentFinance finance)
        {
            string csvResult = "name," + finance.title + ';';

            foreach (FinanceItem item in finance.monthlyBudgetItems)
            {
                csvResult += "item," + item.title + ',' + item.amount + ';';
            }

            foreach (Loan loan in finance.loans)
            {
                csvResult += "loan," + loan.title + ',' + loan.shortName + ',' + loan.amount+ ','
                    + loan.interestPercentage + ',' + loan.monthlyPayment + ';';
            }

            return csvResult + "savings," + finance.currentSavings + ',' + finance.savingsGrowthRate;
        }
    }
}