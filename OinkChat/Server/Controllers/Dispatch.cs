using Shared.Messagers;
using System;
using System.Net.Sockets;
using System.Threading;

namespace Server.Controllers
{
    class Dispatch
    {
        private TcpClient _client;

        private ChatData _data;

        private DispatchSession _session;
        private ServerMailer _mailer;
        public Dispatch(TcpClient client, ChatData chatData)
        {
            _client = client;
            _data = chatData;

            _session = new DispatchSession();

            _session.Receiver = new Receiver(_client);
            _session.Sender = new Sender(_client);
            _mailer = new ServerMailer(_data, _session);
        }

        public void HandleClient()
        {
            Console.WriteLine(_client + " has been dispatched");

            _session.Receiver.Subscription(_mailer.Action);
            _mailer.Subscription(_session.Sender.ReceiveMessage);

            new Thread(_session.Receiver.Run).Start();
            new Thread(_session.Sender.Run).Start();
            new Thread(_mailer.Run).Start();
        }
    }
}
