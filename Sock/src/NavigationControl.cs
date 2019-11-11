namespace Sock
{
    public class NavigationControl
    {
        public Page currentPage;

        CurrentFinancePage currentFinancePage;
        DashboardPage dashboardPage;
        ForecastPage forecastPage;

        public NavigationControl(CurrentFinance finance)
        {
            this.currentFinancePage = new CurrentFinancePage(finance);
            this.dashboardPage = new DashboardPage(finance);
            this.forecastPage = new ForecastPage(finance);

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
                    return true;

                case "back":
                case "home":
                    this.currentPage = dashboardPage;
                    return true;

                case "forecast":
                    this.currentPage = forecastPage;
                    return true;
            }
            
            return false;
        }
    }
}