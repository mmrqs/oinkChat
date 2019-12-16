using System;
using System.Net.Sockets;
using System.Threading;
using Shared;
using Shared.Messages;

namespace Shared.Messagers
{
    /// <summary>
    /// The delegate of the event MessageReceivedEvent
    /// it delegate to the Mailer method : OnMessageReceived
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    public delegate void MessageReceivedEventHandler(object sender, Message message);
    public class Receiver
    {
        /// <summary>
        /// This event is raised when it receives a message
        /// </summary>
        public event MessageReceivedEventHandler MessageReceivedEvent;

        private TcpClient _client;
        private Communicator _communicator;

        /// <summary>
        /// class constructor
        ///
        /// it initializes :
        /// - client : The TcpClient
        /// - communicator : The object allowing the transmission of messages between the server and the client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="communicator"></param>
        public Receiver(TcpClient client, Communicator communicator)
        {
            _client = client;
            _communicator = communicator;
        }

        /// <summary>
        /// while the cancellation isn't requested, it receives a stream and if the input corresponding isn't null it will raises an event
        /// notifying the mailer that there is a new message
        /// </summary>
        /// <param name="token"> the cancellation token </param>
        public void Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Message input = _communicator.Receive(_client.GetStream());
                
                if (input != null)
                    MessageReceivedEvent?.Invoke(this, input);
            }
        }
        
        /// <summary>
        /// Allows the subscription to the MessageReceivedEvent event
        /// </summary>
        /// <param name="method"></param>
        public void Subscription(MessageReceivedEventHandler method)
        {
            MessageReceivedEvent += method;
        }
    }
}