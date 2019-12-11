using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server.Controllers
{
    class ChatServer
    {
        private int _port;
        private IPAddress _address;

        private TcpListener _listener;

        private ChatData _chatData;

        public ChatServer(string address, int port)
        {
            _address = IPAddress.Parse(address);
            _port = port;

            _chatData = new ChatData();
        }

        public void Start()
        {
            _listener = new TcpListener(_address, _port);
            _listener.Start();

            while(true)
            {
                TcpClient client = _listener.AcceptTcpClient();
                Console.WriteLine("Connected to " + client);

                new Thread(new Dispatch(client, _chatData).HandleClient).Start();
            }
        }

        public ChatData ChatData { get { return _chatData; } }
    }
}
