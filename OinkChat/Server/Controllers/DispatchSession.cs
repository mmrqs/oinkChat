using Shared.Models;
using Shared;
using Shared.Messagers;
using System.Collections.Generic;

namespace Server.Controllers
{
    /// <summary>
    /// Stores all the client related data.
    /// </summary>
    class DispatchSession
    {
        public bool IsLogged { get; set; }
        public List<Topic> TopicsJoined { get; set; }
        public Sender Sender { get; set; }
        public Receiver Receiver { get; set; }
        public User User { get; set; }

        public DispatchSession()
        {
            TopicsJoined = new List<Topic>();
        }
    }
}
