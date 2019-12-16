using Shared.Messagers;
using System;
using System.Net.Sockets;
using System.Threading;
using Shared;
using Shared.Messages;

namespace Server.Controllers
{
    class Dispatch
    {
        private TcpClient _client;

        private ChatData _data;

        private DispatchSession _session;
        
        private Communicator _communicator;

        private ServerMailer _serverMailer;
        
        private CancellationTokenSource _cts;
        private CancellationToken _token;

        /// <summary>
        /// Constructor of the class
        ///
        /// It initializes :
        /// 
        /// - client : The TcpClient
        /// - data : All the server data
        /// - cts : CancellationTokenSource
        /// - token : CancellationToken
        /// - communicator : The object allowing the transmission of messages between the server and the client
        /// - session : All the client data
        /// - session.Receiver : stores in the session the object allowing the reception of a message
        /// - session.Sender : stores in the session the object allowing the sending of messages
        /// - serverMailer : the object that will handle the message, create an output and send it to the sender object
        /// 
        /// </summary>
        /// <param name="client"> The TcpClient </param>
        /// <param name="chatData"> All the server data</param>
        public Dispatch(TcpClient client, ChatData chatData)
        {
            _client = client;
            _data = chatData;

            _cts = new CancellationTokenSource();
            _token = _cts.Token;

            _communicator = new Communicator();
            _session = new DispatchSession();

            _session.Receiver = new Receiver(_client, _communicator);
            _session.Sender = new Sender(_client, _communicator);
            _serverMailer = new ServerMailer(_data, _session);
        }

        /// <summary>
        /// Allows the client handling.
        ///
        /// It subscribes the messengers one to another and runs then each in their own thread.
        ///
        /// It displays the welcome messages.
        /// </summary>
        public void HandleClient()
        {
            Console.WriteLine(_client.Client + " has been dispatched");

            _session.Receiver.Subscription(_serverMailer.Action);
            _serverMailer.Subscription(_session.Sender.ReceiveMessage);
            _communicator.Subscription(StopClient);

            new Thread(() => _session.Receiver.Run(_token)).Start();
            new Thread(() => _session.Sender.Run(_token)).Start();
            new Thread(() => _serverMailer.Run(_token)).Start();

            _session.Sender.ReceiveMessage(this, Pig());
            _session.Sender.ReceiveMessage(this, Help());
        }

        /// <summary>
        /// It stops all the threads related to the client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pe"></param>
        public void StopClient(object sender, Message pe)
        {
            _data.DeleteUserOnline(_session.Sender);

            _client.Close();
            _cts.Cancel();
            _cts.Dispose();

            Console.WriteLine(pe.Text);
        }

        /// <summary>
        /// Welcome message
        /// </summary>
        /// <returns></returns>
        private Message Pig()
        {
            return new DumbMessage("Oink oink !",
                "             __,---.__",
                "        __,-'         `-.",
                "       /_ /_,'           \\&",
                "       _,''               \\",
                "      (\" )            .    | ",
                "        ``--|__|--..-'`.__|");
        }

        /// <summary>
        /// Welcome message
        /// </summary>
        /// <returns></returns>
        private Message Help()
        {
            return new DumbMessage("Welcome to OinkChat !",
                "You can ask for help anytime with the command « help ».",
                "Be careful, all the commands are case-sensitve !",
                "Have a nice chat !", "Oink oink !");
        }
    }
}
