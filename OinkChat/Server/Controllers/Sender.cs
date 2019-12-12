using System.Net.Sockets;
using Server.Handlers;
using Shared;
using Shared.Messages;

namespace Server.Controllers
{
    class Sender
    {
        private TcpClient _client;
        private ChatData _data;
        private DispatchSession _session;
        
        public Sender(TcpClient client, ChatData data, DispatchSession session)
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

        public void ReceiveMessage(object sender, Message input)
        {
            Communicator.Send(_client.GetStream(), input);
        }
    }
}