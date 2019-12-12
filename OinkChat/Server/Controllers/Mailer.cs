using System.Net.Sockets;
using Server.Handlers;
using Shared.Messages;

namespace Server.Controllers
{
    public delegate void MessageEventHandler(object sender, Message e);
    class Mailer
    {
        private ChatData _data;
        private TcpClient _client;
        private DispatchSession _session;
        private event MessageEventHandler MessageEvent;
        
        public Mailer(TcpClient client, ChatData data, DispatchSession session)
        {
            _client = client;
            _data = data;
            _session = session;
        }

        public void Run()
        {
            while (true)
            {
                
            }
        }
        public void Action(object sender, Message message )
        {
            Message m = new HandlerFactory().GetHandler(_data, _session).Handle(message);
            
            if (this.MessageEvent != null)
            {
                this.MessageEvent(this, m);
            }
        }
        
        public void Subscription(MessageEventHandler method)
        {
            this.MessageEvent += method;
        }
    }
}