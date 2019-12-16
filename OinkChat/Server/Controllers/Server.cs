using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server.Controllers
{
    class Server
    {
        private int _port;
        private IPAddress _address;

        private TcpListener _listener;

        private ChatData _chatData;

        /// <summary>
        /// Class constructor
        ///
        /// It initializes :
        /// - address : Server IP address
        /// - port : server port
        /// - chatData : all the server data
        ///  
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public Server(string address, int port)
        {
            _address = IPAddress.Parse(address);
            _port = port;

            _chatData = new ChatData();
        }

        /// <summary>
        /// It starts the server.
        ///
        /// While the server runs, it accepts clients and dispatch them in a thread.
        /// </summary>
        public void Start()
        {
            _listener = new TcpListener(_address, _port);
            _listener.Start();

            while(true)
            {
                TcpClient client = _listener.AcceptTcpClient();
                Console.WriteLine("Connected to " + client);

                new Thread(new Dispatch(client, _chatData).Run).Start();
            }
        }
    }
}
