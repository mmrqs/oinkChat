using Shared;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Client
    {
        private string _hostname;
        private int _port;
        private TcpClient _tcpClient;

        public Client(string hostname, int port)
        {
            _hostname = hostname;
            _port = port;
        }

        public void Start()
        {
            _tcpClient = new TcpClient(_hostname, _port);
            Console.WriteLine("Connected");

            while(true)
            {
                string input = Console.ReadLine();
                Console.WriteLine(input);

                Communicator.Send(_tcpClient.GetStream(), new DumbMessage(input));
                Console.WriteLine(Communicator.Receive(_tcpClient.GetStream()));
            }
        }
    }
}
