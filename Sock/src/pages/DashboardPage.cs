using System.Collections.Generic;

namespace Sock
{
    public class DashboardPage : IPage
    {
        public List<string> pageInfo { get; set; }

        CurrentFinance finance;
    
        public DashboardPage(CurrentFinance finance)
        {
            this.finance = finance;
            this.pageInfo = new List<string>
            {
                "Welcome to Socks!","",
                "More info will follow.",
            };
        }

        public void handleCommand(string command)
        {
            switch (command)
            {
                case "budget":

                    break;
            }
        }

        public void renderContent()
        {
            Render.renderColumnContent(new List<string>{"Welcome to Socks!"});
        }
    }
}