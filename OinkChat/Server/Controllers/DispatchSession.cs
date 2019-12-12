using Server.Models;
using Shared.Messagers;

namespace Server.Controllers
{
    class DispatchSession
    {
        public bool IsLogged { get; set; }
        public Topic TopicJoined { get; set; }
        public Sender Sender { get; set; }
        public Receiver Receiver { get; set; }
    }
}
