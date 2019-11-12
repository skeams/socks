using System.Collections.Generic;

namespace Sock
{
    public class DashboardPage : Page
    {
        public override List<string> pageInfo { get; set; }

        CurrentFinance finance;

        public DashboardPage(CurrentFinance finance)
        {
            this.finance = finance;
            this.pageInfo = new List<string>
            {
                "Socks Main Page","",
                "Current budget open: " + finance.title,
            };
        }

        /// -------------------------------------------------------------
        ///
        public override void handleCommand(string command)
        {
            switch (command)
            {
                case "title":
                    editTitleAction();
                    break;
            }
        }

        /// -------------------------------------------------------------
        ///
        public void editTitleAction()
        {
            Render.renderStatus("Enter dataset title", false);
            finance.title = InputHandler.processInput("Title");
            this.pageInfo[2] = "Current budget open: " + finance.title;
        }

        /// -------------------------------------------------------------
        ///
        public override void renderContent()
        {
            Render.renderStatus("budget | forecast | title", false);
            Render.renderColumnContent(new List<string>
            {
                "Welcome to Socks!", "", "To see and update your current finance and monthly budget, go to the Current Finance page (budget).","",
                "To play around with debt and savings forecast of your current budget, go to the Forecast page (forecast).","",
                "Every page has a set of commands, but you can also use the navigational commands wherever you are. The some page information will be " +
                "given in the upper right, and a helper-status will display in the lower right.","",
                "When you open/close the application, your budget data is read/stored in data.csv.",
                "You can set the name of your budget by using the 'title' command.",
            });
        }
    }
}