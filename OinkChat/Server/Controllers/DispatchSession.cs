using Server.Models;

namespace Server.Controllers
{
    class DispatchSession
    {
        public bool IsLogged { get; set; }
        public Topic TopicJoined { get; set; }
        public Dispatch Dispatcher { get; set; }
    }
}
