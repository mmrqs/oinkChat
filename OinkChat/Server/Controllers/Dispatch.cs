using Server.Handlers;
using Shared;
using Shared.Messages;
using System;
using System.Net.Sockets;

namespace Server.Controllers
{
    class Dispatch
    {
        private TcpClient _client;

        private ChatData _data;

        private DispatchSession _session;
        public Dispatch(TcpClient client, ChatData chatData)
        {
            _client = client;
            _data = chatData;
            _session = new DispatchSession();
            _session.Dispatcher = this;
        }

        public void HandleClient()
        {
            Console.WriteLine(_client + " has been dispatched");
            
            while(true)
            {
                IMessage input = Communicator.Receive(_client.GetStream());
                Console.WriteLine("Received " + input);

                Communicator.Send(_client.GetStream(), 
                    new HandlerFactory().GetHandler(_data, _session)
                    .Handle(input));
            }
        }
        public void ReceiveMessage(object sender, ChatMessage c)
        {
            Communicator.Send(_client.GetStream(), c);
        }
    }
    
    
}
