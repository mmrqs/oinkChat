using Shared.Messages;
using System.Net.Sockets;
using System.Threading;

namespace Shared.Messagers
{
    public abstract class Runner
    {
        protected TcpClient Client;
        private Communicator Communicator;

        private CancellationTokenSource CancellationTokenSource;
        private CancellationToken Token;

        protected Receiver Receiver;
        protected Sender Sender;
        protected Mailer Mailer; 

        public Runner(TcpClient client, Mailer mailer)
        {
            Client = client;

            Communicator = new Communicator();
            CancellationTokenSource = new CancellationTokenSource();
            Token = CancellationTokenSource.Token;

            Receiver = new Receiver(Client, Communicator);
            Sender = new Sender(Client, Communicator);
            Mailer = mailer;
        }

        public void Init()
        {
            Receiver.Subscription(Mailer.OnMessageReceived);
            Mailer.Subscription(Sender.ReceiveMessage);
            Communicator.Subscription(Stop);
        }

        public virtual void Run()
        {
            Init();
            new Thread(() => Receiver.Run(Token)).Start();
            new Thread(() => Sender.Run(Token)).Start();
            new Thread(() => Mailer.Run(Token)).Start();
        }

        public virtual void Stop(object sender, Message message)
        {
            Client.Close();
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
        }
    }
}
