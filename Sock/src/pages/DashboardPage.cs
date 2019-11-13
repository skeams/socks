using System.Collections.Generic;

namespace Sock
{
    public class DashboardPage : Page
    {
        public override List<string> pageInfo { get; set; }
        public override Budget currentBudget { get; set; }
        
        List<Budget> budgets;

        public DashboardPage(Budget budget, List<Budget> budgets)
        {
            this.currentBudget = budget;
            this.budgets = budgets;
            this.pageInfo = new List<string>
            {
                "Socks Main Page","",
                "Current budget open: " + budget.title,
            };
        }

        /// -------------------------------------------------------------
        ///
        public override void handleCommand(string command)
        {
            switch (command)
            {
                case "new":
                    newBudgetAction();
                    break;

                case "edit":
                    editTitleAction();
                    break;

                case "delete":
                    deleteBudgetAction();
                    break;
            }
        }

        /// -------------------------------------------------------------
        ///
        public void deleteBudgetAction()
        {
            Render.renderStatus("Which budget do you want to delete?", false);
            string deleteTitle = InputHandler.processInput("Title");

            Budget deleteBudget = null;
            foreach (Budget budget in budgets)
            {
                if (budget.title.ToLower().Equals(deleteTitle.ToLower()))
                {
                    deleteBudget = budget;
                    break;
                }
            }

            if (deleteBudget != null && !deleteBudget.title.Equals(currentBudget.title))
            {
                string confirmation = InputHandler.processInput("Confirm deletion (y/n)");
                if (confirmation.Equals("y"))
                {
                    budgets.Remove(deleteBudget);
                }
            }
        }

        /// -------------------------------------------------------------
        ///
        public void newBudgetAction()
        {
            Render.renderStatus("Enter budget title", false);
            Budget newBudget = new Budget();
            newBudget.title = InputHandler.processInput("Title");
            budgets.Add(newBudget);
        }

        /// -------------------------------------------------------------
        ///
        public void editTitleAction()
        {
            Render.renderStatus("Which budget do you want to edit?", false);
            string editTitle = InputHandler.processInput("Title");

            Budget editBudget = null;
            foreach (Budget budget in budgets)
            {
                if (budget.title.ToLower().Equals(editTitle.ToLower()))
                {
                    editBudget = budget;
                    break;
                }
            }

            if (editBudget != null)
            {
                Render.renderStatus("Edit budget (" + editBudget.title + ")", false);
                editBudget.title = InputHandler.processInput("Title");
            }
        }

        /// -------------------------------------------------------------
        ///
        public override void renderContent()
        {
            Render.renderStatus("new/edit/delete", false);

            List<string> lines = new List<string>
            {
                "Welcome to Socks!", "", "To see and update your current budget and monthly budget, go to the Current Finance page (budget).","",
                "To play around with debt and savings forecast of your current budget, go to the Forecast page (forecast).","",
                "Every page has a set of commands, but you can also use the navigational commands wherever you are. The some page information will be " +
                "given in the upper right, and a helper-status will display in the lower right.","",
                "When you open/close the application, your budget data is read/stored in data.csv.",
                "You can set the name of your budget by using the 'title' command.","","",""
            };

            lines.Add("~~~~  Available budgets  ~~~~~");
            lines.Add("");
            foreach (Budget budget in budgets)
            {
                lines.Add(" - " + budget.title);
            }

            Render.renderColumnContent(lines);
        }
    }
}