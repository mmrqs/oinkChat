using System;
using System.Net.Sockets;
using System.Threading;
using Shared.Messages;

namespace Shared.Messagers
{
    public class Sender
    {
        private TcpClient _client;
        private Communicator _communicator;
        
        public Sender(TcpClient client, Communicator communicator)
        {
            _client = client;
            _communicator = communicator;
        }

        public void Run(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        public void ReceiveMessage(object sender, Message input)
        {
            _communicator.Send(_client.GetStream(), input);
        }
    }
}