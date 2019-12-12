﻿using Shared;
using Shared.Messagers;
using Shared.Messages;
using System;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    public delegate void MessageEventHandler(object sender, Message e);

    public class Client
    {
        private event MessageEventHandler MessageEvent;

        private string _hostname;
        private int _port;

        private Receiver _r;
        private Sender _s;

        private TcpClient _client;

        public Client(string hostname, int port)
        {
            _hostname = hostname;
            _port = port;
        }

        private void Init()
        {
            _client = new TcpClient(_hostname, _port);
            Console.WriteLine("Connected");

            _r = new Receiver(_client);
            _s = new Sender(_client);

            _r.Subscription(Do);
            Subscription(_s.ReceiveMessage);
        }

        public void Run()
        {
            Init();

            new Thread(_s.Run).Start();
            new Thread(_r.Run).Start();

            while (true) 
            {
                MessageEvent(this, new DumbMessage(Console.ReadLine()));
            }
        }

        public void Do(object sender, Message message)
        {
            Console.WriteLine("$ " + message.ToString());
        }

        public void Subscription(MessageEventHandler method)
        {
            MessageEvent += method;
        }
    }
}