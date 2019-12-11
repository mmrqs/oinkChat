using Server.Models;

namespace Server.Controllers
{
    class DispatchSession
    {
        public bool IsLogged { get; set; }
        public Topic JoinTopic { get; set; }
        public Dispatch Dispatcher { get; set; }
    }
}
