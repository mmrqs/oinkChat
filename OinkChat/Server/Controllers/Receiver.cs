using System;
using System.Net.Sockets;
using Shared;
using Shared.Messages;

namespace Server.Controllers
{
    public delegate void MessageReceivedEventHandler(object sender, Message e);
    public class Receiver
    {
        private event MessageReceivedEventHandler MessageReceivedEvent;
        private TcpClient _client;
        public Receiver(TcpClient client)
        {
            _client = client;
        }
        public void Run()
        {
            while (true)
            {
                Message input = Communicator.Receive(_client.GetStream());
               
                Console.WriteLine("Received " + input);
                
                if (this.MessageReceivedEvent != null)
                {
                    this.MessageReceivedEvent(this, input);
                }
            }
        }
        
        public void Subscription(MessageReceivedEventHandler method)
        {
            this.MessageReceivedEvent += method;
        }
    }
}