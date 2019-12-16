using Shared.Messagers;
using System;
using System.Net.Sockets;
using Shared.Messages;

namespace Server.Controllers
{
    class Dispatch : Runner
    {
        private ChatData _data;
        private DispatchSession _session;

        /// <summary>
        /// Constructor of the class
        ///
        /// It initializes :
        /// 
        /// - data : All the server data
        /// - session : All the client data
        /// - session.Receiver : stores in the session the object allowing the reception of a message
        /// - session.Sender : stores in the session the object allowing the sending of messages
        /// - Mailer : the object that will handle the message, create an output and send it to the sender object
        /// 
        /// </summary>
        /// <param name="client"> The TcpClient </param>
        /// <param name="chatData"> All the server data</param>
        public Dispatch(TcpClient client, ChatData data) : base(client, null)
        {
            _data = data;

            _session = new DispatchSession();
            _session.Receiver = Receiver;
            _session.Sender = Sender;

            Mailer = new ServerMailer(_data, _session);
        }

        /// <summary>
        /// override the methods Run in Runner
        ///
        /// Run the method Run in Runner
        /// Display the welcome messages
        /// </summary>
        public override void Run()
        {
            Console.WriteLine(Client.Client + " has been dispatched");

            base.Run();

            _session.Sender.ReceiveMessage(this, Pig());
            _session.Sender.ReceiveMessage(this, Help());
        }

        /// <summary>
        /// Delete the user from the online users list
        /// It stops all the threads related to the client.
        /// Send a message to the server notifying it that a client exit the chat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pe"></param>
        public override void Stop(object sender, Message message)
        {
            _data.DeleteUserOnline(_session.Sender);

            base.Stop(sender, message);

            Console.WriteLine(message.Text);
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
