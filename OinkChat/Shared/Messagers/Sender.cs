using System;
using System.Net.Sockets;
using Shared.Messages;

namespace Shared.Messagers
{
    public class Sender
    {
        private TcpClient _client;
        
        public Sender(TcpClient client)
        {
            _client = client;
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