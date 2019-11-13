using System.Collections.Generic;

namespace Sock
{
    public abstract class Page
    {
        public abstract  List<string> pageInfo { get; set; }
        public abstract Budget currentBudget { get; set; }
        
        public abstract void renderContent();
        public abstract void handleCommand(string command);
    }
}