using System.Net.Sockets;
using Server.Handlers;
using Shared.Messages;

namespace Server.Controllers
{
    public delegate void MessageEventHandler(object sender, Message e);
    class ServerMailer
    {
        private event MessageEventHandler MessageEvent;
        
        private ChatData _data;
        private DispatchSession _session;
        
        public ServerMailer(ChatData data, DispatchSession session)
        {
            _data = data;
            _session = session;
        }

        public void Run()
        {
            while (true)
            {
                // lol
            }
        }
        public void Action(object sender, Message message)
        {
            Message m = new HandlerFactory().GetHandler(_data, _session).Handle(message);
            
            //Invoke : execute the delegate
            if (m != null)
                MessageEvent?.Invoke(this, m);
        }
        
        public void Subscription(MessageEventHandler method)
        {
            MessageEvent += method;
        }
    }
}