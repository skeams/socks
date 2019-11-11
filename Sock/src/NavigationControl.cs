namespace Sock
{
    public class NavigationControl
    {
        public Page currentPage;

        CurrentFinancePage currentFinancePage;
        DashboardPage dashboardPage;

        public NavigationControl(CurrentFinance finance)
        {
            this.currentFinancePage = new CurrentFinancePage(finance);
            this.dashboardPage = new DashboardPage(finance);

            this.currentPage = this.dashboardPage;
        }

        /// -------------------------------------------------------------
        ///
        public bool navigate(string command)
        {
            switch (command)
            {
                case "budget":
                    this.currentPage = currentFinancePage;
                    break;

                case "back":
                case "home":
                    this.currentPage = dashboardPage;
                    break;

            }
            return false;
        }
    }
}