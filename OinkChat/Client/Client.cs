using Shared;
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
        
        /// <summary>
        /// Initialize the client.
        /// Create a new Sender and Receiver.
        /// Subscribe every component one to another :
        /// - Client subscribes to Receiver
        /// - Sender subscribe to Client
        /// </summary>
        private void Init()
        {
            _client = new TcpClient(_hostname, _port);
            Console.WriteLine("Connected");

            _r = new Receiver(_client, _communicator);
            _s = new Sender(_client, _communicator);

            _r.Subscription(Do);
            Subscription(_s.ReceiveMessage);
        }

        /// <summary>
        /// It runs the Init()
        /// It starts the threads Sender and Receiver
        /// It enters in an infinite loop (while the cancelation isn't requested) where it listens the keyboard.
        /// </summary>
        public void Run()
        {
            Init();
            new Thread(() => _s.Run(_token)).Start();
            new Thread(() => _r.Run(_token)).Start();
            
            while (!_token.IsCancellationRequested) 
            {
                MessageEvent(this, new ClientMessage(Console.ReadLine()));
            }
        }

        /// <summary>
        /// It display the received message from Receiver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public void Do(object sender, Message message)
        {
            Console.WriteLine("$ " + message.Text);
        }

        /// <summary>
        /// It allows the Sender subscription to the Client.
        /// </summary>
        /// <param name="method"></param>
        public void Subscription(MessageEventHandler method)
        {
            MessageEvent += method;
        }
        
        /// <summary>
        /// It allows to stop all the threads related to the client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pe"></param>
        private void Stop(object sender, Message pe)
        {
            _cts.Cancel();
            _cts.Dispose();           
        }
    }
}