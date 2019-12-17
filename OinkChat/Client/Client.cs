using Shared;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    /// <summary>
    /// Represents the client.
    /// Inherit from the abstract class Runner
    /// </summary>
    class Client : Runner
    {
        public Client(TcpClient client) : base(client, new ClientMailer()) { }
    }
}
