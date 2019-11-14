using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sock
{
    public static class DataLoader
    {

        /// -------------------------------------------------------------
        ///
        public static string readFileContent(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return "";
            }
            else
            {
                return File.ReadAllText(filePath);
            }
        }

        /// -------------------------------------------------------------
        ///
        public static void writeFileContent(string fileData, string filePath)
        {
            if (!File.Exists(filePath))
            {
                FileStream fileStream = File.Create(filePath);
                fileStream.Write(new UTF8Encoding(true).GetBytes(fileData), 0, fileData.Length);
                fileStream.Close();
            }
            else
            {
                File.WriteAllText(filePath, fileData);
            }
        }

        /// -------------------------------------------------------------
        ///
        public static void updateBackup(string filePath, string backupPath)
        {
            if (File.Exists(filePath))
            {
                File.Copy(filePath, backupPath, true);
            }
        }

        /// -------------------------------------------------------------
        ///
        public static List<Budget> csvToBudgets(string fileData)
        {
            List<Budget> budgets = new List<Budget>();

            foreach (string budgetString in fileData.Split('#'))
            {
                Budget budget = new Budget();

                foreach (string line in budgetString.Split(';'))
                {
                    string[] lineParts = line.Split(',');

                    if (lineParts.Length > 0)
                    {
                        switch(lineParts[0])
                        {
                            case "name":
                                if (lineParts.Length == 2)
                                {
                                    budget.title = lineParts[1];
                                }
                                break;

                            case "item":
                                if (lineParts.Length == 3)
                                {
                                    FinanceItem item = new FinanceItem(lineParts[1], double.Parse(lineParts[2]));
                                    budget.monthlyBudgetItems.Add(item);
                                }
                                break;

                            case "loan":
                                if (lineParts.Length == 6)
                                {
                                    budget.addLoan(new Loan(
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
                                    budget.currentSavings = double.Parse(lineParts[1]);
                                    budget.savingsGrowthRate = double.Parse(lineParts[2]);
                                }
                                break;

                            case "goal":
                                if (lineParts.Length == 4)
                                {
                                    if (lineParts[1].Equals("s"))
                                    {
                                        budget.savingsGoals.Add(new FinanceItem(
                                            lineParts[2],
                                            double.Parse(lineParts[3])
                                        ));
                                    }
                                    else
                                    {
                                        budget.debtGoals.Add(new FinanceItem(
                                            lineParts[2],
                                            double.Parse(lineParts[3])
                                        ));
                                    }
                                }
                                break;
                        }
                    }
                }

                budgets.Add(budget);
            }

            return budgets;
        }

        /// -------------------------------------------------------------
        ///
        public static string budgetsToCsv(List<Budget> budgets)
        {
            string csvResult = "";

            for (int b = 0; b < budgets.Count; b++)
            {
                csvResult += "name," + budgets[b].title;

                foreach (FinanceItem item in budgets[b].monthlyBudgetItems)
                {
                    csvResult += ";item," + item.title + ',' + item.amount;
                }

                foreach (Loan loan in budgets[b].loans)
                {
                    csvResult += ";loan," + loan.title + ',' + loan.shortName + ',' + loan.amount+ ','
                        + loan.interestPercentage + ',' + loan.monthlyPayment;
                }

                csvResult += ";savings," + budgets[b].currentSavings + ',' + budgets[b].savingsGrowthRate;

                foreach (FinanceItem goal in budgets[b].savingsGoals)
                {
                    csvResult += ";goal,s," + goal.title + ',' + goal.amount;
                }

                foreach (FinanceItem goal in budgets[b].debtGoals)
                {
                    csvResult += ";goal,d," + goal.title + ',' + goal.amount;
                }

                if (b < budgets.Count - 1)
                {
                    csvResult += '#';
                }
            }

            return csvResult;
        }
    }
}