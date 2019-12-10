using Shared;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Dispatch
    {
        private TcpClient _client;

        public Dispatch(TcpClient client)
        {
            _client = client;
        }

        public void HandleClient()
        {
            Console.WriteLine("Dispatched");
            
            while(true)
            {
                Console.WriteLine(Communicator.Receive(_client.GetStream()));

                Communicator.Send(_client.GetStream(), new DumbMessage("cyka blyat"));
            }
        }
    }
}
