using System.Collections.Generic;

namespace Sock
{
    public interface IPage
    {
        string pageTitle { get; set; }
        List<string> commands { get; set; }
        
        void renderContent();
    }
}