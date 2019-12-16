using Shared.Messagers;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Client : Runner
    {
        public Client(TcpClient client) : base(client, null)
        {
            Mailer = new ClientMailer();
        }
    }
}
