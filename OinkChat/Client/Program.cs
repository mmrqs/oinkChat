using Shared;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            new Client(new TcpClient(Constants.Hostname, Constants.Port)).Run();
        }
    }
}
