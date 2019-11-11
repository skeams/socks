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
                "Welcome to Socks!","",
                "More info will follow.",
            };
        }

        public override void handleCommand(string command)
        {
            switch (command)
            {
                case "about":
                    break;
            }
        }

        public override void renderContent()
        {
            Render.renderColumnContent(new List<string>{"Welcome to Socks!"});
        }
    }
}