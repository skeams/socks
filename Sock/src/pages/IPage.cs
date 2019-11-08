using System.Collections.Generic;

namespace Sock
{
    public interface IPage
    {
        List<string> pageInfo { get; set; }
        
        void renderContent();
        void handleCommand(string command);
    }
}