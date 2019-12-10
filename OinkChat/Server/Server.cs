using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Server
    {
        private int _port;
        private IPAddress _address;

        private TcpListener _listener;

        public Server(string address, int port)
        {
            _address = IPAddress.Parse(address);
            _port = port;
        }

        public void Start()
        {
            _listener = new TcpListener(_address, _port);
            _listener.Start();

            while(true)
            {
                TcpClient client = _listener.AcceptTcpClient();
                Console.WriteLine("Connected to " + client);

                new Thread(new Dispatch(client).HandleClient).Start();
            }
        }
    }
}
