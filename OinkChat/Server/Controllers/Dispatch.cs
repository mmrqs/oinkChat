using Shared.Messagers;
using System;
using System.Net.Sockets;
using System.Threading;
using Shared;
using Shared.Messages;
using System.Threading.Tasks;

namespace Server.Controllers
{
    class Dispatch
    {
        private TcpClient _client;

        private ChatData _data;

        private DispatchSession _session;
        private ServerMailer _mailer;
        private Communicator _communicator;
        
        private Task _receiverTask;
        private Task _senderTask;
        private Task _mailerTask;

        private CancellationTokenSource _cts;
        private CancellationToken _token;

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
            _mailer = new ServerMailer(_data, _session);

            _receiverTask = Task.Factory.StartNew(() => _session.Receiver.Run(_token), _token);
            _senderTask = Task.Factory.StartNew(() => _session.Sender.Run(_token), _token);
            _mailerTask = Task.Factory.StartNew(() => _mailer.Run(_token), _token);
        }

        public void HandleClient()
        {
            Console.WriteLine(_client + " has been dispatched");

            _session.Receiver.Subscription(_mailer.Action);
            _mailer.Subscription(_session.Sender.ReceiveMessage);
            _communicator.Subscription(StopClient);
        }

        public void StopClient(object sender, Message pe)
        {
            _client.Close();
            _cts.Cancel();
            _cts.Dispose();

            Console.WriteLine(pe.ToString());
        }
    }
}
