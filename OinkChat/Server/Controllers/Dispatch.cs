using Server.Handlers;
using Shared;
using Shared.Messages;
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
        private Mailer _mailer;
        public Dispatch(TcpClient client, ChatData chatData)
        {
            _client = client;
            _data = chatData;
            _session = new DispatchSession();
            _session.Receiver = new Receiver(_client);
            _session.Sender = new Sender(_client, _data, _session);
            _mailer = new Mailer(_client, _data, _session);
        }

        public void HandleClient()
        {
            Console.WriteLine(_client + " has been dispatched");
            
            Thread receiver = new Thread(_session.Receiver.Run);
            Thread sender = new Thread(_session.Sender.Run);
            Thread mailer = new Thread(_mailer.Run);

            _session.Receiver.Subscription(_mailer.Action);
            _mailer.Subscription(_session.Sender.ReceiveMessage);
            
            receiver.Start();
            sender.Start();
        }
    }
}
