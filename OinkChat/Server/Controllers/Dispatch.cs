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

        public Dispatch(TcpClient client, ChatData data) : base(client, null)
        {
            _data = data;

            _session = new DispatchSession();
            _session.Receiver = Receiver;
            _session.Sender = Sender;

            Mailer = new ServerMailer(_data, _session);
        }

        public override void Run()
        {
            Console.WriteLine(Client.Client + " has been dispatched");

            base.Run();

            _session.Sender.ReceiveMessage(this, Pig());
            _session.Sender.ReceiveMessage(this, Help());
        }

        public override void Stop(object sender, Message message)
        {
            _data.DeleteUserOnline(_session.Sender);

            base.Stop(sender, message);

            Console.WriteLine(message.Text);
        }

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

        private Message Help()
        {
            return new DumbMessage("Welcome to OinkChat !",
                "You can ask for help anytime with the command « help ».",
                "Be careful, all the commands are case-sensitve !",
                "Have a nice chat !", "Oink oink !");
        }
    }
}
