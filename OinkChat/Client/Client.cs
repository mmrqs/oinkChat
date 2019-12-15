using Shared;
using Shared.Messagers;
using Shared.Messages;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

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
        private Communicator _communicator;
        private TcpClient _client;

        private CancellationTokenSource _cts;
        private CancellationToken _token;
        public Client(string hostname, int port)
        {
            _hostname = hostname;
            _port = port;
            _communicator = new Communicator();

            _cts = new CancellationTokenSource();
            _token = _cts.Token;
        }

        private void Init()
        {
            _client = new TcpClient(_hostname, _port);
            Console.WriteLine("Connected");

            _r = new Receiver(_client, _communicator);
            _s = new Sender(_client, _communicator);

            _r.Subscription(Do);
            Subscription(_s.ReceiveMessage);
        }

        public void Run()
        {
            Init();
            new Thread(() => _s.Run(_token)).Start();
            new Thread(() => _r.Run(_token)).Start();
            
            while (true) 
            {
                MessageEvent(this, new ClientMessage(Console.ReadLine()));
                if (_token.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        public void Do(object sender, Message message)
        {
            Console.WriteLine("$ " + message.Text());
        }

        public void Subscription(MessageEventHandler method)
        {
            MessageEvent += method;
        }

        private void Stop(object sender, Message pe)
        {
            _cts.Cancel();
            _cts.Dispose();

           
        }
    }
}