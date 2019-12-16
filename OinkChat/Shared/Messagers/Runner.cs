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
        
        /// <summary>
        /// Constructor of the class
        ///
        /// It initializes :
        ///
        /// - client : The TcpClient
        /// - communicator : The object allowing the transmission of messages between the server and the client
        /// - CancellationTokenSource : CancellationTokenSource
        /// - token : CancellationToken
        /// - Receiver : object allowing the reception of a message
        /// - Sender : object allowing the sending of messages
        /// - Mailer : the object that will handle the message, create an output and send it to the sender object
        /// </summary>
        /// <param name="client">The TcpClient</param>
        /// <param name="mailer">the object that will handle the message, create an output and send it to the sender object</param>
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

        /// <summary>
        /// Subscribes the sender to the Mailer and the Mailer to the Receiver
        /// Subscribes the method Stop to the communicator.
        /// </summary>
        public void Init()
        {
            Receiver.Subscription(Mailer.OnMessageReceived);
            Mailer.Subscription(Sender.ReceiveMessage);
            Communicator.Subscription(Stop);
        }

        /// <summary>
        /// The method is virtual because Dispatch override it
        /// Run the threads Receiver, Sender, Mailer
        /// </summary>
        public virtual void Run()
        {
            Init();
            new Thread(() => Receiver.Run(Token)).Start();
            new Thread(() => Sender.Run(Token)).Start();
            new Thread(() => Mailer.Run(Token)).Start();
        }

        /// <summary>
        /// The method is virtual because Dispach override it
        /// Allows to stop the threads when the communicator raises a signal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public virtual void Stop(object sender, Message message)
        {
            Client.Close();
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
        }
    }
}
