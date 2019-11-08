using System.Collections.Generic;

namespace Sock
{
    public interface IPage
    {
        List<string> pageInfo { get; set; }
        List<string> commands { get; set; }
        
        void renderContent();
    }
}