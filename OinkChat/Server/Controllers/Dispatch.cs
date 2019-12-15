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

        public void HandleClient()
        {
            Console.WriteLine(_client + " has been dispatched");

            _session.Receiver.Subscription(_serverMailer.Action);
            _serverMailer.Subscription(_session.Sender.ReceiveMessage);
            _communicator.Subscription(StopClient);

            new Thread(() => _session.Receiver.Run(_token)).Start();
            new Thread(() => _session.Sender.Run(_token)).Start();
            new Thread(() => _serverMailer.Run(_token)).Start();
        }

        public void StopClient(object sender, Message pe)
        {
            _data.DeleteUserOnline(_session.Sender);

            _client.Close();
            _cts.Cancel();
            _cts.Dispose();

            Console.WriteLine(pe.Text);
        }
    }
}
