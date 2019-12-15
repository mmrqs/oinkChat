using System;
using System.Net.Sockets;
using System.Threading;
using Shared;
using Shared.Messages;

namespace Shared.Messagers
{
    public delegate void MessageReceivedEventHandler(object sender, Message e);
    public class Receiver
    {
        public event MessageReceivedEventHandler MessageReceivedEvent;

        private TcpClient _client;
        private Communicator _communicator;
        public Receiver(TcpClient client, Communicator communicator)
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
                else 
                { 
                    Message input = _communicator.Receive(_client.GetStream());
                
                    if (input != null && MessageReceivedEvent != null)
                    {
                        MessageReceivedEvent(this, input);
                    }
                }
            }
        }
        
        public void Subscription(MessageReceivedEventHandler method)
        {
            MessageReceivedEvent += method;
        }
    }
}