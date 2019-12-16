using System;
using System.Net.Sockets;
using System.Threading;
using Shared.Messages;

namespace Shared.Messagers
{
    /// <summary>
    ///  This class allows the sending of messages
    /// </summary>
    public class Sender
    {
        private TcpClient _client;
        private Communicator _communicator;
        
        /// <summary>
        /// Class constructor
        /// It initializes :
        ///
        /// - client : The TcpClient
        /// - communicator : The object allowing the transmission of messages between the server and the client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="communicator"></param>
        public Sender(TcpClient client, Communicator communicator)
        {
            _client = client;
            _communicator = communicator;
        }

        /// <summary>
        /// While the cancellation isn't requested the thread still exist
        /// Thread.Sleep(3000) avoid the permanent solicitation of the cpu (it's for performance issues)
        /// </summary>
        /// <param name="token"></param>
        public void Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                //To avoid constant solicitation on CPU 
                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// It received messages from the Mailer and call the method Send from the Communicator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="input"></param>
        public void ReceiveMessage(object sender, Message input)
        {
            _communicator.Send(_client.GetStream(), input);
        }
    }
}